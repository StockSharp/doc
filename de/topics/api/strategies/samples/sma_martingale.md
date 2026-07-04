# Gleitende Durchschnitte mit Martingale

## Überblick

`SmaStrategyMartingaleStrategy` ist eine Handelsstrategie auf Basis der Kreuzung zweier einfacher gleitender Durchschnitte ([SimpleMovingAverage](xref:StockSharp.Algo.Indicators.SimpleMovingAverage)) mit Martingale-Elementen. Die Strategie verwendet lange und kurze SMAs zur Bestimmung von Ein- und Ausstiegssignalen und erhöht die Positionsgröße bei jedem neuen Trade.

## Hauptkomponenten

```cs
public class SmaStrategyMartingaleStrategy : Strategy
{
	private readonly StrategyParam<int> _longSmaLength;
	private readonly StrategyParam<int> _shortSmaLength;
	private readonly StrategyParam<DataType> _candleType;

	// Variablen zum Speichern vorheriger Indikatorwerte
	private decimal _prevLongValue;
	private decimal _prevShortValue;
	private bool _isFirstValue = true;
}
```

## Strategieparameter

Die Strategie erlaubt die Anpassung der folgenden Parameter:

- **LongSmaLength** - Periode des langen gleitenden Durchschnitts (Standardwert 80)
- **ShortSmaLength** - Periode des kurzen gleitenden Durchschnitts (Standardwert 30)
- **CandleType** - Kerzentyp, mit dem gearbeitet wird (standardmäßig 5 Minuten)

Alle Parameter stehen mit festgelegten Wertebereichen für die Optimierung zur Verfügung.

## Initialisierung der Strategie

In der Methode [OnStarted2](xref:StockSharp.Algo.Strategies.Strategy.OnStarted2(System.DateTime)) werden SMA-Indikatoren erstellt, das Kerzenabonnement eingerichtet und die Visualisierung vorbereitet:

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);

	// Indikatoren erstellen
	var longSma = new SimpleMovingAverage { Length = LongSmaLength };
	var shortSma = new SimpleMovingAverage { Length = ShortSmaLength };

	// Indikatoren zur Strategie-Sammlung hinzufügen, damit IsFormed automatisch verfolgt wird
	Indicators.Add(longSma);
	Indicators.Add(shortSma);

	// Abonnement erstellen und Indikatoren binden
	var subscription = SubscribeCandles(CandleType);
	subscription
		.Bind(longSma, shortSma, ProcessCandle)
		.Start();

	// Visualisierung im Chart einrichten
	var area = CreateChartArea();
	if (area != null)
	{
		DrawCandles(area, subscription);
		DrawIndicator(area, longSma, System.Drawing.Color.Blue);
		DrawIndicator(area, shortSma, System.Drawing.Color.Red);
		DrawOwnTrades(area);
	}
}
```

## Verarbeitung von Kerzen

Die Methode `ProcessCandle` wird für jede abgeschlossene Kerze aufgerufen und implementiert die Handelslogik:

```cs
private void ProcessCandle(ICandleMessage candle, decimal longValue, decimal shortValue)
{
	// Unvollständige Kerzen überspringen
	if (candle.State != CandleStates.Finished)
		return;

	// Prüfen, ob die Strategie handelsbereit ist
	if (!IsFormedAndOnlineAndAllowTrading())
		return;

	// Beim ersten Wert nur Daten speichern, ohne Signale zu erzeugen
	if (_isFirstValue)
	{
		_prevLongValue = longValue;
		_prevShortValue = shortValue;
		_isFirstValue = false;
		return;
	}

	// Aktuellen und vorherigen Vergleich der Indikatorwerte abrufen
	var isShortLessThenLongCurrent = shortValue < longValue;
	var isShortLessThenLongPrevious = _prevShortValue < _prevLongValue;

	// Aktuelle Werte als vorherige für die nächste Kerze speichern
	_prevLongValue = longValue;
	_prevShortValue = shortValue;

	// Auf Kreuzung prüfen (Signal)
	if (isShortLessThenLongPrevious == isShortLessThenLongCurrent)
		return;

	// Aktive Orders vor dem Platzieren neuer Orders stornieren
	CancelActiveOrders();

	// Handelsrichtung bestimmen
	var direction = isShortLessThenLongCurrent ? Sides.Sell : Sides.Buy;

	// Positionsgröße berechnen (Position mit jedem Trade erhöhen - Martingale-Ansatz)
	var volume = Volume + Math.Abs(Position);

	// Order mit dem passenden Preis erstellen und registrieren
	var price = Security.ShrinkPrice(shortValue);
	RegisterOrder(CreateOrder(direction, price, volume));
}
```

## Handelslogik

- **Kaufsignal**: Der kurze SMA kreuzt den langen SMA von unten.
- **Verkaufssignal**: Der kurze SMA kreuzt den langen SMA von oben.
- Die Positionsgröße erhöht sich bei jedem neuen Trade um den Betrag der aktuellen Position (Martingale-Element).
- Der Orderpreis wird auf den aktuellen Wert des kurzen SMA gesetzt und auf die Tickgröße des Instruments gerundet.

## Funktionen

- Die Strategie bestimmt die zu verwendenden Instrumente automatisch über die Methode `GetWorkingSecurities()`.
- Die Strategie arbeitet nur mit abgeschlossenen Kerzen.
- Die Strategie verfolgt Indikatorkreuzungen, indem sie das aktuelle und vorherige Verhältnis zwischen SMAs vergleicht.
- Alle aktiven Orders werden vor dem Platzieren neuer Orders storniert.
- Das Martingale-Prinzip wird umgesetzt: Die Positionsgröße wird bei jedem neuen Trade erhöht.
- Indikatoren und Trades werden im Chart visualisiert, wenn ein grafischer Bereich verfügbar ist.
- Parameteroptimierung wird unterstützt, um optimale Strategieeinstellungen zu finden.
