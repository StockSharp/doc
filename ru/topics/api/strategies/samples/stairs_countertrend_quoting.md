# Контртрендовая стратегия с котированием

## Обзор

`StairsCountertrendStrategy` - это контртрендовая стратегия, основанная на анализе последовательности свечей. Она использует подход "лестницы" для открытия позиций против текущего тренда.

## Основные компоненты

// Основные компоненты
public class StairsCountertrendStrategy : Strategy
{
    private readonly Subscription _subscription;
    private int _bullLength;
    private int _bearLength;
}

## Конструктор

Конструктор принимает [CandleSeries](xref:StockSharp.Algo.Candles.CandleSeries) для инициализации стратегии.

// Конструктор
public StairsCountertrendStrategy(CandleSeries candleSeries)
{
    _subscription = new(candleSeries);
}

## Свойства

### Length

Определяет минимальное количество последовательных свечей одного направления для определения тренда.

public int Length { get; set; } = 3;

## Методы

### OnStarted

Вызывается при запуске стратегии:

- Настраивает поддержку фильтрованного стакана
- Подписывается на завершение формирования свечей
- Инициализирует обработку свечей

// Метод OnStarted
protected override void OnStarted(DateTimeOffset time)
{
    Connector.SupportFilteredMarketDepth = true;

    this
        .WhenCandlesFinished(_subscription)
        .Do(CandleManager_Processing)
        .Apply(this);
    
    Subscribe(_subscription);

    base.OnStarted(time);
}

### CandleManager_Processing

Основной метод обработки каждой завершенной свечи:

1. Проверяет состояние свечи
2. Анализирует направление свечи (бычья или медвежья)
3. Обновляет счетчики последовательных бычьих и медвежьих свечей
4. Принимает решение об открытии позиции против тренда
5. Создает и добавляет дочернюю стратегию котирования

// Метод CandleManager_Processing
private void CandleManager_Processing(ICandleMessage candle)
{
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

    if (_bullLength >= Length && Position >= 0)
    {
        ChildStrategies.ToList().ForEach(s => s.Stop());
        var strategy = new MarketQuotingStrategy(Sides.Sell, 1)
        {
            WaitAllTrades = true
        };
        ChildStrategies.Add(strategy);
    }
    else if (_bearLength >= Length && Position <= 0)
    {
        ChildStrategies.ToList().ForEach(s => s.Stop());
        var strategy = new MarketQuotingStrategy(Sides.Buy, 1)
        {
            WaitAllTrades = true
        };
        ChildStrategies.Add(strategy);
    }
}

## Логика торговли

- Сигнал на продажу: `Length` последовательных бычьих свечей при отсутствии короткой позиции
- Сигнал на покупку: `Length` последовательных медвежьих свечей при отсутствии длинной позиции
- При получении сигнала создается дочерняя стратегия котирования ([MarketQuotingStrategy](xref:StockSharp.Algo.Strategies.Quoting.MarketQuotingStrategy))

## Особенности

- Стратегия работает с завершенными свечами
- Использует дочерние стратегии котирования для входа в рынок
- Применяет контртрендовый подход, открывая позиции против установившегося тренда
- Поддерживает фильтрованный стакан для повышения производительности