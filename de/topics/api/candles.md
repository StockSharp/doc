# Kerzen

[S#](../api.md) unterstützt die folgenden Kerzentypen:

- [TimeFrameCandleMessage](xref:StockSharp.Messages.TimeFrameCandleMessage) - eine Kerze, die auf einem Zeitintervall, dem Zeitrahmen, basiert. Sie können sowohl beliebte Intervalle (Minuten, Stunden, täglich) als auch benutzerdefinierte festlegen. Zum Beispiel 21 Sekunden, 4,5 Minuten usw.
- [RangeCandleMessage](xref:StockSharp.Messages.RangeCandleMessage) - eine Preisbereichskerze. Eine neue Kerze wird erstellt, wenn ein Trade mit einem Preis auftritt, der die zulässigen Grenzen überschreitet. Die zulässige Grenze wird jedes Mal auf Basis des Preises des ersten Trades gebildet.
- [VolumeCandleMessage](xref:StockSharp.Messages.VolumeCandleMessage) - eine Kerze wird gebildet, bis das Gesamtvolumen der Trades ein bestimmtes Limit überschreitet. Wenn ein neuer Trade das zulässige Volumen überschreitet, wird er in eine neue Kerze aufgenommen.
- [TickCandleMessage](xref:StockSharp.Messages.TickCandleMessage) - dasselbe wie [VolumeCandleMessage](xref:StockSharp.Messages.VolumeCandleMessage), jedoch wird anstelle des Volumens die Anzahl der Trades als Begrenzung verwendet.
- [PnFCandleMessage](xref:StockSharp.Messages.PnFCandleMessage) - eine Point-and-Figure-Chart-Kerze (X-O-Chart).
- [RenkoCandleMessage](xref:StockSharp.Messages.RenkoCandleMessage) - Renko-Kerze.

Die Arbeit mit Kerzen wird im Beispiel im Ordner *Samples\/02\_Candles\/01\_Realtime* gezeigt.

Die folgenden Bilder zeigen [TimeFrameCandleMessage](xref:StockSharp.Messages.TimeFrameCandleMessage)- und [RangeCandleMessage](xref:StockSharp.Messages.RangeCandleMessage)-Charts:

![Beispiel Zeitrahmen-Kerzen](../../images/sample_timeframecandles.png)

![Beispiel Range-Kerzen](../../images/sample_rangecandles.png)

## Beginn des Datenabrufs

1. Um Kerzen zu erhalten, erstellen Sie ein Abonnement mit der Klasse [Subscription](xref:StockSharp.BusinessEntities.Subscription):

```cs
// Abonnement für 5-Minuten-Kerzen erstellen
var subscription = new Subscription(
	DataType.TimeFrame(TimeSpan.FromMinutes(5)),  // Datentyp mit Zeitrahmenangabe
	security)  // Instrument
{
	// Zusätzliche Parameter über die MarketData-Eigenschaft konfigurieren
	MarketData =
	{
		// Zeitraum, für den historische Daten angefordert werden (letzte 30 Tage)
		From = DateTime.Today.Subtract(TimeSpan.FromDays(30)),
		To = DateTime.Now
	}
};
```

2. Um Kerzen zu empfangen, abonnieren Sie das Ereignis [Connector.CandleReceived](xref:StockSharp.Algo.Connector.CandleReceived), das das Erscheinen eines neuen Werts zur Verarbeitung signalisiert:

```cs
// Ereignis zum Empfang von Kerzen abonnieren
_connector.CandleReceived += OnCandleReceived;

// Handler für das Kerzenempfangsereignis
private void OnCandleReceived(Subscription subscription, ICandleMessage candle)
{
	// Hier ist subscription das von uns erstellte Abonnementobjekt
	// empfangene Kerze

	// Prüfen, ob die Kerze zu unserem Abonnement gehört
	if (subscription == _candleSubscription)
	{
		// Kerze im Diagramm zeichnen
		Chart.Draw(_candleElement, candle);
	}
}
```

> [!TIP]
> Die grafische Komponente [Chart](xref:StockSharp.Xaml.Charting.Chart) wird zur Anzeige von Kerzen verwendet.

3. Als Nächstes starten Sie das Abonnement über die Methode [Connector.Subscribe](xref:StockSharp.Algo.Connector.Subscribe(StockSharp.BusinessEntities.Subscription)):

```cs
// Abonnement starten
_connector.Subscribe(subscription);
```

Danach wird das Ereignis [Connector.CandleReceived](xref:StockSharp.Algo.Connector.CandleReceived) aufgerufen.

4. Das Ereignis [Connector.CandleReceived](xref:StockSharp.Algo.Connector.CandleReceived) wird nicht nur aufgerufen, wenn eine neue Kerze erscheint, sondern auch, wenn sich die aktuelle ändert.

Wenn Sie nur **"vollständige"** Kerzen anzeigen möchten, müssen Sie die Eigenschaft [ICandleMessage.State](xref:StockSharp.Messages.ICandleMessage.State) der empfangenen Kerze überprüfen:

```cs
private void OnCandleReceived(Subscription subscription, ICandleMessage candle)
{
	// Prüfen, ob die Kerze zu unserem Abonnement gehört
	if (subscription != _candleSubscription)
		return;

	// Prüfen, ob die Kerze abgeschlossen ist
	if (candle.State == CandleStates.Finished)
	{
		// Daten zum Zeichnen erstellen
		var chartData = new ChartDrawData();
		chartData.Group(candle.OpenTime).Add(_candleElement, candle);

		// Kerze im Diagramm zeichnen
		this.GuiAsync(() => Chart.Draw(chartData));
	}
}
```

5. Für das Abonnement können zusätzliche Parameter konfiguriert werden:

- **Kerzenerstellungsmodus** - bestimmt, ob fertige Daten angefordert oder aus einem anderen Datentyp erstellt werden:

```cs
// Nur fertige Daten anfordern
subscription.MarketData.BuildMode = MarketDataBuildModes.Load;

// Nur aus einem anderen Datentyp erstellen
subscription.MarketData.BuildMode = MarketDataBuildModes.Build;

// Fertige Daten anfordern und bei Bedarf erstellen
subscription.MarketData.BuildMode = MarketDataBuildModes.LoadAndBuild;
```

- **Quelle für die Kerzenerstellung** - gibt an, aus welchem Datentyp Kerzen erstellt werden sollen, wenn sie nicht direkt verfügbar sind:

```cs
// Kerzen aus Tick-Trades erstellen
subscription.MarketData.BuildFrom = DataType.Ticks;

// Kerzen aus Orderbüchern erstellen
subscription.MarketData.BuildFrom = DataType.MarketDepth;

// Kerzen aus Level1 erstellen
subscription.MarketData.BuildFrom = DataType.Level1;
```

- **Feld für die Kerzenerstellung** - muss für bestimmte Datentypen angegeben werden:

```cs
// Kerzen aus dem besten Bid-Preis in Level1 erstellen
subscription.MarketData.BuildField = Level1Fields.BestBidPrice;

// Kerzen aus dem besten Ask-Preis in Level1 erstellen
subscription.MarketData.BuildField = Level1Fields.BestAskPrice;

// Kerzen aus der Spread-Mitte im Orderbuch erstellen
subscription.MarketData.BuildField = Level1Fields.SpreadMiddle;
```

- **Volumenprofil** - Berechnung des Volumenprofils für Kerzen:

```cs
// Volumenprofilberechnung aktivieren
subscription.MarketData.IsCalcVolumeProfile = true;
```

## Beispiele für Abonnements verschiedener Kerzentypen

### Kerzen mit Standard-Zeitrahmen

```cs
// 5-Minuten-Kerzen
var timeFrameSubscription = new Subscription(
	DataType.TimeFrame(TimeSpan.FromMinutes(5)),
	security);
_connector.Subscribe(timeFrameSubscription);
```

### Nur historische Kerzen laden

```cs
// Nur historische Kerzen laden, ohne in den Echtzeitmodus zu wechseln
var historicalSubscription = new Subscription(
	DataType.TimeFrame(TimeSpan.FromMinutes(5)),
	security)
{
	MarketData =
	{
		From = DateTime.Today.Subtract(TimeSpan.FromDays(30)),
		To = DateTime.Today,  // Enddatum angeben
		BuildMode = MarketDataBuildModes.Load  // Nur fertige Daten laden
	}
};
_connector.Subscribe(historicalSubscription);
```

### Erstellung von Kerzen mit nicht standardmäßigem Zeitrahmen aus Ticks

```cs
// Kerzen mit 21-Sekunden-Zeitrahmen, aus Ticks erstellt
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

### Erstellung von Kerzen aus Orderbuchdaten

```cs
// Kerzen, die aus der Spread-Mitte im Orderbuch erstellt wurden
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

### Kerzen mit Volumenprofil

```cs
// 5-Minuten-Kerzen mit Volumenprofilberechnung
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

### Volumenkerzen

```cs
// Volumenkerzen (jede Kerze enthält 1000 Kontrakte Volumen)
var volumeCandleSubscription = new Subscription(
	DataType.Volume(1000m),  // Kerzentyp und Volumen angeben
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

### Tick-Anzahl-Kerzen

```cs
// Tickanzahl-Kerzen (jede Kerze enthält 1000 Trades)
var tickCandleSubscription = new Subscription(
	DataType.Tick(1000),  // Kerzentyp und Anzahl der Trades angeben
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

### Preisbereichskerzen

```cs
// Price-Range-Kerzen mit einer Spanne von 0,1 Einheiten
var rangeCandleSubscription = new Subscription(
	DataType.Range(0.1m),  // Kerzentyp und Preisspanne angeben
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

### Renko-Kerzen

```cs
// Renko-Kerzen mit einer Schrittweite von 0,1
var renkoCandleSubscription = new Subscription(
	DataType.Renko(0.1m),  // Kerzentyp und Blockgröße angeben
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

### Point-and-Figure-Kerzen (P&F)

```cs
// Point-and-Figure-Kerzen
var pnfCandleSubscription = new Subscription(
	DataType.PnF(new PnfArg { BoxSize = 0.1m, ReversalAmount = 1 }),  // P&F-Parameter angeben
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

[Benutzerdefinierter Kerzentyp](candles/custom_type_of_candle.md)
