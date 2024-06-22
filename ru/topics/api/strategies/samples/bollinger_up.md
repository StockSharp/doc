# Стратегия Боллинджера с фокусом на верхней полосе

## Обзор

`BollingerStrategyUpBandStrategy` - это стратегия, основанная на индикаторе Bollinger Bands. Она открывает длинную позицию при достижении ценой верхней границы полос Боллинджера и закрывает ее при достижении средней линии.

## Основные компоненты

```cs
// Основные компоненты
internal class BollingerStrategyUpBandStrategy : Strategy
{
    private readonly Subscription _subscription;

    public BollingerBands BollingerBands { get; set; }
}
```

## Конструктор

Конструктор принимает [CandleSeries](xref:StockSharp.Algo.Candles.CandleSeries) для инициализации стратегии.

```cs
// Конструктор
public BollingerStrategyUpBandStrategy(CandleSeries series)
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
    this.WhenCandlesFinished(_subscription).Do(ProcessCandle).Apply(this);
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
4. Принимает решение об открытии длинной позиции при достижении верхней полосы Боллинджера
5. Принимает решение о закрытии длинной позиции при достижении средней линии Боллинджера

```cs
// Метод ProcessCandle
private void ProcessCandle(ICandleMessage candle)
{
    BollingerBands.Process(candle);

    if (!BollingerBands.IsFormed) return;
    if (!IsBacktesting && !IsRealTime(candle)) return;

    if (candle.ClosePrice >= BollingerBands.UpBand.GetCurrentValue() && Position == 0)
    {
        RegisterOrder(this.BuyAtMarket(Volume));
    }
    else if (candle.ClosePrice <= BollingerBands.MovingAverage.GetCurrentValue() && Position > 0)
    {
        RegisterOrder(this.SellAtMarket(Math.Abs(Position)));
    }
}
```

## Логика торговли

- Сигнал на покупку: цена закрытия свечи достигает или превышает верхнюю полосу Боллинджера при отсутствии открытой позиции
- Сигнал на продажу (закрытие длинной позиции): цена закрытия свечи достигает или опускается ниже средней линии Боллинджера при наличии длинной позиции
- Объем позиции фиксированный при входе в рынок

## Особенности

- Стратегия работает как с историческими данными, так и в режиме реального времени
- Использует только верхнюю полосу и среднюю линию индикатора Bollinger Bands
- Открывает только длинные позиции
- Применяет проверку на "реальность" свечи в режиме реального времени