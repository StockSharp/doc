# График

Для графического отображения свечей можно использовать специальный компонент [Chart](xref:StockSharp.Xaml.Charting.Chart) (см. [Компоненты для построения графиков](../graphical_user_interface/charts.md)), который отрисовывает свечи следующим образом:

![sample candleschart](../../../images/sample_candleschart.png)

## Базовый подход к отображению свечей

Для отображения свечей на графике можно использовать два подхода. Первый подход - ручная отрисовка свечей при получении данных:

```cs
private IChartArea _areaComb;
private IChartCandleElement _candleElement;

// Инициализация графика
private void InitializeChart()
{
	// Создаем область графика
	_areaComb = _chart.CreateArea();
	_chart.AddArea(_areaComb);

	// Создаем элемент для отображения свечей
	_candleElement = _chart.CreateCandleElement();
	_candleElement.FullTitle = "Candles";
	_chart.AddElement(_areaComb, _candleElement);
	
	// Подписываемся на событие получения свечей
	_connector.CandleReceived += OnCandleReceived;
}

// Создаем подписку на 5-минутные свечи
private void SubscribeToCandles()
{
	var subscription = new Subscription(
		TimeSpan.FromMinutes(5).TimeFrame(),
		_security)
	{
		MarketData = 
		{
			// Запрашиваем исторические данные за 5 дней
			From = DateTime.Today.Subtract(TimeSpan.FromDays(5)),
			To = DateTime.Now
		}
	};
	
	// Запускаем подписку
	_connector.Subscribe(subscription);
}

// Обработчик события получения свечи
private void OnCandleReceived(Subscription subscription, ICandleMessage candle)
{
	// Проверяем, завершена ли свеча
	if (candle.State == CandleStates.Finished) 
	{
		// Создаем данные для отрисовки
		var chartData = _chart.CreateData();
		chartData.Group(candle.OpenTime).Add(_candleElement, candle);

		// Отрисовываем на графике в потоке UI
		this.GuiAsync(() => _chart.Draw(chartData));
	}
}
```

## Автоматическая привязка подписки к элементу графика

Второй подход - это использование автоматической привязки подписки к элементу графика. Это позволяет автоматически отображать получаемые данные:

```cs
// Инициализация графика с автоматической привязкой
private void InitializeChartWithAutoBinding()
{
	// Создаем область графика
	var area = _chart.CreateArea();
	_chart.AddArea(area);

	// Создаем элемент для отображения свечей
	var candleElement = _chart.CreateCandleElement();
	
	// Создаем подписку на свечи
	var subscription = new Subscription(
		TimeSpan.FromMinutes(5).TimeFrame(),
		_security)
	{
		MarketData = 
		{
			From = DateTime.Today.Subtract(TimeSpan.FromDays(5)),
			To = DateTime.Now
		}
	};
	
	// Привязываем элемент к подписке
	_chart.AddElement(area, candleElement, subscription);
	
	// Запускаем подписку
	_connector.Subscribe(subscription);
}
```

## Работа с индикаторами

Для отображения индикаторов на графике вместе со свечами используются элементы типа [ChartIndicatorElement](xref:StockSharp.Xaml.Charting.ChartIndicatorElement):

```cs
// Добавление индикатора на график
private void AddIndicatorToChart()
{
	// Создаем элемент для индикатора
	var smaElement = _chart.CreateIndicatorElement();
	smaElement.FullTitle = "SMA (14)";

	// Добавляем элемент на ту же область, что и свечи
	_chart.AddElement(_areaComb, smaElement);
	
	// Создаем индикатор
	var sma = new SimpleMovingAverage { Length = 14 };
	
	// Подписываемся на событие получения свечей для расчета индикатора
	_connector.CandleReceived += (subscription, candle) =>
	{
		// Вычисляем значение индикатора
		var indicatorValue = sma.Process(candle);
		
		// Отрисовываем значение на графике
		var chartData = _chart.CreateData();
		chartData.Group(candle.OpenTime).Add(smaElement, indicatorValue);

		this.GuiAsync(() => _chart.Draw(chartData));
	};
}
```

## Отображение нескольких индикаторов на разных областях

Можно размещать индикаторы на отдельных областях графика:

```cs
// Добавление индикаторов на разные области
private void AddIndicatorsToSeparateAreas()
{
	// Основная область для свечей
	var candleArea = _chart.CreateArea();
	_chart.AddArea(candleArea);

	// Элемент для свечей
	var candleElement = _chart.CreateCandleElement();
	_chart.AddElement(candleArea, candleElement);

	// Элемент для SMA на той же области
	var smaElement = _chart.CreateIndicatorElement();
	smaElement.FullTitle = "SMA (14)";
	_chart.AddElement(candleArea, smaElement);

	// Отдельная область для RSI
	var rsiArea = _chart.CreateArea();
	_chart.AddArea(rsiArea);

	// Элемент для RSI
	var rsiElement = _chart.CreateIndicatorElement();
	rsiElement.FullTitle = "RSI (14)";
	_chart.AddElement(rsiArea, rsiElement);
	
	// Создаем индикаторы
	var sma = new SimpleMovingAverage { Length = 14 };
	var rsi = new RelativeStrengthIndex { Length = 14 };
	
	// Подписка на свечи
	var subscription = new Subscription(
		TimeSpan.FromMinutes(5).TimeFrame(),
		_security);
	
	// Привязываем элемент свечей к подписке
	_chart.AddElement(candleArea, candleElement, subscription);
	
	// Запускаем подписку и обрабатываем индикаторы
	_connector.Subscribe(subscription);
	
	_connector.CandleReceived += (sub, candle) =>
	{
		if (sub != subscription || candle.State != CandleStates.Finished)
			return;
		
		// Вычисляем значения индикаторов
		var smaValue = sma.Process(candle);
		var rsiValue = rsi.Process(candle);
		
		// Отрисовываем значения на графике
		var chartData = _chart.CreateData();
		chartData
			.Group(candle.OpenTime)
				.Add(smaElement, smaValue)
				.Add(rsiElement, rsiValue);

		this.GuiAsync(() => _chart.Draw(chartData));
	};
}
```

## Отображение заявок и сделок на графике

Для отображения заявок и сделок на графике используются специальные элементы:

```cs
// Добавление элементов для отображения заявок и сделок
private void AddOrdersAndTradesToChart()
{
	// Создаем элементы для отображения заявок и сделок
	var orderElement = _chart.CreateOrderElement();
	var tradeElement = _chart.CreateTradeElement();

	// Добавляем элементы на область графика
	_chart.AddElement(_areaComb, orderElement);
	_chart.AddElement(_areaComb, tradeElement);
	
	// Подписываемся на события получения заявок и сделок
	_connector.OrderReceived += (subscription, order) =>
	{
		if (order.Security != _security)
			return;
		
		// Отрисовываем заявку на графике
		var chartData = _chart.CreateData();
		chartData.Group(order.Time).Add(orderElement, order);

		this.GuiAsync(() => _chart.Draw(chartData));
	};
	
	_connector.OwnTradeReceived += (subscription, trade) =>
	{
		if (trade.Order.Security != _security)
			return;
		
		// Отрисовываем сделку на графике
		var chartData = _chart.CreateData();
		chartData.Group(trade.Time).Add(tradeElement, trade);

		this.GuiAsync(() => _chart.Draw(chartData));
	};
}
```

## Настройка внешнего вида графика

Можно настроить различные аспекты внешнего вида графика:

```cs
// Настройка внешнего вида графика
private void ConfigureChartAppearance()
{
	// Настройка области графика
	_areaComb.Height = 300;

	// Настройка элемента свечей
	_candleElement.DrawStyle = ChartCandleDrawStyles.CandleStick;
	_candleElement.UpFillColor = Colors.Green;
	_candleElement.DownFillColor = Colors.Red;
	_candleElement.StrokeThickness = 1;

	// Настройка всего графика
	_chart.IsAutoRange = true;            // Автоматический масштаб
}
```

## Масштабирование и прокрутка графика

Управление масштабом и прокруткой графика осуществляется через свойства [IChart](xref:StockSharp.Charting.IChart):

```cs
// Настройка масштабирования и прокрутки
private void ConfigureChartZoomAndScroll()
{
	// Включаем автоматическую прокрутку при поступлении новых данных
	_chart.IsAutoScroll = true;

	// Сброс масштаба к автоматическому
	_chart.IsAutoRange = true;
}
```

## Экспорт графика в изображение

Для сохранения графика в файл:

```cs
// Экспорт графика в изображение
private void ExportChartToImage()
{
	// Создаем объект для сохранения изображения
	var saveFileDialog = new SaveFileDialog
	{
		Filter = "PNG Image|*.png|JPEG Image|*.jpg|BMP Image|*.bmp",
		Title = "Save Chart Image"
	};
	
	if (saveFileDialog.ShowDialog() == true)
	{
		// Создаем изображение из графика
		var rtb = new RenderTargetBitmap(
			(int)_chart.ActualWidth, 
			(int)_chart.ActualHeight, 
			96, 96, 
			PixelFormats.Pbgra32);
		
		rtb.Render(_chart);
		
		// Сохраняем изображение в выбранный формат
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

## Очистка графика

Для очистки данных на графике:

```cs
// Очистка графика или его элементов
private void ClearChart()
{
	// Очистка всех элементов графика
	_chart.Reset(_chart.GetElements());

	// Удаление всех областей с графика
	_chart.ClearAreas();
}
```

Пример отображения свечей на графике приведен в пункте [Свечи](../candles.md).

## См. также

[Компоненты для построения графиков](../graphical_user_interface/charts.md)