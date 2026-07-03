# Pruebas con datos de mercado en tiempo real

Las pruebas con datos de mercado en tiempo real implican operar con una conexión real al exchange (cotizaciones "live"), pero sin colocar órdenes reales en el exchange. Todas las órdenes registradas se interceptan y su ejecución se emula en función de los libros de órdenes de mercado. Este tipo de prueba puede ser útil, por ejemplo, al desarrollar un simulador de trading o al comprobar un algoritmo de trading durante un periodo corto con cotizaciones reales.

Para emular trading con datos reales, debe usar [RealTimeEmulationTrader\<TAdapter\>](xref:StockSharp.Algo.Testing.RealTimeEmulationTrader`1), que actúa como un "wrapper" para un conector específico de sistema de trading ([Binance](../connectors/crypto_exchanges/binance.md), [Interactive Brokers](../connectors/stock_market/interactive_brokers.md), etc.).

## Creación de un conector de emulación

Para crear un conector de emulación, primero cree un conector normal para recibir datos de mercado y luego cree un conector de emulación basado en él:

```csharp
// Create a regular connector for receiving market data
private readonly Connector _realConnector = new();

// Create an emulation connector
_emuConnector = new RealTimeEmulationTrader<IMessageAdapter>(_realConnector.Adapter, _realConnector, _emuPf, false);

// Configure emulation parameters
var settings = _emuConnector.EmulationAdapter.Emulator.Settings;
settings.TimeZone = TimeHelper.Est;
settings.ConvertTime = true;
```

Para la emulación de operaciones, debe usar un portfolio especial:

```csharp
private readonly Portfolio _emuPf = Portfolio.CreateSimulator();
```

## Suscripción a eventos

Igual que un conector normal, el conector de emulación genera eventos al recibir datos de mercado y ejecutar transacciones:

```csharp
// Subscribe to connector events
_emuConnector.Connected += () =>
{
	// update gui labels
	this.GuiAsync(() => { ChangeConnectStatus(true); });
};

_emuConnector.Disconnected += () =>
{
	// update gui labels
	this.GuiAsync(() => { ChangeConnectStatus(false); });
};

_emuConnector.ConnectionError += error => this.GuiAsync(() =>
{
	// update gui labels
	ChangeConnectStatus(false);
	MessageBox.Show(this, error.ToString(), LocalizedStrings.ErrorConnection);
});

_emuConnector.OrderBookReceived += OnDepth;
_emuConnector.PositionReceived += (sub, p) => PortfolioGrid.Positions.TryAdd(p);
_emuConnector.OwnTradeReceived += (s, t) => TradeGrid.Trades.TryAdd(t);
_emuConnector.OrderReceived += (s, o) =>
{
	if (!_fistTimeOrders.Add(o))
		return;

	_bufferOrders.Add(o);
	OrderGrid.Orders.Add(o);
};

// Subscribe to order registration errors
_emuConnector.OrderRegisterFailReceived += (s, f) => OrderGrid.AddRegistrationFail(f);

_emuConnector.CandleReceived += (s, candle) =>
{
	if (s == _candlesSubscription)
		_buffer.Add(candle);
};
```

## Suscripción a datos de mercado

Para trabajar con datos de mercado, debe suscribirse a los tipos de datos correspondientes:

```csharp
// Subscribe to order books, ticks, and Level1 for the emulation connector
_emuConnector.Subscribe(new(DataType.MarketDepth, security));
_emuConnector.Subscribe(new(DataType.Ticks, security));
_emuConnector.Subscribe(new(DataType.Level1, security));

// Subscribe to order books for the real connector (needed for emulation)
_realConnector.Subscribe(new(DataType.MarketDepth, security));

// Subscribe to candles
_candlesSubscription = new(CandleDataTypeEdit.DataType, security)
{
	From = DateTimeOffset.UtcNow - TimeSpan.FromDays(10),
};
_emuConnector.Subscribe(_candlesSubscription);
```

## Registro y gestión de órdenes

Las órdenes se registran mediante el conector de emulación de forma similar a un conector normal:

```csharp
// Order registration
_emuConnector.RegisterOrder(order);

// Order cancellation
_emuConnector.CancelOrder(order);

// Order replacement
_emuConnector.ReRegisterOrder(order, newPrice, order.Balance);
```

## Configuración de parámetros de emulación

Puede usar la propiedad [MarketEmulatorSettings](xref:StockSharp.Algo.Testing.MarketEmulatorSettings) para configurar parámetros de emulación:

```csharp
var settings = _emuConnector.EmulationAdapter.Emulator.Settings;

// Set timezone
settings.TimeZone = TimeHelper.Est;

// Convert time
settings.ConvertTime = true;

// Match orders on price touch
settings.MatchOnTouch = false;

// Emulate order execution latency
settings.Latency = TimeSpan.FromMilliseconds(100);
```

## Ejemplo de interfaz

El ejemplo SampleRealTimeEmulation demuestra la posibilidad de mostrar simultáneamente datos tanto del conector real como del conector de emulación:

![sample realtime emulation](../../../images/sample_realtime_emulation.png)

La interfaz de la aplicación contiene los siguientes elementos:
- Gráficos para mostrar velas y órdenes
- Tablas de órdenes y operaciones propias
- Libros de órdenes del mercado real y de la emulación
- Controles para crear y cancelar órdenes

## Ventajas y limitaciones

Las pruebas con datos de mercado en tiempo real tienen las siguientes ventajas:
- Uso de datos de mercado reales sin riesgos financieros
- Pruebas de algoritmos en condiciones muy cercanas al trading real
- Posibilidad de comparar resultados con el mercado real en tiempo real

Limitaciones:
- La velocidad de prueba está limitada por la velocidad de los datos reales
- Imposibilidad de probar periodos históricos
- Dependencia de la calidad e integridad de los datos de mercado recibidos
