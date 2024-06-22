# Контртрендовая стратегия лестницей

## Обзор

`StairsCountertrendStrategy` - это контртрендовая торговая стратегия, которая открывает позиции против установившегося тренда определенной длины.

## Основные компоненты

// Основные компоненты
public class StairsCountertrendStrategy : Strategy
{
    private readonly CandleSeries _candleSeries;
    private Subscription _subscription;

    private int _bullLength;
    private int _bearLength;
}

## Конструктор

Конструктор принимает `CandleSeries` для инициализации стратегии.

// Конструктор
public StairsCountertrendStrategy(CandleSeries candleSeries)
{
    _candleSeries = candleSeries;
}

## Свойства

### Length

Определяет минимальное количество последовательных свечей одного направления для определения тренда.

// Свойство Length
private int Length { get; set; } = 3;

## Методы

### OnStarted

Вызывается при запуске стратегии:

- Подписывается на получение свечей
- Инициализирует подписку на серию свечей

// Метод OnStarted
protected override void OnStarted(DateTimeOffset time)
{
    CandleReceived += OnCandleReceived;
    _subscription = this.SubscribeCandles(_candleSeries);

    base.OnStarted(time);
}

### OnStopped

Вызывается при остановке стратегии:

- Отменяет подписку на свечи

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

### OnCandleReceived

Основной метод обработки каждой завершенной свечи:

1. Проверяет, относится ли свеча к нужной подписке
2. Анализирует направление свечи (бычья или медвежья)
3. Обновляет счетчики последовательных бычьих и медвежьих свечей
4. Принимает решение об открытии позиции против тренда

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

    if (_bullLength >= Length && Position >= 0)
    {
        RegisterOrder(this.SellAtMarket(Volume + Math.Abs(Position)));
    }
    else if (_bearLength >= Length && Position <= 0)
    {
        RegisterOrder(this.BuyAtMarket(Volume + Math.Abs(Position)));
    }
}

## Логика торговли

- Сигнал на продажу: `Length` последовательных бычьих свечей при отсутствии короткой позиции
- Сигнал на покупку: `Length` последовательных медвежьих свечей при отсутствии длинной позиции
- Объем позиции увеличивается на величину текущей позиции при каждой новой сделке

## Особенности

- Стратегия работает с завершенными свечами
- Использует рыночные ордера для входа в позицию
- Применяет контртрендовый подход, открывая позиции против установившегося тренда