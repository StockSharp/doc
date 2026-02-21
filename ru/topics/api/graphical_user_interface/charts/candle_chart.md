# Свечной график

[Chart](xref:StockSharp.Xaml.Charting.Chart) - графический компонент, который позволяет строить биржевые графики: свечи, индикаторы и отображать на графиках маркеры заявок и сделок.

Ниже приведен пример построения графика при помощи компонента [Chart](xref:StockSharp.Xaml.Charting.Chart). За основу взят пример из Samples\/02\_Candles\/01\_Realtime, в который внесены некоторые изменения.

![Gui ChartSample](../../../../images/gui_chartsample.png)

## Пример построения графика при помощи Chart

1. В XAML создаем окно и добавляем в него графический компонент [Chart](xref:StockSharp.Xaml.Charting.Chart). Присваиваем компоненту имя **Chart**. Обратите внимание, что при создании окна нужно добавить пространство имен *http:\/\/schemas.stocksharp.com\/xaml*. 

   ```xaml
   <Window x:Class="SampleCandles.ChartWindow"
           xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
           xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
           xmlns:charting="http://schemas.stocksharp.com/xaml"
           Title="ChartWindow" Height="300" Width="300">
      <charting:Chart x:Name="Chart" x:FieldModifier="public" />
   </Window>
   ```

2. В коде главного окна декларируем переменные для областей графика, элементов графика, индикаторов и подписок. 

   ```cs
   private readonly Dictionary<Subscription, ChartWindow> _chartWindows = new Dictionary<Subscription, ChartWindow>();
   private readonly Connector _connector = new Connector();
   private readonly LogManager _logManager;
   private ChartArea _candlesArea;
   private ChartArea _indicatorsArea;
   private ChartIndicatorElement _smaChartElement;
   private ChartIndicatorElement _macdChartElement;
   private ChartCandleElement _candlesElem;
   private SimpleMovingAverage _sma;
   private MovingAverageConvergenceDivergence _macd;
   ```

3. В обработчике события **Click** кнопки **Connect**, наряду с подпиской на события коннектора и вызовом метода [IConnector.Connect](xref:StockSharp.BusinessEntities.IConnector.Connect), подписываемся на событие [Connector.CandleReceived](xref:StockSharp.Algo.Connector.CandleReceived). В обработчике этого события при получении новой свечи будет выполняться отрисовка графика. 

   ```cs
   private void ConnectClick(object sender, RoutedEventArgs e)
   {
       _connector.CandleReceived += OnCandleReceived;
       
       // Подписываемся на другие необходимые события
       _connector.Connected += () => this.GuiAsync(() => { /* Обработка подключения */ });
       _connector.Disconnected += () => this.GuiAsync(() => { /* Обработка отключения */ });
       
       // Подключаемся к торговой системе
       _connector.Connect();
   }
   ```

4. В обработчике кнопки **ShowChart** создаем объекты индикаторов, областей и элементов графика. Добавляем элементы к областям, а области к чарту. Открываем окно графика и запускаем подписку на свечи. 

   ```cs
   private void ShowChartClick(object sender, RoutedEventArgs e)
   {
       var security = SelectedSecurity;
       
       // Создаем подписку на свечи
       var subscription = new Subscription(
           TimeSpan.FromMinutes(5).TimeFrame(),
           security)
       {
           MarketData = 
           {
               // Запрашиваем исторические данные за 30 дней
               From = DateTime.Today.Subtract(TimeSpan.FromDays(30)),
               To = DateTime.Now,
               // Получаем только завершенные свечи
               IsFinishedOnly = true
           }
       };
       
       // Создаем окно графика
       _chartWindows.SafeAdd(subscription, key =>
       {
           var wnd = new ChartWindow
           {
               Title = $"{security.Code} {TimeSpan.FromMinutes(5)}"
           };
           wnd.MakeHideable();
           
           // Инициализируем индикаторы
           _sma = new SimpleMovingAverage() { Length = 11 };
           _macd = new MovingAverageConvergenceDivergence();
           
           // Инициализируем элементы графика
           _smaChartElement = new ChartIndicatorElement();
           _macdChartElement = new ChartIndicatorElement();
           _candlesElem = new ChartCandleElement();
           
           // Устанавливаем стиль отображения MACD в виде гистограммы
           _macdChartElement.DrawStyle = DrawStyles.Histogram;
           
           // Инициализируем области графика
           _candlesArea = new ChartArea();
           _indicatorsArea = new ChartArea();
           
           // Добавляем области к чарту
           wnd.Chart.Areas.Add(_candlesArea);
           wnd.Chart.Areas.Add(_indicatorsArea);
           
           // Добавляем элементы к областям
           _candlesArea.Elements.Add(_candlesElem);
           _candlesArea.Elements.Add(_smaChartElement);
           _indicatorsArea.Elements.Add(_macdChartElement);
           
           // Привязываем элементы графика к подписке для автоматической отрисовки
           wnd.Chart.AddElement(_candlesArea, _candlesElem, subscription);
           wnd.Chart.AddElement(_candlesArea, _smaChartElement, subscription);
           wnd.Chart.AddElement(_indicatorsArea, _macdChartElement, subscription);
           
           return wnd;
       }).Show();
       
       // Запускаем подписку на свечи
       _connector.Subscribe(subscription);
   }
   ```

5. В обработчике события [Connector.CandleReceived](xref:StockSharp.Algo.Connector.CandleReceived) производим отрисовку свечи и значений индикаторов для каждой завершенной свечи. 

   ```cs
   private void OnCandleReceived(Subscription subscription, ICandleMessage candle)
   {
       var wnd = _chartWindows.TryGetValue(subscription);
       if (wnd == null)
           return;
       
       // Обрабатываем только завершенные свечи
       if (candle.State != CandleStates.Finished)
           return;
       
       // Вычисляем значения индикаторов
       var smaValue = _sma.Process(candle);
       var macdValue = _macd.Process(candle);
       
       // Создаем данные для отрисовки
       var data = new ChartDrawData();
       data
           .Group(candle.OpenTime)
               .Add(_candlesElem, candle)
               .Add(_smaChartElement, smaValue)
               .Add(_macdChartElement, macdValue);
       
       // Отрисовываем данные на графике в потоке пользовательского интерфейса
       this.GuiAsync(() => wnd.Chart.Draw(data));
   }
   ```

## Пример с автоматической отрисовкой графика

Начиная с последних версий StockSharp, имеется возможность настроить автоматическую отрисовку графика без необходимости явного вызова метода Draw. Для этого при настройке Chart используется метод AddElement, который связывает элемент графика с подпиской:

```cs
private void SetupAutoDrawingChart()
{
	var security = SelectedSecurity;
	
	// Создаем элементы графика
	var candleElement = new ChartCandleElement();
	var smaElement = new ChartIndicatorElement { Title = "SMA" };
	
	// Создаем области графика
	var area = new ChartArea();
	
	// Добавляем область к графику
	Chart.Areas.Add(area);
	
	// Создаем подписку на свечи
	var subscription = new Subscription(
		TimeSpan.FromMinutes(5).TimeFrame(),
		security)
	{
		MarketData = 
		{
			From = DateTime.Today.Subtract(TimeSpan.FromDays(30)),
			To = DateTime.Now
		}
	};
	
	// Привязываем элементы к области графика и подписке
	Chart.AddElement(area, candleElement, subscription);
	Chart.AddElement(area, smaElement, subscription);
	
	// Создаем индикатор
	var sma = new SimpleMovingAverage { Length = 14 };
	
	// Подписываемся на событие получения свечей для обработки индикатором
	_connector.CandleReceived += (sub, candle) => 
	{
		if (sub == subscription && candle.State == CandleStates.Finished)
		{
			// Обрабатываем свечу индикатором и получаем значение
			var smaValue = sma.Process(candle);
			
			// Отрисовываем значение индикатора
			var data = new ChartDrawData();
			data
				.Group(candle.OpenTime)
					.Add(smaElement, smaValue);
			
			this.GuiAsync(() => Chart.Draw(data));
		}
	};
	
	// Запускаем подписку
	_connector.Subscribe(subscription);
}
```

## Отображение заявок и сделок на графике

Можно отображать маркеры заявок и сделок непосредственно на графике:

```cs
// Создаем элементы для отображения заявок и сделок
var orderElement = new ChartOrderElement();
var tradeElement = new ChartTradeElement();

// Добавляем элементы на область графика
_candlesArea.Elements.Add(orderElement);
_candlesArea.Elements.Add(tradeElement);

// Подписываемся на события получения заявок и сделок
_connector.OrderReceived += (sub, order) => 
{
	if (order.Security != _security)
		return;
	
	// Отрисовываем заявку на графике
	var data = new ChartDrawData();
	data.Group(order.Time).Add(orderElement, order);
	
	this.GuiAsync(() => Chart.Draw(data));
};

_connector.OwnTradeReceived += (sub, trade) => 
{
	if (trade.Order.Security != _security)
		return;
	
	// Отрисовываем сделку на графике
	var data = new ChartDrawData();
	data.Group(trade.Time).Add(tradeElement, trade);
	
	this.GuiAsync(() => Chart.Draw(data));
};
```

## Очистка графика

Для очистки графика можно использовать метод Reset:

```cs
// Очистка всего графика
Chart.Reset();

// Очистка конкретной области
_candlesArea.Reset();

// Очистка конкретного элемента
_candlesElem.Reset();
```