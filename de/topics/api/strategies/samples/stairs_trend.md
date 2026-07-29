# Stairs-Trendstrategie

## Überblick

`StairsTrendStrategy` ist eine Handelsstrategie auf Basis der Analyse aufeinanderfolgender Kerzen zur Trendbestimmung. Die Strategie eröffnet Positionen, wenn sich ein anhaltender Trend einer bestimmten Länge bildet.

## Hauptkomponenten

```cs
public class StairsTrendStrategy : Strategy
{
	private readonly StrategyParam<int> _lengthParam;
	private readonly StrategyParam<DataType> _candleType;

	private int _bullLength;
	private int _bearLength;
}
```

## Strategieparameter

Die Strategie erlaubt die Anpassung der folgenden Parameter:

- `Length` - Anzahl aufeinanderfolgender Kerzen in eine Richtung zur Erkennung eines Trends (Standardwert 3)
- `CandleType` - Kerzentyp, mit dem gearbeitet wird (standardmäßig 5 Minuten)

Der Parameter Length steht für die Optimierung im Bereich von 2 bis 10 mit einer Schrittweite von 1 zur Verfügung.

## Initialisierung der Strategie

In der Methode [OnStarted2](xref:StockSharp.Algo.Strategies.Strategy.OnStarted2(System.DateTime)) werden die Zähler zurückgesetzt, das Kerzenabonnement erstellt und die Visualisierung vorbereitet:

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);

	// Zähler zurücksetzen
	_bullLength = 0;
	_bearLength = 0;

	// Abonnement erstellen
	var subscription = SubscribeCandles(CandleType);

	subscription
		.Bind(ProcessCandle)
		.Start();

	// Visualisierung im Chart einrichten
	var area = CreateChartArea();
	if (area != null)
	{
		DrawCandles(area, subscription);
		DrawOwnTrades(area);
	}
}
```

## Verarbeitung von Kerzen

Die Methode `ProcessCandle` wird für jede abgeschlossene Kerze aufgerufen und implementiert die Handelslogik:

```cs
private void ProcessCandle(ICandleMessage candle)
{
	// Prüfen, ob die Kerze abgeschlossen ist
	if (candle.State != CandleStates.Finished)
		return;

	// Prüfen, ob die Strategie handelsbereit ist
	if (!IsFormedAndOnlineAndAllowTrading())
		return;

	// Zähler anhand der Kerzenrichtung aktualisieren
	if (candle.OpenPrice < candle.ClosePrice)
	{
		// Bullische Kerze
		_bullLength++;
		_bearLength = 0;
	}
	else if (candle.OpenPrice > candle.ClosePrice)
	{
		// Bärische Kerze
		_bullLength = 0;
		_bearLength++;
	}

	// Trendstrategie:
	// Nach Length aufeinanderfolgenden bullischen Kerzen kaufen
	if (_bullLength >= Length && Position <= 0)
	{
		BuyMarket(Volume + Math.Abs(Position));
	}
	// Nach Length aufeinanderfolgenden bärischen Kerzen verkaufen
	else if (_bearLength >= Length && Position >= 0)
	{
		SellMarket(Volume + Math.Abs(Position));
	}
}
```

## Handelslogik

- **Kaufsignal**: `Length` aufeinanderfolgende bullische Kerzen (Schlusskurs über Eröffnungskurs), wenn keine Long-Position vorhanden ist.
- **Verkaufssignal**: `Length` aufeinanderfolgende bärische Kerzen (Schlusskurs unter Eröffnungskurs), wenn keine Short-Position vorhanden ist.
- Das Positionsvolumen erhöht sich bei jedem neuen Trade um den Betrag der aktuellen Position.

## Funktionen

- Die Strategie bestimmt die zu verwendenden Instrumente automatisch über die Methode `GetWorkingSecurities()`.
- Die Strategie arbeitet nur mit abgeschlossenen Kerzen.
- Die Strategie verwendet Market-Orders für den Positionseinstieg.
- Die Strategie nutzt eine einfache Trenderkennung auf Basis einer Kerzensequenz.
- Kerzenzähler werden zurückgesetzt, wenn eine Kerze in Gegenrichtung erscheint.
- Kerzen und Trades werden im Chart visualisiert, wenn ein grafischer Bereich verfügbar ist.
- Die Optimierung der Sequenzlänge wird unterstützt, um optimale Strategieeinstellungen zu finden.
