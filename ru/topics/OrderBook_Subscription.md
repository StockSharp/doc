# Подписка и отписка

## Подписка на стакан заявок

Для [подписки](API_ConnectorsSubscriptions.md) на стакан в StockSharp необходимо выполнить следующие шаги:

1. Подписаться на событие получения стаканов [Connector.OrderBookReceived](xref:StockSharp.Algo.Connector.OrderBookReceived) и обрабатывать объекты интерфейса [IOrderBookMessage](xref:StockSharp.Messages.IOrderBookMessage):

```cs
private void OnOrderBookReceived(Subscription subscription, IOrderBookMessage orderBook)
{
    // Здесь вы можете обработать данные стакана, например, вывести их на экран или использовать в своей торговой стратегии
    Console.WriteLine($"Получен стакан для {orderBook.SecurityId}. Лучшая цена покупки: {orderBook.GetBestBid()?.Price}, Лучшая цена продажи: {orderBook.GetBestBid()?.Price}");
}
```
2. Инициировать подписку через метод [Connector.Subscribe](xref:StockSharp.Algo.Connector.Subscribe):

```cs
var security = GetSecurity(); // Получите объект Security, на который хотите подписаться
				
// подписаться на стакан
var subscription = new Subscription(DataType.MarketDepth, security);
connector.Subscribe(subscription);

// или так
//var subscription = connector.SubscribeMarketDepth(security);
```

## Отписка от стакана заявок

Для отписки от стакана необходимо вызвать метод [Connector.UnSubscribe](xref:StockSharp.Algo.Connector.UnSubscribe):

```cs
connector.UnSubscribe(subscription);
```

## Уточнение о приеме стаканов
При работе с событием [Connector.OrderBookReceived](xref:StockSharp.Algo.Connector.OrderBookReceived) важно понимать, что стаканы заявок, поступающие через это событие, приходят уже собранными и готовыми к использованию. Это означает, что независимо от способа отправки данных источником — будь то дифференциальные данные (только изменения в стакане) или полные снэпшоты стакана — платформа StockSharp обрабатывает эти данные таким образом, чтобы трейдер получал полный и актуализированный стакан заявок.

Платформа автоматически интегрирует изменения в стакан, обновляя его содержимое до актуального состояния перед вызовом события [Connector.OrderBookReceived](xref:StockSharp.Algo.Connector.OrderBookReceived). Это упрощает работу с данными, поскольку трейдерам не нужно самостоятельно обрабатывать дифференциальные данные или собирать стакан из последовательных снэпшотов. Таким образом, вы можете быть уверены, что данные, получаемые в обработчике события, отражают последнее состояние стакана заявок на момент срабатывания события.

Это значительно упрощает разработку торговых стратегий и анализ рынка, так как трейдеры могут сосредоточиться непосредственно на логике своих стратегий, не тратя время на технические аспекты сборки и обработки данных стаканов заявок.

## См. также

[Подписки](API_ConnectorsSubscriptions.md)