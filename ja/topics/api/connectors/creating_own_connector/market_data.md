# マーケットデータ

取引所と連携する独自アダプターを作成する場合、さまざまな種類のマーケットデータへサブスクライブするためのメソッドを実装する必要があります。これらのメソッドは [MarketDataMessage](xref:StockSharp.Messages.MarketDataMessage) メッセージを受信したときに呼び出され、取引所からデータを受信して処理するための機能を提供します。

サブスクリプションまたはサブスクリプション解除要求を処理するアルゴリズムは、概略として次のようになります。

1. [SendSubscriptionReplyAsync](xref:StockSharp.Messages.MessageAdapter.SendSubscriptionReplyAsync(System.Int64,System.Exception)) メソッドを使用して、サブスクリプション要求の受信確認を送信します。
2. [MarketDataMessage.IsSubscribe](xref:StockSharp.Messages.MarketDataMessage.IsSubscribe) プロパティを使用して、要求がサブスクリプションかサブスクリプション解除かを確認します。
3. サブスクリプションの場合、WebSocket または別の仕組み（取引所ごとに固有）を介してリアルタイムデータを受信するサブスクリプションを設定します。
4. サブスクリプション解除の場合、対応するサブスクリプションを解除します（取引所ごとに固有）。
5. サブスクリプションの種類と操作結果に応じて、[SendSubscriptionResultAsync](xref:StockSharp.Messages.MessageAdapter.SendSubscriptionResultAsync(StockSharp.Messages.ISubscriptionMessage)) または [SendSubscriptionFinishedAsync](xref:StockSharp.Messages.MessageAdapter.SendSubscriptionFinishedAsync(System.Int64,System.Nullable{System.DateTimeOffset})) メソッドを使用して、サブスクリプション結果に関するメッセージを送信します。

## ローソク足データ

独自アダプターでローソク足データへのサブスクリプションを実装する場合は、取引所がこのデータ型をどのように扱うかを考慮してください。Coinbase では、次のメソッドとプロパティがオーバーライドされています。

### サポートされる時間枠

`TimeFrames` プロパティは、アダプターがローソク足でサポートする時間枠の一覧を定義します。これにより StockSharp は、このアダプターを通じてどの時間枠を要求できるかを把握できます。

```cs
protected override IEnumerable<TimeSpan> TimeFrames { get; } = Extensions.TimeFrames.Keys.ToArray();
```

### ローソク足更新のサポート

`IsSupportCandlesUpdates` メソッドは、特定のサブスクリプション要求に対して、アダプターがリアルタイムのローソク足更新をサポートするかどうかを判定します。Coinbase の場合、5 分足の更新のみがサポートされます。

```cs
private static readonly DataType _tf5min = DataType.TimeFrame(TimeSpan.FromMinutes(5));

public override bool IsSupportCandlesUpdates(MarketDataMessage subscription)
{
	// Coinbase は WebSocket 経由の更新で 5 分足のみをサポートします
	// そのため、他の時間枠はティックから構築されます（StockSharp コアにより自動的に実行）
	return subscription.DataType2 == _tf5min;
}
```

これらのメソッドとプロパティをオーバーライドすることで、アダプターは Coinbase API の仕様を考慮しながら、ローソク足データのサブスクリプション要求を正しく処理できます。たとえば、5 分以外の時間枠が要求された場合、StockSharp は他の時間枠のローソク足を構築するためにティックデータを使用する必要があることを認識します。

### ローソク足データへのサブスクライブ

ローソク足データにサブスクライブするには、**OnTFCandlesSubscriptionAsync** メソッドを実装します。このメソッドは、ティックデータにサブスクライブするメソッドと同様に、履歴データを要求できるほか、新しいローソク足をリアルタイムで受信するサブスクリプションも設定できます。

```cs
protected override async ValueTask OnTFCandlesSubscriptionAsync(MarketDataMessage mdMsg, CancellationToken cancellationToken)
{
	// サブスクリプション要求の受信確認を送信
	await SendSubscriptionReplyAsync(mdMsg.TransactionId, cancellationToken);

	var symbol = mdMsg.SecurityId.ToSymbol();

	if (mdMsg.IsSubscribe)
	{
		var tf = mdMsg.GetTimeFrame();

		// 履歴データが要求されている場合
		if (mdMsg.From is not null)
		{
			var from = (long)mdMsg.From.Value.ToUnix();
			var to = (long)(mdMsg.To ?? DateTimeOffset.UtcNow).ToUnix();
			var left = mdMsg.Count ?? long.MaxValue;
			var step = (long)tf.Multiply(200).TotalSeconds;
			var granularity = mdMsg.GetTimeFrame().ToNative();

			while (from < to)
			{
				// 履歴ローソク足を要求
				var candles = await _restClient.GetCandles(symbol, from, from + step, granularity, cancellationToken);
				var needBreak = true;
				var last = from;

				foreach (var candle in candles.OrderBy(t => t.Time))
				{
					cancellationToken.ThrowIfCancellationRequested();

					var time = (long)candle.Time.ToUnix();

					if (time < from)
						continue;

					if (time > to)
					{
						needBreak = true;
						break;
					}

					// 各履歴ローソク足に関する情報を送信
					await SendOutMessageAsync(new TimeFrameCandleMessage
					{
						OpenPrice = (decimal)candle.Open,
						ClosePrice = (decimal)candle.Close,
						HighPrice = (decimal)candle.High,
						LowPrice = (decimal)candle.Low,
						TotalVolume = (decimal)candle.Volume,
						OpenTime = candle.Time,
						State = CandleStates.Finished,

						// サブスクリプションによってデータを識別する場合、銘柄情報の設定は不要です
						OriginalTransactionId = mdMsg.TransactionId,
					}, cancellationToken);

					if (--left <= 0)
					{
						needBreak = true;
						break;
					}

					last = time;
					needBreak = false;
				}

				if (needBreak)
					break;

				from = last;
			}
		}

		if (!mdMsg.IsHistoryOnly() && mdMsg.DataType2 == _tf5min)
		{
			// 新しいローソク足をリアルタイムで受信するようサブスクライブ
			_candlesTransIds[symbol] = mdMsg.TransactionId;
			await _socketClient.SubscribeCandles(symbol, cancellationToken);

			// サブスクリプションがオンライン状態へ移行したことを通知
			await SendSubscriptionResultAsync(mdMsg, cancellationToken);
		}
		else
		{
			// サブスクリプションが完了した（オンラインではない）ことを示す応答を送信
			await SendSubscriptionFinishedAsync(mdMsg.TransactionId, cancellationToken);
		}
	}
	else
	{
		// ローソク足の受信をサブスクリプション解除
		_candlesTransIds.Remove(symbol);
		await _socketClient.UnSubscribeCandles(symbol, cancellationToken);
	}
}
```

### ローソク足データの処理

取引所からリアルタイムで受信したローソク足データを処理するには、通常、**SessionOnCandleReceived** メソッドのようなコードを持つメソッドを実装します。このメソッドは受信データを [TimeFrameCandleMessage](xref:StockSharp.Messages.TimeFrameCandleMessage) メッセージに変換し、SendOutMessageAsync メソッドを使用して送信します。

```cs
private async ValueTask SessionOnCandleReceived(Ohlc candle, CancellationToken cancellationToken)
{
	// この銘柄に対するローソク足のアクティブなサブスクリプションがあるか確認
	if (!_candlesTransIds.TryGetValue(candle.Symbol, out var transId))
		return;

	// 新しいローソク足に関するメッセージを作成して送信
	await SendOutMessageAsync(new TimeFrameCandleMessage
	{
		OpenPrice = (decimal)candle.Open,
		ClosePrice = (decimal)candle.Close,
		HighPrice = (decimal)candle.High,
		LowPrice = (decimal)candle.Low,
		TotalVolume = (decimal)candle.Volume,
		OpenTime = candle.Time,
		State = CandleStates.Active,  // ローソク足はまだ変化する可能性があるためアクティブと見なされます

		// サブスクリプションによってデータを識別する場合、銘柄情報の設定は不要です
		OriginalTransactionId = transId,
	}, cancellationToken);
}
```

## Level 1（最良気配価格、最終価格）

### Level 1 データへのサブスクライブ

Level 1 の変更にサブスクライブするには、**OnLevel1SubscriptionAsync** メソッドを実装します。このメソッドは通常、次の処理を行います。

```cs
protected override async ValueTask OnLevel1SubscriptionAsync(MarketDataMessage mdMsg, CancellationToken cancellationToken)
{
	// サブスクリプション要求の受信確認を送信
	// これにより、要求が受信され処理中であることをシステムに通知します
	await SendSubscriptionReplyAsync(mdMsg.TransactionId, cancellationToken);

	// 銘柄識別子を取引所が理解するシンボルへ変換
	var symbol = mdMsg.SecurityId.ToSymbol();

	if (mdMsg.IsSubscribe)
	{
		// これがサブスクリプション要求の場合
		// WebSocket 経由で Level 1 データを受信するようサブスクライブ
		await _socketClient.SubscribeTicker(symbol, cancellationToken);

		// サブスクリプション成功に関するメッセージを送信
		// これにより、サブスクリプションが設定されデータが受信されることをシステムに通知します
		await SendSubscriptionResultAsync(mdMsg, cancellationToken);
	}
	else
	{
		// これがサブスクリプション解除要求の場合
		// Level 1 データ受信用サブスクリプションを解除
		await _socketClient.UnSubscribeTicker(symbol, cancellationToken);
	}
}
```

### Level 1 データの処理

取引所からリアルタイムで受信した Level 1 データを処理するには、通常、**SessionOnTickerChanged** の例のようなコードを持つメソッドを実装します。このメソッドは受信データを [Level1ChangeMessage](xref:StockSharp.Messages.Level1ChangeMessage) メッセージに変換し、SendOutMessageAsync メソッドを使用して送信します。

```cs
private async ValueTask SessionOnTickerChanged(Ticker ticker, CancellationToken cancellationToken)
{
	// Level 1 データ変更を含むメッセージを作成
	await SendOutMessageAsync(new Level1ChangeMessage
	{
		// 銘柄識別子を指定
		SecurityId = ticker.Product.ToStockSharp(),
		// データ受信時刻を設定
		ServerTime = CurrentTime.ConvertToUtc(),
	}
	// 取引所からのデータに存在する場合、各種 Level 1 フィールドを追加
	.TryAdd(Level1Fields.LastTradeId, ticker.LastTradeId)
	.TryAdd(Level1Fields.LastTradePrice, ticker.LastTradePrice?.ToDecimal())
	.TryAdd(Level1Fields.LastTradeVolume, ticker.LastTradePrice?.ToDecimal())
	.TryAdd(Level1Fields.LastTradeOrigin, ticker.LastTradeSide?.ToSide())
	.TryAdd(Level1Fields.HighPrice, ticker.High?.ToDecimal())
	.TryAdd(Level1Fields.LowPrice, ticker.Low?.ToDecimal())
	.TryAdd(Level1Fields.Volume, ticker.Volume?.ToDecimal())
	.TryAdd(Level1Fields.Change, ticker.Change?.ToDecimal())
	.TryAdd(Level1Fields.BestBidPrice, ticker.Bid?.ToDecimal())
	.TryAdd(Level1Fields.BestAskPrice, ticker.Ask?.ToDecimal())
	.TryAdd(Level1Fields.BestBidVolume, ticker.BidSize?.ToDecimal())
	.TryAdd(Level1Fields.BestAskVolume, ticker.AskSize?.ToDecimal())
	, cancellationToken);
}
```

## 板情報

### 差分板更新のサポート

独自アダプターで板情報機能を実装する場合は、取引所が差分板更新をサポートしているか確認してください。Coinbase アダプターでは、このために `IsSupportOrderBookIncrements` プロパティをオーバーライドしています。

```cs
public override bool IsSupportOrderBookIncrements => true;
```

`IsSupportOrderBookIncrements` プロパティは、アダプターが差分板更新をサポートするかどうかを示します。このプロパティを `true` に設定すると、取引所が変更ごとに完全なスナップショットではなく、部分的な板更新を送信できることを意味します。

このプロパティをオーバーライドすることで、StockSharp は板情報データの処理を最適化できます。このプロパティが `true` に設定されている場合、システムは差分更新を想定し、それを正しく処理します。

### 板情報データへのサブスクライブ

板情報の変更にサブスクライブするには、**OnMarketDepthSubscriptionAsync** メソッドを実装します。このメソッドは OnLevel1SubscriptionAsync メソッドと似た処理を行いますが、対象は板情報データです。

```cs
protected override async ValueTask OnMarketDepthSubscriptionAsync(MarketDataMessage mdMsg, CancellationToken cancellationToken)
{
	// サブスクリプション要求の受信確認を送信
	await SendSubscriptionReplyAsync(mdMsg.TransactionId, cancellationToken);

	// 銘柄識別子を取引所が理解するシンボルへ変換
	var symbol = mdMsg.SecurityId.ToSymbol();

	if (mdMsg.IsSubscribe)
	{
		// これがサブスクリプション要求の場合
		// WebSocket 経由で板情報データを受信するようサブスクライブ
		await _socketClient.SubscribeOrderBook(symbol, cancellationToken);

		// サブスクリプション成功に関するメッセージを送信
		await SendSubscriptionResultAsync(mdMsg, cancellationToken);
	}
	else
	{
		// これがサブスクリプション解除要求の場合
		// 板情報データ受信用サブスクリプションを解除
		await _socketClient.UnSubscribeOrderBook(symbol, cancellationToken);
	}
}
```

### 板情報データの処理

取引所からリアルタイムで受信した板情報データを処理するには、通常、**SessionOnOrderBookReceived** メソッドのようなコードを持つメソッドを実装します。このメソッドは受信データを [QuoteChangeMessage](xref:StockSharp.Messages.QuoteChangeMessage) メッセージに変換し、SendOutMessageAsync メソッドを使用して送信します。

```cs
private async ValueTask SessionOnOrderBookReceived(string type, string symbol, IEnumerable<OrderBookChange> changes, CancellationToken cancellationToken)
{
	var bids = new List<QuoteChange>();
	var asks = new List<QuoteChange>();

	// 変更を bids と asks に振り分ける
	foreach (var change in changes)
	{
		var side = change.Side.ToSide();
		var quotes = side == Sides.Buy ? bids : asks;
		quotes.Add(new((decimal)change.Price, (decimal)change.Size));
	}

	// 板情報の変更を含むメッセージを作成して送信
	await SendOutMessageAsync(new QuoteChangeMessage
	{
		SecurityId = symbol.ToStockSharp(),
		Bids = bids.ToArray(),
		Asks = asks.ToArray(),
		ServerTime = CurrentTime.ConvertToUtc(),

		// これが完全な板情報スナップショットか差分更新かを判定します。
		// 取引所が常に完全な板情報のみを送信し、差分をサポートしない場合、
		// このプロパティを設定する必要はまったくありません
		State = type == "snapshot" ? QuoteChangeStates.SnapshotComplete : QuoteChangeStates.Increment,
	}, cancellationToken);
}
```

## ティックデータ（約定）

### ティックデータへのサブスクライブ

ティックデータにサブスクライブするには、**OnTicksSubscriptionAsync** メソッドを実装します。このメソッドは、前述のサブスクリプションメソッドと同様の処理に加えて、要求で指定されている場合は履歴データも要求できます。

```cs
protected override async ValueTask OnTicksSubscriptionAsync(MarketDataMessage mdMsg, CancellationToken cancellationToken)
{
	// サブスクリプション要求の受信確認を送信
	await SendSubscriptionReplyAsync(mdMsg.TransactionId, cancellationToken);

	var symbol = mdMsg.SecurityId.ToSymbol();

	if (mdMsg.IsSubscribe)
	{
		// 履歴データが要求されている場合
		if (mdMsg.From is not null)
		{
			var from = (long)mdMsg.From.Value.ToUnix(false);
			var to = (long)(mdMsg.To ?? DateTimeOffset.UtcNow).ToUnix(false);
			var left = mdMsg.Count ?? long.MaxValue;

			while (from < to)
			{
				// 履歴約定を要求
				var trades = await _restClient.GetTrades(symbol, from, to, cancellationToken);
				var needBreak = true;
				var last = from;

				foreach (var trade in trades.OrderBy(t => t.Time))
				{
					cancellationToken.ThrowIfCancellationRequested();

					var time = (long)trade.Time.ToUnix();

					if (time < from)
						continue;

					if (time > to)
					{
						needBreak = true;
						break;
					}

					// 各履歴約定に関する情報を送信
					await SendOutMessageAsync(new ExecutionMessage
					{
						// メッセージがティック約定に関する情報を運ぶことを設定
						// （注文や自己約定のようなトランザクションではありません）
						DataTypeEx = DataType.Ticks,

						TradeId = trade.TradeId,
						TradePrice = trade.Price?.ToDecimal(),
						TradeVolume = trade.Size?.ToDecimal(),
						ServerTime = trade.Time,
						OriginSide = trade.Side.ToSide(),

						// 履歴では、常にサブスクリプション識別子を設定します。
						// これにより、外部コードはどのサブスクリプション向けにデータを受信したかを判別できます。
						// サブスクリプションによってデータを識別する場合、銘柄情報の設定は不要です
						OriginalTransactionId = mdMsg.TransactionId,
					}, cancellationToken);

					if (--left <= 0)
					{
						needBreak = true;
						break;
					}

					last = time;
					needBreak = false;
				}

				if (needBreak)
					break;

				from = last;
			}
		}

		if (!mdMsg.IsHistoryOnly())
		{
			// 新しい約定をリアルタイムで受信するようサブスクライブ
			await _socketClient.SubscribeTrades(symbol, cancellationToken);
		}

		// サブスクリプション成功に関するメッセージを送信
		await SendSubscriptionResultAsync(mdMsg, cancellationToken);
	}
	else
	{
		// 約定のリアルタイム受信をサブスクリプション解除
		await _socketClient.UnSubscribeTrades(symbol, cancellationToken);
	}
}
```

### ティックデータの処理

取引所からリアルタイムで受信したティックデータを処理するには、通常、**SessionOnTradeReceived** メソッドのようなコードを持つメソッドを実装します。このメソッドは受信データを [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage) メッセージへ変換し、型に [DataType.Ticks](xref:StockSharp.Messages.DataType.Ticks) を設定して、SendOutMessageAsync メソッドを使用して送信します。

```cs
private async ValueTask SessionOnTradeReceived(Trade trade, CancellationToken cancellationToken)
{
	// 新しい約定に関するメッセージを作成して送信
	await SendOutMessageAsync(new ExecutionMessage
	{
		// メッセージがティック約定に関する情報を運ぶことを設定
		// （注文や自己約定のようなトランザクションではありません）
		DataTypeEx = DataType.Ticks,

		SecurityId = trade.ProductId.ToStockSharp(),
		TradeId = trade.TradeId,
		TradePrice = (decimal)trade.Price,
		TradeVolume = (decimal)trade.Size,
		ServerTime = trade.Time,
		OriginSide = trade.Side.ToSide(),
	}, cancellationToken);
}
```

## 注文ログへのサブスクライブ

注文ログは、注文の追加、変更、削除を含む、板情報内のすべての変更に関する詳細情報です。このデータは特殊であり、すべてのデータソースで提供されるわけではありません。たとえば、Coinbase は注文ログの提供をサポートしていません。

アダプターで注文ログへのサブスクリプションを実装するには、**OnOrderLogSubscriptionAsync** メソッドを使用します。このメソッドは、[MarketDataMessage](xref:StockSharp.Messages.MarketDataMessage) メッセージを [DataType.OrderLog](xref:StockSharp.Messages.DataType.OrderLog) データ型で受信したときに呼び出されます。

以下は、注文ログをサポートする [BitStamp](https://github.com/StockSharp/Connectors/tree/main/BitStamp) コネクターから取った、このメソッドの実装例です。

```cs
protected override async ValueTask OnOrderLogSubscriptionAsync(MarketDataMessage mdMsg, CancellationToken cancellationToken)
{
	// サブスクリプション要求の受信確認を送信
	await SendSubscriptionReplyAsync(mdMsg.TransactionId, cancellationToken);

	// 銘柄識別子を通貨ペアへ変換
	var symbol = mdMsg.SecurityId.ToCurrency();

	if (mdMsg.IsSubscribe)
	{
		if (!mdMsg.IsHistoryOnly())
		{
			// 注文ログをリアルタイムで受信するようサブスクライブ
			await _pusherClient.SubscribeOrderLog(symbol, cancellationToken);
		}

		// サブスクリプション成功に関するメッセージを送信
		await SendSubscriptionResultAsync(mdMsg, cancellationToken);
	}
	else
		// 注文ログの受信をサブスクリプション解除
		await _pusherClient.UnSubscribeOrderLog(symbol, cancellationToken);
}
```

取引所から受信した注文ログデータを処理する場合は、通常、受信データを [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage) メッセージへ変換し、型に [ExecutionTypes.OrderLog](xref:StockSharp.Messages.ExecutionTypes.OrderLog) を設定する別のメソッドを使用します。

```cs
private async ValueTask SessionOnNewOrderLog(string symbol, OrderStates state, Order order, CancellationToken cancellationToken)
{
	// 注文ログ内の新しいエントリーに関する情報を含むメッセージを作成して送信
	await SendOutMessageAsync(new ExecutionMessage
	{
		DataTypeEx = DataType.OrderLog,
		SecurityId = symbol.ToStockSharp(),
		ServerTime = order.Time,
		OrderVolume = (decimal)order.Amount,
		OrderPrice = (decimal)order.Price,
		OrderId = order.Id,
		Side = order.Type.ToSide(),
		OrderState = state,
	}, cancellationToken);
}
```

アダプターのコンストラクターで、このデータ型のサポートを追加します。

```cs
this.AddSupportedMarketDataType(DataType.OrderLog);
```

## 履歴データとライブデータの処理に関する仕様

独自アダプターで履歴データ要求とライブデータ処理を実装する場合は、次の点に注意してください。

### 履歴データ

要求への応答として履歴データを送信する場合:

1. [OriginalTransactionId](xref:StockSharp.Messages.IOriginalTransactionIdMessage.OriginalTransactionId) の設定は必須です。これにより、システムは受信データを元の要求に関連付けられます。

2. [SecurityId](xref:StockSharp.Messages.SecurityId) または（ローソク足の場合）[TimeFrameCandleMessage.TimeFrame](xref:StockSharp.Messages.TimeFrameCandleMessage.TimeFrame) の設定は必須ではありませんが、禁止されているわけでもありません。StockSharp コアは、元の要求から必要な値でこれらのフィールドを自動的に設定します。

### ライブデータ

WebSocket 経由で受信した場合など、ライブデータを処理する場合:

1. [OriginalTransactionId](xref:StockSharp.Messages.IOriginalTransactionIdMessage.OriginalTransactionId) の設定は任意です。トランザクション ID が設定されていない場合、システムは対応する銘柄とデータ型のすべてのアクティブなサブスクリプションへデータを配信します。

2. [SecurityId](xref:StockSharp.Messages.SecurityId) およびその他の固有フィールド（たとえば、ローソク足の場合は [TimeFrameCandleMessage.TimeFrame](xref:StockSharp.Messages.TimeFrameCandleMessage.TimeFrame)）の設定は必須です。この情報は、システム内でデータを正しくルーティングするために必要です。
