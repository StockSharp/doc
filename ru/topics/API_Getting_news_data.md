# Получение новостных данных

Для того, чтобы начать получить новости, необходимо подписаться на событие [NewNews](xref:StockSharp.Algo.Connector.NewNews) и вызвать метод [SubscribeNews](xref:StockSharp.Algo.Connector.SubscribeNews(StockSharp.BusinessEntities.Security,System.Nullable{System.DateTimeOffset},System.Nullable{System.DateTimeOffset},System.Nullable{System.Int64},StockSharp.Messages.IMessageAdapter)).

В примере, показанном в предыдущих разделах, поступающие новости в событии [NewNews](xref:StockSharp.Algo.Connector.NewNews), передаются для отображения в специальный визуальный элемент [NewsPanel](xref:StockSharp.Xaml.NewsPanel).

```cs
...
Connector.NewNews += news => _newsWindow.NewsPanel.NewsGrid.News.Add(news);
...
							
```
