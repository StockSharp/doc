# Velas

[S#](../api.md) admite los siguientes tipos de velas:

- [TimeFrameCandleMessage](xref:StockSharp.Messages.TimeFrameCandleMessage) - una vela basada en un intervalo de tiempo, timeframe. Se pueden configurar tanto intervalos populares (minutos, horas, diario) como personalizados. Por ejemplo, 21 segundos, 4.5 minutos, etc.
- [RangeCandleMessage](xref:StockSharp.Messages.RangeCandleMessage) - una vela de rango de precio. Se crea una nueva vela cuando aparece una operación con un precio que excede los límites aceptables. El límite aceptable se forma cada vez basándose en el precio de la primera operación.
- [VolumeCandleMessage](xref:StockSharp.Messages.VolumeCandleMessage) - una vela se forma hasta que el volumen total de operaciones supera un límite especificado. Si una nueva operación excede el volumen permitido, se incluye en una nueva vela.
- [TickCandleMessage](xref:StockSharp.Messages.TickCandleMessage) - igual que [VolumeCandleMessage](xref:StockSharp.Messages.VolumeCandleMessage), pero se usa el número de operaciones como limitación en lugar del volumen.
- [PnFCandleMessage](xref:StockSharp.Messages.PnFCandleMessage) - una vela de gráfico punto y figura (gráfico X-O).
- [RenkoCandleMessage](xref:StockSharp.Messages.RenkoCandleMessage) - vela Renko.

Cómo trabajar con velas se muestra en el ejemplo ubicado en la carpeta *Samples\/02\_Candles\/01\_Realtime*.

Las siguientes imágenes muestran los gráficos de [TimeFrameCandleMessage](xref:StockSharp.Messages.TimeFrameCandleMessage) y [RangeCandleMessage](xref:StockSharp.Messages.RangeCandleMessage):

![sample timeframecandles](../../images/sample_timeframecandles.png)

![sample rangecandles](../../images/sample_rangecandles.png)

## Inicio de la recepción de datos

1. Para obtener velas, cree una suscripción usando la clase [Subscription](xref:StockSharp.BusinessEntities.Subscription):

```cs
// Create a subscription to 5-minute candles
var subscription = new Subscription(
	DataType.TimeFrame(TimeSpan.FromMinutes(5)),  // Data type with timeframe specification
	security)  // Instrument
{
	// Configure additional parameters through the MarketData property
	MarketData =
	{
		// Period for which we request historical data (last 30 days)
		From = DateTime.Today.Subtract(TimeSpan.FromDays(30)),
		To = DateTime.Now
	}
};
```

2. Para recibir velas, suscríbase al evento [Connector.CandleReceived](xref:StockSharp.Algo.Connector.CandleReceived), que señala la aparición de un nuevo valor para procesar:

```cs
// Subscribe to the candle reception event
_connector.CandleReceived += OnCandleReceived;

// Candle reception event handler
private void OnCandleReceived(Subscription subscription, ICandleMessage candle)
{
	// Here subscription is the subscription object we created
	// candle - the received candle

	// Check if the candle belongs to our subscription
	if (subscription == _candleSubscription)
	{
		// Draw the candle on the chart
		Chart.Draw(_candleElement, candle);
	}
}
```

> [!TIP]
> El componente gráfico [Chart](xref:StockSharp.Xaml.Charting.Chart) se usa para mostrar velas.

3. A continuación, inicie la suscripción mediante el método [Connector.Subscribe](xref:StockSharp.Algo.Connector.Subscribe(StockSharp.BusinessEntities.Subscription)):

```cs
// Start the subscription
_connector.Subscribe(subscription);
```

Después de esto, el evento [Connector.CandleReceived](xref:StockSharp.Algo.Connector.CandleReceived) comenzará a ser llamado.

4. El evento [Connector.CandleReceived](xref:StockSharp.Algo.Connector.CandleReceived) se llama no solo cuando aparece una nueva vela, sino también cuando cambia la actual.

Si necesita mostrar solo velas **"completas"**, debe comprobar la propiedad [ICandleMessage.State](xref:StockSharp.Messages.ICandleMessage.State) de la vela recibida:

```cs
private void OnCandleReceived(Subscription subscription, ICandleMessage candle)
{
	// Check if the candle belongs to our subscription
	if (subscription != _candleSubscription)
		return;

	// Check if the candle is completed
	if (candle.State == CandleStates.Finished)
	{
		// Create data for drawing
		var chartData = new ChartDrawData();
		chartData.Group(candle.OpenTime).Add(_candleElement, candle);

		// Draw the candle on the chart
		this.GuiAsync(() => Chart.Draw(chartData));
	}
}
```

5. Se pueden configurar parámetros adicionales para la suscripción:

- **Modo de construcción de velas** - determina si se solicitarán datos ya preparados o si se construirán a partir de otro tipo de datos:

```cs
// Request only ready-made data
subscription.MarketData.BuildMode = MarketDataBuildModes.Load;

// Only build from another data type
subscription.MarketData.BuildMode = MarketDataBuildModes.Build;

// Request ready-made data, and if not available - build
subscription.MarketData.BuildMode = MarketDataBuildModes.LoadAndBuild;
```

- **Fuente para construir velas** - indica a partir de qué tipo de datos construir las velas si no están disponibles directamente:

```cs
// Building candles from tick trades
subscription.MarketData.BuildFrom = DataType.Ticks;

// Building candles from order books
subscription.MarketData.BuildFrom = DataType.MarketDepth;

// Building candles from Level1
subscription.MarketData.BuildFrom = DataType.Level1;
```

- **Campo para construir velas** - debe especificarse para ciertos tipos de datos:

```cs
// Building candles from the best bid price in Level1
subscription.MarketData.BuildField = Level1Fields.BestBidPrice;

// Building candles from the best ask price in Level1
subscription.MarketData.BuildField = Level1Fields.BestAskPrice;

// Building candles from the middle of the spread in the order book
subscription.MarketData.BuildField = Level1Fields.SpreadMiddle;
```

- **Perfil de volumen** - cálculo del perfil de volumen para las velas:

```cs
// Enable volume profile calculation
subscription.MarketData.IsCalcVolumeProfile = true;
```

## Ejemplos de suscripciones a diferentes tipos de velas

### Velas con timeframe estándar

```cs
// 5-minute candles
var timeFrameSubscription = new Subscription(
	DataType.TimeFrame(TimeSpan.FromMinutes(5)),
	security);
_connector.Subscribe(timeFrameSubscription);
```

### Carga de solo velas históricas

```cs
// Loading only historical candles without transitioning to real-time
var historicalSubscription = new Subscription(
	DataType.TimeFrame(TimeSpan.FromMinutes(5)),
	security)
{
	MarketData =
	{
		From = DateTime.Today.Subtract(TimeSpan.FromDays(30)),
		To = DateTime.Today,  // Specify end date
		BuildMode = MarketDataBuildModes.Load  // Only load ready-made data
	}
};
_connector.Subscribe(historicalSubscription);
```

### Construcción de velas de timeframe no estándar a partir de ticks

```cs
// Candles with a 21-second timeframe, built from ticks
var customTimeFrameSubscription = new Subscription(
	DataType.TimeFrame(TimeSpan.FromSeconds(21)),
	security)
{
	MarketData =
	{
		BuildMode = MarketDataBuildModes.Build,
		BuildFrom = DataType.Ticks
	}
};
_connector.Subscribe(customTimeFrameSubscription);
```

### Construcción de velas a partir de datos del libro de órdenes

```cs
// Candles built from the middle of the spread in the order book
var depthBasedSubscription = new Subscription(
	DataType.TimeFrame(TimeSpan.FromMinutes(1)),
	security)
{
	MarketData =
	{
		BuildMode = MarketDataBuildModes.Build,
		BuildFrom = DataType.MarketDepth,
		BuildField = Level1Fields.SpreadMiddle
	}
};
_connector.Subscribe(depthBasedSubscription);
```

### Velas con perfil de volumen

```cs
// 5-minute candles with volume profile calculation
var volumeProfileSubscription = new Subscription(
	DataType.TimeFrame(TimeSpan.FromMinutes(5)),
	security)
{
	MarketData =
	{
		BuildMode = MarketDataBuildModes.LoadAndBuild,
		BuildFrom = DataType.Ticks,
		IsCalcVolumeProfile = true
	}
};
_connector.Subscribe(volumeProfileSubscription);
```

### Velas de volumen

```cs
// Volume candles (each candle contains 1000 contracts in volume)
var volumeCandleSubscription = new Subscription(
	DataType.Volume(1000m),  // Specify candle type and volume
	security)
{
	MarketData =
	{
		BuildMode = MarketDataBuildModes.Build,
		BuildFrom = DataType.Ticks
	}
};
_connector.Subscribe(volumeCandleSubscription);
```

### Velas de cantidad de ticks

```cs
// Tick count candles (each candle contains 1000 trades)
var tickCandleSubscription = new Subscription(
	DataType.Tick(1000),  // Specify candle type and number of trades
	security)
{
	MarketData =
	{
		BuildMode = MarketDataBuildModes.Build,
		BuildFrom = DataType.Ticks
	}
};
_connector.Subscribe(tickCandleSubscription);
```

### Velas de rango de precio

```cs
// Price range candles with a range of 0.1 units
var rangeCandleSubscription = new Subscription(
	DataType.Range(0.1m),  // Specify candle type and price range
	security)
{
	MarketData =
	{
		BuildMode = MarketDataBuildModes.Build,
		BuildFrom = DataType.Ticks
	}
};
_connector.Subscribe(rangeCandleSubscription);
```

### Velas Renko

```cs
// Renko candles with a step of 0.1
var renkoCandleSubscription = new Subscription(
	DataType.Renko(0.1m),  // Specify candle type and block size
	security)
{
	MarketData =
	{
		BuildMode = MarketDataBuildModes.Build,
		BuildFrom = DataType.Ticks
	}
};
_connector.Subscribe(renkoCandleSubscription);
```

### Velas de punto y figura (P&F)

```cs
// Point and Figure candles
var pnfCandleSubscription = new Subscription(
	DataType.PnF(new PnfArg { BoxSize = 0.1m, ReversalAmount = 1 }),  // Specify P&F parameters
	security)
{
	MarketData =
	{
		BuildMode = MarketDataBuildModes.Build,
		BuildFrom = DataType.Ticks
	}
};
_connector.Subscribe(pnfCandleSubscription);
```

## Próximos pasos

[Gráfico](candles/chart.md)

[Tipo de vela personalizado](candles/custom_type_of_candle.md)