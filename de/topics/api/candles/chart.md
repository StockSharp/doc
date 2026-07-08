# Chart

Für die grafische Darstellung von Candles können Sie die spezielle Komponente [Chart](xref:StockSharp.Xaml.Charting.Chart) verwenden (siehe [Komponenten für die Chart-Erstellung](../graphical_user_interface/charts.md)), die Candles wie folgt darstellt:

![sample candleschart](../../../images/sample_candleschart.png)

## Grundlegender Ansatz zur Anzeige von Candles

Es gibt zwei Ansätze zur Anzeige von Candles in einem Chart. Der erste Ansatz ist das manuelle Zeichnen von Candles beim Empfang von Daten:

```cs
// CandlesChart - StockSharp.Xaml.Chart
private ChartArea _areaComb;
private ChartCandleElement _candleElement;

// Diagramm initialisieren
private void InitializeChart()
{
	// Diagrammbereich erstellen
	_areaComb = new ChartArea();
	_chart.Areas.Add(_areaComb);

	// Diagrammelement für Kerzen erstellen
	_candleElement = new ChartCandleElement() { FullTitle = "Candles" };
	_areaComb.Elements.Add(_candleElement);

	// Ereignis zum Empfang von Kerzen abonnieren
	_connector.CandleReceived += OnCandleReceived;
}

// Abonnement für 5-Minuten-Kerzen erstellen
private void SubscribeToCandles()
{
	var subscription = new Subscription(
		DataType.TimeFrame(TimeSpan.FromMinutes(5)),
		_security)
	{
		MarketData =
		{
			// Historische Daten für 5 Tage anfordern
			From = DateTime.Today.Subtract(TimeSpan.FromDays(5)),
			To = DateTime.Now
		}
	};

	// Abonnement starten
	_connector.Subscribe(subscription);
}

// Handler für das Kerzenempfangsereignis
private void OnCandleReceived(Subscription subscription, ICandleMessage candle)
{
	// Prüfen, ob die Kerze abgeschlossen ist
	if (candle.State == CandleStates.Finished)
	{
		// Daten zum Zeichnen erstellen
		var chartData = new ChartDrawData();
		chartData.Group(candle.OpenTime).Add(_candleElement, candle);

		// Im UI-Thread im Diagramm zeichnen
		this.GuiAsync(() => _chart.Draw(chartData));
	}
}
```

## Automatische Bindung des Abonnements an ein Chart-Element

Der zweite Ansatz besteht darin, die automatische Bindung eines Abonnements an ein Chart-Element zu verwenden. Dies ermöglicht die automatische Anzeige der empfangenen Daten:

```cs
// Diagramminitialisierung mit automatischer Bindung
private void InitializeChartWithAutoBinding()
{
	// Diagrammbereich erstellen
	var area = new ChartArea();
	_chart.Areas.Add(area);

	// Element zur Anzeige von Kerzen erstellen
	var candleElement = new ChartCandleElement();

	// Kerzenabonnement erstellen
	var subscription = new Subscription(
		DataType.TimeFrame(TimeSpan.FromMinutes(5)),
		_security)
	{
		MarketData =
		{
			From = DateTime.Today.Subtract(TimeSpan.FromDays(5)),
			To = DateTime.Now
		}
	};

	// Element an Abonnement binden
	_chart.AddElement(area, candleElement, subscription);

	// Abonnement starten
	_connector.Subscribe(subscription);
}
```

## Arbeiten mit Indikatoren

Um Indikatoren zusammen mit Candles im Chart anzuzeigen, werden Elemente vom Typ [ChartIndicatorElement](xref:StockSharp.Xaml.Charting.ChartIndicatorElement) verwendet:

```cs
// Indikator zum Diagramm hinzufügen
private void AddIndicatorToChart()
{
	// Element für Indikator erstellen
	var smaElement = new ChartIndicatorElement
	{
		Title = "SMA (14)",
		Color = Colors.Red
	};

	// Element zum selben Bereich wie die Kerzen hinzufügen
	_areaComb.Elements.Add(smaElement);

	// Indikator erstellen
	var sma = new SimpleMovingAverage { Length = 14 };

	// Kerzenempfangsereignis zur Indikatorberechnung abonnieren
	_connector.CandleReceived += (subscription, candle) =>
	{
		// Indikatorwert berechnen
		var indicatorValue = sma.Process(candle);

		// Wert im Diagramm zeichnen
		var chartData = new ChartDrawData();
		chartData.Group(candle.OpenTime).Add(smaElement, indicatorValue);

		this.GuiAsync(() => _chart.Draw(chartData));
	};
}
```

## Anzeige mehrerer Indikatoren in verschiedenen Bereichen

Indikatoren können in separaten Chart-Bereichen platziert werden:

```cs
// Indikatoren zu verschiedenen Bereichen hinzufügen
private void AddIndicatorsToSeparateAreas()
{
	// Hauptbereich für Kerzen
	var candleArea = new ChartArea();
	_chart.Areas.Add(candleArea);

	// Element für Kerzen
	var candleElement = new ChartCandleElement();
	candleArea.Elements.Add(candleElement);

	// Element für SMA im selben Bereich
	var smaElement = new ChartIndicatorElement { Title = "SMA (14)" };
	candleArea.Elements.Add(smaElement);

	// Separater Bereich für RSI
	var rsiArea = new ChartArea();
	_chart.Areas.Add(rsiArea);

	// Element für RSI
	var rsiElement = new ChartIndicatorElement { Title = "RSI (14)" };
	rsiArea.Elements.Add(rsiElement);

	// Indikatoren erstellen
	var sma = new SimpleMovingAverage { Length = 14 };
	var rsi = new RelativeStrengthIndex { Length = 14 };

	// Kerzenabonnement
	var subscription = new Subscription(
		DataType.TimeFrame(TimeSpan.FromMinutes(5)),
		_security);

	// Kerzenelement an Abonnement binden
	_chart.AddElement(candleArea, candleElement, subscription);

	// Abonnement starten und Indikatoren verarbeiten
	_connector.Subscribe(subscription);

	_connector.CandleReceived += (sub, candle) =>
	{
		if (sub != subscription || candle.State != CandleStates.Finished)
			return;

		// Indikatorwerte berechnen
		var smaValue = sma.Process(candle);
		var rsiValue = rsi.Process(candle);

		// Werte im Diagramm zeichnen
		var chartData = new ChartDrawData();
		chartData
			.Group(candle.OpenTime)
				.Add(smaElement, smaValue)
				.Add(rsiElement, rsiValue);

		this.GuiAsync(() => _chart.Draw(chartData));
	};
}
```

## Anzeige von Orders und Trades im Chart

Für die Anzeige von Orders und Trades im Chart werden spezielle Elemente verwendet:

```cs
// Elemente zur Anzeige von Orders und Trades hinzufügen
private void AddOrdersAndTradesToChart()
{
	// Elemente zur Anzeige von Orders und Trades erstellen
	var orderElement = new ChartOrderElement();
	var tradeElement = new ChartTradeElement();

	// Elemente zum Diagrammbereich hinzufügen
	_areaComb.Elements.Add(orderElement);
	_areaComb.Elements.Add(tradeElement);

	// Ereignisse zum Empfang von Orders und Trades abonnieren
	_connector.OrderReceived += (subscription, order) =>
	{
		if (order.Security != _security)
			return;

		// Order im Diagramm zeichnen
		var chartData = new ChartDrawData();
		chartData.Group(order.Time).Add(orderElement, order);

		this.GuiAsync(() => _chart.Draw(chartData));
	};

	_connector.OwnTradeReceived += (subscription, trade) =>
	{
		if (trade.Order.Security != _security)
			return;

		// Trade im Diagramm zeichnen
		var chartData = new ChartDrawData();
		chartData.Group(trade.Time).Add(tradeElement, trade);

		this.GuiAsync(() => _chart.Draw(chartData));
	};
}
```

## Konfiguration des Chart-Erscheinungsbilds

Verschiedene Aspekte des Chart-Erscheinungsbilds können konfiguriert werden:

```cs
// Diagrammdarstellung konfigurieren
private void ConfigureChartAppearance()
{
	// Diagrammbereich konfigurieren
	_areaComb.Height = 300;
	_areaComb.BackgroundMajorGridColor = Colors.Gray;
	_areaComb.BackgroundMinorGridColor = Colors.LightGray;

	// Kerzenelement konfigurieren
	_candleElement.DrawStyle = ChartCandleDrawStyles.CandleStick;
	_candleElement.UpBrush = Brushes.Green;
	_candleElement.DownBrush = Brushes.Red;
	_candleElement.StrokeThickness = 1;

	// Gesamtes Diagramm konfigurieren
	_chart.IsAutoRange = true;            // Automatic scaling
	_chart.IsManualVerticalValues = false; // Automatic calculation of vertical values
	_chart.BidEnabled = false;            // Disable display of best bid price
	_chart.AskEnabled = false;            // Disable display of best ask price
}
```

## Chart-Zoom und -Scrollen

Verwaltung von Chart-Zoom und -Scrollen:

```cs
// Zoom und Scrollen konfigurieren
private void ConfigureChartZoomAndScroll()
{
	// Start- und Enddatum für die Anzeige festlegen
	_chart.SetXRange(DateTime.Today.AddDays(-10), DateTime.Today);

	// Y-Achsenbereich festlegen
	_chart.SetYRange(100, 150);

	// Schaltflächen zur Zoomsteuerung
	zoomInButton.Click += (s, e) => _chart.ZoomIn();
	zoomOutButton.Click += (s, e) => _chart.ZoomOut();

	// Schaltflächen zum Scrollen
	scrollLeftButton.Click += (s, e) => _chart.ScrollLeft();
	scrollRightButton.Click += (s, e) => _chart.ScrollRight();

	// Zoom auf automatisch zurücksetzen
	resetZoomButton.Click += (s, e) => _chart.IsAutoRange = true;
}
```

## Exportieren des Charts als Bild

Um das Chart in einer Datei zu speichern:

```cs
// Diagramm als Bild exportieren
private void ExportChartToImage()
{
	// Objekt zum Speichern des Bildes erstellen
	var saveFileDialog = new SaveFileDialog
	{
		Filter = "PNG Image|*.png|JPEG Image|*.jpg|BMP Image|*.bmp",
		Title = "Save Chart Image"
	};

	if (saveFileDialog.ShowDialog() == true)
	{
		// Bild aus Diagramm erstellen
		var rtb = new RenderTargetBitmap(
			(int)_chart.ActualWidth,
			(int)_chart.ActualHeight,
			96, 96,
			PixelFormats.Pbgra32);

		rtb.Render(_chart);

		// Bild im ausgewählten Format speichern
		BitmapEncoder encoder;

		switch (Path.GetExtension(saveFileDialog.FileName).ToLower())
		{
			case ".jpg":
				encoder = new JpegBitmapEncoder();
				break;
			case ".bmp":
				encoder = new BmpBitmapEncoder();
				break;
			default:
				encoder = new PngBitmapEncoder();
				break;
		}

		encoder.Frames.Add(BitmapFrame.Create(rtb));

		using (var fileStream = new FileStream(saveFileDialog.FileName, FileMode.Create))
		{
			encoder.Save(fileStream);
		}
	}
}
```

## Löschen des Charts

Um Daten im Chart zu löschen:

```cs
// Diagramm oder seine Elemente leeren
private void ClearChart()
{
	// Gesamtes Diagramm leeren
	_chart.Reset();

	// Bestimmten Bereich leeren
	_areaComb.Reset();

	// Bestimmtes Element leeren
	_candleElement.Reset();
}
```

Ein Beispiel für die Anzeige von Candles in einem Chart finden Sie im Abschnitt [Kerzen](../candles.md).

## Siehe auch

[Komponenten für die Chart-Erstellung](../graphical_user_interface/charts.md)
