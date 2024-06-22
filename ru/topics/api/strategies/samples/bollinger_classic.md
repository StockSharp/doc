# Классическая стратегия Боллинджера

## Обзор

`BollingerStrategyClassicStrategy` - это стратегия, основанная на индикаторе Bollinger Bands. Она открывает позиции при достижении ценой верхней или нижней границы полос Боллинджера.

## Основные компоненты

```cs
// Основные компоненты
internal class BollingerStrategyClassicStrategy : Strategy
{
    private readonly Subscription _subscription;

    public BollingerBands BollingerBands { get; set; }
}
```

## Конструктор

Конструктор принимает [CandleSeries](xref:StockSharp.Algo.Candles.CandleSeries) для инициализации стратегии.

```cs
// Конструктор
public BollingerStrategyClassicStrategy(CandleSeries series)
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
4. Принимает решение об открытии позиции на основе положения цены закрытия относительно полос Боллинджера

```cs
// Метод ProcessCandle
private void ProcessCandle(ICandleMessage candle)
{
    BollingerBands.Process(candle);

    if (!BollingerBands.IsFormed) return;
    if (!IsBacktesting && !IsRealTime(candle)) return;

    if (candle.ClosePrice >= BollingerBands.UpBand.GetCurrentValue() && Position >= 0)
    {
        RegisterOrder(this.SellAtMarket(Volume + Math.Abs(Position)));
    }
    else if (candle.ClosePrice <= BollingerBands.LowBand.GetCurrentValue() && Position <= 0)
    {
        RegisterOrder(this.BuyAtMarket(Volume + Math.Abs(Position)));
    }
}
```

## Логика торговли

- Сигнал на продажу: цена закрытия свечи достигает или превышает верхнюю полосу Боллинджера при отсутствии короткой позиции
- Сигнал на покупку: цена закрытия свечи достигает или опускается ниже нижней полосы Боллинджера при отсутствии длинной позиции
- Объем позиции увеличивается на величину текущей позиции при каждой новой сделке

## Особенности

- Стратегия работает как с историческими данными, так и в режиме реального времени
- Использует индикатор Bollinger Bands для определения моментов входа в рынок
- Применяет проверку на "реальность" свечи в режиме реального времени