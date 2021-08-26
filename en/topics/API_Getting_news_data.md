# Getting news data

To start getting the news, you need to subscribe to the [NewNews](../api/StockSharp.Algo.Connector.NewNews.html) event and call the [SubscribeNews](../api/StockSharp.Algo.Connector.SubscribeNews.html) method.

In the example shown in the previous sections, news arriving at the [NewNews](../api/StockSharp.Algo.Connector.NewNews.html), event is passed for display to the special [NewsPanel](../api/StockSharp.Xaml.NewsPanel.html) visual element.

```cs
...
Connector.NewNews += news => _newsWindow.NewsPanel.NewsGrid.News.Add(news);
...
							
```
