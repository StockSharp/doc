# Bollinger-Strategie mit Fokus auf das obere Band

## Überblick

`BollingerStrategyUpBandStrategy` ist eine Strategie auf Basis des Indikators [BollingerBands](xref:StockSharp.Algo.Indicators.BollingerBands). Sie eröffnet eine Long-Position, wenn der Preis die obere Begrenzung der Bollinger-Bänder erreicht, und schließt sie, wenn der Preis die Mittellinie erreicht.

## Hauptkomponenten

Die Strategie erbt von [Strategy](xref:StockSharp.Algo.Strategies.Strategy) und verwendet Parameter zur Konfiguration:

```cs
public class BollingerStrategyUpBandStrategy : Strategy
{
	private readonly StrategyParam<int> _bollingerLength;
	private readonly StrategyParam<decimal> _bollingerDeviation;
	private readonly StrategyParam<DataType> _candleType;

	private BollingerBands _bollingerBands;
}
```

## Strategieparameter

Die Strategie erlaubt die Anpassung der folgenden Parameter:

- **BollingerLength** - Periode des Bollinger-Bänder-Indikators (Standardwert 20)
- **BollingerDeviation** - Multiplikator der Standardabweichung (Standardwert 2.0)
- **CandleType** - Kerzentyp, mit dem gearbeitet wird (standardmäßig 5 Minuten)

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
	// Kaufen, wenn der Preis das obere Band berührt (nur wenn keine Position besteht)
	if (candle.ClosePrice >= typed.UpBand && Position == 0)
	{
		BuyMarket(Volume);
	}
	// Verkaufen, um die Position zu schließen, wenn der Preis die Mittellinie erreicht (nur bei Long-Position)
	else if (candle.ClosePrice <= typed.MiddleBand && Position > 0)
	{
		SellMarket(Math.Abs(Position));
	}
}
```

## Handelslogik

- **Kaufsignal**: Der Schlusskurs der Kerze erreicht oder überschreitet das obere Bollinger-Band, wenn keine offene Position vorhanden ist.
- **Verkaufssignal** (Schließen der Long-Position): Der Schlusskurs der Kerze erreicht die Mittellinie der Bollinger-Bänder oder fällt darunter, wenn eine Long-Position vorhanden ist.
- Das Positionsvolumen ist beim Öffnen fest und entspricht beim Schließen der gesamten aktuellen Position.

## Funktionen

- Die Strategie bestimmt die zu verwendenden Instrumente automatisch über die Methode `GetWorkingSecurities()`.
- Die Strategie arbeitet nur mit abgeschlossenen Kerzen.
- Die Strategie verwendet nur das obere Band und die Mittellinie des Bollinger-Bänder-Indikators.
- Es werden nur Long-Positionen eröffnet.
- Indikator und Trades werden in einem Chart visualisiert, wenn ein grafischer Bereich verfügbar ist.
- Parameteroptimierung wird unterstützt, um optimale Strategieeinstellungen zu finden.
