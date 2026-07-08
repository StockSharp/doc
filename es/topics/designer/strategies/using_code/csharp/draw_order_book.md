# Dibujo del libro de órdenes

Una estrategia creada desde código puede dibujar datos en el panel [Libro de órdenes](../../../user_interface/components/order_book.md) de forma similar al cubo [Order Book](../../using_visual_designer/elements/market_depths/order_book_panel.md). Para ello, debe escribirse el siguiente código.

1. Cree un descendiente de la interfaz [IOrderBookSource](xref:StockSharp.Algo.Strategies.IOrderBookSource), que **Designer** usa para identificar la fuente. En el ejemplo se usa la clase [OrderBookSource](xref:StockSharp.Algo.Strategies.OrderBookSource), que es la implementación predeterminada de la interfaz:

```cs
private static readonly OrderBookSource _bookSource = new OrderBookSource("SMA");
```

2. Sobrescriba la propiedad [OrderBookSources](xref:StockSharp.Algo.Strategies.Strategy.OrderBookSources):

```cs
public override IEnumerable<IOrderBookSource> OrderBookSources
	=> new[] { _bookSource };
```

Así, la estrategia indicará al código externo (en este caso, al panel [Libro de órdenes](../../../user_interface/components/order_book.md)) qué fuentes de libros de órdenes están disponibles. La multiplicidad de fuentes aparece cuando la estrategia trabaja con varios libros de órdenes (distintos instrumentos, o libros de órdenes con diversas modificaciones, como por ejemplo un [libro de órdenes reducido](../../using_visual_designer/elements/market_depths/sparse_order_book.md)).

3. Añada la inicialización de la suscripción al libro de órdenes en el código de la estrategia. En el caso de SmaStrategy, se añade al final del método [OnStarted](xref:StockSharp.Algo.Strategies.Strategy.OnStarted):

```cs
var bookSubscription = new Subscription(DataType.MarketDepth, Security);
			
bookSubscription
	.WhenOrderBookReceived(this)
	.Do(book =>
	{
		// dibujar el libro de órdenes
		DrawOrderBook(bookSubscription, _bookSource, book);
	})
	.Apply(this);
			
Subscribe(bookSubscription);
```

En el manejador Do se llama al método [DrawOrderBook](xref:StockSharp.Algo.Strategies.Strategy.DrawOrderBook(StockSharp.BusinessEntities.Subscription,StockSharp.Algo.Strategies.IOrderBookSource,StockSharp.Messages.IOrderBookMessage)), que envía el libro de órdenes para dibujarlo.

4. Añada el panel [Libro de órdenes](../../../user_interface/components/order_book.md) y seleccione la fuente creada en el código:

  ![Designer_Source_Code_OrderBook_00](../../../../../images/designer_source_code_orderbook_00.png)

5. Después de iniciar la estrategia para pruebas, el libro de órdenes se llenará con datos:

  ![Designer_Source_Code_OrderBook_01](../../../../../images/designer_source_code_orderbook_01.png)
