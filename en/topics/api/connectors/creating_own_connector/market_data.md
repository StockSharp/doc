# Market Data

When creating your own adapter for working with an exchange, you need to implement methods for subscribing to various types of market data. These methods are called when a [MarketDataMessage](xref:StockSharp.Messages.MarketDataMessage) message is received and provide **for** receiving and processing data from the exchange.

Schematically, the algorithm for processing a subscription or unsubscription request looks like this:

1. Sends a confirmation of receiving the subscription request using the [SendSubscriptionReplyAsync](xref:StockSharp.Messages.MessageAdapter.SendSubscriptionReplyAsync(System.Int64,System.Exception)) method.
2. Checks whether the request is a subscription or unsubscription using the [MarketDataMessage.IsSubscribe](xref:StockSharp.Messages.MarketDataMessage.IsSubscribe) property.
3. In case of a subscription, sets up a subscription to receive real-time data via WebSocket or another mechanism (specific to each exchange).
4. In case of an unsubscription, cancels the corresponding subscription (specific to each exchange).
5. Sends a message about the subscription result using the [SendSubscriptionResultAsync](xref:StockSharp.Messages.MessageAdapter.SendSubscriptionResultAsync(StockSharp.Messages.ISubscriptionMessage)) or [SendSubscriptionFinishedAsync](xref:StockSharp.Messages.MessageAdapter.SendSubscriptionFinishedAsync(System.Int64,System.Nullable{System.DateTimeOffset})) methods, depending on the subscription type and the operation result.

## Candle Data

When implementing a subscription to candle data in your own adapter, it is important to consider the specifics of how a particular exchange works with this type of data. In the case of Coinbase, the following methods and properties were overridden:

### Supported Timeframes

The `TimeFrames` property defines the list of timeframes supported by the adapter for candles. This allows StockSharp to know which timeframes can be requested through this adapter.

```cs
protected override IEnumerable<TimeSpan> TimeFrames { get; } = Extensions.TimeFrames.Keys.ToArray();
```

### Support for Candle Updates

The `IsSupportCandlesUpdates` method determines whether the adapter supports real-time candle updates for a specific subscription request. In the case of Coinbase, only updates for 5-minute candles are supported.

```cs
private static readonly DataType _tf5min = DataType.TimeFrame(TimeSpan.FromMinutes(5));

public override bool IsSupportCandlesUpdates(MarketDataMessage subscription)
{
	// Coinbase only supports 5-minute candles for updating via websockets
	// Therefore, other timeframes will be built from ticks (automatically by the StockSharp core)
	return subscription.DataType2 == _tf5min;
}
```

Overriding these methods and properties allows the adapter to correctly handle requests for subscribing to candle data, taking into account the specifics of the Coinbase API. For example, if a timeframe other than 5 minutes is requested, StockSharp will know that it needs to use tick data to build candles of other timeframes.

### Subscribing to Candle Data

To subscribe to candle data, the **OnTFCandlesSubscriptionAsync** method is implemented. This method, like the method for subscribing to tick data, can request historical data, as well as set up a subscription to receive new candles in real time.

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

### Processing Candle Data

To process candle data received from the exchange in real time, a method with code like in the **SessionOnCandleReceived** method is usually implemented. This method converts the received data into a [TimeFrameCandleMessage](xref:StockSharp.Messages.TimeFrameCandleMessage) message and sends it using the SendOutMessageAsync method.

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

## Level 1 (Best Bid and Ask Prices, Last Price)

### Subscribing to Level 1 Data

To subscribe to Level 1 changes, the **OnLevel1SubscriptionAsync** method is implemented. This method usually performs the following actions:

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

### Processing Level 1 Data

To process Level 1 data received from the exchange in real time, a method with code like in the **SessionOnTickerChanged** example is usually implemented. This method converts the received data into a [Level1ChangeMessage](xref:StockSharp.Messages.Level1ChangeMessage) message and sends it using the SendOutMessageAsync method.

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

## Order Book

### Support for Incremental Order Book Updates

When implementing order book functionality in your own adapter, it is important to consider whether the exchange supports incremental order book updates. For this, the `IsSupportOrderBookIncrements` property was overridden in the Coinbase adapter:

```cs
public override bool IsSupportOrderBookIncrements => true;
```

The `IsSupportOrderBookIncrements` property indicates whether the adapter supports incremental order book updates. Setting this property to `true` means that the exchange can send partial order book updates rather than a full snapshot with each change.

Overriding this property allows StockSharp to optimize the processing of order book data. If the property is set to `true`, the system will expect and correctly handle incremental updates.

### Subscribing to Order Book Data

To subscribe to order book changes, the **OnMarketDepthSubscriptionAsync** method is implemented. This method performs actions similar to the OnLevel1SubscriptionAsync method, but for order book data.

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

### Processing Order Book Data

To process order book data received from the exchange in real time, a method with code like in the **SessionOnOrderBookReceived** method is usually implemented. This method converts the received data into a [QuoteChangeMessage](xref:StockSharp.Messages.QuoteChangeMessage) message and sends it using the SendOutMessageAsync method.

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

## Tick Data (Trades)

### Subscribing to Tick Data

To subscribe to tick data, the **OnTicksSubscriptionAsync** method is implemented. This method, in addition to actions similar to the previous subscription methods, can also request historical data if specified in the request.

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

### Processing Tick Data

To process tick data received from the exchange in real time, a method with code like in the **SessionOnTradeReceived** method is usually implemented. This method converts the received data into an [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage) message with the [DataType.Ticks](xref:StockSharp.Messages.DataType.Ticks) type and sends it using the SendOutMessageAsync method.

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

## Subscribing to Order Log

The order log is detailed information about all changes in the order book, including the addition, modification, and deletion of orders. This data is specific and not provided by all data sources. For example, Coinbase does not support providing an order log.

To implement a subscription to an order log in an adapter, the **OnOrderLogSubscriptionAsync** method is used. This method is called when a [MarketDataMessage](xref:StockSharp.Messages.MarketDataMessage) message with the [DataType.OrderLog](xref:StockSharp.Messages.DataType.OrderLog) data type is received.

Below is an example implementation of this method taken from the [BitStamp](https://github.com/StockSharp/StockSharp/tree/master/Connectors/BitStamp) connector, which supports order log:

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

When processing order log data received from the exchange, a separate method is usually used, which converts the received data into [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage) messages with the [ExecutionTypes.OrderLog](xref:StockSharp.Messages.ExecutionTypes.OrderLog) type:

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

It is important not to forget to add support for this data type in the adapter constructor:

```cs
this.AddSupportedMarketDataType(DataType.OrderLog);
```

## Specifics of Processing Historical and Live Data

When implementing requests for historical data and processing live data in your own adapter, it is important to consider the following points:

### Historical Data

When sending historical data in response to a request:

1. Setting [OriginalTransactionId](xref:StockSharp.Messages.IOriginalTransactionIdMessage.OriginalTransactionId) is mandatory. This allows the system to associate the received data with the original request.

2. Setting [SecurityId](xref:StockSharp.Messages.SecurityId) or [TimeFrameCandleMessage.TimeFrame](xref:StockSharp.Messages.TimeFrameCandleMessage.TimeFrame) (in the case of candles) is not required, but also not prohibited. The StockSharp core will automatically fill these fields with the necessary values from the original request.

### Live Data

When processing live data, for example, received via WebSocket:

1. Setting [OriginalTransactionId](xref:StockSharp.Messages.IOriginalTransactionIdMessage.OriginalTransactionId) is optional. If the transaction ID is not set, the system will distribute the data to all active subscriptions for the corresponding instrument and data type.

2. Setting [SecurityId](xref:StockSharp.Messages.SecurityId) and other specific fields (for example, [TimeFrameCandleMessage.TimeFrame](xref:StockSharp.Messages.TimeFrameCandleMessage.TimeFrame) for candles) is mandatory, as this information is necessary for the correct routing of data in the system.