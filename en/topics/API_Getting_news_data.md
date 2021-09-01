# Getting news data

To start getting the news, you need to subscribe to the [NewNews](xref:StockSharp.Algo.Connector.NewNews) event and call the [SubscribeNews](xref:StockSharp.Algo.Connector.SubscribeNews(StockSharp.BusinessEntities.Security,System.Nullable{System.DateTimeOffset},System.Nullable{System.DateTimeOffset},System.Nullable{System.Int64},StockSharp.Messages.IMessageAdapter)) method.

In the example shown in the previous sections, news arriving at the [NewNews](xref:StockSharp.Algo.Connector.NewNews), event is passed for display to the special [NewsPanel](xref:StockSharp.Xaml.NewsPanel) visual element.

```cs
...
Connector.NewNews += news => _newsWindow.NewsPanel.NewsGrid.News.Add(news);
...
							
```
