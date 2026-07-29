# One-Kerze-Countertrend-Strategie

## Überblick

`OneCandleCountertrendStrategy` ist eine einfache Gegentrendstrategie, die Entscheidungen auf Basis der Analyse einer einzelnen Kerze trifft.

## Hauptkomponenten

```cs
public class OneCandleCountertrendStrategy : Strategy
{
	private readonly StrategyParam<DataType> _candleType;
}
```

## Strategieparameter

Die Strategie erlaubt die Anpassung der folgenden Parameter:

- `CandleType` - Kerzentyp, mit dem gearbeitet wird (standardmäßig 5 Minuten)

## Initialisierung der Strategie

In der Methode [OnStarted2](xref:StockSharp.Algo.Strategies.Strategy.OnStarted2(System.DateTime)) wird das Kerzenabonnement erstellt und die Visualisierung vorbereitet:

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);

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

	// Gegentrendstrategie: bei bärischer Kerze kaufen, bei bullischer Kerze verkaufen
	if (candle.OpenPrice < candle.ClosePrice && Position >= 0)
	{
		// Bullische Kerze - verkaufen
		SellMarket(Volume + Math.Abs(Position));
	}
	else if (candle.OpenPrice > candle.ClosePrice && Position <= 0)
	{
		// Bärische Kerze - kaufen
		BuyMarket(Volume + Math.Abs(Position));
	}
}
```

## Handelslogik

- **Verkaufssignal**: bullische Kerze (Schlusskurs über Eröffnungskurs), wenn keine Short-Position vorhanden ist.
- **Kaufsignal**: bärische Kerze (Schlusskurs unter Eröffnungskurs), wenn keine Long-Position vorhanden ist.
- Das Positionsvolumen erhöht sich bei jedem neuen Trade um den Betrag der aktuellen Position.

## Funktionen

- Die Strategie bestimmt die zu verwendenden Instrumente automatisch über die Methode `GetWorkingSecurities()`.
- Die Strategie arbeitet nur mit abgeschlossenen Kerzen.
- Die Strategie verwendet Market-Orders für den Positionseinstieg.
- Die Strategie nutzt eine einfache Gegentrenderkennung auf Basis einer einzelnen Kerze.
- Kerzen und Trades werden im Chart visualisiert, wenn ein grafischer Bereich verfügbar ist.
