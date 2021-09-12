# Historical data

If a history is requested, a subscription comes to the adapter with the initialized [MarketDataMessage.From](xref:StockSharp.Messages.MarketDataMessage.From) and [MarketDataMessage.To](xref:StockSharp.Messages.MarketDataMessage.To) values. The adapter should process the received request by returning the requested data. Sending the [SubscriptionFinishedMessage](xref:StockSharp.Messages.SubscriptionFinishedMessage) message is a sign of the request end. Thereby, the external code understands that all the requested data has been received. If there is no data (the connection does not support history or the requested period is not available), then the resulting [SubscriptionFinishedMessage](xref:StockSharp.Messages.SubscriptionFinishedMessage) value is also required. 

Some messages that implement [ISubscriptionMessage](xref:StockSharp.Messages.ISubscriptionMessage) (for example, [SecurityLookupMessage](xref:StockSharp.Messages.SecurityLookupMessage)) always have [ISubscriptionMessage.From](xref:StockSharp.Messages.ISubscriptionMessage.From) and [ISubscriptionMessage.To](xref:StockSharp.Messages.ISubscriptionMessage.To) initialized. For such messages, a subscription to online data is impossible, and only historical data are sent via them (or a request for meta\-data is sent, such as instruments, boards, etc.).

When processing the history results, the [IOriginalTransactionIdMessage.OriginalTransactionId](xref:StockSharp.Messages.IOriginalTransactionIdMessage.OriginalTransactionId) property should be initialized for historical messages. Thereby, the external code will understand that the data returned by the adapter is related to a specific historical subscription: 

```cs
      		SendOutMessage(new TimeFrameCandleMessage
			{
				OpenPrice = (decimal)candle.Open,
				ClosePrice = (decimal)candle.Close,
				HighPrice = (decimal)candle.High,
				LowPrice = (decimal)candle.Low,
				TotalVolume = (decimal)candle.AssetVolume,
				OpenTime = candle.StartTime,
				State = candle.IsFormed ? CandleStates.Finished : CandleStates.Active,
				TotalTicks = candle.TradesCount,
				OriginalTransactionId = _candleTransactions.TryGetValue(Tuple.Create(secId, tf)), // <- fill in the subscription ID, which is used by the external code to determine which instrument and timeframe was in the subscription
			});
      
```

Special [PartialDownloadMessageAdapter](xref:StockSharp.Algo.PartialDownloadMessageAdapter) adapter splits subscriptions with a large range of data (see [Adapters chain](Messages_adapters_chain.md) for details) into many requests with small ranges. Each adapter can communicate the maximum time range it supports by the [IMessageAdapter.GetHistoryStepSize](xref:StockSharp.Messages.IMessageAdapter.GetHistoryStepSize(StockSharp.Messages.DataType,System.TimeSpan@))**(**[StockSharp.Messages.DataType](xref:StockSharp.Messages.DataType) dataType, **out** [System.TimeSpan](xref:System.TimeSpan) iterationInterval**)** method restart: 

```cs
public partial class MyOwnMessageAdapter : MessageAdapter
{
	// ...
	
	/// <inheritdoc />
	public override TimeSpan GetHistoryStepSize(DataType dataType, out TimeSpan iterationInterval)
	{
		var step = base.GetHistoryStepSize(dataType, out iterationInterval);
			
		if (dataType == DataType.Ticks) // tick data supports the maximum range of one day
			step = TimeSpan.FromDays(1);
		return step;
	}
}
```

For online data, no [IOriginalTransactionIdMessage.OriginalTransactionId](xref:StockSharp.Messages.IOriginalTransactionIdMessage.OriginalTransactionId) filling is required because the [SubscriptionOnlineMessageAdapter](xref:StockSharp.Algo.SubscriptionOnlineMessageAdapter) passes only one online data subscription to the adapter. 
