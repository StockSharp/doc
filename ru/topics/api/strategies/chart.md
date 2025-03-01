# Работа с графиком в стратегии

В StockSharp класс [Strategy](xref:StockSharp.Algo.Strategies.Strategy) предоставляет удобный интерфейс для визуализации торговой активности на графике. В этой статье мы рассмотрим, как получить доступ к графику из стратегии, как создавать области (ChartArea), добавлять различные элементы (свечи, индикаторы, сделки) и отрисовывать данные.

## Получение доступа к графику

### Метод GetChart

Для получения доступа к графику из стратегии используется метод [Strategy.GetChart()](xref:StockSharp.Algo.Strategies.Strategy.GetChart):

```cs
protected override void OnStarted(DateTimeOffset time)
{
    base.OnStarted(time);
    
    // Получение графика
    _chart = GetChart();
    
    // Проверка доступности графика
    if (_chart != null)
    {
        // Инициализация графика
        InitializeChart();
    }
    else
    {
        // График недоступен, например, при запуске в консольном режиме
        LogInfo("График недоступен. Визуализация отключена.");
    }
}
```

Метод [GetChart()](xref:StockSharp.Algo.Strategies.Strategy.GetChart) возвращает интерфейс [IChart](xref:StockSharp.Charting.IChart), который предоставляет доступ к функциям графика. Важно проверять результат на `null`, так как график может быть недоступен, например, при запуске стратегии в консольном режиме или в облачном тестировании.

### Метод SetChart

В некоторых случаях график может быть установлен извне. Для этого используется метод [Strategy.SetChart](xref:StockSharp.Algo.Strategies.Strategy.SetChart(StockSharp.Charting.IChart)):

```cs
// Установка графика из внешнего источника
public void ConfigureVisualization(IChart chart)
{
    SetChart(chart);
    
    if (chart != null)
    {
        InitializeChart();
    }
}
```

## Создание областей графика (ChartArea)

После получения доступа к графику можно создать одну или несколько областей для отображения различных данных. Для этого используется метод [CreateChartArea](xref:StockSharp.Algo.Strategies.Strategy.CreateChartArea):

```cs
private void InitializeChart()
{
    // Создание основной области для свечей и индикаторов
    _mainArea = CreateChartArea();
    
    // Создание дополнительной области для объема
    _volumeArea = CreateChartArea();
    
    // Настройка областей и добавление элементов
    ConfigureChartElements();
}
```

Также можно использовать метод [IChart.AddArea](xref:StockSharp.Charting.ChartingInterfacesExtensions.AddArea(StockSharp.Charting.IChart)) напрямую:

```cs
private void InitializeChart()
{
    // Очистка существующих областей, если необходимо
    foreach (var area in _chart.Areas.ToArray())
        _chart.RemoveArea(area);
    
    // Создание основной области для свечей и индикаторов
    _mainArea = _chart.AddArea();
    
    // Создание дополнительной области для объема
    _volumeArea = _chart.AddArea();
    
    // Настройка областей и добавление элементов
    ConfigureChartElements();
}
```

## Добавление элементов на график

После создания областей графика можно добавить различные элементы для отображения данных. StockSharp поддерживает различные типы элементов, такие как свечи, индикаторы, сделки и заявки.

### Добавление свечей

Для отображения свечей используется метод [AddCandles](xref:StockSharp.Charting.ChartingInterfacesExtensions.AddCandles(StockSharp.Charting.IChartArea)) области графика:

```cs
private void ConfigureChartElements()
{
    // Добавление элемента для отображения свечей в основной области
    _candleElement = _mainArea.AddCandles();
    
    // Настройка отображения свечей
    _candleElement.DrawStyle = ChartCandleDrawStyles.CandleStick; // Японские свечи
    _candleElement.AntiAliasing = true; // Сглаживание
    _candleElement.UpFillColor = Color.Green; // Цвет тела растущей свечи
    _candleElement.DownFillColor = Color.Red; // Цвет тела падающей свечи
    _candleElement.UpBorderColor = Color.DarkGreen; // Цвет границы растущей свечи
    _candleElement.DownBorderColor = Color.DarkRed; // Цвет границы падающей свечи
    _candleElement.StrokeThickness = 1; // Толщина линии
    _candleElement.ShowAxisMarker = true; // Показывать маркер на оси Y
}
```

Интерфейс [IChartCandleElement](xref:StockSharp.Charting.IChartCandleElement) предоставляет множество свойств для настройки отображения свечей:

- **DrawStyle** - стиль отображения свечей:
  - **CandleStick** - японские свечи
  - **Ohlc** - бары
  - **LineOpen/LineHigh/LineLow/LineClose** - линии по соответствующей цене
  - **BoxVolume** - объемные боксы
  - **ClusterProfile** - кластерный профиль
  - **Area** - область
  - **PnF** - график типа крестики-нолики

- **Цветовые настройки**:
  - **UpFillColor/DownFillColor** - цвет тела растущей/падающей свечи
  - **UpBorderColor/DownBorderColor** - цвет границы растущей/падающей свечи
  - **LineColor** - цвет линии для линейных типов графиков
  - **AreaColor** - цвет области для типа Area

- **Другие настройки**:
  - **StrokeThickness** - толщина линии
  - **AntiAliasing** - сглаживание
  - **ShowAxisMarker** - показывать маркер на оси Y

### Добавление индикаторов

Для отображения индикаторов используется метод [DrawIndicator](xref:StockSharp.Algo.Strategies.Strategy.DrawIndicator(StockSharp.Charting.IChartArea,StockSharp.Algo.Indicators.IIndicator,System.Nullable{System.Drawing.Color},System.Nullable{System.Drawing.Color})):

```cs
// Создание индикаторов
_sma = new SimpleMovingAverage { Length = SmaLength };
_bollinger = new BollingerBands
{
    Length = BollingerLength,
    Deviation = BollingerDeviation
};

// Добавление индикаторов в коллекцию стратегии
Indicators.Add(_sma);
Indicators.Add(_bollinger);

// Визуализация индикаторов
_smaElement = DrawIndicator(_mainArea, _sma, Color.Blue);
_bollingerUpperElement = DrawIndicator(_mainArea, _bollinger, Color.Purple);
_bollingerLowerElement = DrawIndicator(_mainArea, _bollinger, Color.Purple);
_bollingerMiddleElement = DrawIndicator(_mainArea, _bollinger, Color.Gray);
```

Метод [DrawIndicator](xref:StockSharp.Algo.Strategies.Strategy.DrawIndicator(StockSharp.Charting.IChartArea,StockSharp.Algo.Indicators.IIndicator,System.Nullable{System.Drawing.Color},System.Nullable{System.Drawing.Color})) автоматически создает элемент индикатора и добавляет его в указанную область графика. Можно указать цвет и дополнительный цвет для отображения.

Также можно добавить элемент индикатора напрямую через метод [AddIndicator](xref:StockSharp.Charting.ChartingInterfacesExtensions.AddIndicator(StockSharp.Charting.IChartArea,StockSharp.Algo.Indicators.IIndicator)) области графика:

```cs
// Добавление SMA непосредственно через область графика
var smaElement = _mainArea.AddIndicator(_sma);
smaElement.Color = Color.Blue;
smaElement.StrokeThickness = 2;
smaElement.DrawStyle = DrawStyles.Line;
smaElement.AntiAliasing = true;
smaElement.ShowAxisMarker = true;
smaElement.AutoAssignYAxis = true; // Автоматически назначить ось Y
```

Интерфейс [IChartIndicatorElement](xref:StockSharp.Charting.IChartIndicatorElement) предоставляет следующие свойства для настройки:

- **Color** - основной цвет индикатора
- **AdditionalColor** - дополнительный цвет (для индикаторов с двумя линиями)
- **StrokeThickness** - толщина линии
- **AntiAliasing** - сглаживание
- **DrawStyle** - стиль рисования (линия, точки, гистограмма и т.д.)
- **ShowAxisMarker** - показывать маркер на оси Y
- **AutoAssignYAxis** - автоматически назначить ось Y

### Добавление сделок

Для отображения сделок используется метод [DrawOwnTrades](xref:StockSharp.Algo.Strategies.Strategy.DrawOwnTrades(StockSharp.Charting.IChartArea)):

```cs
// Добавление элемента для отображения сделок
_tradesElement = DrawOwnTrades(_mainArea);

// Настройка отображения сделок
_tradesElement.BuyBrush = Color.Green;  // Цвет покупок
_tradesElement.SellBrush = Color.Red;   // Цвет продаж
_tradesElement.PointSize = 10;          // Размер точки
```

### Добавление заявок

Для отображения заявок используется метод [DrawOrders](xref:StockSharp.Algo.Strategies.Strategy.DrawOrders(StockSharp.Charting.IChartArea)):

```cs
// Добавление элемента для отображения заявок
_ordersElement = DrawOrders(_mainArea);

// Настройка отображения заявок
_ordersElement.ActiveBrush = Color.Blue;     // Цвет активных заявок
_ordersElement.CanceledBrush = Color.Gray;   // Цвет отмененных заявок
_ordersElement.DoneBrush = Color.Green;      // Цвет исполненных заявок
_ordersElement.ErrorColor = Color.Red;       // Цвет ошибок
_ordersElement.PointSize = 8;                // Размер точки
```

Интерфейс [IChartOrderElement](xref:StockSharp.Charting.IChartOrderElement) предоставляет следующие свойства для настройки:

- **ActiveBrush** - цвет активных заявок
- **CanceledBrush** - цвет отмененных заявок
- **DoneBrush** - цвет исполненных заявок
- **ErrorColor** - цвет ошибок
- **ErrorStrokeColor** - цвет обводки ошибок
- **Filter** - фильтр отображения заявок

## Отрисовка данных на графике

После настройки всех элементов графика можно приступить к отрисовке данных. Для этого используются различные методы в зависимости от типа данных.

### Отрисовка свечей и индикаторов

Наиболее эффективный способ отрисовки данных — использование метода [IChart.Draw](xref:StockSharp.Charting.IThemeableChart.Draw(StockSharp.Charting.IChartDrawData)) с объектом [IChartDrawData](xref:StockSharp.Charting.IChartDrawData):

```cs
private void ProcessCandle(ICandleMessage candle)
{
    // Обработка свечи в индикаторах
    var smaValue = _sma.Process(candle);
    var bollingerValue = _bollinger.Process(candle);
    
    // Если график недоступен, пропускаем отрисовку
    if (_chart == null)
        return;
    
    // Создаем данные для отрисовки
    var drawData = _chart.CreateData();
    
    // Группируем данные по времени свечи
    var group = drawData.Group(candle.OpenTime);
    
    // Добавляем свечу
    group.Add(_candleElement, 
        candle.DataType, 
        candle.SecurityId, 
        candle.OpenPrice, 
        candle.HighPrice, 
        candle.LowPrice, 
        candle.ClosePrice, 
        candle.PriceLevels, 
        candle.State);
    
    // Добавляем значения индикаторов
    group.Add(_smaElement, smaValue);
    
    if (bollingerValue != null)
    {
        group.Add(_bollingerUpperElement, bollingerValue);
        group.Add(_bollingerMiddleElement, bollingerValue);
        group.Add(_bollingerLowerElement, bollingerValue);
    }
    
    // Отрисовываем данные на графике
    _chart.Draw(drawData);
}
```

Метод [IChart.CreateData](xref:StockSharp.Charting.IThemeableChart.CreateData) создает объект [IChartDrawData](xref:StockSharp.Charting.IChartDrawData), который используется для группировки и добавления данных для разных элементов графика. Группировка данных производится по временной метке с помощью метода [Group](xref:StockSharp.Charting.IChartDrawData.Group(System.DateTimeOffset)).

Для добавления данных разного типа используются различные перегрузки метода [Add](xref:StockSharp.Charting.IChartDrawData.IChartDrawDataItem.Add(StockSharp.Charting.IChartCandleElement,StockSharp.Messages.DataType,StockSharp.Messages.SecurityId,System.Decimal,System.Decimal,System.Decimal,System.Decimal,StockSharp.Messages.CandlePriceLevel[],StockSharp.Messages.CandleStates)) объекта [IChartDrawDataItem](xref:StockSharp.Charting.IChartDrawData.IChartDrawDataItem).

### Отрисовка сделок и заявок

Для отрисовки сделок и заявок обычно используется автоматический механизм, который срабатывает при получении новых сделок или изменении заявок. Однако, если требуется отрисовать их вручную, можно использовать следующий код:

```cs
// Отрисовка сделки
var tradeDrawData = _chart.CreateData();
var tradeGroup = tradeDrawData.Group(trade.Time);
tradeGroup.Add(_tradesElement, trade.Id, trade.StringId, trade.Side, trade.Price, trade.Volume);
_chart.Draw(tradeDrawData);

// Отрисовка заявки
var orderDrawData = _chart.CreateData();
var orderGroup = orderDrawData.Group(order.Time);
orderGroup.Add(_ordersElement, order.Id, order.StringId, order.Side, order.Price, order.Volume);
_chart.Draw(orderDrawData);
```

## Полный пример отрисовки графика в стратегии

Ниже приведен полный пример стратегии с настройкой и отрисовкой графика:

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
    
    protected override void OnStarted(DateTimeOffset time)
    {
        base.OnStarted(time);
        
        // Создание индикаторов
        _sma = new SimpleMovingAverage { Length = SmaLength };
        _bollinger = new BollingerBands
        {
            Length = BollingerLength,
            Deviation = BollingerDeviation
        };
        
        // Добавление индикаторов в коллекцию стратегии
        Indicators.Add(_sma);
        Indicators.Add(_bollinger);
        
        // Получение графика
        _chart = GetChart();
        
        // Инициализация графика, если он доступен
        if (_chart != null)
        {
            InitializeChart();
        }
        
        // Подписка на свечи
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
        // Очистка существующих областей
        foreach (var area in _chart.Areas.ToArray())
            _chart.RemoveArea(area);
        
        // Создание основной области для свечей и индикаторов
        _mainArea = _chart.AddArea();
        
        // Создание дополнительной области для объема
        _volumeArea = _chart.AddArea();
        
        // Настройка элементов графика
        ConfigureChartElements();
    }
    
    private void ConfigureChartElements()
    {
        // Добавление элемента для отображения свечей
        _candleElement = _mainArea.AddCandles();
        _candleElement.DrawStyle = ChartCandleDrawStyles.CandleStick;
        _candleElement.AntiAliasing = true;
        _candleElement.UpFillColor = Color.Green;
        _candleElement.DownFillColor = Color.Red;
        _candleElement.UpBorderColor = Color.DarkGreen;
        _candleElement.DownBorderColor = Color.DarkRed;
        _candleElement.StrokeThickness = 1;
        _candleElement.ShowAxisMarker = true;
        
        // Добавление элементов для индикаторов
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
        
        // Добавление элементов для заявок и сделок
        _ordersElement = DrawOrders(_mainArea);
        _tradesElement = DrawOwnTrades(_mainArea);
    }
    
    private void ProcessCandle(ICandleMessage candle)
    {
        // Обработка свечи индикаторами
        var smaValue = _sma.Process(candle);
        var bollingerValue = _bollinger.Process(candle);
        
        // Если график недоступен, пропускаем отрисовку
        if (_chart == null)
            return;
        
        // Отрисовка данных на графике
        var drawData = _chart.CreateData();
        var group = drawData.Group(candle.OpenTime);
        
        // Добавление свечи
        group.Add(_candleElement, 
            candle.DataType, 
            candle.SecurityId, 
            candle.OpenPrice, 
            candle.HighPrice, 
            candle.LowPrice, 
            candle.ClosePrice, 
            candle.PriceLevels, 
            candle.State);
        
        // Добавление значений индикаторов
        group.Add(_smaElement, smaValue);
        
        if (bollingerValue != null)
        {
            group.Add(_bollingerUpperElement, bollingerValue);
            group.Add(_bollingerMiddleElement, bollingerValue);
            group.Add(_bollingerLowerElement, bollingerValue);
        }
        
        // Отрисовка данных на графике
        _chart.Draw(drawData);
        
        // Торговая логика
        if (!IsFormed)
            return;
            
        // ... реализация торговой логики ...
    }
}
```

## Заключение

Использование графиков в стратегиях StockSharp позволяет визуализировать торговую активность, что значительно упрощает разработку, отладку и мониторинг торговых стратегий. Класс [Strategy](xref:StockSharp.Algo.Strategies.Strategy) предоставляет множество методов для работы с графиками, позволяющих легко добавлять различные элементы и отрисовывать данные.

При разработке стратегии с графическим интерфейсом всегда следует учитывать, что график может быть недоступен, например, при запуске в консольном режиме или в облачном тестировании. Поэтому важно проверять результат метода [GetChart()](xref:StockSharp.Algo.Strategies.Strategy.GetChart) на `null` и предусматривать альтернативный сценарий работы стратегии без визуализации.

## См. также

- [Индикаторы в стратегии](indicators.md)
- [Торговые операции в стратегиях](trading_operations.md)
