# Свечной график

[Chart](xref:StockSharp.Xaml.Charting.Chart) \- графический компонет, который позволяет строить биржевые графики: свечи, индикаторы, и отображать на графиках маркеры заявок и сделок. 

Ниже приведен пример построения графика при помощи компонета [Chart](xref:StockSharp.Xaml.Charting.Chart). За основу взят пример из Samples\/Common\/SampleConnection, в который внесены некоторые изменения. 

![Gui ChartSample](../images/Gui_ChartSample.png)

### Пример построения графика при помощи Chart

Пример построения графика при помощи Chart

1. В XAML создаем окно и добавляем в него графический компонент [StockSharp.Xaml.Charting.Chart](xref:StockSharp.Xaml.Charting.Chart). Присваиваем компоненту имя **Chart**. Обратите внимание, что при создании окна нужно добавить пространство имен *http:\/\/schemas.stocksharp.com\/xaml*. 

   ```xaml
   <Window x:Class="SampleCandles.ChartWindow"
           xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
           xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
           xmlns:charting="http://schemas.stocksharp.com/xaml"
           Title="ChartWindow" Height="300" Width="300">
      <charting:Chart x:Name="Chart" x:FieldModifier="public" />
   </Window>
   	  				
   ```
2. В коде главного окна декларируем переменные для областей графика, элементов графика и индикаторов. 

   ```cs
                 		
   private readonly Dictionary<CandleSeries, ChartWindow> _chartWindows = new Dictionary<CandleSeries, ChartWindow>();
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
3. В обработчике события **Click** кнопки **Connect**, наряду с подпиской на события коннектора и вызовом метода [Connect](xref:StockSharp.BusinessEntities.IConnector.Connect), подписываемся на событие [Connector.CandleSeriesProcessing](xref:StockSharp.Algo.Connector.CandleSeriesProcessing). В обработчике этого события при получении новой свечи будет выполнятся отрисовка графика. 

   ```cs
                 		
   private void ConnectClick(object sender, RoutedEventArgs e)
   {
   		..........................................		 
   		_connector.CandleSeriesProcessing += DrawCandle;
   {
   		..........................................		 
   }
   	  				
   ```
4. В обработчике кнопки **ShowChart** создаем объекты индикаторов, областей и элементов графика. Добавляем элементы к областям, а области к чарту. Открываем окно графика и запускаем работу кандлменеджера. 

   ```cs
   private void ShowChartClick(object sender, RoutedEventArgs e)
   {
   	var security = SelectedSecurity;
   	var series = new CandleSeries(CandlesSettings.Settings.CandleType, security, CandlesSettings.Settings.Arg);
   	_chartWindows.SafeAdd(series, key =>
   	{
   		var wnd = new ChartWindow
   		{
   			Title = "{0} {1} {2}".Put(security.Code, series.CandleType.Name, series.Arg)
   		};
   		wnd.MakeHideable();
   		// инициализируем индикаторы
           _sma = new SimpleMovingAverage() { Length = 11 };
           _macd = new MovingAverageConvergenceDivergence();
   		// инициализируем элементы графика
           _smaChartElement = new ChartIndicatorElement();
           _macdChartElement = new ChartIndicatorElement();
           _candlesElem = new ChartCandleElement();
   		// устанавливаем стиль отображения MACD в виде гистограммы
           _macdChartElement.DrawStyle = ChartIndicatorDrawStyles.Histogram;
   		// инициализируем области графика
           _candlesArea = new ChartArea();
           _indicatorsArea = new ChartArea();
   		// добавляем области к чарту
           wnd.Chart.Areas.Add(_candlesArea);
           wnd.Chart.Areas.Add(_indicatorsArea);
   		// добавляем элементы к областям
           _candlesArea.Elements.Add(_candlesElem);
           _candlesArea.Elements.Add(_smaChartElement);
           _indicatorsArea.Elements.Add(_macdChartElement);
           return wnd;
   	}).Show();
   	_connector.SubscribeCandles(series, DateTime.Today.Subtract(TimeSpan.FromDays(30)), DateTime.Now);
   }
   	  				
   ```
5. В обработчике события [Connector.CandleSeriesProcessing](xref:StockSharp.Algo.Connector.CandleSeriesProcessing) производим отрисовку свечи и значений индикаторов для каждой завершенной свечи. Для этого: 
   1. Вычисляем значения индикаторов.
   2. Заполняем словарь **elements** парами "объект элемента \- значение элемента"
   3. Для отрисовки графика вызываем метод [Chart.Draw](xref:StockSharp.Xaml.Charting.Chart.Draw), в который передаем время и словарь элементов.

   Результат работы программы представлен на рисунке выше. 

   ```cs
   private void DrawCandle(CandleSeries series, Candle candle)
   {
   	var wnd = _chartWindows.TryGetValue(series);
   	if (wnd != null)
   	{
   		if (candle.State != CandleStates.Finished)
   			return;
   		var smaValue = _sma.Process(candle);
   		var macdValue = _macd.Process(candle);
   		var data = new ChartDrawData();
   		data
   		  .Group(candle.OpenTime)
   		    .Add(_candlesElem, candle)
   		    .Add(_smaChartElement, smaValue)
   		    .Add(_macdChartElement, macdValue);
           	wnd.Chart.Draw(data);
                  
   	}
   }
   ```
