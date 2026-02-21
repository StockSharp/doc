# Совместимость стратегий с платформами StockSharp

При разработке торговых стратегий в StockSharp важно учитывать их совместимость с различными платформами: [Designer](../../designer.md), [Shell](../../shell.md), [Runner](../../runner.md) и [облачным тестированием](../../designer/backtesting/cloud_backtesting.md). Следуя приведенным ниже рекомендациям, вы создадите стратегию, которая будет корректно работать во всех средах.

## Параметры в конструкторе стратегии

### Избегайте параметров в конструкторе

Для обеспечения совместимости с платформами StockSharp, особенно с облачным тестированием, **не следует добавлять параметры в конструктор стратегии**:

```cs
// Правильно: конструктор без параметров
public class SmaStrategy : Strategy
{
	public SmaStrategy()
	{
		// Инициализация параметров
	}
}

// Неправильно: конструктор с параметрами
public class SmaStrategy : Strategy
{
	public SmaStrategy(int longLength, int shortLength) // Не используйте такой подход
	{
		// ...
	}
}
```

Платформы StockSharp создают экземпляры стратегий, используя конструктор без параметров. Если ваша стратегия требует конструктор с параметрами, она не сможет быть корректно инициализирована.

## Использование StrategyParam вместо обычных свойств

### Преимущества StrategyParam

Вместо создания обычных свойств C# с последующим переопределением методов `Save` и `Load`, используйте [StrategyParam\<T\>](xref:StockSharp.Algo.Strategies.StrategyParam`1) для всех настраиваемых параметров:

```cs
// Правильно: использование StrategyParam
private readonly StrategyParam<int> _longSmaLength;

public int LongSmaLength
{
	get => _longSmaLength.Value;
	set => _longSmaLength.Value = value;
}

public SmaStrategy()
{
	_longSmaLength = Param(nameof(LongSmaLength), 80)
						.SetDisplay("Long SMA length", string.Empty, "Base settings");
}

// Неправильно: использование обычных свойств
private int _longSmaLength = 80; // Не используйте такой подход

public int LongSmaLength
{
	get => _longSmaLength;
	set => _longSmaLength = value;
}
```

Параметры, созданные через [StrategyParam\<T\>](xref:StockSharp.Algo.Strategies.StrategyParam`1), будут автоматически:
- Отображаться в пользовательском интерфейсе платформ
- Сохраняться и загружаться без переопределения методов `Save` и `Load`
- Использоваться в оптимизации
- Корректно сериализоваться при отправке в облачное тестирование

## Работа с графическим интерфейсом

### Используйте абстракции вместо прямого обращения к UI

Вместо прямого обращения к элементам пользовательского интерфейса, используйте абстракции, предоставляемые StockSharp:

```cs
// Правильный подход: использование IChart
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);

	// Получаем график, предоставленный средой выполнения
	_chart = GetChart();
	
	if (_chart != null)
	{
		// График доступен (например, в Designer или Shell)
		InitChart();
	}
	else
	{
		// График недоступен (например, в Runner или облачном тестировании)
		// Стратегия продолжает работу без визуализации
	}
}

private void InitChart()
{
	// Настройка графика через абстрактный интерфейс
	_chart.ClearAreas();
	var area = _chart.AddArea();
	_chartCandleElement = area.AddCandles();
	// ...
}
```

Метод [Strategy.GetChart()](xref:StockSharp.Algo.Strategies.Strategy.GetChart) возвращает интерфейс [IChart](xref:StockSharp.Charting.IChart), если график доступен в текущей среде выполнения. Если стратегия запущена в консольном [Runner](../../runner.md) или облачном тестировании, где нет графического интерфейса, метод вернет `null`.

Интерфейс [IChart](xref:StockSharp.Charting.IChart) предоставляет методы для работы с графиком:
- [AddArea](xref:StockSharp.Charting.IChart.AddArea(StockSharp.Charting.IChartArea)) - для добавления области на график
- [RemoveArea](xref:StockSharp.Charting.IChart.RemoveArea(StockSharp.Charting.IChartArea)) - для удаления области
- [AddElement](xref:StockSharp.Charting.IChart.AddElement(StockSharp.Charting.IChartArea,StockSharp.Charting.IChartElement)) - для добавления элемента на график
- [RemoveElement](xref:StockSharp.Charting.IChart.RemoveElement(StockSharp.Charting.IChartArea,StockSharp.Charting.IChartElement)) - для удаления элемента
- [Reset](xref:StockSharp.Charting.IChart.Reset(System.Collections.Generic.IEnumerable{StockSharp.Charting.IChartElement})) - для сброса значений элементов

### Проверяйте доступность графика

Всегда проверяйте доступность графика перед его использованием:

```cs
private void DrawCandlesAndIndicators(ICandleMessage candle, IIndicatorValue longSma, IIndicatorValue shortSma)
{
	if (_chart == null) return; // Важная проверка
	
	var data = _chart.CreateData();
	data.Group(candle.OpenTime)
		.Add(_chartCandleElement, candle)
		.Add(_longSmaIndicatorElement, longSma)
		.Add(_shortSmaIndicatorElement, shortSma);
	_chart.Draw(data);
}
```

## Потоки и синхронизация

### Избегайте создания дополнительных потоков

В StockSharp **не требуется создавать дополнительные потоки** для обработки данных. Все события (маркет-данные, транзакции) приходят в одном потоке:

```cs
// Правильно: использование стандартных обработчиков событий
private void ProcessCandle(ICandleMessage candle)
{
	// Обработка свечи в основном потоке
	var longSmaIsFormedPrev = _longSma.IsFormed;
	var ls = _longSma.Process(candle);
	var ss = _shortSma.Process(candle);
	
	// ...
}

// Неправильно: создание дополнительных потоков
private void ProcessCandle(ICandleMessage candle)
{
	// НЕ делайте так
	Task.Run(() => {
		var longSmaIsFormedPrev = _longSma.IsFormed;
		// ...
	});
}
```

### Избегайте объектов синхронизации

Поскольку все события обрабатываются в одном потоке, **нет необходимости в использовании объектов синхронизации**:

```cs
// Правильно: обычная обработка без синхронизации
private void ProcessCandle(ICandleMessage candle)
{
	var ls = _longSma.Process(candle);
	var ss = _shortSma.Process(candle);
	// ...
}

// Неправильно: излишняя синхронизация
private readonly object _syncLock = new object(); // Не нужно

private void ProcessCandle(ICandleMessage candle)
{
	lock (_syncLock) // Не нужно
	{
		var ls = _longSma.Process(candle);
		// ...
	}
}
```

## Внешние ресурсы

### Используйте инфраструктуру StockSharp

Вместо прямого обращения к внешним ресурсам (файлы, базы данных, сеть), используйте возможности, предоставляемые платформами StockSharp:

```cs
// Правильно: использование встроенных механизмов для сохранения данных
protected override void OnStopped()
{
	// Данные сохраняются автоматически через параметры стратегии
	base.OnStopped();
}

// Неправильно: прямое обращение к внешним ресурсам
protected override void OnStopped()
{
	// НЕ делайте так
	File.WriteAllText("results.txt", $"PnL: {PnL}");
	
	// или так
	using (var connection = new SqlConnection("..."))
	{
		// ...
	}
	
	base.OnStopped();
}
```

### Хранение данных

Для сохранения результатов работы стратегии используйте:

- [Параметры стратегии](parameters.md) для настроек и конфигурации
- Встроенные механизмы хранения в [Designer](../../designer.md) и [Shell](../../shell.md)
- [Статистику](xref:StockSharp.Algo.Statistics.StatisticManager) для сбора метрик торговли

### Save и Load методы

Методы [Strategy.Save](xref:StockSharp.Algo.Strategies.Strategy.Save(Ecng.Serialization.SettingsStorage)) и [Strategy.Load](xref:StockSharp.Algo.Strategies.Strategy.Load(Ecng.Serialization.SettingsStorage)) специально предназначены для сохранения дополнительных данных стратегии, которые не являются настройками или параметрами. Это идеальное место для сохранения данных, необходимых для восстановления состояния стратегии:

```cs
public override void Save(SettingsStorage settings)
{
	base.Save(settings); // Сначала сохраняем параметры стратегии
	
	// Затем сохраняем собственные данные
	settings.SetValue("CustomState", _customState);
	settings.SetValue("LastSignalTime", _lastSignalTime);
}

public override void Load(SettingsStorage settings)
{
	base.Load(settings); // Сначала загружаем параметры стратегии
	
	// Затем загружаем собственные данные
	if (settings.Contains("CustomState"))
		_customState = settings.GetValue<string>("CustomState");
	
	if (settings.Contains("LastSignalTime"))
		_lastSignalTime = settings.GetValue<DateTimeOffset>("LastSignalTime");
}
```

При этом основные настраиваемые параметры всё равно следует реализовывать через [StrategyParam\<T\>](xref:StockSharp.Algo.Strategies.StrategyParam`1), как описано выше, поскольку это обеспечит их автоматическое отображение в пользовательском интерфейсе.

## Подписка на маркет-данные

### Используйте правила вместо прямой подписки

Для обработки маркет-данных рекомендуется использовать [Событийную модель](event_model.md) и правила:

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);

	_shortSma = new SimpleMovingAverage { Length = ShortSmaLength };
	_longSma = new SimpleMovingAverage { Length = LongSmaLength };

	Indicators.Add(_shortSma);
	Indicators.Add(_longSma);

	var subscription = new Subscription(Series, Security);

	// Правильно: использование правил для обработки данных
	Connector
		.WhenCandlesFinished(subscription)
		.Do(ProcessCandle)
		.Apply(this);

	Subscribe(subscription);
}
```

Правила имеют несколько важных преимуществ перед обычными обработчиками событий:

1. **Автоматическая отписка** - правила сами отписываются от событий, когда стратегия останавливается или когда они больше не нужны. Вам не нужно вручную управлять подписками.

2. **Высокоуровневый API** - правила предоставляют более понятный и удобный интерфейс, чем стандартные обработчики событий. Например, `WhenCandlesFinished` гораздо нагляднее, чем подписка на событие `CandleReceived` с последующей проверкой состояния свечи.

3. **Комбинирование условий** - правила можно объединять с помощью операторов `And`, `Or` и других, создавая сложные условия активации:

```cs
// Пример комбинирования правил
var tickSub = new Subscription(DataType.Ticks, Security);

tickSub
	.WhenTickTradeReceived(this)
	.And(Portfolio.WhenChanged(Connector))
	.Do(() => {
		// Код, который выполнится только когда будет новая сделка
		// И изменится баланс портфеля
	})
	.Apply(this);

Subscribe(tickSub);
```

4. **Управление жизненным циклом** - правила можно делать одноразовыми (`Once()`), устанавливать условия отмены (`Until()`), добавлять отложенные действия и т.д.
```

## Примеры совместимой стратегии

Ниже приведен пример стратегии, которая следует всем рекомендациям и будет корректно работать во всех платформах StockSharp:

```cs
public class SmaStrategy : Strategy
{
    private readonly StrategyParam<DataType> _series;
    private readonly StrategyParam<int> _longSmaLength;
    private readonly StrategyParam<int> _shortSmaLength;

    public DataType Series
    {
        get => _series.Value;
        set => _series.Value = value;
    }

    public int LongSmaLength
    {
        get => _longSmaLength.Value;
        set => _longSmaLength.Value = value;
    }

    public int ShortSmaLength
    {
        get => _shortSmaLength.Value;
        set => _shortSmaLength.Value = value;
    }

    private SimpleMovingAverage _longSma;
    private SimpleMovingAverage _shortSma;
    private IChart _chart;
    private IChartCandleElement _chartCandleElement;
    private IChartIndicatorElement _longSmaIndicatorElement;
    private IChartIndicatorElement _shortSmaIndicatorElement;

    public SmaStrategy()
    {
        _longSmaLength = Param(nameof(LongSmaLength), 80)
                          .SetDisplay("Long SMA length", string.Empty, "Base settings")
                          .SetCanOptimize(true);
                          
        _shortSmaLength = Param(nameof(ShortSmaLength), 30)
                          .SetDisplay("Short SMA length", string.Empty, "Base settings")
                          .SetCanOptimize(true);
                          
        _series = Param(nameof(Series), TimeSpan.FromMinutes(15).TimeFrame())
                 .SetDisplay("Series", string.Empty, "Base settings");
    }

    protected override void OnStarted2(DateTime time)
    {
        base.OnStarted2(time);

        _longSma = new SimpleMovingAverage { Length = LongSmaLength };
        _shortSma = new SimpleMovingAverage { Length = ShortSmaLength };

        Indicators.Add(_shortSma);
        Indicators.Add(_longSma);

        // Инициализация графика, если он доступен
        _chart = GetChart();
        if (_chart != null)
            InitChart();

        var subscription = new Subscription(Series, Security);

        Connector
            .WhenCandlesFinished(subscription)
            .Do(ProcessCandle)
            .Apply(this);

        Subscribe(subscription);
    }

    private void InitChart()
    {
        _chart.ClearAreas();
        var area = _chart.AddArea();
        
        _chartCandleElement = area.AddCandles();
        
        _longSmaIndicatorElement = area.AddIndicator(_longSma);
        _longSmaIndicatorElement.Color = System.Drawing.Color.Brown;
        _longSmaIndicatorElement.DrawStyle = DrawStyles.Line;
        
        _shortSmaIndicatorElement = area.AddIndicator(_shortSma);
        _shortSmaIndicatorElement.Color = System.Drawing.Color.Blue;
        _shortSmaIndicatorElement.DrawStyle = DrawStyles.Line;
    }

    private void ProcessCandle(ICandleMessage candle)
    {
        var ls = _longSma.Process(candle);
        var ss = _shortSma.Process(candle);
        
        // Отрисовка на графике, если он доступен
        if (_chart != null)
        {
            var data = _chart.CreateData();
            data.Group(candle.OpenTime)
                .Add(_chartCandleElement, candle)
                .Add(_longSmaIndicatorElement, ls)
                .Add(_shortSmaIndicatorElement, ss);
            _chart.Draw(data);
        }
        
        if (!_longSma.IsFormed)
            return;
            
        var isShortLessCurrent = _shortSma.GetCurrentValue() < _longSma.GetCurrentValue();
        var isShortLessPrev = _shortSma.GetValue(1) < _longSma.GetValue(1);

        if (isShortLessCurrent == isShortLessPrev)
            return;
            
        // Торговая логика
        var volume = Volume + Math.Abs(Position);

        if (isShortLessCurrent)
            SellMarket(volume);
        else
            BuyMarket(volume);
    }
}
```

## См. также

- [Параметры стратегии](parameters.md)
- [Событийная модель](event_model.md)
- [Логирование в стратегии](logging.md)
