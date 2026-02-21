# Управление рисками

## Обзор

Каждая стратегия в StockSharp имеет встроенный менеджер рисков `RiskManager`, который позволяет автоматически контролировать торговые риски. При срабатывании правила менеджер может закрыть позиции, отменить активные заявки или полностью остановить торговлю стратегии.

Риск-менеджер обрабатывает все торговые сообщения, проходящие через стратегию, и при выполнении условий правила автоматически выполняет заданное действие без дополнительного кода в логике стратегии.

## Свойства стратегии

### RiskManager

Объект `IRiskManager`, управляющий списком правил. Создается автоматически при инициализации стратегии:

```csharp
// Доступ к менеджеру рисков
IRiskManager manager = strategy.RiskManager;
```

### RiskRules

Удобное свойство для чтения и установки списка правил:

```csharp
// Чтение текущих правил
IEnumerable<IRiskRule> rules = strategy.RiskRules;

// Установка новых правил
strategy.RiskRules = new IRiskRule[]
{
    new RiskPnLRule { PnL = -1000m, Action = RiskActions.StopTrading },
    new RiskPositionSizeRule { Position = 100m, Action = RiskActions.ClosePositions },
};
```

## Действия при срабатывании

Перечисление `RiskActions` определяет возможные действия:

| Действие | Описание |
|----------|----------|
| `ClosePositions` | Закрыть все открытые позиции рыночной заявкой |
| `StopTrading` | Заблокировать торговлю стратегии |
| `CancelOrders` | Отменить все активные заявки |

При срабатывании правила стратегия записывает предупреждение в лог с указанием имени правила, его параметров и выполненного действия.

## Доступные правила

### RiskPnLRule -- контроль прибыли/убытка

Срабатывает при достижении заданного уровня P&L. Положительное значение -- контроль прибыли, отрицательное -- контроль убытка:

```csharp
// Остановить торговлю при убытке более 5000
new RiskPnLRule
{
    PnL = -5000m,
    Action = RiskActions.StopTrading
}
```

### RiskPositionSizeRule -- контроль размера позиции

Срабатывает при достижении указанного размера позиции:

```csharp
// Отменить заявки при позиции >= 100
new RiskPositionSizeRule
{
    Position = 100m,
    Action = RiskActions.CancelOrders
}
```

### RiskOrderFreqRule -- контроль частоты заявок

Срабатывает, когда количество заявок за указанный интервал превышает лимит:

```csharp
// Остановить при более 50 заявок за минуту
new RiskOrderFreqRule
{
    Count = 50,
    Interval = TimeSpan.FromMinutes(1),
    Action = RiskActions.StopTrading
}
```

### RiskOrderVolumeRule -- контроль объема заявки

Срабатывает при превышении объема одной заявки.

### RiskOrderPriceRule -- контроль цены заявки

Срабатывает при выходе цены заявки за установленные границы.

### RiskTradeVolumeRule -- контроль объема сделки

Срабатывает при превышении объема одной сделки.

### RiskTradePriceRule -- контроль цены сделки

Срабатывает при выходе цены сделки за установленные границы.

### RiskTradeFreqRule -- контроль частоты сделок

Срабатывает при превышении количества сделок за интервал.

### RiskPositionTimeRule -- контроль времени удержания позиции

Срабатывает, когда позиция удерживается дольше установленного времени.

### RiskCommissionRule -- контроль комиссии

Срабатывает при превышении суммарной комиссии.

### RiskSlippageRule -- контроль проскальзывания

Срабатывает при превышении уровня проскальзывания.

### RiskErrorRule -- контроль ошибок

Срабатывает при накоплении определенного количества ошибок.

### RiskOrderErrorRule -- контроль ошибок регистрации заявок

Срабатывает при накоплении ошибок регистрации заявок.

## Пример использования

```csharp
public class RiskAwareStrategy : Strategy
{
    private readonly StrategyParam<DataType> _candleType;

    public DataType CandleType
    {
        get => _candleType.Value;
        set => _candleType.Value = value;
    }

    public RiskAwareStrategy()
    {
        _candleType = Param(nameof(CandleType), TimeSpan.FromMinutes(5).TimeFrame());
    }

    protected override void OnStarted2(DateTime time)
    {
        base.OnStarted2(time);

        // Настройка правил риск-менеджмента
        RiskRules = new IRiskRule[]
        {
            // Закрыть позиции при убытке более 10000
            new RiskPnLRule
            {
                PnL = -10000m,
                Action = RiskActions.ClosePositions
            },

            // Остановить торговлю при позиции более 500 контрактов
            new RiskPositionSizeRule
            {
                Position = 500m,
                Action = RiskActions.StopTrading
            },

            // Отменить заявки при частоте более 100 в минуту
            new RiskOrderFreqRule
            {
                Count = 100,
                Interval = TimeSpan.FromMinutes(1),
                Action = RiskActions.CancelOrders
            },
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

        // Торговая логика
        if (candle.OpenPrice < candle.ClosePrice && Position <= 0)
            BuyMarket(Volume + Math.Abs(Position));
        else if (candle.OpenPrice > candle.ClosePrice && Position >= 0)
            SellMarket(Volume + Math.Abs(Position));
    }
}
```

В этом примере правила риск-менеджмента устанавливаются при запуске стратегии. При достижении убытка в 10000 позиции будут автоматически закрыты. При превышении размера позиции в 500 контрактов торговля будет заблокирована. При слишком частом выставлении заявок активные заявки будут отменены. Все эти проверки выполняются автоматически без дополнительного кода в методе `ProcessCandle`.
