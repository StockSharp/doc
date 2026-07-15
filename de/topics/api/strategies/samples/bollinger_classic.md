# Klassische Bollinger-Strategie

## Überblick

`BollingerStrategyClassicStrategy` ist eine Strategie auf Basis des Indikators [BollingerBands](xref:StockSharp.Algo.Indicators.BollingerBands). Sie eröffnet Positionen, wenn der Preis die obere oder untere Begrenzung der Bollinger-Bänder erreicht.

## Hauptkomponenten

Die Strategie erbt von [Strategy](xref:StockSharp.Algo.Strategies.Strategy) und verwendet Parameter zur Konfiguration:

```cs
public class BollingerStrategyClassicStrategy : Strategy
{
	private readonly StrategyParam<int> _bollingerLength;
	private readonly StrategyParam<decimal> _bollingerDeviation;
	private readonly StrategyParam<DataType> _candleType;

	private BollingerBands _bollingerBands;
}
```

## Strategieparameter

Die Strategie erlaubt die Anpassung der folgenden Parameter:

- **Bollinger-Länge** - Periode des Bollinger-Bänder-Indikators (Standardwert 20)
- **Bollinger-Abweichung** - Multiplikator der Standardabweichung (Standardwert 2.0)
- **Kerzentyp** - Kerzentyp, mit dem gearbeitet wird (standardmäßig 5 Minuten)

Alle Parameter stehen mit festgelegten Wertebereichen für die Optimierung zur Verfügung.

## Initialisierung der Strategie

In der Methode [OnStarted2](xref:StockSharp.Algo.Strategies.Strategy.OnStarted2(System.DateTime)) wird der Bollinger-Bänder-Indikator erstellt, das Kerzenabonnement eingerichtet und die Visualisierung vorbereitet:

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);

	// Indikator erstellen
	_bollingerBands = new BollingerBands
	{
		Length = BollingerLength,
		Width = BollingerDeviation
	};

	// Abonnement erstellen und Indikator binden
	var subscription = SubscribeCandles(CandleType);
	subscription
		.BindEx(_bollingerBands, ProcessCandle)
		.Start();

	// Visualisierung im Chart einrichten
	var area = CreateChartArea();
	if (area != null)
	{
		DrawCandles(area, subscription);
		DrawIndicator(area, _bollingerBands, System.Drawing.Color.Purple);
		DrawOwnTrades(area);
	}
}
```

## Verarbeitung von Kerzen

Die Methode `ProcessCandle` wird für jede abgeschlossene Kerze aufgerufen und implementiert die Handelslogik:

```cs
private void ProcessCandle(ICandleMessage candle, IIndicatorValue bollingerValue)
{
	// Unvollständige Kerzen überspringen
	if (candle.State != CandleStates.Finished)
		return;

	// Prüfen, ob die Strategie handelsbereit ist
	if (!IsFormedAndOnlineAndAllowTrading())
		return;

	var typed = (BollingerBandsValue)bollingerValue;

	// Handelslogik:
	// Verkaufen, wenn der Preis das obere Band erreicht oder überschreitet
	if (candle.ClosePrice >= typed.UpBand && Position >= 0)
	{
		SellMarket(Volume + Math.Abs(Position));
	}
	// Kaufen, wenn der Preis das untere Band erreicht oder darunter fällt
	else if (candle.ClosePrice <= typed.LowBand && Position <= 0)
	{
		BuyMarket(Volume + Math.Abs(Position));
	}
}
```

## Handelslogik

- **Verkaufssignal**: Der Schlusskurs der Kerze erreicht oder überschreitet das obere Bollinger-Band, wenn keine Short-Position vorhanden ist.
- **Kaufsignal**: Der Schlusskurs der Kerze erreicht das untere Bollinger-Band oder fällt darunter, wenn keine Long-Position vorhanden ist.
- Das Positionsvolumen erhöht sich bei jedem neuen Trade um den Betrag der aktuellen Position.

## Funktionen

- Die Strategie bestimmt die zu verwendenden Instrumente automatisch über die Methode `GetWorkingSecurities()`.
- Die Strategie arbeitet nur mit abgeschlossenen Kerzen.
- Indikator und Trades werden in einem Chart visualisiert, wenn ein grafischer Bereich verfügbar ist.
- Parameteroptimierung wird unterstützt, um optimale Strategieeinstellungen zu finden.
