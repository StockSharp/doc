# Получение новостных данных

Для того, чтобы начать получить новости, необходимо подписаться на событие [NewNews](../api/StockSharp.Algo.Connector.NewNews.html) и вызвать метод [SubscribeNews](../api/StockSharp.Algo.Connector.SubscribeNews.html).

В примере, показанном в предыдущих разделах, поступающие новости в событии [NewNews](../api/StockSharp.Algo.Connector.NewNews.html), передаются для отображения в специальный визуальный элемент [NewsPanel](../api/StockSharp.Xaml.NewsPanel.html).

```cs
...
Connector.NewNews += news => _newsWindow.NewsPanel.NewsGrid.News.Add(news);
...
							
```
