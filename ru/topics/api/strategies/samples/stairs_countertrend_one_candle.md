# Контртрендовая стратегия

## Обзор

`OneCandleCountertrendStrategy` - это простая контртрендовая стратегия, которая принимает решения на основе анализа одной свечи.

## Основные компоненты

```cs
// Основные компоненты
public class OneCandleCountertrendStrategy : Strategy
{
    private readonly CandleSeries _candleSeries;
    private Subscription _subscription;
}
```

## Конструктор

Конструктор принимает [CandleSeries](xref:StockSharp.Algo.Candles.CandleSeries) для инициализации стратегии.

```cs
// Конструктор
public OneCandleCountertrendStrategy(CandleSeries candleSeries)
{
    _candleSeries = candleSeries;
}
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
3. Принимает решение об открытии позиции против тренда на основе текущей свечи и позиции

```cs
// Метод OnCandleReceived
private void OnCandleReceived(Subscription subscription, ICandleMessage candle)
{
    if (subscription != _subscription)
        return;

    if (candle.State != CandleStates.Finished) return;

    if (candle.OpenPrice < candle.ClosePrice && Position >= 0)
    {
        RegisterOrder(this.SellAtMarket(Volume + Math.Abs(Position)));
    }
    else if (candle.OpenPrice > candle.ClosePrice && Position <= 0)
    {
        RegisterOrder(this.BuyAtMarket(Volume + Math.Abs(Position)));
    }
}
```

## Логика торговли

- Сигнал на продажу: бычья свеча (цена закрытия выше цены открытия) при отсутствии короткой позиции
- Сигнал на покупку: медвежья свеча (цена закрытия ниже цены открытия) при отсутствии длинной позиции
- Объем позиции увеличивается на величину текущей позиции при каждой новой сделке

## Особенности

- Стратегия работает с завершенными свечами
- Использует рыночные ордера для входа в позицию
- Применяет простую логику определения контртренда на основе одной свечи