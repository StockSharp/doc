# Gegentrendstrategie mit Quoting

## Überblick

`StairsCountertrendStrategy` ist eine Gegentrend-Handelsstrategie, die Positionen gegen einen etablierten Trend einer bestimmten Länge eröffnet und dafür einen Quoting-Mechanismus für einen präziseren Markteinstieg nutzt.

## Hauptkomponenten

```cs
public class StairsCountertrendStrategy : Strategy
{
	private readonly StrategyParam<DataType> _candleDataType;
	private readonly StrategyParam<int> _length;
	private QuotingProcessor _quotingProcessor;

	private int _bullLength;
	private int _bearLength;
}
```

## Strategieparameter

Die Strategie erlaubt die Anpassung der folgenden Parameter:

- **CandleDataType** - Kerzentyp, mit dem gearbeitet wird (standardmäßig 1 Minute)
- `Length` - Anzahl aufeinanderfolgender Kerzen in eine Richtung zur Erkennung eines Trends (Standardwert 5)

Der Parameter Length steht für die Optimierung im Bereich von 2 bis 10 mit einer Schrittweite von 1 zur Verfügung.

## Initialisierung der Strategie

In der Methode [OnStarted2](xref:StockSharp.Algo.Strategies.Strategy.OnStarted2(System.DateTime)) werden die Zähler zurückgesetzt, das Kerzenabonnement erstellt und die Visualisierung vorbereitet:

```cs
protected override void OnStarted2(DateTime time)
{
	// Zähler beim Start zurücksetzen
	_bullLength = 0;
	_bearLength = 0;

	// Kerzenabonnement erstellen
	var subscription = SubscribeCandles(CandleDataType);

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

	base.OnStarted2(time);
}
```

## Verarbeitung von Kerzen

Die Methode `ProcessCandle` wird für jede abgeschlossene Kerze aufgerufen und implementiert die Logik zur Trenderkennung sowie zur Verwaltung des Quoting-Prozessors:

```cs
private void ProcessCandle(ICandleMessage candle)
{
	if (candle.State != CandleStates.Finished)
		return;

	// Bullische oder bärische Kerze erkennen
	if (candle.OpenPrice < candle.ClosePrice)
	{
		_bullLength++;
		_bearLength = 0;

		this.AddInfoLog($"Bullische Kerze erkannt. Serie: {_bullLength}");
	}
	else if (candle.OpenPrice > candle.ClosePrice)
	{
		_bullLength = 0;
		_bearLength++;

		this.AddInfoLog($"Bärische Kerze erkannt. Serie: {_bearLength}");
	}

	// Vorhandenen Prozessor stoppen, wenn ein Richtungswechsel erforderlich ist
	if (_quotingProcessor != null)
	{
		// Prüfen, ob der Prozessor bereinigt werden muss (Trendwechsel oder Position)
		var shouldClearProcessor = false;

		// Verkauf erforderlich, wenn bullischer Trend und keine Short-Position
		if (_bullLength >= Length && Position >= 0)
			shouldClearProcessor = true;
		// Kauf erforderlich, wenn bärischer Trend und keine Long-Position
		else if (_bearLength >= Length && Position <= 0)
			shouldClearProcessor = true;

		if (shouldClearProcessor)
		{
			_quotingProcessor?.Dispose();
			_quotingProcessor = null;
		}
	}

	// Bei Bedarf neuen Quoting-Prozessor erstellen
	if (_quotingProcessor == null && IsFormedAndOnlineAndAllowTrading())
	{
		if (_bullLength >= Length && Position >= 0)
		{
			// Bullischer Trend - Short-Position eröffnen
			CreateQuotingProcessor(Sides.Sell);
			this.AddInfoLog($"Starte Verkaufs-Quoting nach {_bullLength} bullischen Kerzen");
		}
		else if (_bearLength >= Length && Position <= 0)
		{
			// Bärischer Trend - Long-Position eröffnen
			CreateQuotingProcessor(Sides.Buy);
			this.AddInfoLog($"Starte Kauf-Quoting nach {_bearLength} bärischen Kerzen");
		}
	}
}
```

## Erstellung des Quoting-Prozessors

Die Methode `CreateQuotingProcessor` erstellt einen Quoting-Prozessor mit der angegebenen Richtung:

```cs
private void CreateQuotingProcessor(Sides side)
{
	// Verhalten für Market Quoting erstellen
	var behavior = new MarketQuotingBehavior(
		0, // Kein Preisabstand
		new Unit(0.1m, UnitTypes.Percent), // 0.1 % als Mindestabweichung verwenden
		MarketPriceTypes.Following // Marktpreis folgen
	);

	// Quoting-Prozessor erstellen
	_quotingProcessor = new(
		behavior,
		Security,
		Portfolio,
		side,
		Volume, // Quoting-Volumen
		Volume, // Maximales Ordervolumen
		TimeSpan.Zero, // Kein Timeout
		this, // Strategy implementiert ISubscriptionProvider
		this, // Strategy implementiert IMarketRuleContainer
		this, // Strategy implementiert ITransactionProvider
		this, // Strategy implementiert ITimeProvider
		this, // Strategy implementiert IMarketDataProvider
		IsFormedAndOnlineAndAllowTrading, // Handelsberechtigung prüfen
		true, // Orderbuchpreise verwenden
		true // Letzten Trade-Preis verwenden, wenn Orderbuch leer ist
	)
	{
		Parent = this
	};

	// Prozessorereignisse abonnieren
	_quotingProcessor.OrderRegistered += order =>
		this.AddInfoLog($"Order {order.TransactionId} zum Preis {order.Price} registriert");

	_quotingProcessor.OrderFailed += fail =>
		this.AddInfoLog($"Order fehlgeschlagen: {fail.Error.Message}");

	_quotingProcessor.OwnTrade += trade =>
		this.AddInfoLog($"Trade ausgeführt: {trade.Trade.Volume} zu {trade.Trade.Price}");

	_quotingProcessor.Finished += isOk =>
	{
		_quotingProcessor?.Dispose();
		_quotingProcessor = null;
	};

	// Prozessor initialisieren
	_quotingProcessor.Start();
}
```

## Handelslogik

- **Verkaufssignal**: `Length` aufeinanderfolgende bullische Kerzen (Schlusskurs über Eröffnungskurs), wenn keine Short-Position vorhanden ist.
- **Kaufsignal**: `Length` aufeinanderfolgende bärische Kerzen (Schlusskurs unter Eröffnungskurs), wenn keine Long-Position vorhanden ist.
- Der Quoting-Prozessor wird für den Markteinstieg verwendet und folgt dem Marktpreis.

## Funktionen

- Die Strategie bestimmt die zu verwendenden Instrumente automatisch über die Methode `GetWorkingSecurities()`.
- Die Strategie arbeitet nur mit abgeschlossenen Kerzen.
- Für einen effizienteren Markteinstieg wird Quoting statt Market-Orders verwendet.
- Die Strategie nutzt einen Gegentrendansatz und eröffnet Positionen gegen den etablierten Trend.
- Detaillierte Protokollierung der wichtigsten Ereignisse für das Debugging ist implementiert.
- Der Quoting-Prozessor wird automatisch bereinigt, wenn sich die Trendrichtung ändert oder Ziele erreicht werden.
- Die Visualisierung von Kerzen und Trades im Chart wird unterstützt.
- Die Optimierung des Parameters für die Sequenzlänge ist für die Strategiekonfiguration implementiert.
