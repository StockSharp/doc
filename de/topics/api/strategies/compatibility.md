# Strategiekompatibilität mit StockSharp-Plattformen

Bei der Entwicklung von Handelsstrategien in StockSharp ist es wichtig, ihre Kompatibilität mit verschiedenen Plattformen zu berücksichtigen: [Designer](../../designer.md), [Shell](../../shell.md), [Runner](../../runner.md) und [Cloud-Rücktests](../../designer/backtesting/cloud_backtesting.md). Wenn Sie die folgenden Empfehlungen beachten, erstellen Sie eine Strategie, die in allen Umgebungen korrekt funktioniert.

## Parameter im Strategiekonstruktor

### Parameter im Konstruktor vermeiden

Um die Kompatibilität mit StockSharp-Plattformen sicherzustellen, insbesondere mit Cloud-Rücktests, **sollten Sie dem Strategiekonstruktor keine Parameter hinzufügen**:

```cs
// Korrekt: Konstruktor ohne Parameter
public class SmaStrategy : Strategy
{
	public SmaStrategy()
	{
		// Parameterinitialisierung
	}
}

// Falsch: Konstruktor mit Parametern
public class SmaStrategy : Strategy
{
	public SmaStrategy(int longLength, int shortLength) // Diesen Ansatz nicht verwenden
	{
		// ...
	}
}
```

StockSharp-Plattformen erstellen Strategieinstanzen mit einem parameterlosen Konstruktor. Wenn Ihre Strategie einen Konstruktor mit Parametern erfordert, wird sie nicht korrekt initialisiert.

## StrategyParam statt normaler Eigenschaften verwenden

### Vorteile von StrategyParam

Statt normale C#-Eigenschaften zu erstellen und anschließend die Methoden `Save` und `Load` zu überschreiben, verwenden Sie [StrategyParam\<T\>](xref:StockSharp.Algo.Strategies.StrategyParam`1) für alle anpassbaren Parameter:

```cs
// Korrekt: Verwendung von StrategyParam
private readonly StrategyParam<int> _longSmaLength;

public int LongSmaLength
{
	get => _longSmaLength.Value;
	set => _longSmaLength.Value = value;
}

public SmaStrategy()
{
	_longSmaLength = Param(nameof(LongSmaLength), 80)
						.SetDisplay("Länge der langen SMA", string.Empty, "Grundeinstellungen");
}

// Falsch: Verwendung normaler Eigenschaften
private int _longSmaLength = 80; // Diesen Ansatz nicht verwenden

public int LongSmaLength
{
	get => _longSmaLength;
	set => _longSmaLength = value;
}
```

Über [StrategyParam\<T\>](xref:StockSharp.Algo.Strategies.StrategyParam`1) erstellte Parameter werden automatisch:
- In den Benutzeroberflächen der Plattformen angezeigt
- Ohne Überschreiben der Methoden `Save` und `Load` gespeichert und geladen
- In der Optimierung verwendet
- Beim Senden an Cloud-Rücktests korrekt serialisiert

## Arbeit mit der Benutzeroberfläche

### Abstraktionen statt direktem UI-Zugriff verwenden

Statt direkt auf Elemente der Benutzeroberfläche zuzugreifen, verwenden Sie die von StockSharp bereitgestellten Abstraktionen:

```cs
// Korrekter Ansatz: Verwendung von IChart
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);

	// Den von der Laufzeitumgebung bereitgestellten Chart abrufen
	_chart = GetChart();

	if (_chart != null)
	{
		// Chart ist verfügbar (z. B. im Designer oder in Shell)
		InitChart();
	}
	else
	{
		// Chart ist nicht verfügbar (z. B. in Runner oder Cloud-Rücktests)
		// Strategie arbeitet ohne Visualisierung weiter
	}
}

private void InitChart()
{
	// Chart über die abstrakte Schnittstelle konfigurieren
	_chart.ClearAreas();
	var area = _chart.AddArea();
	_chartCandleElement = area.AddCandles();
	// ...
}
```

Die Methode [Strategy.GetChart()](xref:StockSharp.Algo.Strategies.Strategy.GetChart) gibt eine [IChart](xref:StockSharp.Charting.IChart)-Schnittstelle zurück, wenn in der aktuellen Laufzeitumgebung ein Chart verfügbar ist. Wenn die Strategie im Konsolen-[Runner](../../runner.md) oder in Cloud-Rücktests ausgeführt wird, wo keine grafische Oberfläche vorhanden ist, gibt die Methode `null` zurück.

Die Schnittstelle [IChart](xref:StockSharp.Charting.IChart) stellt Methoden zur Arbeit mit Charts bereit:
- [AddArea](xref:StockSharp.Charting.IChart.AddArea(StockSharp.Charting.IChartArea)) - zum Hinzufügen eines Bereichs zum Chart
- [RemoveArea](xref:StockSharp.Charting.IChart.RemoveArea(StockSharp.Charting.IChartArea)) - zum Entfernen eines Bereichs
- [AddElement](xref:StockSharp.Charting.IChart.AddElement(StockSharp.Charting.IChartArea,StockSharp.Charting.IChartElement)) - zum Hinzufügen eines Elements zum Chart
- [RemoveElement](xref:StockSharp.Charting.IChart.RemoveElement(StockSharp.Charting.IChartArea,StockSharp.Charting.IChartElement)) - zum Entfernen eines Elements
- [Reset](xref:StockSharp.Charting.IChart.Reset(System.Collections.Generic.IEnumerable{StockSharp.Charting.IChartElement})) - zum Zurücksetzen von Elementwerten

### Chartverfügbarkeit prüfen

Prüfen Sie immer die Chartverfügbarkeit, bevor Sie den Chart verwenden:

```cs
private void DrawCandlesAndIndicators(ICandleMessage candle, IIndicatorValue longSma, IIndicatorValue shortSma)
{
	if (_chart == null) return; // Wichtige Prüfung

	var data = _chart.CreateData();
	data.Group(candle.OpenTime)
		.Add(_chartCandleElement, candle)
		.Add(_longSmaIndicatorElement, longSma)
		.Add(_shortSmaIndicatorElement, shortSma);
	_chart.Draw(data);
}
```

## Threads und Synchronisierung

### Zusätzliche Threads vermeiden

In StockSharp **müssen Sie keine zusätzlichen Threads** für die Datenverarbeitung erstellen. Alle Ereignisse (Marktdaten, Transaktionen) kommen in einem einzigen Thread an:

```cs
// Korrekt: Verwendung standardmäßiger Ereignishandler
private void ProcessCandle(ICandleMessage candle)
{
	// Kerze im Hauptthread verarbeiten
	var longSmaIsFormedPrev = _longSma.IsFormed;
	var ls = _longSma.Process(candle);
	var ss = _shortSma.Process(candle);

	// ...
}

// Falsch: zusätzliche Threads erstellen
private void ProcessCandle(ICandleMessage candle)
{
	// Nicht so vorgehen
	Task.Run(() => {
		var longSmaIsFormedPrev = _longSma.IsFormed;
		// ...
	});
}
```

### Synchronisationsobjekte vermeiden

Da alle Ereignisse in einem einzigen Thread verarbeitet werden, **müssen keine Synchronisationsobjekte verwendet werden**:

```cs
// Korrekt: normale Verarbeitung ohne Synchronisierung
private void ProcessCandle(ICandleMessage candle)
{
	var ls = _longSma.Process(candle);
	var ss = _shortSma.Process(candle);
	// ...
}

// Falsch: unnötige Synchronisierung
private readonly object _syncLock = new object(); // Nicht erforderlich

private void ProcessCandle(ICandleMessage candle)
{
	lock (_syncLock) // Nicht erforderlich
	{
		var ls = _longSma.Process(candle);
		// ...
	}
}
```

## Externe Ressourcen

### StockSharp-Infrastruktur verwenden

Statt direkt auf externe Ressourcen wie Dateien, Datenbanken oder Netzwerk zuzugreifen, verwenden Sie die von den StockSharp-Plattformen bereitgestellten Möglichkeiten:

```cs
// Korrekt: integrierte Mechanismen zur Datenspeicherung verwenden
protected override void OnStopped()
{
	// Daten werden automatisch über Strategieparameter gespeichert
	base.OnStopped();
}

// Falsch: direkter Zugriff auf externe Ressourcen
protected override void OnStopped()
{
	// Nicht so vorgehen
	File.WriteAllText("results.txt", $"PnL: {PnL}");

	// oder so
	using (var connection = new SqlConnection("..."))
	{
		// ...
	}

	base.OnStopped();
}
```

### Datenspeicherung

Zum Speichern von Strategieergebnissen verwenden Sie:

- [Strategieparameter](parameters.md) für Einstellungen und Konfiguration
- Integrierte Speichermechanismen im [Designer](../../designer.md) und in [Shell](../../shell.md)
- [Statistics](xref:StockSharp.Algo.Statistics.StatisticManager) zum Sammeln von Handelskennzahlen

### Speicher- und Lademethoden

Die Methoden [Strategy.Save](xref:StockSharp.Algo.Strategies.Strategy.Save(Ecng.Serialization.SettingsStorage)) und [Strategy.Load](xref:StockSharp.Algo.Strategies.Strategy.Load(Ecng.Serialization.SettingsStorage)) sind speziell zum Speichern zusätzlicher Strategiedaten vorgesehen, die keine Einstellungen oder Parameter sind. Dies ist der ideale Ort, um Daten zu speichern, die zur Wiederherstellung des Strategiezustands benötigt werden:

```cs
public override void Save(SettingsStorage settings)
{
	base.Save(settings); // Zuerst Strategieparameter speichern

	// Dann benutzerdefinierte Daten speichern
	settings.SetValue("CustomState", _customState);
	settings.SetValue("LastSignalTime", _lastSignalTime);
}

public override void Load(SettingsStorage settings)
{
	base.Load(settings); // Zuerst Strategieparameter laden

	// Dann benutzerdefinierte Daten laden
	if (settings.Contains("CustomState"))
		_customState = settings.GetValue<string>("CustomState");

	if (settings.Contains("LastSignalTime"))
		_lastSignalTime = settings.GetValue<DateTimeOffset>("LastSignalTime");
}
```

Die wichtigsten konfigurierbaren Parameter sollten jedoch weiterhin über [StrategyParam\<T\>](xref:StockSharp.Algo.Strategies.StrategyParam`1) implementiert werden, wie oben beschrieben, da dadurch ihre automatische Anzeige in der Benutzeroberfläche sichergestellt wird.

## Marktdatenabonnement

### Regeln statt direktem Abonnement verwenden

Für die Verarbeitung von Marktdaten wird empfohlen, das [Ereignismodell](event_model.md) und Regeln zu verwenden:

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);

	_shortSma = new SimpleMovingAverage { Length = ShortSmaLength };
	_longSma = new SimpleMovingAverage { Length = LongSmaLength };

	Indicators.Add(_shortSma);
	Indicators.Add(_longSma);

	var subscription = new Subscription(Series, Security);

	// Korrekt: Regeln für die Datenverarbeitung verwenden
	Connector
		.WhenCandlesFinished(subscription)
		.Do(ProcessCandle)
		.Apply(this);

	Subscribe(subscription);
}
```

Regeln haben gegenüber normalen Ereignishandlern mehrere wichtige Vorteile:

1. **Automatisches Abmelden** - Regeln melden sich automatisch von Ereignissen ab, wenn die Strategie stoppt oder wenn sie nicht mehr benötigt werden. Sie müssen Abonnements nicht manuell verwalten.

2. **API auf höherer Ebene** - Regeln bieten eine verständlichere und bequemere Schnittstelle als standardmäßige Ereignishandler. Beispielsweise ist `WhenCandlesFinished` deutlich klarer als das Abonnieren des Ereignisses `CandleReceived` mit anschließender Prüfung des Kerzenzustands.

3. **Kombinieren von Bedingungen** - Regeln können mit Operatoren wie `And`, `Or` und anderen kombiniert werden, um komplexe Aktivierungsbedingungen zu erstellen:

```cs
// Beispiel für das Kombinieren von Regeln
var tickSub = new Subscription(DataType.Ticks, Security);

tickSub
	.WhenTickTradeReceived(this)
	.And(Portfolio.WhenChanged(Connector))
	.Do(() => {
		// Code, der nur ausgeführt wird, wenn es einen neuen Trade gibt
		// und sich der Portfoliosaldo ändert
	})
	.Apply(this);

Subscribe(tickSub);
```

4. **Lebenszyklusverwaltung** - Regeln können einmalig gemacht werden (`Once()`), Stornierungsbedingungen erhalten (`Until()`), verzögerte Aktionen hinzufügen usw.

## Beispiel einer kompatiblen Strategie

Unten sehen Sie ein Beispiel für eine Strategie, die alle Empfehlungen befolgt und auf allen StockSharp-Plattformen korrekt funktioniert:

```cs
public class SmaStrategy : Strategy
{
	private readonly StrategyParam<DataType> _series;
	private readonly StrategyParam<int> _longSmaLength;
	private readonly StrategyParam<int> _shortSmaLength;

	public DataType Series
	{
		get => _series.Value;
		set => _series.Value = value;
	}

	public int LongSmaLength
	{
		get => _longSmaLength.Value;
		set => _longSmaLength.Value = value;
	}

	public int ShortSmaLength
	{
		get => _shortSmaLength.Value;
		set => _shortSmaLength.Value = value;
	}

	private SimpleMovingAverage _longSma;
	private SimpleMovingAverage _shortSma;
	private IChart _chart;
	private IChartCandleElement _chartCandleElement;
	private IChartIndicatorElement _longSmaIndicatorElement;
	private IChartIndicatorElement _shortSmaIndicatorElement;

	public SmaStrategy()
	{
		_longSmaLength = Param(nameof(LongSmaLength), 80)
							.SetDisplay("Länge der langen SMA", string.Empty, "Grundeinstellungen")
							.SetCanOptimize(true);

		_shortSmaLength = Param(nameof(ShortSmaLength), 30)
							.SetDisplay("Länge der kurzen SMA", string.Empty, "Grundeinstellungen")
							.SetCanOptimize(true);

		_series = Param(nameof(Series), TimeSpan.FromMinutes(15).TimeFrame())
					.SetDisplay("Serie", string.Empty, "Grundeinstellungen");
	}

	protected override void OnStarted2(DateTime time)
	{
		base.OnStarted2(time);

		_longSma = new SimpleMovingAverage { Length = LongSmaLength };
		_shortSma = new SimpleMovingAverage { Length = ShortSmaLength };

		Indicators.Add(_shortSma);
		Indicators.Add(_longSma);

		// Chart initialisieren, falls verfügbar
		_chart = GetChart();
		if (_chart != null)
			InitChart();

		var subscription = new Subscription(Series, Security);

		Connector
			.WhenCandlesFinished(subscription)
			.Do(ProcessCandle)
			.Apply(this);

		Subscribe(subscription);
	}

	private void InitChart()
	{
		_chart.ClearAreas();
		var area = _chart.AddArea();

		_chartCandleElement = area.AddCandles();

		_longSmaIndicatorElement = area.AddIndicator(_longSma);
		_longSmaIndicatorElement.Color = System.Drawing.Color.Brown;
		_longSmaIndicatorElement.DrawStyle = DrawStyles.Line;

		_shortSmaIndicatorElement = area.AddIndicator(_shortSma);
		_shortSmaIndicatorElement.Color = System.Drawing.Color.Blue;
		_shortSmaIndicatorElement.DrawStyle = DrawStyles.Line;
	}

	private void ProcessCandle(ICandleMessage candle)
	{
		var ls = _longSma.Process(candle);
		var ss = _shortSma.Process(candle);

		// Im Chart zeichnen, falls verfügbar
		if (_chart != null)
		{
			var data = _chart.CreateData();
			data.Group(candle.OpenTime)
				.Add(_chartCandleElement, candle)
				.Add(_longSmaIndicatorElement, ls)
				.Add(_shortSmaIndicatorElement, ss);
			_chart.Draw(data);
		}

		if (!_longSma.IsFormed)
			return;

		var isShortLessCurrent = _shortSma.GetCurrentValue() < _longSma.GetCurrentValue();
		var isShortLessPrev = _shortSma.GetValue(1) < _longSma.GetValue(1);

		if (isShortLessCurrent == isShortLessPrev)
			return;

		// Handelslogik
		var volume = Volume + Math.Abs(Position);

		if (isShortLessCurrent)
			SellMarket(volume);
		else
			BuyMarket(volume);
	}
}
```

## Siehe auch

- [Strategieparameter](parameters.md)
- [Ereignismodell](event_model.md)
- [Strategieprotokollierung](logging.md)

