# Получение новостных данных

Для того, чтобы начать получить новости, необходимо подписаться на событие [NewNews](xref:StockSharp.Algo.Connector.NewNews) и вызвать метод [Connector.Subscribe](xref:StockSharp.Algo.Connector.Subscribe(StockSharp.Algo.Subscription)), передав в него подписку, основанная на [StockSharp.Messages.MarketDataMessage](xref:StockSharp.Messages.MarketDataMessage).

В примере, показанном в предыдущих разделах, поступающие новости в событии [NewNews](xref:StockSharp.Algo.Connector.NewNews), передаются для отображения в специальный визуальный элемент [NewsPanel](xref:StockSharp.Xaml.NewsPanel).

```cs
...
Connector.NewNews += news => _newsWindow.NewsPanel.NewsGrid.News.Add(news);
...
							
```
