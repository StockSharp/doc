# Paarhandelsstrategie

## Überblick

`PairsTradingStrategy` ist eine Paarhandelsstrategie auf Basis statistischer Arbitrage zwischen zwei verwandten Instrumenten. Sie verfolgt den Spread zwischen den Preisen zweier Assets und eröffnet Positionen, wenn der Spread deutlich vom Mittelwert abweicht, in Erwartung einer Rückkehr zum Mittelwert.

## Hauptkomponenten

Die Strategie erbt von [Strategy](xref:StockSharp.Algo.Strategies.Strategy) und verwendet Parameter zur Konfiguration:

```cs
public class PairsTradingStrategy : Strategy
{
	private readonly StrategyParam<int> _spreadLength;
	private readonly StrategyParam<decimal> _entryThreshold;
	private readonly StrategyParam<decimal> _exitThreshold;
	private readonly StrategyParam<DataType> _candleType;

	// Letzte Preise für jedes Instrument
	private decimal? _lastPrice1;
	private decimal? _lastPrice2;
}
```

## Strategieparameter

Die Strategie erlaubt die Anpassung der folgenden Parameter:

- **SpreadLength** - Periode zur Berechnung des Spread-Mittelwerts und der Standardabweichung (Standardwert 20)
- **EntryThreshold** - Z-Score-Schwellenwert für den Positionseinstieg (Standardwert 2.0)
- **ExitThreshold** - Z-Score-Schwellenwert für den Positionsausstieg (Standardwert 0.5)
- **CandleType** - Kerzentyp, mit dem gearbeitet wird (standardmäßig 5 Minuten)

Alle Parameter stehen mit festgelegten Wertebereichen für die Optimierung zur Verfügung.

## Initialisierung der Strategie

In der Methode [OnStarted2](xref:StockSharp.Algo.Strategies.Strategy.OnStarted2(System.DateTime)) werden Indikatoren erstellt und Kerzenabonnements für zwei Instrumente eingerichtet:

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);

	// Zwei Instrumente für den Paarhandel abrufen
	var securities = GetWorkingSecurities().ToArray();
	if (securities.Length < 2)
		throw new InvalidOperationException("Zwei Instrumente müssen angegeben werden.");

	var sec1 = securities[0].sec;
	var sec2 = securities[1].sec;

	// Indikatoren zur Berechnung von Spread-Mittelwert und Standardabweichung
	var sma = new SimpleMovingAverage { Length = SpreadLength };
	var stdDev = new StandardDeviation { Length = SpreadLength };

	_lastPrice1 = null;
	_lastPrice2 = null;

	// Kerzen des ersten Instruments abonnieren
	SubscribeCandles(CandleType, security: sec1)
		.Bind(c =>
		{
			if (c.State != CandleStates.Finished)
				return;

			_lastPrice1 = c.ClosePrice;
		})
		.Start();

	// Kerzen des zweiten Instruments mit Spread-Verarbeitung abonnieren
	SubscribeCandles(CandleType, security: sec2)
		.Bind(c =>
		{
			if (c.State != CandleStates.Finished)
				return;

			_lastPrice2 = c.ClosePrice;

			if (_lastPrice1 == null)
				return;

			ProcessSpread(_lastPrice1.Value, _lastPrice2.Value, sma, stdDev);
		})
		.Start();
}
```

## Spread-Verarbeitung

Die Methode `ProcessSpread` berechnet den Z-Score des Spreads und erzeugt Handelssignale:

```cs
private void ProcessSpread(decimal price1, decimal price2,
	SimpleMovingAverage sma, StandardDeviation stdDev)
{
	// Spread als Preisdifferenz berechnen
	var spread = price1 - price2;

	// Indikatoren verarbeiten
	var smaValue = sma.Process(new DecimalIndicatorValue(sma, spread));
	var devValue = stdDev.Process(new DecimalIndicatorValue(stdDev, spread));

	if (!sma.IsFormed || !stdDev.IsFormed)
		return;

	if (!IsFormedAndOnlineAndAllowTrading())
		return;

	var mean = smaValue.ToDecimal();
	var dev = devValue.ToDecimal();

	if (dev == 0)
		return;

	// Z-Score berechnen: Spread-Abweichung vom Mittelwert in Einheiten der Standardabweichung
	var zScore = (spread - mean) / dev;

	// Spread zu hoch: erstes Instrument verkaufen, zweites kaufen
	if (zScore > EntryThreshold && Position >= 0)
	{
		SellMarket(Volume + Math.Abs(Position));
	}
	// Spread zu niedrig: erstes Instrument kaufen, zweites verkaufen
	else if (zScore < -EntryThreshold && Position <= 0)
	{
		BuyMarket(Volume + Math.Abs(Position));
	}
	// Rückkehr zum Mittelwert: Position schließen
	else if (Math.Abs(zScore) < ExitThreshold && Position != 0)
	{
		ClosePosition();
	}
}
```

## Handelslogik

- **Verkaufssignal**: Der Z-Score des Spreads überschreitet den Einstiegsschwellenwert (Standardwert 2.0), wenn keine Short-Position vorhanden ist.
- **Kaufsignal**: Der Z-Score des Spreads fällt unter den negativen Einstiegsschwellenwert (Standardwert -2.0), wenn keine Long-Position vorhanden ist.
- **Position schließen**: Der absolute Z-Score-Wert fällt unter den Ausstiegsschwellenwert (Standardwert 0.5) und signalisiert, dass der Spread zum Mittelwert zurückkehrt.

## Funktionen

- Die Strategie arbeitet mit zwei Instrumenten, die über die Methode `GetWorkingSecurities()` abgerufen werden.
- Der Spread wird als Differenz der Schlusskurse von Kerzen aus zwei Instrumenten berechnet.
- Der Z-Score wird verwendet, um die Spread-Abweichung vom Mittelwert zu normalisieren.
- Die Strategie implementiert das klassische Konzept der Rückkehr zum Mittelwert.
- Die Strategie arbeitet nur mit abgeschlossenen Kerzen.
- Parameteroptimierung wird unterstützt, um optimale Strategieeinstellungen zu finden.
