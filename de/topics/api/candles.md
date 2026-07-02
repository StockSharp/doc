# Candles

[S#](../api.md) unterstützt die folgenden Candle-Typen:

- [TimeFrameCandleMessage](xref:StockSharp.Messages.TimeFrameCandleMessage) - eine Candle, die auf einem Zeitintervall, dem Zeitrahmen, basiert. Sie können sowohl beliebte Intervalle (Minuten, Stunden, täglich) als auch benutzerdefinierte festlegen. Zum Beispiel 21 Sekunden, 4,5 Minuten usw.
- [RangeCandleMessage](xref:StockSharp.Messages.RangeCandleMessage) - eine Preisbereichs-Candle. Eine neue Candle wird erstellt, wenn ein Trade mit einem Preis auftritt, der die zulässigen Grenzen überschreitet. Die zulässige Grenze wird jedes Mal auf Basis des Preises des ersten Trades gebildet.
- [VolumeCandleMessage](xref:StockSharp.Messages.VolumeCandleMessage) - eine Candle wird gebildet, bis das Gesamtvolumen der Trades ein bestimmtes Limit überschreitet. Wenn ein neuer Trade das zulässige Volumen überschreitet, wird er in eine neue Candle aufgenommen.
- [TickCandleMessage](xref:StockSharp.Messages.TickCandleMessage) - dasselbe wie [VolumeCandleMessage](xref:StockSharp.Messages.VolumeCandleMessage), jedoch wird anstelle des Volumens die Anzahl der Trades als Begrenzung verwendet.
- [PnFCandleMessage](xref:StockSharp.Messages.PnFCandleMessage) - eine Point-and-Figure-Chart-Candle (X-O-Chart).
- [RenkoCandleMessage](xref:StockSharp.Messages.RenkoCandleMessage) - Renko-Candle.

Die Arbeit mit Candles wird im Beispiel im Ordner *Samples\/02\_Candles\/01\_Realtime* gezeigt.

Die folgenden Bilder zeigen [TimeFrameCandleMessage](xref:StockSharp.Messages.TimeFrameCandleMessage)- und [RangeCandleMessage](xref:StockSharp.Messages.RangeCandleMessage)-Charts:

![sample timeframecandles](../../images/sample_timeframecandles.png)

![sample rangecandles](../../images/sample_rangecandles.png)

## Beginn des Datenabrufs

1. Um Candles zu erhalten, erstellen Sie ein Abonnement mit der Klasse [Subscription](xref:StockSharp.BusinessEntities.Subscription):

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

2. Um Candles zu empfangen, abonnieren Sie das Ereignis [Connector.CandleReceived](xref:StockSharp.Algo.Connector.CandleReceived), das das Erscheinen eines neuen Werts zur Verarbeitung signalisiert:

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
> Die grafische Komponente [Chart](xref:StockSharp.Xaml.Charting.Chart) wird zur Anzeige von Candles verwendet.

3. Als Nächstes starten Sie das Abonnement über die Methode [Connector.Subscribe](xref:StockSharp.Algo.Connector.Subscribe(StockSharp.BusinessEntities.Subscription)):

```cs
// Start the subscription
_connector.Subscribe(subscription);
```

Danach wird das Ereignis [Connector.CandleReceived](xref:StockSharp.Algo.Connector.CandleReceived) aufgerufen.

4. Das Ereignis [Connector.CandleReceived](xref:StockSharp.Algo.Connector.CandleReceived) wird nicht nur aufgerufen, wenn eine neue Candle erscheint, sondern auch, wenn sich die aktuelle ändert.

Wenn Sie nur **"vollständige"** Candles anzeigen möchten, müssen Sie die Eigenschaft [ICandleMessage.State](xref:StockSharp.Messages.ICandleMessage.State) der empfangenen Candle überprüfen:

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

5. Für das Abonnement können zusätzliche Parameter konfiguriert werden:

- **Candle-Erstellungsmodus** - bestimmt, ob fertige Daten angefordert oder aus einem anderen Datentyp erstellt werden:

```cs
// Request only ready-made data
subscription.MarketData.BuildMode = MarketDataBuildModes.Load;

// Only build from another data type
subscription.MarketData.BuildMode = MarketDataBuildModes.Build;

// Request ready-made data, and if not available - build
subscription.MarketData.BuildMode = MarketDataBuildModes.LoadAndBuild;
```

- **Quelle für die Candle-Erstellung** - gibt an, aus welchem Datentyp Candles erstellt werden sollen, wenn sie nicht direkt verfügbar sind:

```cs
// Building candles from tick trades
subscription.MarketData.BuildFrom = DataType.Ticks;

// Building candles from order books
subscription.MarketData.BuildFrom = DataType.MarketDepth;

// Building candles from Level1
subscription.MarketData.BuildFrom = DataType.Level1;
```

- **Feld für die Candle-Erstellung** - muss für bestimmte Datentypen angegeben werden:

```cs
// Building candles from the best bid price in Level1
subscription.MarketData.BuildField = Level1Fields.BestBidPrice;

// Building candles from the best ask price in Level1
subscription.MarketData.BuildField = Level1Fields.BestAskPrice;

// Building candles from the middle of the spread in the order book
subscription.MarketData.BuildField = Level1Fields.SpreadMiddle;
```

- **Volumenprofil** - Berechnung des Volumenprofils für Candles:

```cs
// Enable volume profile calculation
subscription.MarketData.IsCalcVolumeProfile = true;
```

## Beispiele für Abonnements verschiedener Candle-Typen

### Candles mit Standard-Zeitrahmen

```cs
// 5-minute candles
var timeFrameSubscription = new Subscription(
	DataType.TimeFrame(TimeSpan.FromMinutes(5)),
	security);
_connector.Subscribe(timeFrameSubscription);
```

### Nur historische Candles laden

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

### Erstellung von Candles mit nicht standardmäßigem Zeitrahmen aus Ticks

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

### Erstellung von Candles aus Orderbuchdaten

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

### Candles mit Volumenprofil

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

### Volumen-Candles

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

### Tick-Anzahl-Candles

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

### Preisbereichs-Candles

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

### Renko-Candles

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

### Point-and-Figure-Candles (P&F)

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

## Nächste Schritte

[Chart](candles/chart.md)

[Benutzerdefinierter Candle-Typ](candles/custom_type_of_candle.md)
