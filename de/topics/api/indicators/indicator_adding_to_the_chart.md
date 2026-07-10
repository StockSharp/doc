# Hinzufügen eines Indikators zum Chart

Das folgende Beispiel zeigt, wie ein Indikator zum Zeichnen in einem Chart hinzugefügt wird:

```cs
private readonly Connector _connector = new Connector();
private Security _security;
private Subscription _candleSubscription;
private SimpleMovingAverage _sma;
readonly TimeSpan _timeFrame = TimeSpan.FromMinutes(1);
private ChartArea _area;
private ChartCandleElement _candlesElem;
private ChartIndicatorElement _longMaElem;

// Chart und Indikator initialisieren
private void InitializeChart()
{
	// _chart - StockSharp.Xaml.Charting.Chart
	// Chartbereich erstellen
	_area = new ChartArea();
	_chart.Areas.Add(_area);

	// Chartelement erstellen, das Kerzen darstellt
	_candlesElem = new ChartCandleElement();
	_area.Elements.Add(_candlesElem);

	// Chartelement erstellen, das den Indikator darstellt
	_longMaElem = new ChartIndicatorElement
	{
		Title = "Lang"
	};
	_area.Elements.Add(_longMaElem);

	// Indikator erstellen
	_sma = new SimpleMovingAverage() { Length = 80 };

	// Ereignis für den Kerzenempfang abonnieren
	_connector.CandleReceived += OnCandleReceived;
}

// Methode zum Abonnieren von Kerzen
private void SubscribeToCandles()
{
	// Subscription auf Kerzen mit dem angegebenen Zeitrahmen erstellen
	_candleSubscription = new Subscription(
		DataType.TimeFrame(_timeFrame),
		_security)
	{
		MarketData =
		{
			// Historische Daten für 30 Tage anfordern
			From = DateTime.Today.Subtract(TimeSpan.FromDays(30)),
			To = DateTime.Now,
			// Nur abgeschlossene Kerzen empfangen
			IsFinishedOnly = true
		}
	};

	// Subscription starten
	_connector.Subscribe(_candleSubscription);
}

// Handler für das Kerzenempfangsereignis
private void OnCandleReceived(Subscription subscription, ICandleMessage candle)
{
	// Prüfen, ob die Kerze zu unserer Subscription gehört
	if (subscription != _candleSubscription)
		return;

	// Kerzenstatus prüfen
	if (candle.State != CandleStates.Finished)
		return;

	// Kerze mit dem Indikator verarbeiten
	var longValue = _sma.Process(candle);

	// Daten zum Zeichnen erstellen
	var data = new ChartDrawData();
	data
		.Group(candle.OpenTime)
			.Add(_candlesElem, candle)
			.Add(_longMaElem, longValue);

	// Im UI-Thread auf dem Chart zeichnen
	this.GuiAsync(() => _chart.Draw(data));
}

// Methode zum Abbestellen beim Schließen des Fensters
private void UnsubscribeFromCandles()
{
	if (_candleSubscription != null)
	{
		_connector.CandleReceived -= OnCandleReceived;
		_connector.UnSubscribe(_candleSubscription);
		_candleSubscription = null;
	}
}
```

![indicators chart](../../../images/indicators_chart.png)

## Beispiel für die Arbeit mit mehreren Indikatoren

```cs
private readonly Connector _connector = new Connector();
private Security _security;
private Subscription _candleSubscription;
private SimpleMovingAverage _shortSma;
private SimpleMovingAverage _longSma;
private ChartArea _mainArea;
private ChartArea _indicatorArea;
private ChartCandleElement _candlesElem;
private ChartIndicatorElement _shortSmaElem;
private ChartIndicatorElement _longSmaElem;
private RelativeStrengthIndex _rsi;
private ChartIndicatorElement _rsiElem;

// Chart und Indikatoren initialisieren
private void InitializeChartWithMultipleIndicators()
{
	// Hauptbereich für Kerzen und gleitende Durchschnitte erstellen
	_mainArea = new ChartArea();
	_chart.Areas.Add(_mainArea);

	// Bereich für RSI erstellen
	_indicatorArea = new ChartArea();
	_chart.Areas.Add(_indicatorArea);

	// Chartelemente erstellen
	_candlesElem = new ChartCandleElement();
	_shortSmaElem = new ChartIndicatorElement { Title = "SMA (kurz)" };
	_longSmaElem = new ChartIndicatorElement { Title = "SMA (lang)" };
	_rsiElem = new ChartIndicatorElement { Title = "RSI" };

	// Elementfarben festlegen
	_shortSmaElem.Color = Colors.Red;
	_longSmaElem.Color = Colors.Blue;
	_rsiElem.Color = Colors.Green;

	// Elemente zu ihren jeweiligen Bereichen hinzufügen
	_mainArea.Elements.Add(_candlesElem);
	_mainArea.Elements.Add(_shortSmaElem);
	_mainArea.Elements.Add(_longSmaElem);
	_indicatorArea.Elements.Add(_rsiElem);

	// Indikatoren erstellen
	_shortSma = new SimpleMovingAverage { Length = 9 };
	_longSma = new SimpleMovingAverage { Length = 20 };
	_rsi = new RelativeStrengthIndex { Length = 14 };

	// Ereignis für den Kerzenempfang abonnieren
	_connector.CandleReceived += OnCandleReceivedMultipleIndicators;

	// Subscription auf Kerzen erstellen
	_candleSubscription = new Subscription(
		DataType.TimeFrame(TimeSpan.FromMinutes(5)),
		_security)
	{
		MarketData =
		{
			From = DateTime.Today.Subtract(TimeSpan.FromDays(30)),
			To = DateTime.Now,
			IsFinishedOnly = true
		}
	};

	// Subscription starten
	_connector.Subscribe(_candleSubscription);
}

// Handler für das Kerzenempfangsereignis bei mehreren Indikatoren
private void OnCandleReceivedMultipleIndicators(Subscription subscription, ICandleMessage candle)
{
	// Prüfen, ob die Kerze zu unserer Subscription gehört
	if (subscription != _candleSubscription)
		return;

	if (candle.State != CandleStates.Finished)
		return;

	// Kerze mit Indikatoren verarbeiten
	var shortSmaValue = _shortSma.Process(candle);
	var longSmaValue = _longSma.Process(candle);
	var rsiValue = _rsi.Process(candle);

	// Daten zum Zeichnen erstellen
	var data = new ChartDrawData();
	data
		.Group(candle.OpenTime)
			.Add(_candlesElem, candle)
			.Add(_shortSmaElem, shortSmaValue)
			.Add(_longSmaElem, longSmaValue)
			.Add(_rsiElem, rsiValue);

	// Im UI-Thread auf dem Chart zeichnen
	this.GuiAsync(() => _chart.Draw(data));
}
```

## Siehe auch

[Komponenten zum Erstellen von Charts](../graphical_user_interface/charts.md)
