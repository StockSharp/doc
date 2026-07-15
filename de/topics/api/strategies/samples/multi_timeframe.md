# Multi-Zeitrahmen-Strategie

## Überblick

`MultiTimeframeStrategy` ist eine Strategie, die zwei Zeitrahmen für Handelsentscheidungen verwendet. Stundenkerzen bestimmen die Trendrichtung über Kreuzungen gleitender Durchschnitte, während 5-Minuten-Kerzen mit dem Indikator [RelativeStrengthIndex](xref:StockSharp.Algo.Indicators.RelativeStrengthIndex) für den präzisen Einstieg in Trendrichtung genutzt werden.

## Hauptkomponenten

Die Strategie erbt von [Strategy](xref:StockSharp.Algo.Strategies.Strategy) und verwendet Parameter zur Konfiguration:

```cs
public class MultiTimeframeStrategy : Strategy
{
	private readonly StrategyParam<int> _fastSmaLength;
	private readonly StrategyParam<int> _slowSmaLength;
	private readonly StrategyParam<int> _rsiLength;
	private readonly StrategyParam<decimal> _takeProfit;
	private readonly StrategyParam<decimal> _stopLoss;

	// Trendrichtung im höheren Zeitrahmen
	private Sides? _hourlyTrend;
}
```

## Strategieparameter

Die Strategie erlaubt die Anpassung der folgenden Parameter:

- **Zeitraum des schnellen gleitenden Durchschnitts** - Periode des schnellen gleitenden Durchschnitts für den Stundenchart (Standardwert 10)
- **Zeitraum des langsamen gleitenden Durchschnitts** - Periode des langsamen gleitenden Durchschnitts für den Stundenchart (Standardwert 30)
- **RSI-Zeitraum** - RSI-Periode für den 5-Minuten-Chart (Standardwert 14)
- **Gewinnmitnahme** - Take-Profit-Größe in Prozent (Standardwert 2)
- **Verlustbegrenzung** - Stop-Loss-Größe in Prozent (Standardwert 1)

Alle Parameter stehen mit festgelegten Wertebereichen für die Optimierung zur Verfügung.

## Initialisierung der Strategie

In der Methode [OnStarted2](xref:StockSharp.Algo.Strategies.Strategy.OnStarted2(System.DateTime)) werden Indikatoren erstellt und Kerzenabonnements für zwei Zeitrahmen eingerichtet:

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);

	var fastSma = new SimpleMovingAverage { Length = FastSmaLength };
	var slowSma = new SimpleMovingAverage { Length = SlowSmaLength };
	var rsi = new RelativeStrengthIndex { Length = RsiLength };

	_hourlyTrend = null;

	// Stundenkerzen zur Trenderkennung (SMA-Kreuzung)
	SubscribeCandles(TimeSpan.FromHours(1))
		.Bind(fastSma, slowSma, ProcessHourlyCandle)
		.Start();

	// 5-Minuten-Kerzen für präzisen Einstieg (RSI)
	SubscribeCandles(TimeSpan.FromMinutes(5))
		.Bind(rsi, ProcessEntryCandle)
		.Start();

	// Positionenschutz einrichten (Take-Profit und Stop-Loss)
	StartProtection(
		new Unit(TakeProfit, UnitTypes.Percent),
		new Unit(StopLoss, UnitTypes.Percent)
	);

	// Visualisierung im Chart einrichten
	var area = CreateChartArea();
	if (area != null)
	{
		DrawIndicator(area, fastSma, System.Drawing.Color.Blue);
		DrawIndicator(area, slowSma, System.Drawing.Color.Red);
		DrawOwnTrades(area);
	}
}
```

## Verarbeitung von Stundenkerzen

Die Methode `ProcessHourlyCandle` bestimmt die Trendrichtung im höheren Zeitrahmen:

```cs
private void ProcessHourlyCandle(ICandleMessage candle, decimal fastValue, decimal slowValue)
{
	if (candle.State != CandleStates.Finished)
		return;

	// Trend durch Kreuzung gleitender Durchschnitte bestimmen
	_hourlyTrend = fastValue > slowValue ? Sides.Buy : Sides.Sell;
}
```

## Verarbeitung von 5-Minuten-Kerzen

Die Methode `ProcessEntryCandle` implementiert den Positionseinstieg anhand des RSI-Signals in Trendrichtung:

```cs
private void ProcessEntryCandle(ICandleMessage candle, decimal rsiValue)
{
	if (candle.State != CandleStates.Finished)
		return;

	if (_hourlyTrend == null || !IsFormedAndOnlineAndAllowTrading())
		return;

	// Kaufen: Aufwärtstrend und RSI im überverkauften Bereich
	if (_hourlyTrend == Sides.Buy && rsiValue < 30 && Position <= 0)
	{
		BuyMarket(Volume + Math.Abs(Position));
	}
	// Verkaufen: Abwärtstrend und RSI im überkauften Bereich
	else if (_hourlyTrend == Sides.Sell && rsiValue > 70 && Position >= 0)
	{
		SellMarket(Volume + Math.Abs(Position));
	}
}
```

## Handelslogik

- **Trenderkennung**: Ein schneller SMA über dem langsamen SMA im Stundenchart zeigt einen Aufwärtstrend an, darunter einen Abwärtstrend.
- **Kaufsignal**: Aufwärtstrend im Stundenchart und RSI < 30 im 5-Minuten-Chart, wenn keine Long-Position vorhanden ist.
- **Verkaufssignal**: Abwärtstrend im Stundenchart und RSI > 70 im 5-Minuten-Chart, wenn keine Short-Position vorhanden ist.
- **Positionsschutz**: automatischer Take-Profit und Stop-Loss über `StartProtection`.

## Funktionen

- Die Strategie verwendet zwei Zeitrahmen: stündlich für den Trend und 5 Minuten für den Einstieg.
- Der Positionseinstieg erfolgt nur in Richtung des Trends im höheren Zeitrahmen.
- RSI wird als Filter verwendet, um optimale Einstiegspunkte zu finden (überverkauft/überkauft).
- Positionen werden automatisch mit Stop-Loss und Take-Profit geschützt.
- Die Strategie arbeitet nur mit abgeschlossenen Kerzen.
- Indikatoren und Trades werden im Chart visualisiert, wenn ein grafischer Bereich verfügbar ist.
- Parameteroptimierung wird unterstützt, um optimale Strategieeinstellungen zu finden.
