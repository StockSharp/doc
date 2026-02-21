# Стратегии

## Обзор

Класс `Strategy` является базовым классом для создания торговых стратегий в StockSharp. Он предоставляет полный набор инструментов для подписки на рыночные данные, управления заявками и позициями, расчета статистики и генерации отчетов.

Основные возможности класса `Strategy`:

- Подписка на свечи, стаканы, тики и другие рыночные данные
- Выставление, изменение и отмена заявок
- Управление целевой позицией
- Расчет PnL, комиссии и статистики
- Управление рисками
- Система таймеров и правил
- Оповещения
- Генерация отчетов

> [!WARNING]
> Функционал дочерних стратегий (`ChildStrategies`) объявлен устаревшим и более не поддерживается. Свойство `ChildStrategies` помечено атрибутом `[Obsolete("Child strategies no longer supported.")]`. Если ваш код использует дочерние стратегии, рекомендуется провести рефакторинг -- запускать каждую стратегию как самостоятельный экземпляр.

## Разделы документации

- [Управление целевой позицией](target_position_management.md) -- декларативное управление размером позиции через `SetTargetPosition`
- [Торговые режимы](trading_modes.md) -- ограничение торговой активности через `StrategyTradingModes`
- [Система оповещений](alert_system.md) -- отправка уведомлений (popup, звук, лог, Telegram)
- [Система таймеров](timer_system.md) -- периодическое выполнение действий
- [Управление рисками](risk_management.md) -- правила управления рисками
- [Высокоуровневые подписки](high_level_subscriptions.md) -- упрощенная подписка на рыночные данные
- [Отчеты стратегий](reporting.md) -- генерация отчетов по результатам торговли
- [Дополнительные возможности](advanced_features.md) -- комментарии к заявкам, расписание, безрисковая ставка, источник индикаторов

## Минимальная стратегия

```csharp
public class MyStrategy : Strategy
{
    private readonly StrategyParam<DataType> _candleType;

    public DataType CandleType
    {
        get => _candleType.Value;
        set => _candleType.Value = value;
    }

    public MyStrategy()
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
        if (!IsFormedAndOnlineAndAllowTrading())
            return;

        // Торговая логика
    }
}
```

## Жизненный цикл стратегии

1. **Создание** -- конструктор, объявление параметров через `Param<T>`.
2. **Настройка** -- установка `Security`, `Portfolio`, `Connector`, параметров.
3. **Запуск** -- вызов `Start()`, переход в состояние `ProcessStates.Started`, вызов `OnStarted2(DateTime)`.
4. **Работа** -- обработка рыночных данных, выставление заявок.
5. **Остановка** -- вызов `Stop()`, переход через `ProcessStates.Stopping` в `ProcessStates.Stopped`, вызов `OnStopped()`.
