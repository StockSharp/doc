# Трендовая стратегия лестницей

## Обзор

`StairsTrendStrategy` - это торговая стратегия, основанная на анализе последовательных свечей для определения тренда. Стратегия открывает позиции при формировании устойчивого тренда определенной длины.

## Основные компоненты

```cs
// Основные компоненты
public class StairsTrendStrategy : Strategy
{
    private readonly CandleSeries _candleSeries;
    private Subscription _subscription;

    private int _bullLength;
    private int _bearLength;
}
```

## Конструктор

Конструктор принимает `CandleSeries` для инициализации стратегии.

```cs
// Конструктор
public StairsTrendStrategy(CandleSeries candleSeries)
{
    _candleSeries = candleSeries;
}
```

## Свойства

### Length

Определяет минимальное количество последовательных свечей одного направления для определения тренда.

```cs
// Свойство Length
public int Length { get; set; } = 3;
```

## Методы

### OnStarted

Вызывается при запуске стратегии:

- Подписывается на получение свечей
- Инициализирует подписку на серию свечей

```cs
// Метод OnStarted
protected override void OnStarted(DateTimeOffset time)
{
    CandleReceived += OnCandleReceived;
    _subscription = this.SubscribeCandles(_candleSeries);

    base.OnStarted(time);
}
```

### OnStopped

Вызывается при остановке стратегии:

- Отменяет подписку на свечи

```cs
// Метод OnStopped
protected override void OnStopped()
{
    if (_subscription != null)
    {
        UnSubscribe(_subscription);
        _subscription = null;
    }

    base.OnStopped();
}
```

### OnCandleReceived

Основной метод обработки каждой завершенной свечи:

1. Проверяет, относится ли свеча к нужной подписке
2. Анализирует направление свечи (бычья или медвежья)
3. Обновляет счетчики последовательных бычьих и медвежьих свечей
4. Принимает решение об открытии позиции на основе длины тренда

```cs
// Метод OnCandleReceived
private void OnCandleReceived(Subscription subscription, ICandleMessage candle)
{
    if (subscription != _subscription)
        return;

    if (candle.State != CandleStates.Finished) return;

    if (candle.OpenPrice < candle.ClosePrice)
    {
        _bullLength++;
        _bearLength = 0;
    }
    else if (candle.OpenPrice > candle.ClosePrice)
    {
        _bullLength = 0;
        _bearLength++;
    }

    if (_bullLength >= Length && Position <= 0)
    {
        RegisterOrder(this.BuyAtMarket(Volume + Math.Abs(Position)));
    }
    else if (_bearLength >= Length && Position >= 0)
    {
        RegisterOrder(this.SellAtMarket(Volume + Math.Abs(Position)));
    }
}
```

## Логика торговли

- Сигнал на покупку: `Length` последовательных бычьих свечей при отсутствии длинной позиции
- Сигнал на продажу: `Length` последовательных медвежьих свечей при отсутствии короткой позиции
- Объем позиции увеличивается на величину текущей позиции при каждой новой сделке

## Особенности

- Стратегия работает с завершенными свечами
- Использует рыночные ордера для входа в позицию
- Применяет простую логику определения тренда на основе последовательности свечей