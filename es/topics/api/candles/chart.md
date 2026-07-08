# Gráfico

Para la visualización gráfica de velas, puede usar el componente especial [Chart](xref:StockSharp.Xaml.Charting.Chart) (véase [Componentes para construir gráficos](../graphical_user_interface/charts.md)), que renderiza las velas de la siguiente manera:

![sample candleschart](../../../images/sample_candleschart.png)

## Enfoque básico para mostrar velas

Existen dos enfoques para mostrar velas en un gráfico. El primer enfoque es el dibujo manual de velas al recibir datos:

```cs
// CandlesChart - StockSharp.Xaml.Chart
private ChartArea _areaComb;
private ChartCandleElement _candleElement;

// Inicialización del gráfico
private void InitializeChart()
{
	// Crear área del gráfico
	_areaComb = new ChartArea();
	_chart.Areas.Add(_areaComb);

	// Crear elemento del gráfico que representa velas
	_candleElement = new ChartCandleElement() { FullTitle = "Candles" };
	_areaComb.Elements.Add(_candleElement);

	// Suscribirse al evento de recepción de velas
	_connector.CandleReceived += OnCandleReceived;
}

// Crear suscripción a velas de 5 minutos
private void SubscribeToCandles()
{
	var subscription = new Subscription(
		DataType.TimeFrame(TimeSpan.FromMinutes(5)),
		_security)
	{
		MarketData =
		{
			// Solicitar datos históricos de 5 días
			From = DateTime.Today.Subtract(TimeSpan.FromDays(5)),
			To = DateTime.Now
		}
	};

	// Iniciar suscripción
	_connector.Subscribe(subscription);
}

// Controlador del evento de recepción de velas
private void OnCandleReceived(Subscription subscription, ICandleMessage candle)
{
	// Comprobar si la vela está completada
	if (candle.State == CandleStates.Finished)
	{
		// Crear datos para el dibujo
		var chartData = new ChartDrawData();
		chartData.Group(candle.OpenTime).Add(_candleElement, candle);

		// Dibujar en el gráfico en el hilo de UI
		this.GuiAsync(() => _chart.Draw(chartData));
	}
}
```

## Vinculación automática de suscripción al elemento del gráfico

El segundo enfoque es usar la vinculación automática de la suscripción al elemento del gráfico. Esto permite mostrar automáticamente los datos recibidos:

```cs
// Inicialización del gráfico con enlace automático
private void InitializeChartWithAutoBinding()
{
	// Crear área del gráfico
	var area = new ChartArea();
	_chart.Areas.Add(area);

	// Crear elemento para mostrar velas
	var candleElement = new ChartCandleElement();

	// Crear suscripción a velas
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

	// Vincular elemento a la suscripción
	_chart.AddElement(area, candleElement, subscription);

	// Iniciar suscripción
	_connector.Subscribe(subscription);
}
```

## Trabajando con indicadores

Para mostrar indicadores en el gráfico junto con las velas, se usan elementos de tipo [ChartIndicatorElement](xref:StockSharp.Xaml.Charting.ChartIndicatorElement):

```cs
// Añadir indicador al gráfico
private void AddIndicatorToChart()
{
	// Crear elemento para el indicador
	var smaElement = new ChartIndicatorElement
	{
		Title = "SMA (14)",
		Color = Colors.Red
	};

	// Añadir elemento a la misma área que las velas
	_areaComb.Elements.Add(smaElement);

	// Crear indicador
	var sma = new SimpleMovingAverage { Length = 14 };

	// Suscribirse al evento de recepción de velas para calcular el indicador
	_connector.CandleReceived += (subscription, candle) =>
	{
		// Calcular valor del indicador
		var indicatorValue = sma.Process(candle);

		// Dibujar valor en el gráfico
		var chartData = new ChartDrawData();
		chartData.Group(candle.OpenTime).Add(smaElement, indicatorValue);

		this.GuiAsync(() => _chart.Draw(chartData));
	};
}
```

## Mostrando múltiples indicadores en diferentes áreas

Los indicadores pueden colocarse en áreas separadas del gráfico:

```cs
// Añadir indicadores a diferentes áreas
private void AddIndicatorsToSeparateAreas()
{
	// Área principal para velas
	var candleArea = new ChartArea();
	_chart.Areas.Add(candleArea);

	// Elemento para velas
	var candleElement = new ChartCandleElement();
	candleArea.Elements.Add(candleElement);

	// Elemento para SMA en la misma área
	var smaElement = new ChartIndicatorElement { Title = "SMA (14)" };
	candleArea.Elements.Add(smaElement);

	// Área separada para RSI
	var rsiArea = new ChartArea();
	_chart.Areas.Add(rsiArea);

	// Elemento para RSI
	var rsiElement = new ChartIndicatorElement { Title = "RSI (14)" };
	rsiArea.Elements.Add(rsiElement);

	// Crear indicadores
	var sma = new SimpleMovingAverage { Length = 14 };
	var rsi = new RelativeStrengthIndex { Length = 14 };

	// Suscripción a velas
	var subscription = new Subscription(
		DataType.TimeFrame(TimeSpan.FromMinutes(5)),
		_security);

	// Vincular elemento de vela a la suscripción
	_chart.AddElement(candleArea, candleElement, subscription);

	// Iniciar suscripción y procesar indicadores
	_connector.Subscribe(subscription);

	_connector.CandleReceived += (sub, candle) =>
	{
		if (sub != subscription || candle.State != CandleStates.Finished)
			return;

		// Calcular valores del indicador
		var smaValue = sma.Process(candle);
		var rsiValue = rsi.Process(candle);

		// Dibujar valores en el gráfico
		var chartData = new ChartDrawData();
		chartData
			.Group(candle.OpenTime)
				.Add(smaElement, smaValue)
				.Add(rsiElement, rsiValue);

		this.GuiAsync(() => _chart.Draw(chartData));
	};
}
```

## Mostrando órdenes y operaciones en el gráfico

Se usan elementos especiales para mostrar órdenes y operaciones en el gráfico:

```cs
// Añadir elementos para mostrar órdenes y operaciones
private void AddOrdersAndTradesToChart()
{
	// Crear elementos para mostrar órdenes y operaciones
	var orderElement = new ChartOrderElement();
	var tradeElement = new ChartTradeElement();

	// Añadir elementos al área del gráfico
	_areaComb.Elements.Add(orderElement);
	_areaComb.Elements.Add(tradeElement);

	// Suscribirse a eventos de recepción de órdenes y operaciones
	_connector.OrderReceived += (subscription, order) =>
	{
		if (order.Security != _security)
			return;

		// Dibujar orden en el gráfico
		var chartData = new ChartDrawData();
		chartData.Group(order.Time).Add(orderElement, order);

		this.GuiAsync(() => _chart.Draw(chartData));
	};

	_connector.OwnTradeReceived += (subscription, trade) =>
	{
		if (trade.Order.Security != _security)
			return;

		// Dibujar operación en el gráfico
		var chartData = new ChartDrawData();
		chartData.Group(trade.Time).Add(tradeElement, trade);

		this.GuiAsync(() => _chart.Draw(chartData));
	};
}
```

## Configuración de la apariencia del gráfico

Se pueden configurar varios aspectos de la apariencia del gráfico:

```cs
// Configurar apariencia del gráfico
private void ConfigureChartAppearance()
{
	// Configurar área del gráfico
	_areaComb.Height = 300;
	_areaComb.BackgroundMajorGridColor = Colors.Gray;
	_areaComb.BackgroundMinorGridColor = Colors.LightGray;

	// Configurar elemento de vela
	_candleElement.DrawStyle = ChartCandleDrawStyles.CandleStick;
	_candleElement.UpBrush = Brushes.Green;
	_candleElement.DownBrush = Brushes.Red;
	_candleElement.StrokeThickness = 1;

	// Configurar gráfico completo
	_chart.IsAutoRange = true;            // Automatic scaling
	_chart.IsManualVerticalValues = false; // Automatic calculation of vertical values
	_chart.BidEnabled = false;            // Disable display of best bid price
	_chart.AskEnabled = false;            // Disable display of best ask price
}
```

## Zoom y desplazamiento del gráfico

Gestión del zoom y desplazamiento del gráfico:

```cs
// Configurar zoom y desplazamiento
private void ConfigureChartZoomAndScroll()
{
	// Establecer fechas inicial y final para mostrar
	_chart.SetXRange(DateTime.Today.AddDays(-10), DateTime.Today);

	// Establecer rango del eje Y
	_chart.SetYRange(100, 150);

	// Botones para controlar el zoom
	zoomInButton.Click += (s, e) => _chart.ZoomIn();
	zoomOutButton.Click += (s, e) => _chart.ZoomOut();

	// Botones para desplazamiento
	scrollLeftButton.Click += (s, e) => _chart.ScrollLeft();
	scrollRightButton.Click += (s, e) => _chart.ScrollRight();

	// Restablecer zoom a automático
	resetZoomButton.Click += (s, e) => _chart.IsAutoRange = true;
}
```

## Exportación del gráfico a imagen

Para guardar el gráfico en un archivo:

```cs
// Exportar gráfico a imagen
private void ExportChartToImage()
{
	// Crear objeto para guardar la imagen
	var saveFileDialog = new SaveFileDialog
	{
		Filter = "PNG Image|*.png|JPEG Image|*.jpg|BMP Image|*.bmp",
		Title = "Save Chart Image"
	};

	if (saveFileDialog.ShowDialog() == true)
	{
		// Crear imagen desde el gráfico
		var rtb = new RenderTargetBitmap(
			(int)_chart.ActualWidth,
			(int)_chart.ActualHeight,
			96, 96,
			PixelFormats.Pbgra32);

		rtb.Render(_chart);

		// Guardar imagen en el formato seleccionado
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

## Limpieza del gráfico

Para limpiar los datos en el gráfico:

```cs
// Limpiar gráfico o sus elementos
private void ClearChart()
{
	// Limpiar gráfico completo
	_chart.Reset();

	// Limpiar área específica
	_areaComb.Reset();

	// Limpiar elemento específico
	_candleElement.Reset();
}
```

Un ejemplo de visualización de velas en un gráfico se proporciona en la sección [Velas](../candles.md).

## Véase también

[Componentes para construir gráficos](../graphical_user_interface/charts.md)