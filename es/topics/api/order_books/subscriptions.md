# Suscripciones

## Suscripción al libro de órdenes

Para suscribirse al libro de órdenes en StockSharp, debe realizar los siguientes pasos:

1. Suscríbase al evento de recepción de libros de órdenes [Connector.OrderBookReceived](xref:StockSharp.Algo.Connector.OrderBookReceived) y procese los objetos de la interfaz [IOrderBookMessage](xref:StockSharp.Messages.IOrderBookMessage):

```cs
// manejador de evento
private void OnOrderBookReceived(Subscription subscription, IOrderBookMessage orderBook)
{
	// Aquí puede procesar los datos del libro de órdenes, por ejemplo, mostrarlos en pantalla o usarlos en su estrategia de trading
	Console.WriteLine($"Libro de órdenes recibido para {orderBook.SecurityId}. Mejor precio de compra: {orderBook.GetBestBid()?.Price}, mejor precio de venta: {orderBook.GetBestAsk()?.Price}");
}

// suscripción al evento
connector.OrderBookReceived += OnOrderBookReceived;
```

Es importante suscribirse al evento [Connector.OrderBookReceived](xref:StockSharp.Algo.Connector.OrderBookReceived) **antes** de enviar una solicitud de suscripción al libro de órdenes. Esto garantiza que no se pierda ningún dato si los libros de órdenes empiezan a llegar muy rápido después de enviar la solicitud de suscripción.

2. Envíe una solicitud de suscripción usando el método [Connector.Subscribe](xref:StockSharp.Algo.Connector.Subscribe(StockSharp.BusinessEntities.Subscription)):

```cs
var security = GetSecurity(); // Obtener el objeto Security al que desea suscribirse
				
// suscribirse al libro de órdenes
var subscription = new Subscription(DataType.MarketDepth, security);
connector.Subscribe(subscription);
```

## Cancelación de la suscripción al libro de órdenes

Para cancelar la suscripción al libro de órdenes, llame al método [Connector.UnSubscribe](xref:StockSharp.Algo.Connector.UnSubscribe(StockSharp.BusinessEntities.Subscription)):

```cs
connector.UnSubscribe(subscription);
```

## Aclaración sobre la recepción de libros de órdenes

Al trabajar con el evento [Connector.OrderBookReceived](xref:StockSharp.Algo.Connector.OrderBookReceived), es importante entender que los libros de órdenes que llegan mediante este evento ya están compilados y listos para usar. Esto significa que, independientemente del método de transmisión de datos por la fuente - ya sean datos diferenciales (solo cambios en el libro de órdenes) o snapshots completos del libro de órdenes - la plataforma StockSharp procesa estos datos de tal forma que el trader recibe un libro de órdenes completo y actualizado.

La plataforma integra automáticamente los cambios en el libro de órdenes, actualizando su contenido al estado actual antes de llamar al evento [Connector.OrderBookReceived](xref:StockSharp.Algo.Connector.OrderBookReceived). Esto simplifica el trabajo con datos, ya que los traders no necesitan procesar de forma independiente datos diferenciales ni compilar el libro de órdenes a partir de snapshots consecutivos. Por lo tanto, puede confiar en que los datos recibidos en el manejador de eventos reflejan el último estado del libro de órdenes en el momento del evento.

Esto simplifica significativamente el desarrollo de estrategias de trading y análisis de mercado, ya que los traders pueden centrarse directamente en la lógica de sus estrategias, sin dedicar tiempo a los aspectos técnicos de compilación y procesamiento de datos del libro de órdenes.

## Ejemplo de uso

Los ejemplos de uso del libro de órdenes están disponibles en el proyecto *Samples\/01\_Basic\/02\_MarketDepths* en [GitHub](https://github.com/StockSharp/StockSharp/) o en el archivo de StockSharp API, que se puede obtener mediante el [Installer](../../installer.md). Estos ejemplos proporcionan ilustraciones prácticas de conexión a un sistema de trading, suscripción a un libro de órdenes filtrado y procesamiento de los datos recibidos, lo que puede servir como un buen punto de partida para desarrollar sus propias estrategias de trading.

## Véase también

[Suscripciones](../market_data/subscriptions.md)
