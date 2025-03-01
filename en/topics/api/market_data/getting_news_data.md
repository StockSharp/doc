# Getting news data

To start getting the news, you need to subscribe to the [Connector.NewNews](xref:StockSharp.Algo.Connector.NewNews) event and call the [Connector.Subscribe](xref:StockSharp.Algo.Connector.Subscribe(StockSharp.BusinessEntities.Subscription))**(**[StockSharp.BusinessEntities.Subscription](xref:StockSharp.BusinessEntities.Subscription) subscription **)** method with passed into a subscription based on [MarketDataMessage](xref:StockSharp.Messages.MarketDataMessage) message.

In the example shown in the previous sections, news arriving at the [Connector.NewNews](xref:StockSharp.Algo.Connector.NewNews), event is passed for display to the special [NewsPanel](xref:StockSharp.Xaml.NewsPanel) visual element.

```cs
...
Connector.NewNews += news => _newsWindow.NewsPanel.NewsGrid.News.Add(news);
...
							
```
