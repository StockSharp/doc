# Создание свечей по корзине инструментов

Для создания свечей [ContinuousSecurity](xref:StockSharp.Algo.ContinuousSecurity), [WeightedIndexSecurity](xref:StockSharp.Algo.WeightedIndexSecurity) или [ExpressionIndexSecurity](xref:StockSharp.Algo.Expressions.ExpressionIndexSecurity) используется тот же механизм подписок, что и для обычных инструментов [Security](xref:StockSharp.BusinessEntities.Security).

Ниже приведен пример создания 1-минутных свечей для спреда GZM5 - LKM5:

```cs
private Connector _connector;
private Security _instr1;
private Security _instr2;
private WeightedIndexSecurity _indexInstr;
private Subscription _indexSubscription;
private const string _secCode1 = "GZM5";
private const string _secCode2 = "LKM5";
readonly TimeSpan _timeFrame = TimeSpan.FromMinutes(1);
private ChartArea _area;
private ChartCandleElement _candleElement;

// Настройка подключения и конфигурация коннектора
private void ConfigureConnector()
{
    if (_connector.Configure(this))
    {
        _connector.Save().Serialize(_connectorFile);
    }
}

// Настройка графика
private void SetupChart()
{
    _area = new ChartArea();
    _chart.Areas.Add(_area);
    _candleElement = new ChartCandleElement();
    _area.Elements.Add(_candleElement);
    
    // Подписываемся на событие получения свечей
    _connector.CandleReceived += OnCandleReceived;
}

// Настройка сервисов
private void RegisterServices()
{
    ConfigManager.RegisterService<ISecurityProvider>(_connector);
    ConfigManager.RegisterService<ICompilerService>(new RoslynCompilerService());
}

// Создание индексного инструмента и подписка на свечи
private void CreateIndexAndSubscribe()
{
    // Создаем индексный инструмент (спред)
    _indexInstr = new WeightedIndexSecurity() 
    { 
        Board = ExchangeBoard.Nyse, 
        Id = "IndexInstr" 
    };
    
    // Добавляем инструменты с весами (1 и -1 для спреда)
    _indexInstr.Weights.Add(_instr1, 1);
    _indexInstr.Weights.Add(_instr2, -1);
    
    // Создаем подписку на свечи индексного инструмента
    _indexSubscription = new Subscription(
        DataType.TimeFrame(_timeFrame),  // 1-минутные свечи
        _indexInstr)  // Наш индексный инструмент
    {
        MarketData = 
        {
            // Настраиваем подписку для построения свечей из тиков
            BuildMode = MarketDataBuildModes.Build,
            BuildFrom = DataType.Ticks,
            
            // Запрашиваем исторические данные за 30 дней
            From = DateTime.Today.Subtract(TimeSpan.FromDays(30)),
            To = DateTime.Now
        }
    };
    
    // Добавляем элемент на график и привязываем его к подписке
    _chart.AddElement(_area, _candleElement, _indexSubscription);
    
    // Запускаем подписку
    _connector.Subscribe(_indexSubscription);
}

// Обработчик события получения свечи
private void OnCandleReceived(Subscription subscription, ICandleMessage candle)
{
    // Проверяем, относится ли свеча к нашей подписке
    if (subscription != _indexSubscription)
        return;
    
    // Если нужно, ограничиваем обработку только завершенными свечами
    if (candle.State != CandleStates.Finished)
        return;
    
    // Отрисовываем свечу на графике
    var chartData = new ChartDrawData();
    chartData.Group(candle.OpenTime).Add(_candleElement, candle);
    
    this.GuiAsync(() => _chart.Draw(chartData));
}

// Отписка при закрытии приложения
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

## Другие варианты использования подписок для индексов

### Создание подписки на свечи индекса из свечей компонентов

```cs
// Создаем подписку для построения свечей индекса из свечей компонентов
var indexFromCandlesSubscription = new Subscription(
    DataType.TimeFrame(TimeSpan.FromMinutes(5)),
    _indexInstr)
{
    MarketData = 
    {
        // Настраиваем подписку для построения из свечей компонентов
        BuildMode = MarketDataBuildModes.Build,
        BuildFrom = DataType.TimeFrame(TimeSpan.FromMinutes(5)),
        From = DateTime.Today.Subtract(TimeSpan.FromDays(30)),
        To = DateTime.Now
    }
};

// Запускаем подписку
_connector.Subscribe(indexFromCandlesSubscription);
```

### Создание подписки на свечи индекса из стаканов

```cs
// Создаем подписку для построения свечей индекса из стаканов
var indexFromDepthSubscription = new Subscription(
    DataType.TimeFrame(TimeSpan.FromMinutes(1)),
    _indexInstr)
{
    MarketData = 
    {
        // Настраиваем подписку для построения из стаканов
        BuildMode = MarketDataBuildModes.Build,
        BuildFrom = DataType.MarketDepth,
        BuildField = Level1Fields.SpreadMiddle,  // Используем середину спреда
        From = DateTime.Today.Subtract(TimeSpan.FromDays(7)),
        To = DateTime.Now
    }
};

// Запускаем подписку
_connector.Subscribe(indexFromDepthSubscription);
```

### Работа с индексом волатильности

```cs
// Создаем индекс волатильности на основе экспрессии
var volatilityIndex = new ExpressionIndexSecurity
{
    Board = ExchangeBoard.Nyse,
    Id = "VOLX",
    Expression = "StdDev({0}, 20) / SMA({0}, 20) * 100",  // Формула вычисления волатильности
};

// Добавляем основной инструмент в индекс
volatilityIndex.InnerSecurityIds.Add(_instr1.ToSecurityId());

// Создаем подписку на свечи индекса волатильности
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

// Запускаем подписку
_connector.Subscribe(volatilitySubscription);
```

## См. также

[Непрерывный фьючерс](../instruments/continuous_futures.md)

[Индекс](../instruments/index.md)
