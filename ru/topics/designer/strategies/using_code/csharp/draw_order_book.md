# Отрисовка стакана

Стратегия из кода может отрисовывать на панели [Стакана](../../../user_interface/components/order_book.md) данные аналогично кубику [Стакан](../../using_visual_designer/elements/market_depths/order_book_panel.md). Для этого необходимо написать следующий код.

1. Создать наследник интерфейса [IOrderBookSource](xref:StockSharp.Algo.Strategies.IOrderBookSource), который **Дизайнер** использует для идентификации источника. В случае примера используется класс [OrderBookSource](xref:StockSharp.Algo.Strategies.OrderBookSource), который является реализацией интерфейса по-умолчанию:

```cs
private static readonly OrderBookSource _bookSource = new OrderBookSource("SMA");
```

2. Переопределить свойство [OrderBookSources](xref:StockSharp.Algo.Strategies.Strategy.OrderBookSources):

```cs
public override IEnumerable<IOrderBookSource> OrderBookSources
	=> new[] { _bookSource };
```

Таким образом стратегия будет указывать внешнем коду (в данном случае, панели [Стакана](../../../user_interface/components/order_book.md)) какие источники стаканов доступны. Множество источников бывает в том случае, когда стратегия работает с несколькими стаканами (разные инструменты, или стаканы с различными модификациями, как например, [разреженный стакан](../../using_visual_designer/elements/market_depths/sparse_order_book.md)).

3. Добавить инициализацию подписки на стакан в код стратегии. В случае SmaStrategy добавляется в конец [OnStarted](xref:StockSharp.Algo.Strategies.Strategy.OnStarted) метода:

```cs
var bookSubscription = new Subscription(DataType.MarketDepth, Security);
			
bookSubscription
	.WhenOrderBookReceived(this)
	.Do(book =>
	{
		// drawing order book
		DrawOrderBook(bookSubscription, _bookSource, book);
	})
	.Apply(this);
			
Subscribe(bookSubscription);
```

В обработчике Do делается вызов метода [DrawOrderBook](xref:StockSharp.Algo.Strategies.Strategy.DrawOrderBook(StockSharp.Algo.Subscription,StockSharp.Algo.Strategies.IOrderBookSource,StockSharp.Messages.IOrderBookMessage)), который отправляет стакан на отрисовку.

4. Добавить панель [Стакана](../../../user_interface/components/order_book.md) и выбрать созданный в коде источник:

  ![Designer_Source_Code_OrderBook_00](../../../../../images/designer_source_code_orderbook_00.png)

5. После запуска стратегии на тестирование стакан будет заполнятся данными:

  ![Designer_Source_Code_OrderBook_01](../../../../../images/designer_source_code_orderbook_01.png)