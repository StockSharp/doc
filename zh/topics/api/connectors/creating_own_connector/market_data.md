# 市场数据

在为交易所创建自己的适配器时，需要实现用于订阅各种类型市场数据的方法。当收到 [MarketDataMessage](xref:StockSharp.Messages.MarketDataMessage) 消息时会调用这些方法，用于**接收和处理**来自交易所的数据。

处理订阅或取消订阅请求的算法示意如下：

1. 使用 [SendSubscriptionReplyAsync](xref:StockSharp.Messages.MessageAdapter.SendSubscriptionReplyAsync(System.Int64,System.Exception)) 方法发送已收到订阅请求的确认。
2. 使用 [MarketDataMessage.IsSubscribe](xref:StockSharp.Messages.MarketDataMessage.IsSubscribe) 属性检查该请求是订阅还是取消订阅。
3. 如果是订阅，则设置通过 WebSocket 或其他机制（因交易所而异）接收实时数据的订阅。
4. 如果是取消订阅，则取消相应的订阅（因交易所而异）。
5. 根据订阅类型和操作结果，使用 [SendSubscriptionResultAsync](xref:StockSharp.Messages.MessageAdapter.SendSubscriptionResultAsync(StockSharp.Messages.ISubscriptionMessage)) 或 [SendSubscriptionFinishedAsync](xref:StockSharp.Messages.MessageAdapter.SendSubscriptionFinishedAsync(System.Int64,System.Nullable{System.DateTimeOffset})) 方法发送订阅结果消息。

## K线数据

在自己的适配器中实现对K线数据的订阅时，需要考虑交易所处理该数据类型的方式。在 Coinbase 中，重写了以下方法和属性：

### 支持的时间框架

`TimeFrames` 属性定义了适配器为K线支持的时间框架列表。这使 StockSharp 能够知道可以通过该适配器请求哪些时间框架。

```cs
protected override IEnumerable<TimeSpan> TimeFrames { get; } = Extensions.TimeFrames.Keys.ToArray();
```

### 支持K线更新

`IsSupportCandlesUpdates` 方法确定适配器是否支持针对特定订阅请求的实时K线更新。对于 Coinbase，只支持5分钟K线的更新。

```cs
private static readonly DataType _tf5min = DataType.TimeFrame(TimeSpan.FromMinutes(5));

public override bool IsSupportCandlesUpdates(MarketDataMessage subscription)
{
	// Coinbase 仅支持通过 WebSocket 更新 5 分钟 K线
	// Therefore, other timeframes will be built from ticks (automatically by the StockSharp core)
	return subscription.DataType2 == _tf5min;
}
```

重写这些方法和属性使适配器能够在考虑 Coinbase API 具体特性的情况下正确处理K线数据订阅请求。例如，如果请求了非5分钟的时间框架，StockSharp 将知道需要使用逐笔成交数据来构建其他时间框架的K线。

### 订阅K线数据

要订阅K线数据，需要实现 **OnTFCandlesSubscriptionAsync** 方法。该方法与逐笔成交数据订阅方法类似，可以请求历史数据，也可以设置实时接收新K线的订阅。

```cs
protected override async ValueTask OnTFCandlesSubscriptionAsync(MarketDataMessage mdMsg, CancellationToken cancellationToken)
{
	// 发送已收到订阅请求的确认
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
				// 请求历史 K线
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

					// 发送每根历史 K线的信息
					await SendOutMessageAsync(new TimeFrameCandleMessage
					{
						OpenPrice = (decimal)candle.Open,
						ClosePrice = (decimal)candle.Close,
						HighPrice = (decimal)candle.High,
						LowPrice = (decimal)candle.Low,
						TotalVolume = (decimal)candle.Volume,
						OpenTime = candle.Time,
						State = CandleStates.Finished,

						// 按订阅识别数据时无需填写交易品种信息
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
			// 订阅实时接收新 K线
			_candlesTransIds[symbol] = mdMsg.TransactionId;
			await _socketClient.SubscribeCandles(symbol, cancellationToken);

			// 通知订阅已切换到在线状态
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
		// 取消接收 K线的订阅
		_candlesTransIds.Remove(symbol);
		await _socketClient.UnSubscribeCandles(symbol, cancellationToken);
	}
}
```

### 处理K线数据

为了处理从交易所实时接收的K线数据，通常会实现类似 **SessionOnCandleReceived** 方法的代码。该方法将接收到的数据转换为 [TimeFrameCandleMessage](xref:StockSharp.Messages.TimeFrameCandleMessage) 消息，并使用 SendOutMessageAsync 方法发送。

```cs
private async ValueTask SessionOnCandleReceived(Ohlc candle, CancellationToken cancellationToken)
{
	// 检查该交易品种是否有活跃的 K线订阅
	if (!_candlesTransIds.TryGetValue(candle.Symbol, out var transId))
		return;

	// 创建并发送新 K线消息
	await SendOutMessageAsync(new TimeFrameCandleMessage
	{
		OpenPrice = (decimal)candle.Open,
		ClosePrice = (decimal)candle.Close,
		HighPrice = (decimal)candle.High,
		LowPrice = (decimal)candle.Low,
		TotalVolume = (decimal)candle.Volume,
		OpenTime = candle.Time,
		State = CandleStates.Active,  // The candle is considered active as it may still change

		// 按订阅识别数据时无需填写交易品种信息
		OriginalTransactionId = transId,
	}, cancellationToken);
}
```

## Level 1（最优买卖价、最新成交价）

### 订阅 Level 1 数据

要订阅 Level 1 变化，需要实现 **OnLevel1SubscriptionAsync** 方法。该方法通常执行以下操作：

```cs
protected override async ValueTask OnLevel1SubscriptionAsync(MarketDataMessage mdMsg, CancellationToken cancellationToken)
{
	// 发送已收到订阅请求的确认
	// 这会通知系统请求已收到并正在处理
	await SendSubscriptionReplyAsync(mdMsg.TransactionId, cancellationToken);

	// 将交易品种标识转换为交易所可识别的代码
	var symbol = mdMsg.SecurityId.ToSymbol();

	if (mdMsg.IsSubscribe)
	{
		// If this is a subscription request
		// 订阅通过 WebSocket 接收 Level1 数据
		await _socketClient.SubscribeTicker(symbol, cancellationToken);

		// 发送订阅成功消息
		// 这会通知系统订阅已设置并将接收数据
		await SendSubscriptionResultAsync(mdMsg, cancellationToken);
	}
	else
	{
		// If this is an unsubscription request
		// 取消接收 Level1 数据的订阅
		await _socketClient.UnSubscribeTicker(symbol, cancellationToken);
	}
}
```

### 处理 Level 1 数据

为了处理从交易所实时接收的 Level 1 数据，通常会实现类似 **SessionOnTickerChanged** 示例的代码。该方法将接收到的数据转换为 [Level1ChangeMessage](xref:StockSharp.Messages.Level1ChangeMessage) 消息，并使用 SendOutMessageAsync 方法发送。

```cs
private async ValueTask SessionOnTickerChanged(Ticker ticker, CancellationToken cancellationToken)
{
	// 创建包含 Level1 数据变更的消息
	await SendOutMessageAsync(new Level1ChangeMessage
	{
		// 指定交易品种标识符
		SecurityId = ticker.Product.ToStockSharp(),
		// 设置数据接收时间
		ServerTime = CurrentTime.ConvertToUtc(),
	}
	// 如果交易所数据中存在，则添加各种 Level1 字段
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

在自己的适配器中实现订单簿功能时，需要检查交易所是否支持增量订单簿更新。Coinbase 适配器为此重写了 `IsSupportOrderBookIncrements` 属性：

```cs
public override bool IsSupportOrderBookIncrements => true;
```

`IsSupportOrderBookIncrements` 属性指示适配器是否支持增量订单簿更新。将该属性设置为 `true` 意味着交易所可以发送部分订单簿更新，而不是每次变化都发送完整快照。

重写此属性使 StockSharp 能够优化订单簿数据的处理。如果该属性设置为 `true`，系统将预期并正确处理增量更新。

### 订阅订单簿数据

要订阅订单簿变化，需要实现 **OnMarketDepthSubscriptionAsync** 方法。该方法执行的操作与 OnLevel1SubscriptionAsync 方法类似，但针对的是订单簿数据。

```cs
protected override async ValueTask OnMarketDepthSubscriptionAsync(MarketDataMessage mdMsg, CancellationToken cancellationToken)
{
	// 发送已收到订阅请求的确认
	await SendSubscriptionReplyAsync(mdMsg.TransactionId, cancellationToken);

	// 将交易品种标识转换为交易所可识别的代码
	var symbol = mdMsg.SecurityId.ToSymbol();

	if (mdMsg.IsSubscribe)
	{
		// If this is a subscription request
		// 订阅通过 WebSocket 接收订单簿数据
		await _socketClient.SubscribeOrderBook(symbol, cancellationToken);

		// 发送订阅成功消息
		await SendSubscriptionResultAsync(mdMsg, cancellationToken);
	}
	else
	{
		// If this is an unsubscription request
		// 取消接收订单簿数据的订阅
		await _socketClient.UnSubscribeOrderBook(symbol, cancellationToken);
	}
}
```

### 处理订单簿数据

为了处理从交易所实时接收的订单簿数据，通常会实现类似 **SessionOnOrderBookReceived** 方法的代码。该方法将接收到的数据转换为 [QuoteChangeMessage](xref:StockSharp.Messages.QuoteChangeMessage) 消息，并使用 SendOutMessageAsync 方法发送。

```cs
private async ValueTask SessionOnOrderBookReceived(string type, string symbol, IEnumerable<OrderBookChange> changes, CancellationToken cancellationToken)
{
	var bids = new List<QuoteChange>();
	var asks = new List<QuoteChange>();

	// 按 bids 和 asks 分配变更
	foreach (var change in changes)
	{
		var side = change.Side.ToSide();
		var quotes = side == Sides.Buy ? bids : asks;
		quotes.Add(new((decimal)change.Price, (decimal)change.Size));
	}

	// 创建并发送订单簿变更消息
	await SendOutMessageAsync(new QuoteChangeMessage
	{
		SecurityId = symbol.ToStockSharp(),
		Bids = bids.ToArray(),
		Asks = asks.ToArray(),
		ServerTime = CurrentTime.ConvertToUtc(),

		// 确定这是完整订单簿快照还是增量更新。
		// If the exchange always sends only full order books and does not support incrementality,
		// 则完全不需要设置此属性
		State = type == "snapshot" ? QuoteChangeStates.SnapshotComplete : QuoteChangeStates.Increment,
	}, cancellationToken);
}
```

## 逐笔成交数据（Ticks）

### 订阅逐笔成交数据

要订阅逐笔成交数据，需要实现 **OnTicksSubscriptionAsync** 方法。除了执行与前面订阅方法类似的操作外，如果请求中指定了历史数据，该方法还可以请求历史数据。

```cs
protected override async ValueTask OnTicksSubscriptionAsync(MarketDataMessage mdMsg, CancellationToken cancellationToken)
{
	// 发送已收到订阅请求的确认
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
				// 请求历史成交
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

					// 发送每笔历史成交的信息
					await SendOutMessageAsync(new ExecutionMessage
					{
						// 设置消息包含 tick 成交信息
						// (not a transaction like an order or own trade)
						DataTypeEx = DataType.Ticks,

						TradeId = trade.TradeId,
						TradePrice = trade.Price?.ToDecimal(),
						TradeVolume = trade.Size?.ToDecimal(),
						ServerTime = trade.Time,
						OriginSide = trade.Side.ToSide(),

						// For history, always set the subscription identifier,
						// 以便外部代码理解数据属于哪个订阅。
						// 按订阅识别数据时无需填写交易品种信息
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
			// 订阅实时接收新成交
			await _socketClient.SubscribeTrades(symbol, cancellationToken);
		}

		// 发送订阅成功消息
		await SendSubscriptionResultAsync(mdMsg, cancellationToken);
	}
	else
	{
		// 取消实时接收成交的订阅
		await _socketClient.UnSubscribeTrades(symbol, cancellationToken);
	}
}
```

### 处理逐笔成交数据

为了处理从交易所实时接收的逐笔成交数据，通常会实现类似 **SessionOnTradeReceived** 方法的代码。该方法将接收到的数据转换为 [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage) 消息，其类型为 [DataType.Ticks](xref:StockSharp.Messages.DataType.Ticks)，并使用 SendOutMessageAsync 方法发送。

```cs
private async ValueTask SessionOnTradeReceived(Trade trade, CancellationToken cancellationToken)
{
	// 创建并发送新成交消息
	await SendOutMessageAsync(new ExecutionMessage
	{
		// 设置消息包含 tick 成交信息
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

订单日志是关于订单簿所有变化的详细信息，包括订单的添加、修改和删除。此数据具有特殊性，并非所有数据源都提供。例如，Coinbase 不支持提供订单日志。

要在适配器中实现对订单日志的订阅，使用 **OnOrderLogSubscriptionAsync** 方法。当收到 [MarketDataMessage](xref:StockSharp.Messages.MarketDataMessage) 消息且其数据类型为 [DataType.OrderLog](xref:StockSharp.Messages.DataType.OrderLog) 时会调用该方法。

以下是从支持订单日志的 [BitStamp](https://github.com/StockSharp/StockSharp/tree/master/Connectors/BitStamp) 连接器中提取的该方法实现示例：

```cs
protected override async ValueTask OnOrderLogSubscriptionAsync(MarketDataMessage mdMsg, CancellationToken cancellationToken)
{
	// 发送已收到订阅请求的确认
	await SendSubscriptionReplyAsync(mdMsg.TransactionId, cancellationToken);

	// 将交易品种标识符转换为货币对
	var symbol = mdMsg.SecurityId.ToCurrency();

	if (mdMsg.IsSubscribe)
	{
		if (!mdMsg.IsHistoryOnly())
		{
			// 订阅实时接收订单日志
			await _pusherClient.SubscribeOrderLog(symbol, cancellationToken);
		}

		// 发送订阅成功消息
		await SendSubscriptionResultAsync(mdMsg, cancellationToken);
	}
	else
		// 取消接收订单日志的订阅
		await _pusherClient.UnSubscribeOrderLog(symbol, cancellationToken);
}
```

在处理从交易所接收的订单日志数据时，通常使用一个单独的方法，将接收到的数据转换为 [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage) 消息，其类型为 [ExecutionTypes.OrderLog](xref:StockSharp.Messages.ExecutionTypes.OrderLog)：

```cs
private async ValueTask SessionOnNewOrderLog(string symbol, OrderStates state, Order order, CancellationToken cancellationToken)
{
	// 创建并发送包含订单日志新条目信息的消息
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

在适配器构造函数中添加对该数据类型的支持：

```cs
this.AddSupportedMarketDataType(DataType.OrderLog);
```

## 处理历史数据和实时数据的特点

在自己的适配器中实现历史数据请求和实时数据处理时，请注意以下几点：

### 历史数据

在响应请求发送历史数据时：

1. 设置 [OriginalTransactionId](xref:StockSharp.Messages.IOriginalTransactionIdMessage.OriginalTransactionId) 是必须的。这使系统能够将接收到的数据与原始请求关联起来。

2. 设置 [SecurityId](xref:StockSharp.Messages.SecurityId) 或 [TimeFrameCandleMessage.TimeFrame](xref:StockSharp.Messages.TimeFrameCandleMessage.TimeFrame)（对于K线而言）不是必须的，但也不被禁止。StockSharp 核心会自动用原始请求中所需的值填充这些字段。

### 实时数据

在处理实时数据（例如通过 WebSocket 接收的数据）时：

1. 设置 [OriginalTransactionId](xref:StockSharp.Messages.IOriginalTransactionIdMessage.OriginalTransactionId) 是可选的。如果未设置事务ID，系统会将数据分发给该品种和数据类型的所有活跃订阅。

2. 设置 [SecurityId](xref:StockSharp.Messages.SecurityId) 及其他特定字段（例如K线的 [TimeFrameCandleMessage.TimeFrame](xref:StockSharp.Messages.TimeFrameCandleMessage.TimeFrame)）是必须的，因为该信息对于系统中数据的正确路由是必要的。
