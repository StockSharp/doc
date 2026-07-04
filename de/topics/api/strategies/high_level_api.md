# High-Level-API in Strategien

StockSharp stellt eine Reihe von High-Level-APIs bereit, die die Arbeit mit häufigen Aufgaben in Handelsstrategien vereinfachen. Diese Schnittstellen ermöglichen saubereren Code, der sich auf die Handelslogik statt auf technische Details konzentriert.

## Vereinfachte Abonnementverwaltung

High-Level-Methoden für die Arbeit mit Abonnements verbergen die Komplexität der Verwaltung des Abonnementlebenszyklus und der Datenverarbeitung.

### SubscribeCandles-Methode

Statt ein Abonnement manuell zu erstellen und Ereignishandler einzurichten, können Sie die Methode [SubscribeCandles](xref:StockSharp.Algo.Strategies.Strategy.SubscribeCandles(System.TimeSpan,System.Boolean,StockSharp.BusinessEntities.Security)) verwenden:

```cs
// Kerzenabonnement in einer einzigen Zeile erstellen und konfigurieren
var subscription = SubscribeCandles(CandleType);
```

Diese Methode gibt ein Objekt vom Typ [ISubscriptionHandler\<ICandleMessage\>](xref:StockSharp.Algo.Strategies.ISubscriptionHandler`1) zurück, das eine bequeme Schnittstelle für die weitere Abonnementkonfiguration bereitstellt.

### Automatische Bindung von Indikatoren an ein Abonnement

Die High-Level-API erleichtert das Binden von Indikatoren an ein Datenabonnement:

```cs
var longSma = new SMA { Length = Long };
var shortSma = new SMA { Length = Short };

subscription
	// Indikatoren an Kerzenabonnement binden
	.Bind(longSma, shortSma, OnProcess)
	// Verarbeitung starten
	.Start();
```

#### Automatisches Hinzufügen von Indikatoren zur Sammlung Strategy.Indicators

Wichtig ist, dass Sie bei Verwendung der Methode [Bind](xref:StockSharp.Algo.Strategies.ISubscriptionHandler`1.Bind(StockSharp.Algo.Indicators.IIndicator,StockSharp.Algo.Indicators.IIndicator,System.Action{`0,System.Decimal,System.Decimal})) zum Verknüpfen von Indikatoren mit einem Abonnement diese Indikatoren **nicht zusätzlich** zur Sammlung [Strategy.Indicators](xref:StockSharp.Algo.Strategies.Strategy.Indicators) hinzufügen müssen, wie es im traditionellen Ansatz üblich ist (beschrieben in der [Indikatorendokumentation](indicators.md)). Das System erledigt automatisch:

1. Hinzufügen von Indikatoren zur Sammlung [Indicators](xref:StockSharp.Algo.Strategies.Strategy.Indicators)
2. Verfolgen des Bildungszustands der Indikatoren
3. Aktualisieren des Zustands [IsFormed](xref:StockSharp.Algo.Strategies.Strategy.IsFormed) der Strategie

Dies vereinfacht den Code erheblich und reduziert die Fehlerwahrscheinlichkeit.

Wenn Sie Indikatorwerte auch dann erhalten müssen, wenn einige davon noch keine Daten haben (`IIndicatorValue.IsEmpty` ist `true`), verwenden Sie die Methode `BindWithEmpty`. In diesem Fall müssen die Handlerargumente vom Typ `decimal?` sein. Sie können auch `BindEx` verwenden, um die rohen `IIndicatorValue`-Objekte direkt zu untersuchen.

#### Verwendung von BindEx für rohe Indikatorwerte

Wenn ein Indikator nicht standardmäßige Werte zurückgibt, also nicht nur Zahlen, können Sie die Methode [BindEx](xref:StockSharp.Algo.Strategies.ISubscriptionHandler`1.BindEx(StockSharp.Algo.Indicators.IIndicator,System.Action{`0,StockSharp.Algo.Indicators.IIndicatorValue},System.Boolean)) verwenden, die Zugriff auf das ursprüngliche Objekt [IIndicatorValue](xref:StockSharp.Algo.Indicators.IIndicatorValue) bietet:

```cs
subscription
	.BindEx(indicator, OnProcessWithRawValue)
	.Start();

// Handler empfängt das ursprüngliche IIndicatorValue
private void OnProcessWithRawValue(ICandleMessage candle, IIndicatorValue value)
{
	// Zugriff auf Eigenschaften von IIndicatorValue
	if (value.IsFinal)
	{
		// Für Indikatoren, die boolesche Werte zurückgeben
		var boolValue = value.GetValue<bool>();
		
		// Oder andere Datentypen, die für einen bestimmten Indikator spezifisch sind
		// ...
	}
}
```

Die Methode [BindEx](xref:StockSharp.Algo.Strategies.ISubscriptionHandler`1.BindEx(StockSharp.Algo.Indicators.IIndicator,System.Action{`0,StockSharp.Algo.Indicators.IIndicatorValue},System.Boolean)) ist besonders in folgenden Fällen nützlich:

- Arbeit mit Indikatoren, die boolesche Werte zurückgeben, z. B. [Fractals](xref:StockSharp.Algo.Indicators.Fractals)
- Zugriff auf zusätzliche Eigenschaften des Indikatorwerttyps, z. B. das Flag [IsFinal](xref:StockSharp.Algo.Indicators.IIndicatorValue.IsFinal)
- Arbeit mit Indikatoren, die strukturierte Daten zurückgeben

#### Arbeit mit komplexen Indikatoren (IComplexIndicator)

Für komplexe Indikatoren, die mehrere interne Indikatoren enthalten, z. B. [BollingerBands](xref:StockSharp.Algo.Indicators.BollingerBands), [MACD](xref:StockSharp.Algo.Indicators.MovingAverageConvergenceDivergence), stellt die API spezielle Überladungen der Methoden `Bind` und `BindEx` bereit:

```cs
// Komplexen Indikator erstellen
var bollinger = new BollingerBands 
{ 
	Length = 20, 
	Deviation = 2 
};

// Komplexen Indikator an ein Abonnement binden
subscription
	.BindEx(bollinger, OnProcessBollinger)
	.Start();

// Handler empfängt die BollingerBandsValue-Instanz
private void OnProcessBollinger(ICandleMessage candle, IIndicatorValue value)
{
	var typed = (BollingerBandsValue)value;

	// Werte der Bollinger-Bänder verwenden
	if (candle.ClosePrice >= typed.UpBand && Position >= 0)
		SellMarket(Volume + Math.Abs(Position));
	else if (candle.ClosePrice <= typed.LowBand && Position <= 0)
		BuyMarket(Volume + Math.Abs(Position));
}
```

Für flexiblere Arbeit können Sie [BindEx](xref:StockSharp.Algo.Strategies.ISubscriptionHandler`1.BindEx(StockSharp.Algo.Indicators.IIndicator,System.Action{`0,StockSharp.Algo.Indicators.IIndicatorValue},System.Boolean)) mit direktem Zugriff auf den komplexen Indikatorwert verwenden:

```cs
subscription.BindEx(bollinger, (candle, indicatorValue) =>
{
	var typed = (BollingerBandsValue)indicatorValue;

	if (candle.ClosePrice >= typed.UpBand && Position >= 0)
		SellMarket(Volume + Math.Abs(Position));
	else if (candle.ClosePrice <= typed.LowBand && Position <= 0)
		BuyMarket(Volume + Math.Abs(Position));
});
```

Die Methode [BindEx](xref:StockSharp.Algo.Strategies.ISubscriptionHandler`1.BindEx(StockSharp.Algo.Indicators.IIndicator,System.Action{`0,StockSharp.Algo.Indicators.IIndicatorValue},System.Boolean)) für komplexe Indikatoren führt automatisch aus:

1. Verarbeitung der Eingabedaten durch den komplexen Indikator
2. Übergabe des resultierenden `IIndicatorValue` an den angegebenen Handler

Konvertieren Sie den Wert in den dedizierten **Werttyp** des Indikators, um mit seinen einzelnen Feldern zu arbeiten.

### Die Methode `Bind` stellt eine Verbindung zwischen Abonnementdaten und Indikatoren her. Wenn eine neue Kerze empfangen wird:

1. Die Kerze wird automatisch zur Verarbeitung an die Indikatoren gesendet.
2. Verarbeitungsergebnisse werden an den angegebenen Handler übergeben, im Beispiel an die Methode `OnProcess`.
3. Der gesamte Code für Synchronisierung und Zustandsverwaltung bleibt vor dem Entwickler verborgen.

Der Handler erhält sofort verwendbare Werte als einfache `decimal`-Typen. Die Methode wird nur aufgerufen, wenn alle gebundenen Indikatoren Daten zurückgeben:

```cs
private void OnProcess(ICandleMessage candle, decimal longValue, decimal shortValue)
{
	// Direkt mit fertigen Indikatorwerten arbeiten
	var isShortLessThenLong = shortValue < longValue;
	
	// Handelslogik verwendet saubere numerische Werte
	// ohne sie aus IIndicatorValue extrahieren zu müssen
	// ...
}
```

Dies vereinfacht den Code erheblich und macht ihn besser lesbar, da der Entwickler nicht:
- Das Ereignis zum Empfang einer Kerze manuell behandeln muss
- Daten manuell an Indikatoren übergeben muss
- Werte aus Indikatorergebnissen extrahieren muss

## Vereinfachte Chartverwaltung

### Automatische Visualisierung

Die High-Level-API stellt einfache Methoden zum Binden von Abonnements und Indikatoren an Chartelemente bereit:

```cs
var area = CreateChartArea();

// area kann null sein, wenn ohne GUI ausgeführt wird
if (area != null)
{
	// Automatische Bindung von Kerzen an den Chartbereich
	DrawCandles(area, subscription);

	// Indikatoren mit Farbanpassung zeichnen
	DrawIndicator(area, shortSma, System.Drawing.Color.Coral);
	DrawIndicator(area, longSma);
	
	// Eigene Trades zeichnen
	DrawOwnTrades(area);
	
	// Orders zeichnen
	DrawOrders(area);
}
```

#### DrawCandles-Methode

Die Methode [DrawCandles](xref:StockSharp.Algo.Strategies.Strategy.DrawCandles(StockSharp.Charting.IChartArea,StockSharp.BusinessEntities.Subscription)) verknüpft ein Kerzenabonnement automatisch mit einem Chart-Kerzenelement:

```cs
// Chartelement zur Anzeige von Kerzen erstellen
IChartCandleElement candles = DrawCandles(area, subscription);

// Zusätzliche Elementparameter können konfiguriert werden
candles.DrawOpenClose = true;  // Open-/Close-Linien anzeigen
candles.DrawHigh = true;       // Hochs anzeigen
candles.DrawLow = true;        // Tiefs anzeigen
```

Die Methode gibt ein Chartelement [IChartCandleElement](xref:StockSharp.Charting.IChartCandleElement) zurück, das weiter angepasst werden kann.

#### DrawIndicator-Methode

Die Methode [DrawIndicator](xref:StockSharp.Algo.Strategies.Strategy.DrawIndicator(StockSharp.Charting.IChartArea,StockSharp.Algo.Indicators.IIndicator,System.Nullable{System.Drawing.Color},System.Nullable{System.Drawing.Color})) erstellt und konfiguriert ein Chartelement zur Anzeige von Indikatorwerten:

```cs
// Einfaches Hinzufügen eines Indikators zum Chart mit Standardfarbe
IChartIndicatorElement smaElem = DrawIndicator(area, sma);

// Hinzufügen eines Indikators mit angegebener Primärfarbe
IChartIndicatorElement rsiFast = DrawIndicator(area, rsi, System.Drawing.Color.Red);

// Hinzufügen eines Indikators mit angegebener Primär- und Sekundärfarbe
IChartIndicatorElement bollingerElem = DrawIndicator(
	area, 
	bollinger, 
	System.Drawing.Color.Blue,    // Primärfarbe
	System.Drawing.Color.Gray     // Sekundärfarbe (für die zweite Linie)
);

// Zusätzliche Elementkonfiguration
smaElem.DrawStyle = DrawStyles.Line;           // Zeichenstil: Linie
rsiFast.DrawStyle = DrawStyles.Dot;            // Zeichenstil: Punkte
bollingerElem.DrawStyle = DrawStyles.Dashdot;  // Zeichenstil: Strich-Punkt
```

Die Methode gibt ein Chartelement [IChartIndicatorElement](xref:StockSharp.Charting.IChartIndicatorElement) zurück, das angepasst werden kann. Bei Indikatoren mit mehreren Werten, z. B. [BollingerBands](xref:StockSharp.Algo.Indicators.BollingerBands), wird die Primärfarbe auf den ersten Wert und die Sekundärfarbe auf den zweiten angewendet.

#### DrawOwnTrades-Methode

Die Methode [DrawOwnTrades](xref:StockSharp.Algo.Strategies.Strategy.DrawOwnTrades(StockSharp.Charting.IChartArea)) erstellt ein Element zur Anzeige der eigenen Trades der Strategie im Chart:

```cs
// Element zur Anzeige von Trades erstellen
IChartTradeElement trades = DrawOwnTrades(area);

// Elementkonfiguration
trades.BuyColor = System.Drawing.Color.Green;   // Farbe für Kauftrades
trades.SellColor = System.Drawing.Color.Red;    // Farbe für Verkaufstrades
trades.FullTitle = "My Strategy Trades";        // Elementtitel
```

Diese Methode richtet automatisch die Anzeige aller von der Strategie ausgeführten Trades ein. Trades werden im Chart als Marker an den Punkten angezeigt, an denen sie ausgeführt wurden, unter Berücksichtigung der Trade-Seite (Kauf/Verkauf).

#### DrawOrders-Methode

Die Methode [DrawOrders](xref:StockSharp.Algo.Strategies.Strategy.DrawOrders(StockSharp.Charting.IChartArea)) erstellt ein Element zur Anzeige von Orders im Chart:

```cs
// Element zur Anzeige von Orders erstellen
IChartOrderElement orders = DrawOrders(area);

// Elementkonfiguration
orders.BuyPendingColor = System.Drawing.Color.DarkGreen;   // Farbe für aktive Kauforders
orders.SellPendingColor = System.Drawing.Color.DarkRed;    // Farbe für aktive Verkaufsorders
orders.BuyColor = System.Drawing.Color.Green;              // Farbe für ausgeführte Kauforders
orders.SellColor = System.Drawing.Color.Red;               // Farbe für ausgeführte Verkaufsorders
orders.CancelColor = System.Drawing.Color.Gray;            // Farbe für stornierte Orders
```

Diese Methode richtet automatisch die Anzeige aller von der Strategie platzierten Orders ein. Orders werden als Marker auf ihren Preisniveaus mit unterschiedlicher Farbcodierung für verschiedene Orderzustände angezeigt.

#### CreateChartArea-Methode

Die Methode [CreateChartArea](xref:StockSharp.Algo.Strategies.Strategy.CreateChartArea) erstellt einen neuen Bereich im Strategiechart:

```cs
// Ersten Bereich für Kerzen und Indikatoren erstellen
var mainArea = CreateChartArea();
DrawCandles(mainArea, subscription);
DrawIndicator(mainArea, sma);

// Zweiten Bereich für separate Indikatoren erstellen (z. B. RSI)
var secondArea = CreateChartArea();
DrawIndicator(secondArea, rsi);
```

Die Aufteilung des Charts in Bereiche ermöglicht eine anschaulichere Darstellung unterschiedlicher Datentypen. Indikatoren mit einem anderen Wertebereich als der Preis, beispielsweise RSI oder Stochastic, werden besser in separaten Bereichen angezeigt.

Vorteile der High-Level-Visualisierungsmethoden:
- Keine manuelle Erstellung von `ChartDrawData`-Objekten erforderlich
- Keine manuelle Verwaltung der Datengruppierung nach Zeit erforderlich
- Kein Aufruf von `chart.Draw()` zur Aktualisierung des Charts erforderlich
- Automatische Datensynchronisierung zwischen Abonnements und Chartelementen
- Vereinfachte Verwaltung des Erscheinungsbilds grafischer Elemente

Das System aktualisiert den Chart automatisch, wenn neue Daten empfangen werden, sodass sich der Entwickler nicht auf technische Visualisierungsdetails konzentrieren muss.

## Positionsschutz

### StartProtection-Methode

Zum Schutz offener Positionen stellt StockSharp die High-Level-Methode [StartProtection](xref:StockSharp.Algo.Strategies.Strategy.StartProtection(StockSharp.Messages.Unit,StockSharp.Messages.Unit,System.Boolean,System.Nullable{System.TimeSpan},System.Nullable{System.TimeSpan},System.Boolean)) bereit:

```cs
// Positionsschutz mit Take-Profit- und Stop-Loss-Niveaus starten
StartProtection(TakeValue, StopValue);
```

Diese Methode richtet automatisch den Schutz für alle offenen Positionen ein:
- Verfolgt Preisänderungen
- Erstellt automatisch Orders zum Schließen von Positionen, wenn Take-Profit- oder Stop-Loss-Niveaus erreicht werden
- Unterstützt verschiedene Arten von Maßeinheiten (absolute Werte, Prozentwerte, Punkte)
- Kann Trailing Stop für adaptiven Positionsschutz verwenden

Beispiel mit zusätzlichen Parametern:

```cs
// Schutz mit Trailing Stop und Market-Orders starten
StartProtection(
	takeProfit: new Unit(50, UnitTypes.Absolute), // Take Profit
	stopLoss: new Unit(2, UnitTypes.Percent),     // Stop Loss in Prozent
	isStopTrailing: true,                         // Trailing Stop aktivieren
	useMarketOrders: true                         // Market-Orders verwenden
);
```

## Vorteile der High-Level-API

Die High-Level-API in StockSharp-Strategien bietet die folgenden Vorteile:

1. **Reduzierter Codeumfang** - häufige Aufgaben erfordern weniger Codezeilen.

2. **Trennung der Verantwortlichkeiten** - Handelslogik wird von technischen Details der Datenverarbeitung und Visualisierung getrennt.

3. **Verbesserte Lesbarkeit** - Code wird verständlicher und ausdrucksstärker, mit Fokus auf der Geschäftslogik.

4. **Geringere Fehlerwahrscheinlichkeit** - viele typische Fehler werden durch Automatisierung von Routineaufgaben eliminiert.

5. **Arbeit mit einfachen Datentypen** - statt mit komplexen Objekten können Sie mit einfachen Datentypen arbeiten, z. B. `decimal`.

## Beispielstrategie mit High-Level-API

Unten sehen Sie ein vollständiges Beispiel einer Strategie, das die Verwendung der High-Level-API demonstriert:

```cs
public class SmaStrategy : Strategy
{
	private bool? _isShortLessThenLong;

	public SmaStrategy()
	{
		_candleType = Param(nameof(CandleType), DataType.TimeFrame(TimeSpan.FromMinutes(1)));
		_long = Param(nameof(Long), 80);
		_short = Param(nameof(Short), 30);
		_takeValue = Param(nameof(TakeValue), new Unit(50, UnitTypes.Absolute));
		_stopValue = Param(nameof(StopValue), new Unit(2, UnitTypes.Percent));
	}

	private readonly StrategyParam<DataType> _candleType;
	public DataType CandleType
	{
		get => _candleType.Value;
		set => _candleType.Value = value;
	}

	private readonly StrategyParam<int> _long;
	public int Long
	{
		get => _long.Value;
		set => _long.Value = value;
	}

	private readonly StrategyParam<int> _short;
	public int Short
	{
		get => _short.Value;
		set => _short.Value = value;
	}

	private readonly StrategyParam<Unit> _takeValue;
	public Unit TakeValue
	{
		get => _takeValue.Value;
		set => _takeValue.Value = value;
	}

	private readonly StrategyParam<Unit> _stopValue;
	public Unit StopValue
	{
		get => _stopValue.Value;
		set => _stopValue.Value = value;
	}

	protected override void OnStarted2(DateTime time)
	{
		base.OnStarted2(time);

		// Indikatoren erstellen
		var longSma = new SMA { Length = Long };
		var shortSma = new SMA { Length = Short };

		// Kerzenabonnement erstellen und an Indikatoren binden
		var subscription = SubscribeCandles(CandleType);
		subscription
			.Bind(longSma, shortSma, OnProcess)
			.Start();

		// Visualisierung konfigurieren
		var area = CreateChartArea();
		if (area != null)
		{
			DrawCandles(area, subscription);
			DrawIndicator(area, shortSma, System.Drawing.Color.Coral);
			DrawIndicator(area, longSma);
			DrawOwnTrades(area);
		}

		// Positionsschutz starten
		StartProtection(TakeValue, StopValue);
	}

	private void OnProcess(ICandleMessage candle, decimal longValue, decimal shortValue)
	{
		// Nur abgeschlossene Kerzen verarbeiten
		if (candle.State != CandleStates.Finished)
			return;

		// Handelslogik auf Basis der Indikatorkreuzung
		var isShortLessThenLong = shortValue < longValue;

		if (_isShortLessThenLong == null)
		{
			_isShortLessThenLong = isShortLessThenLong;
		}
		else if (_isShortLessThenLong != isShortLessThenLong)
		{
			// Kreuzung ist aufgetreten
			var direction = isShortLessThenLong ? Sides.Sell : Sides.Buy;
			var volume = Position == 0 ? Volume : Position.Abs().Min(Volume) * 2;
			var priceStep = GetSecurity().PriceStep ?? 1;
			var price = candle.ClosePrice + (direction == Sides.Buy ? priceStep : -priceStep);

			// Order platzieren
			if (direction == Sides.Buy)
				BuyLimit(price, volume);
			else
				SellLimit(price, volume);

			// Aktuelle Indikatorposition speichern
			_isShortLessThenLong = isShortLessThenLong;
		}
	}
}
```

## Fazit

Die High-Level-API in StockSharp vereinfacht die Entwicklung von Handelsstrategien erheblich, da sich Entwickler auf die Handelslogik statt auf technische Details konzentrieren können. Sie ist besonders nützlich für typische Anwendungsfälle, in denen keine Feinabstimmung der Datenverarbeitung oder Visualisierung erforderlich ist.

In Kombination mit dem Strategieparametersystem, dem Ereignismodell und den Mechanismen zum Positionsschutz macht die High-Level-API StockSharp zu einem leistungsfähigen und bequemen Werkzeug für algorithmischen Handel, geeignet sowohl für Einsteiger als auch für erfahrene Entwickler.

