# Торговые режимы стратегий

## Обзор

Свойство `TradingMode` позволяет ограничить торговую активность стратегии, не останавливая её полностью. Это полезно для управления рисками -- например, запретить открытие новых позиций, разрешив только закрытие существующих, или полностью заблокировать отправку заявок.

Режим задается перечислением `StrategyTradingModes` и может быть изменен во время работы стратегии.

## Перечисление StrategyTradingModes

| Значение | Описание |
|----------|----------|
| `Full` | Полный доступ к торговле. Никаких ограничений на заявки. Значение по умолчанию. |
| `Disabled` | Торговля полностью запрещена. Все попытки выставить заявку будут отклонены. |
| `CancelOrdersOnly` | Разрешена только отмена заявок. Новые заявки и изменение существующих запрещены. |
| `ReducePositionOnly` | Разрешены только заявки, уменьшающие текущую позицию. Открытие новых позиций и увеличение существующих запрещены. |
| `LongOnly` | Разрешены только длинные позиции. Продажа разрешена только для закрытия существующей длинной позиции (объем продажи не может превышать текущую позицию). Открытие коротких позиций запрещено. |

## Установка режима

```csharp
// При создании стратегии
var strategy = new MyStrategy();
strategy.TradingMode = StrategyTradingModes.ReducePositionOnly;

// Динамическое изменение во время работы
strategy.TradingMode = StrategyTradingModes.Disabled;
```

## Логика проверки режима

При попытке зарегистрировать заявку стратегия проверяет текущий режим:

- **`Disabled`** -- заявка отклоняется с причиной "торговля запрещена".
- **`ReducePositionOnly`** -- заявка отклоняется, если текущая позиция равна нулю, если направление заявки совпадает с направлением позиции, или если объем заявки превышает абсолютное значение позиции.
- **`LongOnly`** -- заявка на продажу отклоняется, если текущая позиция неположительная или если объем продажи превышает текущую позицию.
- **`Full`** -- ограничений нет.
- **`CancelOrdersOnly`** -- разрешена только отмена заявок.

## Метод IsFormedAndOnlineAndAllowTrading

Метод расширения `IsFormedAndOnlineAndAllowTrading` проверяет, что стратегия сформирована (`IsFormed`), находится в онлайн-состоянии (`IsOnline`) и торговый режим позволяет выполнить требуемое действие:

```csharp
// Проверка разрешения на полную торговлю (по умолчанию)
if (!IsFormedAndOnlineAndAllowTrading())
    return;

// Проверка разрешения только на отмену заявок
if (!IsFormedAndOnlineAndAllowTrading(StrategyTradingModes.CancelOrdersOnly))
    CancelActiveOrders();

// Проверка разрешения на уменьшение позиции
if (!IsFormedAndOnlineAndAllowTrading(StrategyTradingModes.ReducePositionOnly))
    return;
```

Логика разрешений при вызове с параметром `required`:

| Текущий TradingMode \ required | `Full` | `CancelOrdersOnly` | `ReducePositionOnly` |
|-------------------------------|--------|---------------------|---------------------|
| `Full` | да | да | да |
| `Disabled` | нет | нет | нет |
| `CancelOrdersOnly` | нет | да | нет |
| `ReducePositionOnly` | нет | да | да |
| `LongOnly` | нет | да | да |

## Пример использования

```csharp
public class TradingModeStrategy : Strategy
{
    private readonly StrategyParam<DataType> _candleType;

    public DataType CandleType
    {
        get => _candleType.Value;
        set => _candleType.Value = value;
    }

    public TradingModeStrategy()
    {
        _candleType = Param(nameof(CandleType), TimeSpan.FromMinutes(5).TimeFrame());
    }

    protected override void OnStarted2(DateTime time)
    {
        base.OnStarted2(time);

        var subscription = SubscribeCandles(CandleType);

        subscription
            .Bind(ProcessCandle)
            .Start();
    }

    private void ProcessCandle(ICandleMessage candle)
    {
        // Проверка, что стратегия готова к полноценной торговле
        if (!IsFormedAndOnlineAndAllowTrading())
            return;

        if (candle.ClosePrice > candle.OpenPrice)
        {
            BuyMarket(Volume);
        }
        else if (candle.ClosePrice < candle.OpenPrice)
        {
            SellMarket(Volume);
        }
    }
}

// Запуск стратегии с ограничением -- только длинные позиции
var strategy = new TradingModeStrategy();
strategy.TradingMode = StrategyTradingModes.LongOnly;
strategy.Start();

// Позже -- переключить на режим закрытия позиции
strategy.TradingMode = StrategyTradingModes.ReducePositionOnly;

// Полная блокировка торговли
strategy.TradingMode = StrategyTradingModes.Disabled;
```

В этом примере стратегия изначально работает в режиме `LongOnly`, что позволяет только покупки и закрытие длинных позиций. При изменении рыночных условий режим можно переключить на `ReducePositionOnly` для плавного закрытия позиций, а затем на `Disabled` для полной остановки торговой активности.
