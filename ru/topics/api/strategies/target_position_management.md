# Управление целевой позицией

## Обзор

Система управления целевой позицией позволяет стратегии декларативно задавать желаемый размер позиции, а платформа автоматически выставляет необходимые заявки для достижения этого уровня. Вместо того чтобы вручную рассчитывать объем и направление сделки, достаточно вызвать `SetTargetPosition(10)` -- и менеджер сам определит, нужно ли покупать или продавать, и в каком объеме.

Ключевым компонентом является класс `PositionTargetManager`, который автоматически:

- Рассчитывает разницу между текущей и целевой позицией
- Определяет направление и объем заявки
- Обрабатывает исполнение, отмену и ошибки заявок
- Поддерживает повторные попытки при сбоях

## Методы стратегии

### SetTargetPosition

Устанавливает целевую позицию. Доступны два варианта вызова:

```csharp
// Для основного инструмента и портфеля стратегии
SetTargetPosition(decimal target);

// Для произвольного инструмента и портфеля
SetTargetPosition(Security security, Portfolio portfolio, decimal target);
```

Когда `target` больше текущей позиции, менеджер выставит заявку на покупку. Когда меньше -- на продажу. Если позиция уже равна целевой (с учетом допуска `PositionTolerance`), действия не предпринимаются.

### CancelTargetPosition

Отменяет ранее установленную целевую позицию и останавливает все связанные с ней активные заявки:

```csharp
// Для основного инструмента и портфеля стратегии
CancelTargetPosition();

// Для произвольного инструмента и портфеля
CancelTargetPosition(Security security, Portfolio portfolio);
```

### GetTargetPosition

Возвращает текущее значение целевой позиции, или `null`, если цель не установлена:

```csharp
decimal? target = GetTargetPosition();
decimal? target = GetTargetPosition(security, portfolio);
```

## Свойство TargetPositionManager

Свойство `TargetPositionManager` предоставляет прямой доступ к объекту `PositionTargetManager` для более тонкой настройки:

```csharp
// Максимальное количество повторных попыток при ошибке заявки (по умолчанию 3)
TargetPositionManager.MaxRetries = 5;

// Допуск для определения достижения целевой позиции
TargetPositionManager.PositionTolerance = 0.01m;

// Тип заявки (по умолчанию Market)
TargetPositionManager.OrderType = OrderTypes.Market;
```

Менеджер генерирует следующие события:

- `TargetReached` -- целевая позиция достигнута
- `Error` -- произошла ошибка при выполнении заявки
- `OrderRegistered` -- менеджер зарегистрировал заявку

## Свойство TargetAlgoFactory

Свойство `TargetAlgoFactory` позволяет задать фабрику алгоритмов изменения позиции. По умолчанию используется `MarketOrderAlgo`, который создает рыночные заявки:

```csharp
// Использовать пользовательский алгоритм вместо рыночных заявок
TargetAlgoFactory = (side, volume) => new MyCustomAlgo(side, volume);
```

## Пример использования

```csharp
public class TargetPositionStrategy : Strategy
{
    private readonly StrategyParam<DataType> _candleType;

    public DataType CandleType
    {
        get => _candleType.Value;
        set => _candleType.Value = value;
    }

    public TargetPositionStrategy()
    {
        _candleType = Param(nameof(CandleType), TimeSpan.FromMinutes(5).TimeFrame());
    }

    protected override void OnStarted2(DateTime time)
    {
        base.OnStarted2(time);

        // Настройка менеджера целевой позиции
        TargetPositionManager.MaxRetries = 5;
        TargetPositionManager.TargetReached += (sec, pf) =>
        {
            this.AddInfoLog("Целевая позиция достигнута: {0}, {1}", sec, pf);
        };

        var subscription = SubscribeCandles(CandleType);

        subscription
            .Bind(ProcessCandle)
            .Start();
    }

    private void ProcessCandle(ICandleMessage candle)
    {
        if (!IsFormedAndOnlineAndAllowTrading())
            return;

        if (candle.OpenPrice < candle.ClosePrice)
        {
            // Бычья свеча -- установить целевую позицию на покупку
            SetTargetPosition(Volume);
        }
        else if (candle.OpenPrice > candle.ClosePrice)
        {
            // Медвежья свеча -- установить целевую позицию на продажу
            SetTargetPosition(-Volume);
        }
    }
}
```

В этом примере стратегия не занимается ручным расчетом объемов и направлений. Она просто сообщает, какой размер позиции хочет иметь, а `PositionTargetManager` выполняет всю работу по выставлению заявок.
