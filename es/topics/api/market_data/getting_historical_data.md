# Obtención de datos históricos

StockSharp API proporciona mecanismos cómodos para obtener datos históricos, que se pueden usar tanto para probar estrategias de negociación como para construir [Indicadores](../indicators.md).

## Obtención de datos históricos mediante Connector

### Configuración de la conexión

Para obtener datos históricos, primero debe configurar una conexión con el sistema de negociación:

```cs
// Crear una instancia de Connector
var connector = new Connector();

// Agregar un adaptador para conectar con Binance
var messageAdapter = new BinanceMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<Su clave API>",
	Secret = "<Su clave secreta>",
};
connector.Adapter.InnerAdapters.Add(messageAdapter);

// Conectar
connector.Connect();
```

La conexión también se puede configurar mediante la interfaz gráfica, como se describe en la sección [Ventana de configuración de conexión](../graphical_user_interface/connection_settings_window.md).

### Suscripción a velas históricas

Para recibir velas históricas, debe crear una suscripción y especificar los parámetros de los datos solicitados:

```cs
// Crear una suscripción para velas de 5 minutos del instrumento seleccionado
var subscription = new Subscription(
	DataType.TimeFrame(TimeSpan.FromMinutes(5)),
	security)
{
	MarketData =
	{
		// Especificar el período para el que se obtendrán datos históricos
		From = DateTime.Now.Subtract(TimeSpan.FromDays(30)),
		To = DateTime.Now,
		// Establecer la bandera para recibir solo velas completadas
		IsFinishedOnly = true
	}
};

// Suscribirse al evento de recepción de velas
connector.CandleReceived += OnCandleReceived;

// Iniciar la suscripción
connector.Subscribe(subscription);

// Manejador del evento de recepción de velas
private void OnCandleReceived(Subscription subscription, ICandleMessage candle)
{
	// Comprobar que la vela pertenece a nuestra suscripción
	if (subscription != _subscription)
		return;

	// Procesar la vela recibida
	Console.WriteLine($"Vela recibida: {candle.OpenTime}, O:{candle.OpenPrice}, H:{candle.HighPrice}, L:{candle.LowPrice}, C:{candle.ClosePrice}, V:{candle.TotalVolume}");

	// Para mostrar en el gráfico, puede usar:
	// Chart.Draw(_candleElement, candle);
}
```

### Uso de velas para gráficos

Las velas recibidas se pueden mostrar en un gráfico mediante los componentes gráficos integrados de StockSharp:

```cs
// Crear y configurar elementos del gráfico
var chart = new Chart();
var area = new ChartArea();
var candleElement = new ChartCandleElement();

// Agregar área y elemento al gráfico
chart.AddArea(area);
chart.AddElement(area, candleElement, subscription);

// En el manejador CandleReceived, dibujar velas
private void OnCandleReceived(Subscription subscription, ICandleMessage candle)
{
	// Comprobar que la vela pertenece a nuestra suscripción
	if (subscription != _subscription)
		return;

	// Si necesita mostrar solo velas completadas
	if (candle.State == CandleStates.Finished)
	{
		var chartData = new ChartDrawData();
		chartData.Group(candle.OpenTime).Add(candleElement, candle);
		chart.Draw(chartData);
	}
}
```

## Obtención de otros tipos de datos históricos

De forma similar, puede obtener otros tipos de datos históricos:

### Obtención de ticks históricos

```cs
var tickSubscription = new Subscription(DataType.Ticks, security)
{
	MarketData =
	{
		From = DateTime.Now.Subtract(TimeSpan.FromDays(1)),
		To = DateTime.Now
	}
};

connector.TickTradeReceived += (subscription, tick) =>
{
	if (subscription == tickSubscription)
		Console.WriteLine($"Tick: {tick.ServerTime}, Precio: {tick.Price}, Volumen: {tick.Volume}");
};

connector.Subscribe(tickSubscription);
```

### Obtención de libros de órdenes históricos

```cs
var depthSubscription = new Subscription(DataType.MarketDepth, security)
{
	MarketData =
	{
		From = DateTime.Now.Subtract(TimeSpan.FromHours(1)),
		To = DateTime.Now
	}
};

connector.OrderBookReceived += (subscription, depth) =>
{
	if (subscription == depthSubscription)
		Console.WriteLine($"Libro de órdenes: {depth.ServerTime}, mejor bid: {depth.GetBestBid()?.Price}, mejor ask: {depth.GetBestAsk()?.Price}");
};

connector.Subscribe(depthSubscription);
```

## Véase también

- [Velas](../candles.md)
- [Suscripciones](subscriptions.md)
- [Indicadores](../indicators.md)
