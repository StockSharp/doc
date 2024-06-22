# Стратегия Боллинджера с фокусом на нижней полосе

## Обзор

`BollingerStrategyLowBandStrategy` - это стратегия, основанная на индикаторе Bollinger Bands. Она открывает короткую позицию при достижении ценой нижней границы полос Боллинджера и закрывает ее при достижении средней линии.

## Основные компоненты

```cs
// Основные компоненты
internal class BollingerStrategyLowBandStrategy : Strategy
{
    private readonly Subscription _subscription;

    public BollingerBands BollingerBands { get; set; }
}
```

## Конструктор

Конструктор принимает [CandleSeries](xref:StockSharp.Algo.Candles.CandleSeries) для инициализации стратегии.

```cs
// Конструктор
public BollingerStrategyLowBandStrategy(CandleSeries series)
{
    _subscription = new(series);
}
```

## Методы

### OnStarted

Вызывается при запуске стратегии:

- Подписывается на завершение формирования свечей
- Инициализирует обработку свечей

```cs
// Метод OnStarted
protected override void OnStarted(DateTimeOffset time)
{
    this
        .WhenCandlesFinished(_subscription)
        .Do(ProcessCandle)
        .Apply(this);

    Subscribe(_subscription);

    base.OnStarted(time);
}
```

### IsRealTime

Проверяет, является ли свеча "реальной" (недавно закрытой):

```cs
// Метод IsRealTime
private bool IsRealTime(ICandleMessage candle)
{
    return (CurrentTime - candle.CloseTime).TotalSeconds < 10;
}
```

### ProcessCandle

Основной метод обработки каждой завершенной свечи:

1. Обрабатывает свечу индикатором Bollinger Bands
2. Проверяет, сформирован ли индикатор
3. Проверяет режим работы (бэктестинг или реальное время)
4. Принимает решение об открытии короткой позиции при достижении нижней полосы Боллинджера
5. Принимает решение о закрытии короткой позиции при достижении средней линии Боллинджера

```cs
// Метод ProcessCandle
private void ProcessCandle(ICandleMessage candle)
{
    BollingerBands.Process(candle);

    if (!BollingerBands.IsFormed) return;
    if (!IsBacktesting && !IsRealTime(candle)) return;

    if (candle.ClosePrice <= BollingerBands.LowBand.GetCurrentValue() && Position == 0)
    {
        RegisterOrder(this.SellAtMarket(Volume));
    }
    else if (candle.ClosePrice >= BollingerBands.MovingAverage.GetCurrentValue() && Position < 0)
    {
        RegisterOrder(this.BuyAtMarket(Math.Abs(Position)));
    }
}
```

## Логика торговли

- Сигнал на продажу: цена закрытия свечи достигает или опускается ниже нижней полосы Боллинджера при отсутствии открытой позиции
- Сигнал на покупку (закрытие короткой позиции): цена закрытия свечи достигает или превышает среднюю линию Боллинджера при наличии короткой позиции
- Объем позиции фиксированный при входе в рынок

## Особенности

- Стратегия работает как с историческими данными, так и в режиме реального времени
- Использует только нижнюю полосу и среднюю линию индикатора Bollinger Bands
- Открывает только короткие позиции
- Применяет проверку на "реальность" свечи в режиме реального времени