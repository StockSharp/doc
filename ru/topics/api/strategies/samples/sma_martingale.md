# Скользящие средние с мартингейлом

## Обзор

`SmaStrategyMartingaleStrategy` - это торговая стратегия, основанная на пересечении двух простых скользящих средних (SMA) с элементами мартингейла. Стратегия использует длинную и короткую SMA для определения сигналов входа в рынок и выхода из него, увеличивая объем позиции при каждой новой сделке.

## Основные компоненты

```cs
// Основные компоненты
internal class SmaStrategyMartingaleStrategy : Strategy
{
    private readonly Subscription _subscription;

    public SimpleMovingAverage LongSma { get; set; }
    public SimpleMovingAverage ShortSma { get; set; }
}
```

## Конструктор

Конструктор принимает `CandleSeries` и инициализирует подписку на эту серию свечей.

```cs
// Конструктор
public SmaStrategyMartingaleStrategy(CandleSeries series)
{
    _subscription = new(series);
}
```

## Методы

### IsRealTime

Определяет, является ли свеча "реальной" (недавно закрытой):

- Проверяет, прошло ли менее 10 секунд с момента закрытия свечи до текущего времени
- Используется для фильтрации устаревших данных в режиме реального времени

```cs
// Метод IsRealTime
private bool IsRealTime(ICandleMessage candle)
{
    return (CurrentTime - candle.CloseTime).TotalSeconds < 10;
}
```

### OnStarted

Вызывается при запуске стратегии:

- Подписывается на завершение формирования свечей
- Привязывает обработку свечей к методу `ProcessCandle`
- Запускает подписку на серию свечей

```cs
// Метод OnStarted
protected override void OnStarted(DateTimeOffset time)
{
    this.WhenCandlesFinished(_subscription).Do(ProcessCandle).Apply(this);
    Subscribe(_subscription);
    base.OnStarted(time);
}
```

### ProcessCandle

Основной метод обработки каждой завершенной свечи:

1. Обновляет значения длинной и короткой SMA
2. Проверяет, сформированы ли индикаторы
3. В режиме реального времени проверяет актуальность свечи
4. Определяет, произошло ли пересечение SMA
5. При пересечении:
   - Отменяет активные ордера
   - Определяет направление сделки (покупка или продажа)
   - Рассчитывает объем позиции с учетом текущей позиции (элемент мартингейла)
   - Регистрирует новый ордер

```cs
// Метод ProcessCandle
private void ProcessCandle(ICandleMessage candle)
{
    var longSmaIsFormedPrev = LongSma.IsFormed;
    LongSma.Process(candle);
    ShortSma.Process(candle);

    if (!LongSma.IsFormed || !longSmaIsFormedPrev) return;
    if (!IsBacktesting && !IsRealTime(candle)) return;

    var isShortLessThenLongCurrent = ShortSma.GetCurrentValue() < LongSma.GetCurrentValue();
    var isShortLessThenLongPrevios = ShortSma.GetValue(1) < LongSma.GetValue(1);

    if (isShortLessThenLongPrevios == isShortLessThenLongCurrent) return;

    CancelActiveOrders();

    var direction = isShortLessThenLongCurrent ? Sides.Sell : Sides.Buy;

    var volume = Volume + Math.Abs(Position);

    var price = Security.ShrinkPrice(ShortSma.GetCurrentValue());
    RegisterOrder(this.CreateOrder(direction, price, volume));
}
```

## Логика торговли

- Сигнал на покупку: короткая SMA пересекает длинную SMA снизу вверх
- Сигнал на продажу: короткая SMA пересекает длинную SMA сверху вниз
- При каждой новой сделке объем увеличивается на величину текущей позиции
- Цена ордера устанавливается по текущему значению короткой SMA

## Особенности

- Стратегия работает как с историческими данными, так и в режиме реального времени
- Использует механизм подписки на свечи для обработки данных
- Применяет элементы мартингейла, увеличивая объем позиции при каждой новой сделке