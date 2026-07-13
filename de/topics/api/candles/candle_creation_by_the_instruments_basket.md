# Kerzenerstellung durch den Instrumentenkorb

Um Kerzen für [ContinuousSecurity](xref:StockSharp.Algo.ContinuousSecurity), [WeightedIndexSecurity](xref:StockSharp.Algo.WeightedIndexSecurity) oder [ExpressionIndexSecurity](xref:StockSharp.Algo.Expressions.ExpressionIndexSecurity) zu erstellen, wird derselbe Abonnement-Mechanismus verwendet wie für reguläre [Security](xref:StockSharp.BusinessEntities.Security)-Instrumente.

Nachfolgend ein Beispiel für die Erstellung von 1-Minuten-Kerzen für den AAPL-MSFT-Spread:

```cs
private Connector _connector;
private Security _instr1;
private Security _instr2;
private WeightedIndexSecurity _indexInstr;
private Subscription _indexSubscription;
private const string _secCode1 = "AAPL";
private const string _secCode2 = "MSFT";
readonly TimeSpan _timeFrame = TimeSpan.FromMinutes(1);
private ChartArea _area;
private ChartCandleElement _candleElement;

// Verbindungseinrichtung und Connector-Konfiguration
private void ConfigureConnector()
{
	if (_connector.Configure(this))
	{
		_connector.Save().Serialize(_connectorFile);
	}
}

// Diagramm einrichten
private void SetupChart()
{
	_area = new ChartArea();
	_chart.Areas.Add(_area);
	_candleElement = new ChartCandleElement();
	_area.Elements.Add(_candleElement);

	// Ereignis zum Empfang von Kerzen abonnieren
	_connector.CandleReceived += OnCandleReceived;
}

// Dienstregistrierung
private void RegisterServices()
{
	ConfigManager.RegisterService<ISecurityProvider>(_connector);
	ConfigManager.RegisterService<ICompilerService>(new RoslynCompilerService());
}

// Indexinstrument erstellen und Kerzen abonnieren
private void CreateIndexAndSubscribe()
{
	// Indexinstrument erstellen (Spread)
	_indexInstr = new WeightedIndexSecurity()
	{
		Board = ExchangeBoard.Nyse,
		Id = "IndexInstr"
	};

	// Instrumente mit Gewichten hinzufügen (1 und -1 für Spread)
	_indexInstr.Weights.Add(_instr1, 1);
	_indexInstr.Weights.Add(_instr2, -1);

	// Abonnement für Kerzen des Indexinstruments erstellen
	_indexSubscription = new Subscription(
		DataType.TimeFrame(_timeFrame),  // 1-Minuten-Kerzen
		_indexInstr)  // Unser Indexinstrument
	{
		MarketData =
		{
			// Abonnement zum Erstellen von Kerzen aus Ticks konfigurieren
			BuildMode = MarketDataBuildModes.Build,
			BuildFrom = DataType.Ticks,

			// Historische Daten für 30 Tage anfordern
			From = DateTime.Today.Subtract(TimeSpan.FromDays(30)),
			To = DateTime.Now
		}
	};

	// Element zum Diagramm hinzufügen und an das Abonnement binden
	_chart.AddElement(_area, _candleElement, _indexSubscription);

	// Abonnement starten
	_connector.Subscribe(_indexSubscription);
}

// Handler für das Kerzenempfangsereignis
private void OnCandleReceived(Subscription subscription, ICandleMessage candle)
{
	// Prüfen, ob die Kerze zu unserem Abonnement gehört
	if (subscription != _indexSubscription)
		return;

	// Bei Bedarf Verarbeitung auf abgeschlossene Kerzen beschränken
	if (candle.State != CandleStates.Finished)
		return;

	// Kerze im Diagramm zeichnen
	var chartData = new ChartDrawData();
	chartData.Group(candle.OpenTime).Add(_candleElement, candle);

	this.GuiAsync(() => _chart.Draw(chartData));
}

// Beim Schließen der Anwendung abbestellen
private void Unsubscribe()
{
	if (_indexSubscription != null)
	{
		_connector.CandleReceived -= OnCandleReceived;
		_connector.UnSubscribe(_indexSubscription);
		_indexSubscription = null;
	}
}
```

## Weitere Anwendungsfälle für Index-Abonnements

### Erstellung eines Abonnements für Index-Kerzen aus Komponenten-Kerzen

```cs
// Abonnement zum Erstellen von Indexkerzen aus Komponentenkerzen erstellen
var indexFromCandlesSubscription = new Subscription(
	DataType.TimeFrame(TimeSpan.FromMinutes(5)),
	_indexInstr)
{
	MarketData =
	{
		// Abonnement zum Erstellen aus Komponentenkerzen konfigurieren
		BuildMode = MarketDataBuildModes.Build,
		BuildFrom = DataType.TimeFrame(TimeSpan.FromMinutes(5)),
		From = DateTime.Today.Subtract(TimeSpan.FromDays(30)),
		To = DateTime.Now
	}
};

// Abonnement starten
_connector.Subscribe(indexFromCandlesSubscription);
```

### Erstellung eines Abonnements für Index-Kerzen aus Orderbüchern

```cs
// Abonnement zum Erstellen von Indexkerzen aus Orderbüchern erstellen
var indexFromDepthSubscription = new Subscription(
	DataType.TimeFrame(TimeSpan.FromMinutes(1)),
	_indexInstr)
{
	MarketData =
	{
		// Abonnement zum Erstellen aus Orderbüchern konfigurieren
		BuildMode = MarketDataBuildModes.Build,
		BuildFrom = DataType.MarketDepth,
		BuildField = Level1Fields.SpreadMiddle,  // Spread-Mitte verwenden
		From = DateTime.Today.Subtract(TimeSpan.FromDays(7)),
		To = DateTime.Now
	}
};

// Abonnement starten
_connector.Subscribe(indexFromDepthSubscription);
```

### Arbeiten mit dem Volatilitätsindex

```cs
// Volatilitätsindex auf Basis eines Ausdrucks erstellen
var volatilityIndex = new ExpressionIndexSecurity
{
	Board = ExchangeBoard.Nyse,
	Id = "VOLX",
	Expression = "StdDev({0}, 20) / SMA({0}, 20) * 100",  // Formel zur Berechnung der Volatilität
};

// Hauptinstrument zum Index hinzufügen
volatilityIndex.InnerSecurityIds.Add(_instr1.ToSecurityId());

// Abonnement für Kerzen des Volatilitätsindex erstellen
var volatilitySubscription = new Subscription(
	DataType.TimeFrame(TimeSpan.FromMinutes(5)),
	volatilityIndex)
{
	MarketData =
	{
		BuildMode = MarketDataBuildModes.Build,
		BuildFrom = DataType.Ticks,
		From = DateTime.Today.Subtract(TimeSpan.FromDays(30)),
		To = DateTime.Now
	}
};

// Abonnement starten
_connector.Subscribe(volatilitySubscription);
```

## Siehe auch

[Fortlaufende Futures](../instruments/continuous_futures.md)

[Index](../instruments/index.md)
