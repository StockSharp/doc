# Arbeiten mit Charts in Strategien

In StockSharp stellt die Klasse [Strategy](xref:StockSharp.Algo.Strategies.Strategy) eine komfortable Schnittstelle bereit, um Handelsaktivitäten in einem Chart zu visualisieren. In diesem Artikel sehen wir uns an, wie Sie aus einer Strategie auf einen Chart zugreifen, Bereiche (ChartArea) erstellen, verschiedene Elemente hinzufügen (Kerzen, Indikatoren, Trades) und Daten zeichnen.

## Zugriff auf den Chart

### GetChart-Methode

Um aus einer Strategie auf den Chart zuzugreifen, verwenden Sie die Methode [Strategy.GetChart()](xref:StockSharp.Algo.Strategies.Strategy.GetChart):

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);
	
	// Chart abrufen
	_chart = GetChart();
	
	// Chart-Verfügbarkeit prüfen
	if (_chart != null)
	{
		// Chart initialisieren
		InitializeChart();
	}
	else
	{
		// Chart ist nicht verfügbar, z. B. beim Start im Konsolenmodus
		LogInfo("Chart is unavailable. Visualization disabled.");
	}
}
```

Die Methode [GetChart()](xref:StockSharp.Algo.Strategies.Strategy.GetChart) gibt eine [IChart](xref:StockSharp.Charting.IChart)-Schnittstelle zurück, die Zugriff auf Chart-Funktionen bietet. Es ist wichtig, das Ergebnis auf `null` zu prüfen, da der Chart beispielsweise beim Ausführen einer Strategie im Konsolenmodus oder beim Cloud-Testing nicht verfügbar sein kann.

### SetChart-Methode

In manchen Fällen kann der Chart von außen gesetzt werden. Verwenden Sie dafür die Methode [Strategy.SetChart](xref:StockSharp.Algo.Strategies.Strategy.SetChart(StockSharp.Charting.IChart)):

```cs
// Chart aus einer externen Quelle setzen
public void ConfigureVisualization(IChart chart)
{
	SetChart(chart);
	
	if (chart != null)
	{
		InitializeChart();
	}
}
```

## Chart-Bereiche erstellen

Nachdem Sie Zugriff auf den Chart erhalten haben, können Sie einen oder mehrere Bereiche erstellen, um verschiedene Daten anzuzeigen. Verwenden Sie dazu die Methode [CreateChartArea](xref:StockSharp.Algo.Strategies.Strategy.CreateChartArea):

```cs
private void InitializeChart()
{
	// Hauptbereich für Kerzen und Indikatoren erstellen
	_mainArea = CreateChartArea();
	
	// Zusätzlichen Bereich für Volumen erstellen
	_volumeArea = CreateChartArea();
	
	// Bereiche konfigurieren und Elemente hinzufügen
	ConfigureChartElements();
}
```

Sie können auch direkt die Methode [IChart.AddArea](xref:StockSharp.Charting.ChartingInterfacesExtensions.AddArea(StockSharp.Charting.IChart)) verwenden:

```cs
private void InitializeChart()
{
	// Vorhandene Bereiche bei Bedarf löschen
	foreach (var area in _chart.Areas.ToArray())
		_chart.RemoveArea(area);
	
	// Hauptbereich für Kerzen und Indikatoren erstellen
	_mainArea = _chart.AddArea();
	
	// Zusätzlichen Bereich für Volumen erstellen
	_volumeArea = _chart.AddArea();
	
	// Bereiche konfigurieren und Elemente hinzufügen
	ConfigureChartElements();
}
```

## Elemente zum Chart hinzufügen

Nach dem Erstellen der Chart-Bereiche können Sie verschiedene Elemente hinzufügen, um Daten anzuzeigen. StockSharp unterstützt unterschiedliche Elementtypen wie Kerzen, Indikatoren, Trades und Orders.

### Kerzen hinzufügen

Um Kerzen anzuzeigen, verwenden Sie die Methode [AddCandles](xref:StockSharp.Charting.ChartingInterfacesExtensions.AddCandles(StockSharp.Charting.IChartArea)) des Chart-Bereichs:

```cs
private void ConfigureChartElements()
{
	// Kerzenelement zum Hauptbereich hinzufügen
	_candleElement = _mainArea.AddCandles();
	
	// Kerzendarstellung konfigurieren
	_candleElement.DrawStyle = ChartCandleDrawStyles.CandleStick; // japanische Kerzen
	_candleElement.AntiAliasing = true; // Glättung
	_candleElement.UpFillColor = Color.Green; // Körperfarbe steigender Kerzen
	_candleElement.DownFillColor = Color.Red; // Körperfarbe fallender Kerzen
	_candleElement.UpBorderColor = Color.DarkGreen; // Rahmenfarbe steigender Kerzen
	_candleElement.DownBorderColor = Color.DarkRed; // Rahmenfarbe fallender Kerzen
	_candleElement.StrokeThickness = 1; // Linienstärke
	_candleElement.ShowAxisMarker = true; // Y-Achsenmarker anzeigen
}
```

Die Schnittstelle [IChartCandleElement](xref:StockSharp.Charting.IChartCandleElement) bietet viele Eigenschaften zum Konfigurieren der Kerzendarstellung:

- **DrawStyle** - Darstellungsstil der Kerzen:
  - **CandleStick** - japanische Kerzen
  - **Ohlc** - Balken
  - **LineOpen/LineHigh/LineLow/LineClose** - Linien für die jeweiligen Preise
  - **BoxVolume** - Volumenboxen
  - **ClusterProfile** - Clusterprofil
  - **Area** - Fläche
  - **PnF** - Point-and-Figure-Chart

- **Farbeinstellungen**:
  - **UpFillColor/DownFillColor** - Körperfarbe steigender/fallender Kerzen
  - **UpBorderColor/DownBorderColor** - Rahmenfarbe steigender/fallender Kerzen
  - **LineColor** - Linienfarbe für linienbasierte Charts
  - **AreaColor** - Flächenfarbe für den Typ Area

- **Weitere Einstellungen**:
  - **StrokeThickness** - Linienstärke
  - **AntiAliasing** - Glättung
  - **ShowAxisMarker** - Y-Achsenmarker anzeigen

### Indikatoren hinzufügen

Um Indikatoren anzuzeigen, verwenden Sie die Methode [DrawIndicator](xref:StockSharp.Algo.Strategies.Strategy.DrawIndicator(StockSharp.Charting.IChartArea,StockSharp.Algo.Indicators.IIndicator,System.Nullable{System.Drawing.Color},System.Nullable{System.Drawing.Color})):

```cs
// Indikatoren erstellen
_sma = new SimpleMovingAverage { Length = SmaLength };
_bollinger = new BollingerBands
{
	Length = BollingerLength,
	Deviation = BollingerDeviation
};

// Indikatoren zur Strategiesammlung hinzufügen
Indicators.Add(_sma);
Indicators.Add(_bollinger);

// Indikatoren visualisieren
_smaElement = DrawIndicator(_mainArea, _sma, Color.Blue);
_bollingerUpperElement = DrawIndicator(_mainArea, _bollinger, Color.Purple);
_bollingerLowerElement = DrawIndicator(_mainArea, _bollinger, Color.Purple);
_bollingerMiddleElement = DrawIndicator(_mainArea, _bollinger, Color.Gray);
```

Die Methode [DrawIndicator](xref:StockSharp.Algo.Strategies.Strategy.DrawIndicator(StockSharp.Charting.IChartArea,StockSharp.Algo.Indicators.IIndicator,System.Nullable{System.Drawing.Color},System.Nullable{System.Drawing.Color})) erstellt automatisch ein Indikatorelement und fügt es dem angegebenen Chart-Bereich hinzu. Sie können eine Farbe und eine zusätzliche Farbe für die Darstellung angeben.

Sie können ein Indikatorelement auch direkt über die Methode [AddIndicator](xref:StockSharp.Charting.ChartingInterfacesExtensions.AddIndicator(StockSharp.Charting.IChartArea,StockSharp.Algo.Indicators.IIndicator)) des Chart-Bereichs hinzufügen:

```cs
// SMA direkt über den Chart-Bereich hinzufügen
var smaElement = _mainArea.AddIndicator(_sma);
smaElement.Color = Color.Blue;
smaElement.StrokeThickness = 2;
smaElement.DrawStyle = DrawStyles.Line;
smaElement.AntiAliasing = true;
smaElement.ShowAxisMarker = true;
smaElement.AutoAssignYAxis = true; // Y-Achse automatisch zuweisen
```

Die Schnittstelle [IChartIndicatorElement](xref:StockSharp.Charting.IChartIndicatorElement) stellt die folgenden Eigenschaften zur Konfiguration bereit:

- **Color** - Hauptfarbe des Indikators
- **AdditionalColor** - zusätzliche Farbe (für Indikatoren mit zwei Linien)
- **StrokeThickness** - Linienstärke
- **AntiAliasing** - Glättung
- **DrawStyle** - Zeichenstil (Linie, Punkte, Histogramm usw.)
- **ShowAxisMarker** - Y-Achsenmarker anzeigen
- **AutoAssignYAxis** - Y-Achse automatisch zuweisen

### Trades hinzufügen

Um Trades anzuzeigen, verwenden Sie die Methode [DrawOwnTrades](xref:StockSharp.Algo.Strategies.Strategy.DrawOwnTrades(StockSharp.Charting.IChartArea)):

```cs
// Element zur Anzeige von Trades hinzufügen
_tradesElement = DrawOwnTrades(_mainArea);

// Trade-Darstellung konfigurieren
_tradesElement.BuyBrush = Color.Green;  // Kauffarbe
_tradesElement.SellBrush = Color.Red;   // Verkaufsfarbe
_tradesElement.PointSize = 10;          // Punktgröße
```

### Orders hinzufügen

Um Orders anzuzeigen, verwenden Sie die Methode [DrawOrders](xref:StockSharp.Algo.Strategies.Strategy.DrawOrders(StockSharp.Charting.IChartArea)):

```cs
// Element zur Anzeige von Orders hinzufügen
_ordersElement = DrawOrders(_mainArea);

// Order-Darstellung konfigurieren
_ordersElement.ActiveBrush = Color.Blue;     // Farbe aktiver Orders
_ordersElement.CanceledBrush = Color.Gray;   // Farbe stornierter Orders
_ordersElement.DoneBrush = Color.Green;      // Farbe ausgeführter Orders
_ordersElement.ErrorColor = Color.Red;       // Fehlerfarbe
_ordersElement.PointSize = 8;                // Punktgröße
```

Die Schnittstelle [IChartOrderElement](xref:StockSharp.Charting.IChartOrderElement) stellt die folgenden Eigenschaften zur Konfiguration bereit:

- **ActiveBrush** - Farbe aktiver Orders
- **CanceledBrush** - Farbe stornierter Orders
- **DoneBrush** - Farbe ausgeführter Orders
- **ErrorColor** - Fehlerfarbe
- **ErrorStrokeColor** - Fehler-Rahmenfarbe
- **Filter** - Filter für die Order-Anzeige

## Daten im Chart zeichnen

Nachdem alle Chart-Elemente konfiguriert sind, können Sie mit dem Zeichnen von Daten fortfahren. Je nach Datentyp werden unterschiedliche Methoden verwendet.

### Kerzen und Indikatoren zeichnen

Die effizienteste Möglichkeit, Daten zu zeichnen, ist die Verwendung der Methode [IChart.Draw](xref:StockSharp.Charting.IThemeableChart.Draw(StockSharp.Charting.IChartDrawData)) mit einem [IChartDrawData](xref:StockSharp.Charting.IChartDrawData)-Objekt:

```cs
private void ProcessCandle(ICandleMessage candle)
{
	// Kerze in Indikatoren verarbeiten
	var smaValue = _sma.Process(candle);
	var bollingerValue = _bollinger.Process(candle);
	
	// Wenn der Chart nicht verfügbar ist, Zeichnen überspringen
	if (_chart == null)
		return;
	
	// Daten zum Zeichnen erstellen
	var drawData = _chart.CreateData();
	
	// Daten nach Kerzenzeit gruppieren
	var group = drawData.Group(candle.OpenTime);
	
	// Kerze hinzufügen
	group.Add(_candleElement, 
		candle.DataType, 
		candle.SecurityId, 
		candle.OpenPrice, 
		candle.HighPrice, 
		candle.LowPrice, 
		candle.ClosePrice, 
		candle.PriceLevels, 
		candle.State);
	
	// Indikatorwerte hinzufügen
	group.Add(_smaElement, smaValue);
	
	if (bollingerValue != null)
	{
		group.Add(_bollingerUpperElement, bollingerValue);
		group.Add(_bollingerMiddleElement, bollingerValue);
		group.Add(_bollingerLowerElement, bollingerValue);
	}
	
	// Daten im Chart zeichnen
	_chart.Draw(drawData);
}
```

Die Methode [IChart.CreateData](xref:StockSharp.Charting.IThemeableChart.CreateData) erstellt ein [IChartDrawData](xref:StockSharp.Charting.IChartDrawData)-Objekt, das zum Gruppieren und Hinzufügen von Daten für verschiedene Chart-Elemente verwendet wird. Die Gruppierung der Daten erfolgt nach Zeitstempel über die Methode [Group](xref:StockSharp.Charting.IChartDrawData.Group(System.DateTimeOffset)).

Zum Hinzufügen von Daten unterschiedlicher Typen werden verschiedene Überladungen der Methode [Add](xref:StockSharp.Charting.IChartDrawData.IChartDrawDataItem.Add(StockSharp.Charting.IChartCandleElement,StockSharp.Messages.DataType,StockSharp.Messages.SecurityId,System.Decimal,System.Decimal,System.Decimal,System.Decimal,StockSharp.Messages.CandlePriceLevel[],StockSharp.Messages.CandleStates)) des Objekts [IChartDrawDataItem](xref:StockSharp.Charting.IChartDrawData.IChartDrawDataItem) verwendet.

### Trades und Orders zeichnen

Zum Zeichnen von Trades und Orders wird normalerweise ein automatischer Mechanismus verwendet, der ausgelöst wird, wenn neue Trades eingehen oder Orders geändert werden. Wenn manuelles Zeichnen erforderlich ist, können Sie jedoch den folgenden Code verwenden:

```cs
// Trade zeichnen
var tradeDrawData = _chart.CreateData();
var tradeGroup = tradeDrawData.Group(trade.Time);
tradeGroup.Add(_tradesElement, trade.Id, trade.StringId, trade.Side, trade.Price, trade.Volume);
_chart.Draw(tradeDrawData);

// Order zeichnen
var orderDrawData = _chart.CreateData();
var orderGroup = orderDrawData.Group(order.Time);
orderGroup.Add(_ordersElement, order.Id, order.StringId, order.Side, order.Price, order.Volume);
_chart.Draw(orderDrawData);
```

## Vollständiges Beispiel für Chart-Rendering in einer Strategie

Unten sehen Sie ein vollständiges Beispiel einer Strategie mit Chart-Einrichtung und Rendering:

```cs
public class SmaStrategy : Strategy
{
	private readonly StrategyParam<int> _smaLength;
	private readonly StrategyParam<int> _bollingerLength;
	private readonly StrategyParam<decimal> _bollingerDeviation;
	
	private SimpleMovingAverage _sma;
	private BollingerBands _bollinger;
	
	private IChart _chart;
	private IChartArea _mainArea;
	private IChartArea _volumeArea;
	
	private IChartCandleElement _candleElement;
	private IChartIndicatorElement _smaElement;
	private IChartIndicatorElement _bollingerUpperElement;
	private IChartIndicatorElement _bollingerMiddleElement;
	private IChartIndicatorElement _bollingerLowerElement;
	private IChartOrderElement _ordersElement;
	private IChartTradeElement _tradesElement;
	
	public SmaStrategy()
	{
		_smaLength = Param(nameof(SmaLength), 20);
		_bollingerLength = Param(nameof(BollingerLength), 20);
		_bollingerDeviation = Param(nameof(BollingerDeviation), 2m);
	}
	
	public int SmaLength
	{
		get => _smaLength.Value;
		set => _smaLength.Value = value;
	}
	
	public int BollingerLength
	{
		get => _bollingerLength.Value;
		set => _bollingerLength.Value = value;
	}
	
	public decimal BollingerDeviation
	{
		get => _bollingerDeviation.Value;
		set => _bollingerDeviation.Value = value;
	}
	
	protected override void OnStarted2(DateTime time)
	{
		base.OnStarted2(time);
		
		// Indikatoren erstellen
		_sma = new SimpleMovingAverage { Length = SmaLength };
		_bollinger = new BollingerBands
		{
			Length = BollingerLength,
			Deviation = BollingerDeviation
		};
		
		// Indikatoren zur Strategiesammlung hinzufügen
		Indicators.Add(_sma);
		Indicators.Add(_bollinger);
		
		// Chart abrufen
		_chart = GetChart();
		
		// Chart initialisieren, wenn verfügbar
		if (_chart != null)
		{
			InitializeChart();
		}
		
		// Kerzen abonnieren
		var subscription = new Subscription(
			DataType.TimeFrame(TimeSpan.FromMinutes(5)),
			Security);
		
		subscription
			.WhenCandlesFinished(this)
			.Do(ProcessCandle)
			.Apply(this);
		
		Subscribe(subscription);
	}
	
	private void InitializeChart()
	{
		// Vorhandene Bereiche löschen
		foreach (var area in _chart.Areas.ToArray())
			_chart.RemoveArea(area);
		
		// Hauptbereich für Kerzen und Indikatoren erstellen
		_mainArea = _chart.AddArea();
		
		// Zusätzlichen Bereich für Volumen erstellen
		_volumeArea = _chart.AddArea();
		
		// Chart-Elemente konfigurieren
		ConfigureChartElements();
	}
	
	private void ConfigureChartElements()
	{
		// Element zur Anzeige von Kerzen hinzufügen
		_candleElement = _mainArea.AddCandles();
		_candleElement.DrawStyle = ChartCandleDrawStyles.CandleStick;
		_candleElement.AntiAliasing = true;
		_candleElement.UpFillColor = Color.Green;
		_candleElement.DownFillColor = Color.Red;
		_candleElement.UpBorderColor = Color.DarkGreen;
		_candleElement.DownBorderColor = Color.DarkRed;
		_candleElement.StrokeThickness = 1;
		_candleElement.ShowAxisMarker = true;
		
		// Elemente für Indikatoren hinzufügen
		_smaElement = _mainArea.AddIndicator(_sma);
		_smaElement.Color = Color.Blue;
		_smaElement.StrokeThickness = 2;
		
		_bollingerUpperElement = _mainArea.AddIndicator(_bollinger);
		_bollingerUpperElement.Color = Color.Purple;
		_bollingerUpperElement.StrokeThickness = 1;
		
		_bollingerMiddleElement = _mainArea.AddIndicator(_bollinger);
		_bollingerMiddleElement.Color = Color.Gray;
		_bollingerMiddleElement.StrokeThickness = 1;
		
		_bollingerLowerElement = _mainArea.AddIndicator(_bollinger);
		_bollingerLowerElement.Color = Color.Purple;
		_bollingerLowerElement.StrokeThickness = 1;
		
		// Elemente für Orders und Trades hinzufügen
		_ordersElement = DrawOrders(_mainArea);
		_tradesElement = DrawOwnTrades(_mainArea);
	}
	
	private void ProcessCandle(ICandleMessage candle)
	{
		// Kerze mit Indikatoren verarbeiten
		var smaValue = _sma.Process(candle);
		var bollingerValue = _bollinger.Process(candle);
		
		// Wenn der Chart nicht verfügbar ist, Zeichnen überspringen
		if (_chart == null)
			return;
		
		// Daten im Chart zeichnen
		var drawData = _chart.CreateData();
		var group = drawData.Group(candle.OpenTime);
		
		// Kerze hinzufügen
		group.Add(_candleElement, 
			candle.DataType, 
			candle.SecurityId, 
			candle.OpenPrice, 
			candle.HighPrice, 
			candle.LowPrice, 
			candle.ClosePrice, 
			candle.PriceLevels, 
			candle.State);
		
		// Indikatorwerte hinzufügen
		group.Add(_smaElement, smaValue);
		
		if (bollingerValue != null)
		{
			group.Add(_bollingerUpperElement, bollingerValue);
			group.Add(_bollingerMiddleElement, bollingerValue);
			group.Add(_bollingerLowerElement, bollingerValue);
		}
		
		// Daten im Chart zeichnen
		_chart.Draw(drawData);
		
		// Handelslogik
		if (!IsFormed)
			return;
			
		// ... Implementierung der Handelslogik ...
	}
}
```

## Fazit

Charts in StockSharp-Strategien ermöglichen die Visualisierung von Handelsaktivitäten. Das vereinfacht die Entwicklung, das Debugging und die Überwachung von Handelsstrategien erheblich. Die Klasse [Strategy](xref:StockSharp.Algo.Strategies.Strategy) stellt viele Methoden für die Arbeit mit Charts bereit, mit denen sich verschiedene Elemente einfach hinzufügen und Daten rendern lassen.

Wenn Sie eine Strategie mit grafischer Oberfläche entwickeln, berücksichtigen Sie immer, dass der Chart beispielsweise beim Start im Konsolenmodus oder beim Cloud-Testing nicht verfügbar sein kann. Deshalb ist es wichtig, das Ergebnis der Methode [GetChart()](xref:StockSharp.Algo.Strategies.Strategy.GetChart) auf `null` zu prüfen und ein alternatives Szenario vorzusehen, damit die Strategie ohne Visualisierung arbeiten kann.

## Siehe auch

- [Indikatoren in Strategien](indicators.md)
- [Handelsoperationen in Strategien](trading_operations.md)
