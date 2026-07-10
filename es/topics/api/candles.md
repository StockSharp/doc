# Velas

[S#](../api.md) admite los siguientes tipos de velas:

- [TimeFrameCandleMessage](xref:StockSharp.Messages.TimeFrameCandleMessage) - una vela basada en un intervalo de tiempo. Se pueden configurar tanto intervalos populares (minutos, horas, diario) como personalizados. Por ejemplo, 21 segundos, 4.5 minutos, etc.
- [RangeCandleMessage](xref:StockSharp.Messages.RangeCandleMessage) - una vela de rango de precio. Se crea una nueva vela cuando aparece una operación con un precio que excede los límites aceptables. El límite aceptable se forma cada vez basándose en el precio de la primera operación.
- [VolumeCandleMessage](xref:StockSharp.Messages.VolumeCandleMessage) - una vela se forma hasta que el volumen total de operaciones supera un límite especificado. Si una nueva operación excede el volumen permitido, se incluye en una nueva vela.
- [TickCandleMessage](xref:StockSharp.Messages.TickCandleMessage) - igual que [VolumeCandleMessage](xref:StockSharp.Messages.VolumeCandleMessage), pero se usa el número de operaciones como limitación en lugar del volumen.
- [PnFCandleMessage](xref:StockSharp.Messages.PnFCandleMessage) - una vela de gráfico punto y figura (gráfico X-O).
- [RenkoCandleMessage](xref:StockSharp.Messages.RenkoCandleMessage) - vela Renko.

Cómo trabajar con velas se muestra en el ejemplo ubicado en la carpeta *Samples\/02\_Candles\/01\_Realtime*.

Las siguientes imágenes muestran los gráficos de [TimeFrameCandleMessage](xref:StockSharp.Messages.TimeFrameCandleMessage) y [RangeCandleMessage](xref:StockSharp.Messages.RangeCandleMessage):

![Ejemplo de velas por intervalo temporal](../../images/sample_timeframecandles.png)

![Ejemplo de velas por rango](../../images/sample_rangecandles.png)

## Inicio de la recepción de datos

1. Para obtener velas, cree una suscripción usando la clase [Subscription](xref:StockSharp.BusinessEntities.Subscription):

```cs
// Crear una suscripción a velas de 5 minutos
var subscription = new Subscription(
	DataType.TimeFrame(TimeSpan.FromMinutes(5)),  // Tipo de datos con especificación de marco temporal
	security)  // Instrumento
{
	// Configurar parámetros adicionales mediante la propiedad MarketData
	MarketData =
	{
		// Periodo para el que solicitamos datos históricos (últimos 30 días)
		From = DateTime.Today.Subtract(TimeSpan.FromDays(30)),
		To = DateTime.Now
	}
};
```

2. Para recibir velas, suscríbase al evento [Connector.CandleReceived](xref:StockSharp.Algo.Connector.CandleReceived), que señala la aparición de un nuevo valor para procesar:

```cs
// Suscribirse al evento de recepción de velas
_connector.CandleReceived += OnCandleReceived;

// Controlador del evento de recepción de velas
private void OnCandleReceived(Subscription subscription, ICandleMessage candle)
{
	// Aquí subscription es el objeto de suscripción que creamos
	// candle: vela recibida

	// Comprobar si la vela pertenece a nuestra suscripción
	if (subscription == _candleSubscription)
	{
		// Dibujar la vela en el gráfico
		Chart.Draw(_candleElement, candle);
	}
}
```

> [!TIP]
> El componente gráfico [Chart](xref:StockSharp.Xaml.Charting.Chart) se usa para mostrar velas.

3. A continuación, inicie la suscripción mediante el método [Connector.Subscribe](xref:StockSharp.Algo.Connector.Subscribe(StockSharp.BusinessEntities.Subscription)):

```cs
// Iniciar la suscripción
_connector.Subscribe(subscription);
```

Después de esto, el evento [Connector.CandleReceived](xref:StockSharp.Algo.Connector.CandleReceived) comenzará a ser llamado.

4. El evento [Connector.CandleReceived](xref:StockSharp.Algo.Connector.CandleReceived) se llama no solo cuando aparece una nueva vela, sino también cuando cambia la actual.

Si necesita mostrar solo velas **"completas"**, debe comprobar la propiedad [ICandleMessage.State](xref:StockSharp.Messages.ICandleMessage.State) de la vela recibida:

```cs
private void OnCandleReceived(Subscription subscription, ICandleMessage candle)
{
	// Comprobar si la vela pertenece a nuestra suscripción
	if (subscription != _candleSubscription)
		return;

	// Comprobar si la vela está completada
	if (candle.State == CandleStates.Finished)
	{
		// Crear datos para el dibujo
		var chartData = new ChartDrawData();
		chartData.Group(candle.OpenTime).Add(_candleElement, candle);

		// Dibujar la vela en el gráfico
		this.GuiAsync(() => Chart.Draw(chartData));
	}
}
```

5. Se pueden configurar parámetros adicionales para la suscripción:

- **Modo de construcción de velas** - determina si se solicitarán datos ya preparados o si se construirán a partir de otro tipo de datos:

```cs
// Solicitar solo datos ya preparados
subscription.MarketData.BuildMode = MarketDataBuildModes.Load;

// Construir solo desde otro tipo de datos
subscription.MarketData.BuildMode = MarketDataBuildModes.Build;

// Solicitar datos ya preparados y, si no están disponibles, construirlos
subscription.MarketData.BuildMode = MarketDataBuildModes.LoadAndBuild;
```

- **Fuente para construir velas** - indica a partir de qué tipo de datos construir las velas si no están disponibles directamente:

```cs
// Construcción de velas a partir de operaciones tick
subscription.MarketData.BuildFrom = DataType.Ticks;

// Construcción de velas a partir del libro de órdenes
subscription.MarketData.BuildFrom = DataType.MarketDepth;

// Construcción de velas a partir de Level1
subscription.MarketData.BuildFrom = DataType.Level1;
```

- **Campo para construir velas** - debe especificarse para ciertos tipos de datos:

```cs
// Construcción de velas desde el mejor precio bid de Level1
subscription.MarketData.BuildField = Level1Fields.BestBidPrice;

// Construcción de velas desde el mejor precio ask de Level1
subscription.MarketData.BuildField = Level1Fields.BestAskPrice;

// Construcción de velas desde el punto medio del spread en el libro de órdenes
subscription.MarketData.BuildField = Level1Fields.SpreadMiddle;
```

- **Perfil de volumen** - cálculo del perfil de volumen para las velas:

```cs
// Activar el cálculo del perfil de volumen
subscription.MarketData.IsCalcVolumeProfile = true;
```

## Ejemplos de suscripciones a diferentes tipos de velas

### Velas con marco temporal estándar

```cs
// Velas de 5 minutos
var timeFrameSubscription = new Subscription(
	DataType.TimeFrame(TimeSpan.FromMinutes(5)),
	security);
_connector.Subscribe(timeFrameSubscription);
```

### Carga de solo velas históricas

```cs
// Cargar solo velas históricas sin pasar a tiempo real
var historicalSubscription = new Subscription(
	DataType.TimeFrame(TimeSpan.FromMinutes(5)),
	security)
{
	MarketData =
	{
		From = DateTime.Today.Subtract(TimeSpan.FromDays(30)),
		To = DateTime.Today,  // Especificar fecha final
		BuildMode = MarketDataBuildModes.Load  // Cargar solo datos ya preparados
	}
};
_connector.Subscribe(historicalSubscription);
```

### Construcción de velas de marco temporal no estándar a partir de ticks

```cs
// Velas con marco temporal de 21 segundos construidas desde ticks
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
// Velas construidas desde el punto medio del spread en el libro de órdenes
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
// Velas de 5 minutos con cálculo de perfil de volumen
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
// Velas de volumen (cada vela contiene un volumen de 1000 contratos)
var volumeCandleSubscription = new Subscription(
	DataType.Volume(1000m),  // Especificar tipo de vela y volumen
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
// Velas por número de ticks (cada vela contiene 1000 operaciones)
var tickCandleSubscription = new Subscription(
	DataType.Tick(1000),  // Especificar tipo de vela y número de operaciones
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
// Velas de rango de precio con rango de 0,1 unidades
var rangeCandleSubscription = new Subscription(
	DataType.Range(0.1m),  // Especificar tipo de vela y rango de precios
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
// Velas Renko con paso de 0,1
var renkoCandleSubscription = new Subscription(
	DataType.Renko(0.1m),  // Especificar tipo de vela y tamaño de bloque
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
// Velas Point and Figure
var pnfCandleSubscription = new Subscription(
	DataType.PnF(new PnfArg { BoxSize = 0.1m, ReversalAmount = 1 }),  // Especificar parámetros P&F
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
