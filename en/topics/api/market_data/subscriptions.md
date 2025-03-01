# Subscriptions

Starting with 5.0 version, [API](../../api.md) offers a new data acquisition model (market and transactional data). The model is based on subscriptions and has advantages over regular subscription requests: 

- Subscriptions are isolated from each other, so you can run an arbitrary number of subscriptions in parallel (with a history request or not). 
- Subscriptions have states that make it possible to understand whether historical data is being received at the moment, or whether the subscription has gone online. 
- Subscriptions have a universal approach, and their code is the same regardless of the requested data types. 

To work with subscriptions, you should use the [Subscription](xref:StockSharp.BusinessEntities.Subscription) class. Below is an example of subscribing to candles via the new model:

```cs
...
var subscription = new Subscription(new MarketDataMessage
{
	DataType2 = DataType.TimeFrame(TimeSpan.FromMinutes(5)),
	From = DateTimeOffset.Now.Subtract(TimeSpan.FromDays(30)),
	// null means make subscriptions goes online after historical data
	To = null,
}, (SecurityMessage)sec);
// subscribe to events
_connector.CandleReceived += (sub, candle) =>
{
	if (sub != subscription)
		return;
	Console.WriteLine(candle);
};
_connector.SubscriptionOnline += (sub) =>
{
	if (sub != subscription)
		return;
	Console.WriteLine("Online");
};
_connector.SubscriptionFailed += (sub, error, isSubscribe) =>
{
	if (sub != subscription)
		return;
	Console.WriteLine(error);
};
// start subscription
_connector.Subscribe(subscription);
...
			
```

Subscription states:

- [SubscriptionStates.Stopped](xref:StockSharp.Messages.SubscriptionStates.Stopped) \- the subscription is inactive (stopped or did not start). 
- [SubscriptionStates.Active](xref:StockSharp.Messages.SubscriptionStates.Active) \- the subscription is active, and it can transfer historical data until it goes online or is completed. 
- [SubscriptionStates.Error](xref:StockSharp.Messages.SubscriptionStates.Error) \- the subscription is inactive and in an error state. 
- [SubscriptionStates.Finished](xref:StockSharp.Messages.SubscriptionStates.Finished) \- the subscription has finished its work (all data has been received). 
- [SubscriptionStates.Online](xref:StockSharp.Messages.SubscriptionStates.Online) \- the subscription has gone online and only transfers data in real time. 
