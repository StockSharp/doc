# 市场数据

当为交易所创建自己的适配器时，您需要实现用于订阅各种类型市场数据的方法。这些方法在收到 [MarketDataMessage](xref:StockSharp.Messages.MarketDataMessage) 消息时被调用，并提供从交易所接收和处理数据的功能。

在示意上，处理订阅或取消订阅请求的算法如下所示：

1. 使用 [SendSubscriptionReplyAsync](xref:StockSharp.Messages.MessageAdapter.SendSubscriptionReplyAsync(System.Int64,System.Exception) 方法发送接收订阅请求的确认。
2. 使用 [MarketDataMessage.IsSubscribe](xref:StockSharp.Messages.MarketDataMessage.IsSubscribe) 属性检查请求是订阅还是退订。
3. 在订阅的情况下，设置订阅以通过 WebSocket 或其他机制（每个交易所特有）接收实时数据。
4. 在取消订阅的情况下，取消相应的订阅（针对每个交易所具体）。
5. 根据订阅类型和操作结果，使用 [SendSubscriptionResultAsync](xref:StockSharp.Messages.MessageAdapter.SendSubscriptionResultAsync(StockSharp.Messages.ISubscriptionMessage) 或 [SendSubscriptionFinishedAsync](xref:StockSharp.Messages.MessageAdapter.SendSubscriptionFinishedAsync(System.Int64,System.Nullable{System.DateTimeOffset}) 方法发送有关订阅结果的消息。

## 蜡烛数据

在自己的适配器中实现对蜡烛数据的订阅时，重要的是要考虑特定交易所如何处理此类数据的具体细节。在 Coinbase 的情况下，重写了以下方法和属性：

### 支持的时间范围

`TimeFrames` 属性定义了适配器支持的蜡烛时间框架列表。这使得 StockSharp 能够知道可以通过此适配器请求哪些时间框架。

```cs
protected override IEnumerable<TimeSpan> TimeFrames { get; } = Extensions.TimeFrames.Keys.ToArray();
```

### 支持蜡烛更新

`IsSupportCandlesUpdates` 方法用于确定适配器是否支持特定订阅请求的实时K线更新。在 Coinbase 的情况下，只支持5分钟K线的更新。

```cs
private static readonly DataType _tf5min = DataType.TimeFrame(TimeSpan.FromMinutes(5));

public override bool IsSupportCandlesUpdates(MarketDataMessage subscription)
{
	// Coinbase only supports 5-minute candles for updating via websockets
	// Therefore, other timeframes will be built from ticks (automatically by the StockSharp core)
	return subscription.DataType2 == _tf5min;
}
```

重写这些方法和属性允许适配器正确处理订阅蜡烛数据的请求，同时考虑到 Coinbase API 的具体特性。例如，如果请求的时间框架不是 5 分钟，StockSharp 将知道它需要使用逐笔数据来构建其他时间框架的蜡烛图。

### 订阅K线数据

要订阅蜡烛数据，实现了 **OnTFCandlesSubscriptionAsync** 方法。这个方法类似于订阅逐笔数据的方法，可以请求历史数据，也可以设置订阅以实时接收新的蜡烛数据。

```cs
protected override async ValueTask OnTFCandlesSubscriptionAsync(MarketDataMessage mdMsg, CancellationToken cancellationToken)
{
	// Send confirmation of receiving the subscription request
	await SendSubscriptionReplyAsync(mdMsg.TransactionId, cancellationToken);

	var symbol = mdMsg.SecurityId.ToSymbol();

	if (mdMsg.IsSubscribe)
	{
		var tf = mdMsg.GetTimeFrame();

		// If historical data is requested
		if (mdMsg.From is not null)
		{
			var from = (long)mdMsg.From.Value.ToUnix();
			var to = (long)(mdMsg.To ?? DateTimeOffset.UtcNow).ToUnix();
			var left = mdMsg.Count ?? long.MaxValue;
			var step = (long)tf.Multiply(200).TotalSeconds;
			var granularity = mdMsg.GetTimeFrame().ToNative();

			while (from < to)
			{
				// Request historical candles
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

					// Send information about each historical candle
					await SendOutMessageAsync(new TimeFrameCandleMessage
					{
						OpenPrice = (decimal)candle.Open,
						ClosePrice = (decimal)candle.Close,
						HighPrice = (decimal)candle.High,
						LowPrice = (decimal)candle.Low,
						TotalVolume = (decimal)candle.Volume,
						OpenTime = candle.Time,
						State = CandleStates.Finished,

						// In case of identifying data by subscription, filling instrument information is not required
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
			// Subscribe to receive new candles in real time
			_candlesTransIds[symbol] = mdMsg.TransactionId;
			await _socketClient.SubscribeCandles(symbol, cancellationToken);

			// Notify that the subscription has transitioned to online status
			await SendSubscriptionResultAsync(mdMsg, cancellationToken);
		}
		else
		{
			// Send a response that the subscription is finished (not online)
			await SendSubscriptionFinishedAsync(mdMsg.TransactionId, cancellationToken);
		}
	}
	else
	{
		// Unsubscribe from receiving candles
		_candlesTransIds.Remove(symbol);
		await _socketClient.UnSubscribeCandles(symbol, cancellationToken);
	}
}
```

### 处理蜡烛数据

为了实时处理从交易所接收到的蜡烛数据，通常会实现一个类似**SessionOnCandleReceived**方法中的代码的方法。该方法将接收到的数据转换为[TimeFrameCandleMessage](xref:StockSharp.Messages.TimeFrameCandleMessage)消息，并使用SendOutMessageAsync方法发送它。

```cs
private async ValueTask SessionOnCandleReceived(Ohlc candle, CancellationToken cancellationToken)
{
	// Check if there is an active subscription to candles for this instrument
	if (!_candlesTransIds.TryGetValue(candle.Symbol, out var transId))
		return;

	// Create and send a message about a new candle
	await SendOutMessageAsync(new TimeFrameCandleMessage
	{
		OpenPrice = (decimal)candle.Open,
		ClosePrice = (decimal)candle.Close,
		HighPrice = (decimal)candle.High,
		LowPrice = (decimal)candle.Low,
		TotalVolume = (decimal)candle.Volume,
		OpenTime = candle.Time,
		State = CandleStates.Active,  // The candle is considered active as it may still change

		// In case of identifying data by subscription, filling instrument information is not required
		OriginalTransactionId = transId,
	}, cancellationToken);
}
```

## 一级（最佳买卖价，最新价格）

### 订阅一级数据

要订阅一级变动，实现了 **OnLevel1SubscriptionAsync** 方法。此方法通常执行以下操作：

```cs
protected override async ValueTask OnLevel1SubscriptionAsync(MarketDataMessage mdMsg, CancellationToken cancellationToken)
{
	// Send confirmation of receiving the subscription request
	// This informs the system that the request has been received and is being processed
	await SendSubscriptionReplyAsync(mdMsg.TransactionId, cancellationToken);

	// Convert the instrument identifier to a symbol understood by the exchange
	var symbol = mdMsg.SecurityId.ToSymbol();

	if (mdMsg.IsSubscribe)
	{
		// If this is a subscription request
		// Subscribe to receive Level 1 data via WebSocket
		await _socketClient.SubscribeTicker(symbol, cancellationToken);

		// Send a message about successful subscription
		// This informs the system that the subscription is set up and data will be received
		await SendSubscriptionResultAsync(mdMsg, cancellationToken);
	}
	else
	{
		// If this is an unsubscription request
		// Cancel the subscription to receive Level 1 data
		await _socketClient.UnSubscribeTicker(symbol, cancellationToken);
	}
}
```

### 处理一级数据

为了实时处理从交易所接收到的一级数据，通常会实现类似 **SessionOnTickerChanged** 示例中的代码的方法。该方法将接收到的数据转换为 [Level1ChangeMessage](xref:StockSharp.Messages.Level1ChangeMessage) 消息，并使用 SendOutMessageAsync 方法发送。

```cs
private async ValueTask SessionOnTickerChanged(Ticker ticker, CancellationToken cancellationToken)
{
	// Create a message with Level 1 data changes
	await SendOutMessageAsync(new Level1ChangeMessage
	{
		// Specify the instrument identifier
		SecurityId = ticker.Product.ToStockSharp(),
		// Set the time of receiving data
		ServerTime = CurrentTime.ConvertToUtc(),
	}
	// Add various Level 1 fields if they are present in the data from the exchange
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

## 订单簿

### 支持增量订单簿更新

在自己的适配器中实现订单簿功能时，重要的是要考虑交易所是否支持增量订单簿更新。为此，在 Coinbase 适配器中重写了 `IsSupportOrderBookIncrements` 属性：

```cs
public override bool IsSupportOrderBookIncrements => true;
```

`IsSupportOrderBookIncrements` 属性指示适配器是否支持增量订单簿更新。将此属性设置为 `true` 表示交易所可以发送部分订单簿更新，而不是每次变动都发送完整快照。

覆盖此属性可使 StockSharp 优化订单簿数据的处理。如果属性设置为 `true`，系统将会预期并正确处理增量更新。

### 订阅订单簿数据

要订阅订单簿的变化，实施了 **OnMarketDepthSubscriptionAsync** 方法。该方法执行的操作类似于 OnLevel1SubscriptionAsync 方法，但针对的是订单簿数据。

```cs
protected override async ValueTask OnMarketDepthSubscriptionAsync(MarketDataMessage mdMsg, CancellationToken cancellationToken)
{
	// Send confirmation of receiving the subscription request
	await SendSubscriptionReplyAsync(mdMsg.TransactionId, cancellationToken);

	// Convert the instrument identifier to a symbol understood by the exchange
	var symbol = mdMsg.SecurityId.ToSymbol();

	if (mdMsg.IsSubscribe)
	{
		// If this is a subscription request
		// Subscribe to receive order book data via WebSocket
		await _socketClient.SubscribeOrderBook(symbol, cancellationToken);

		// Send a message about successful subscription
		await SendSubscriptionResultAsync(mdMsg, cancellationToken);
	}
	else
	{
		// If this is an unsubscription request
		// Cancel the subscription to receive order book data
		await _socketClient.UnSubscribeOrderBook(symbol, cancellationToken);
	}
}
```

### 处理订单簿数据

为了实时处理从交易所接收到的订单簿数据，通常会实现一个类似于 **SessionOnOrderBookReceived** 方法的代码方法。该方法将接收到的数据转换为 [QuoteChangeMessage](xref:StockSharp.Messages.QuoteChangeMessage) 消息，并使用 SendOutMessageAsync 方法发送它。

```cs
private async ValueTask SessionOnOrderBookReceived(string type, string symbol, IEnumerable<OrderBookChange> changes, CancellationToken cancellationToken)
{
	var bids = new List<QuoteChange>();
	var asks = new List<QuoteChange>();

	// Distribute changes by bids and asks
	foreach (var change in changes)
	{
		var side = change.Side.ToSide();
		var quotes = side == Sides.Buy ? bids : asks;
		quotes.Add(new((decimal)change.Price, (decimal)change.Size));
	}

	// Create and send a message with changes in the order book
	await SendOutMessageAsync(new QuoteChangeMessage
	{
		SecurityId = symbol.ToStockSharp(),
		Bids = bids.ToArray(),
		Asks = asks.ToArray(),
		ServerTime = CurrentTime.ConvertToUtc(),

		// Determine if this is a full order book snapshot or an incremental update.
		// If the exchange always sends only full order books and does not support incrementality,
		// then setting this property is not required at all
		State = type == "snapshot" ? QuoteChangeStates.SnapshotComplete : QuoteChangeStates.Increment,
	}, cancellationToken);
}
```

## 逐笔数据（交易）

### 订阅 Tick 数据

要订阅行情数据，实现了 **OnTicksSubscriptionAsync** 方法。该方法除了执行与前面订阅方法类似的操作外，如果在请求中指定，还可以请求历史数据。

```cs
protected override async ValueTask OnTicksSubscriptionAsync(MarketDataMessage mdMsg, CancellationToken cancellationToken)
{
	// Send confirmation of receiving the subscription request
	await SendSubscriptionReplyAsync(mdMsg.TransactionId, cancellationToken);

	var symbol = mdMsg.SecurityId.ToSymbol();

	if (mdMsg.IsSubscribe)
	{
		// If historical data is requested
		if (mdMsg.From is not null)
		{
			var from = (long)mdMsg.From.Value.ToUnix(false);
			var to = (long)(mdMsg.To ?? DateTimeOffset.UtcNow).ToUnix(false);
			var left = mdMsg.Count ?? long.MaxValue;

			while (from < to)
			{
				// Request historical trades
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

					// Send information about each historical trade
					await SendOutMessageAsync(new ExecutionMessage
					{
						// Set that the message carries information about a tick trade
						// (not a transaction like an order or own trade)
						DataTypeEx = DataType.Ticks,

						TradeId = trade.TradeId,
						TradePrice = trade.Price?.ToDecimal(),
						TradeVolume = trade.Size?.ToDecimal(),
						ServerTime = trade.Time,
						OriginSide = trade.Side.ToSide(),

						// For history, always set the subscription identifier,
						// so that the external code can understand which subscription the data was received for.
						// In case of identifying data by subscription, filling instrument information is not required
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
			// Subscribe to receive new trades in real time
			await _socketClient.SubscribeTrades(symbol, cancellationToken);
		}

		// Send a message about successful subscription
		await SendSubscriptionResultAsync(mdMsg, cancellationToken);
	}
	else
	{
		// Unsubscribe from receiving trades in real time
		await _socketClient.UnSubscribeTrades(symbol, cancellationToken);
	}
}
```

### 处理逐笔数据

要实时处理从交易所接收的行情数据，通常会实现一个类似 **SessionOnTradeReceived** 方法的代码。该方法将接收到的数据转换为具有 [DataType.Ticks](xref:StockSharp.Messages.DataType.Ticks) 类型的 [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage) 消息，并使用 SendOutMessageAsync 方法发送。

```cs
private async ValueTask SessionOnTradeReceived(Trade trade, CancellationToken cancellationToken)
{
	// Create and send a message about a new trade
	await SendOutMessageAsync(new ExecutionMessage
	{
		// Set that the message carries information about a tick trade
		// (not a transaction like an order or own trade)
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

## 订阅订单日志

订单日志是有关订单簿中所有变化的详细信息，包括订单的添加、修改和删除。此数据是特定的，并非所有数据源都提供。例如，Coinbase 不支持提供订单日志。

要在适配器中实现对订单日志的订阅，使用 **OnOrderLogSubscriptionAsync** 方法。当收到包含 [DataType.OrderLog](xref:StockSharp.Messages.DataType.OrderLog) 数据类型的 [MarketDataMessage](xref:StockSharp.Messages.MarketDataMessage) 消息时，将调用此方法。

下方是从支持订单日志的 [BitStamp](https://github.com/StockSharp/StockSharp/tree/master/Connectors/BitStamp) 连接器中提取的该方法实现示例：

```cs
protected override async ValueTask OnOrderLogSubscriptionAsync(MarketDataMessage mdMsg, CancellationToken cancellationToken)
{
	// Send confirmation of receiving the subscription request
	await SendSubscriptionReplyAsync(mdMsg.TransactionId, cancellationToken);

	// Convert the instrument identifier to a currency pair
	var symbol = mdMsg.SecurityId.ToCurrency();

	if (mdMsg.IsSubscribe)
	{
		if (!mdMsg.IsHistoryOnly())
		{
			// Subscribe to receive order log in real time
			await _pusherClient.SubscribeOrderLog(symbol, cancellationToken);
		}

		// Send a message about successful subscription
		await SendSubscriptionResultAsync(mdMsg, cancellationToken);
	}
	else
		// Unsubscribe from receiving order log
		await _pusherClient.UnSubscribeOrderLog(symbol, cancellationToken);
}
```

在处理从交易所接收的订单日志数据时，通常使用一个单独的方法，该方法将接收到的数据转换为具有 [ExecutionTypes.OrderLog](xref:StockSharp.Messages.ExecutionTypes.OrderLog) 类型的 [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage) 消息：

```cs
private async ValueTask SessionOnNewOrderLog(string symbol, OrderStates state, Order order, CancellationToken cancellationToken)
{
	// Create and send a message with information about a new entry in the order log
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

重要的是不要忘记在适配器构造函数中添加对这种数据类型的支持：

```cs
this.AddSupportedMarketDataType(DataType.OrderLog);
```

## 处理历史数据和实时数据的具体细节

在自己的适配器中实现历史数据请求和处理实时数据时，重要的是要考虑以下几点：

### 历史数据

在响应请求时发送历史数据：

1. 设置 [OriginalTransactionId](xref:StockSharp.Messages.IOriginalTransactionIdMessage.OriginalTransactionId) 是必需的。这允许系统将接收到的数据与原始请求关联起来。

2. 设置 [SecurityId](xref:StockSharp.Messages.SecurityId) 或 [TimeFrameCandleMessage.TimeFrame](xref:StockSharp.Messages.TimeFrameCandleMessage.TimeFrame)（在蜡烛的情况下）不是必需的，但也不禁止。StockSharp 核心将自动使用原始请求中的必要值填充这些字段。

### 实时数据

在处理实时数据时，例如，通过 WebSocket 接收的数据：

1. 设置 [OriginalTransactionId](xref:StockSharp.Messages.IOriginalTransactionIdMessage.OriginalTransactionId) 是可选的。如果未设置交易ID，系统将会把数据分发到对应工具和数据类型的所有活跃订阅中。

2. 设置 [SecurityId](xref:StockSharp.Messages.SecurityId) 和其他特定字段（例如，用于蜡烛图的 [TimeFrameCandleMessage.TimeFrame](xref:StockSharp.Messages.TimeFrameCandleMessage.TimeFrame)）是强制性的，因为这些信息对于系统中数据的正确路由是必要的。