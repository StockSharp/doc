# Configuración de pruebas

Esta sección describe los ajustes principales de [HistoryEmulationConnector](xref:StockSharp.Algo.Testing.HistoryEmulationConnector) para probar estrategias de trading.

## Configuración básica del emulador

- [MarketTimeChangedInterval](xref:StockSharp.Algo.Testing.HistoryEmulationConnector.MarketTimeChangedInterval) - intervalo de llegada del evento de cambio de tiempo. Si se usan generadores de operaciones, las operaciones se generarán con esta frecuencia. El valor predeterminado es 1 minuto.
- [MarketEmulatorSettings.Latency](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.Latency) - valor mínimo de retraso para las órdenes enviadas. El valor predeterminado es TimeSpan.Zero, lo que significa aceptación instantánea de las órdenes enviadas por parte del exchange.
- [MarketEmulatorSettings.MatchOnTouch](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.MatchOnTouch) - ejecutar órdenes si el precio "toca" el nivel (esta suposición a veces es demasiado "optimista" y debe desactivarse para pruebas realistas). Si está desactivado, las órdenes limitadas se ejecutarán solo si el precio "las atraviesa" al menos 1 paso. Esta opción funciona en todos los modos excepto en el modo de registro de órdenes. Está desactivada de forma predeterminada.
- [MarketEmulatorSettings.CandlePrice](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.CandlePrice) - precio de la vela usado para ejecutar órdenes: Middle, Open, High, Low o Close. El valor predeterminado es Middle.
- [MarketEmulatorSettings.Failing](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.Failing) - porcentaje de fallos de registro de nuevas órdenes. Los valores van de 0 (sin fallos) a 100. El valor predeterminado está desactivado (0).
- [MarketEmulatorSettings.InitialOrderId](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.InitialOrderId) - número inicial desde el que el emulador empieza a generar identificadores de órdenes.
- [MarketEmulatorSettings.InitialTradeId](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.InitialTradeId) - número inicial desde el que el emulador empieza a generar identificadores de operaciones.
- [MarketEmulatorSettings.SpreadSize](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.SpreadSize) - tamaño del spread en pasos de precio. Se usa al generar un libro de órdenes a partir de operaciones tick. El valor predeterminado es 2.
- [MarketEmulatorSettings.MaxDepth](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.MaxDepth) - profundidad máxima del libro de órdenes generado a partir de ticks. El valor predeterminado es 5.
- [MarketEmulatorSettings.PortfolioRecalcInterval](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.PortfolioRecalcInterval) - intervalo de recálculo de los datos de cartera. Si es igual a TimeSpan.Zero, no se realiza el recálculo.
- [MarketEmulatorSettings.ConvertTime](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.ConvertTime) - convertir las marcas de tiempo de órdenes y operaciones a la hora del exchange. Está desactivado de forma predeterminada.
- [MarketEmulatorSettings.TimeZone](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.TimeZone) - información de la zona horaria del exchange.
- [MarketEmulatorSettings.PriceLimitOffset](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.PriceLimitOffset) - desplazamiento de precio respecto a la operación anterior que define los límites máximo y mínimo de precio para la siguiente sesión. El valor predeterminado es 40%.
- [MarketEmulatorSettings.IncreaseDepthVolume](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.IncreaseDepthVolume) - añadir volumen extra al libro de órdenes al registrar órdenes de gran volumen. Está activado de forma predeterminada.
- [MarketEmulatorSettings.CheckTradingState](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.CheckTradingState) - comprobar el estado de la sesión de trading. Está desactivado de forma predeterminada.
- [MarketEmulatorSettings.CheckMoney](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.CheckMoney) - comprobar el saldo de efectivo. Está desactivado de forma predeterminada.
- [MarketEmulatorSettings.CheckShortable](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.CheckShortable) - comprobar si se permiten posiciones cortas. Está desactivado de forma predeterminada.
- [MarketEmulatorSettings.CheckTradableDates](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.CheckTradableDates) - comprobar si las fechas cargadas son fechas de negociación. Está desactivado de forma predeterminada.
- [MarketEmulatorSettings.CommissionRules](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.CommissionRules) - reglas de cálculo de comisiones.

## Suscripciones a datos de mercado

Para probar correctamente una estrategia, es necesario configurar suscripciones a los tipos de datos de mercado requeridos. Incluso si la estrategia se prueba con velas, para una emulación correcta de operaciones se requiere una suscripción a operaciones tick:

```cs
// Crear una suscripción a operaciones tick
var tickSubscription = new Subscription(DataType.Ticks, security);
_connector.Subscribe(tickSubscription);
```

Si la estrategia requiere datos del libro de órdenes:

```cs
// Crear una suscripción a libros de órdenes
var depthSubscription = new Subscription(DataType.MarketDepth, security);
_connector.Subscribe(depthSubscription);
```

## Generación del libro de órdenes para pruebas

Si no hay libros de órdenes históricos, pero se necesitan para probar la estrategia, puede activar la generación del libro de órdenes:

```cs
// Crear un generador de libro de órdenes con comportamiento de tendencia
var mdGenerator = new TrendMarketDepthGenerator(security.ToSecurityId());

// Enviar un mensaje de suscripción al generador
_connector.MarketDataAdapter.SendInMessage(new GeneratorMessage
{
	IsSubscribe = true,
	Generator = mdGenerator
});
```

### Configuración del generador de libro de órdenes

- Intervalo de actualización del libro de órdenes ([MarketDataGenerator.Interval](xref:StockSharp.Algo.Testing.MarketDataGenerator.Interval)) - las actualizaciones no pueden ser más frecuentes que la llegada de operaciones tick, ya que los libros de órdenes se generan antes de cada operación:

```cs
mdGenerator.Interval = TimeSpan.FromSeconds(1);
```

- Profundidad del libro de órdenes ([MarketDepthGenerator.MaxBidsDepth](xref:StockSharp.Algo.Testing.MarketDepthGenerator.MaxBidsDepth) y [MarketDepthGenerator.MaxAsksDepth](xref:StockSharp.Algo.Testing.MarketDepthGenerator.MaxAsksDepth)) - cuanto mayor sea la profundidad, más lentas serán las pruebas:

```cs
mdGenerator.MaxAsksDepth = 1; 
mdGenerator.MaxBidsDepth = 1;
```

- Para obtener un volumen realista de nivel en el libro de órdenes, puede usar la opción [MarketDepthGenerator.UseTradeVolume](xref:StockSharp.Algo.Testing.MarketDepthGenerator.UseTradeVolume), que toma los volúmenes de las mejores cotizaciones a partir del volumen de operación que se genera:

```cs
mdGenerator.UseTradeVolume = true;
```

- Rango de volumen de nivel ([MarketDataGenerator.MinVolume](xref:StockSharp.Algo.Testing.MarketDataGenerator.MinVolume) y [MarketDataGenerator.MaxVolume](xref:StockSharp.Algo.Testing.MarketDataGenerator.MaxVolume)):

```cs
mdGenerator.MinVolume = 1;
mdGenerator.MaxVolume = 1;
```

- Configuración del spread - el spread mínimo generado es igual a [Security.PriceStep](xref:StockSharp.BusinessEntities.Security.PriceStep). Se recomienda no generar un spread entre las mejores cotizaciones mayor de 5 pasos de precio, para que al generar desde velas el spread no se vuelva demasiado amplio:

```cs
mdGenerator.MinSpreadStepCount = 1;
mdGenerator.MaxSpreadStepCount = 5;
```

## Ejemplo completo de configuración de pruebas

```cs
// Crear una conexión histórica
var connector = new HistoryEmulationConnector();

// Configurar parámetros básicos
connector.MarketTimeChangedInterval = TimeSpan.FromSeconds(10);
connector.EmulationAdapter.Emulator.Settings.Latency = TimeSpan.FromMilliseconds(100);
connector.EmulationAdapter.Emulator.Settings.MatchOnTouch = false;

// Cargar datos históricos
var storage = new StorageRegistry();
var security = new Security { Id = "AAPL", PriceStep = 0.01m };

// Crear una suscripción a velas
var candleSubscription = new Subscription(
	TimeSpan.FromMinutes(5).TimeFrame(),
	security)
{
	MarketData =
	{
		From = DateTime.Today.Subtract(TimeSpan.FromDays(30)),
		To = DateTime.Today,
		BuildMode = MarketDataBuildModes.Load
	}
};
connector.Subscribe(candleSubscription);

// Crear una suscripción a ticks para una emulación correcta
var tickSubscription = new Subscription(DataType.Ticks, security);
connector.Subscribe(tickSubscription);

// Configurar la generación del libro de órdenes
var mdGenerator = new TrendMarketDepthGenerator(security.ToSecurityId())
{
	Interval = TimeSpan.FromSeconds(1),
	MaxAsksDepth = 5,
	MaxBidsDepth = 5,
	UseTradeVolume = true,
	MinVolume = 1,
	MaxVolume = 100,
	MinSpreadStepCount = 1,
	MaxSpreadStepCount = 5
};

connector.MarketDataAdapter.SendInMessage(new GeneratorMessage
{
	IsSubscribe = true,
	Generator = mdGenerator
});

// Suscribirse a la recepción de datos
connector.CandleReceived += OnCandleReceived;
connector.TickTradeReceived += OnTickReceived;
connector.OrderBookReceived += OnOrderBookReceived;

// Iniciar las pruebas
connector.Connect();
```

## Manejo de eventos

```cs
private void OnCandleReceived(Subscription subscription, ICandleMessage candle)
{
	// Procesamiento de las velas recibidas
	Console.WriteLine($"Vela: {candle.OpenTime}, O:{candle.OpenPrice}, H:{candle.HighPrice}, L:{candle.LowPrice}, C:{candle.ClosePrice}");
}

private void OnTickReceived(Subscription subscription, ITickTradeMessage tick)
{
	// Procesamiento de los ticks recibidos
	Console.WriteLine($"Tick: {tick.ServerTime}, Precio: {tick.Price}, Volumen: {tick.Volume}");
}

private void OnOrderBookReceived(Subscription subscription, IOrderBookMessage orderBook)
{
	// Uso de métodos de extensión para IOrderBookMessage
	var bestBid = orderBook.GetBestBid();
	var bestAsk = orderBook.GetBestAsk();
	var spreadMiddle = orderBook.GetSpreadMiddle(Security.PriceStep);
	
	// Procesamiento de los libros de órdenes recibidos
	Console.WriteLine($"Libro de órdenes: {orderBook.ServerTime}, mejor compra: {bestBid?.Price}, mejor venta: {bestAsk?.Price}, centro del spread: {spreadMiddle}");
	
	// Obtener el precio por lado de la orden
	var bidPrice = orderBook.GetPrice(Sides.Buy);
	var askPrice = orderBook.GetPrice(Sides.Sell);
	
	Console.WriteLine($"Precio de compra: {bidPrice}, precio de venta: {askPrice}");
}
```

## Trabajo con datos del libro de órdenes

Al trabajar con libros de órdenes como IOrderBookMessage, puede usar los siguientes métodos de extensión:

```cs
// Obtener el mejor bid
var bestBid = orderBook.GetBestBid();

// Obtener el mejor ask
var bestAsk = orderBook.GetBestAsk();

// Obtener el punto medio del spread
var spreadMiddle = orderBook.GetSpreadMiddle(Security.PriceStep);

// Obtener el precio por lado de la orden
var price = orderBook.GetPrice(Sides.Buy); // o Sides.Sell, o null para el punto medio del spread
```

Al trabajar con datos Level1, también puede obtener el punto medio del spread:

```cs
// Obtener el punto medio del spread a partir de un mensaje Level1
var spreadMiddle = level1.GetSpreadMiddle(Security.PriceStep);
```

Estos métodos de extensión simplifican el acceso a los datos del libro de órdenes y permiten escribir código más limpio y comprensible al trabajar con cotizaciones de exchange.
