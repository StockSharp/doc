# Стратегия котирования по спреду

## Обзор

`MqSpreadStrategy` - это стратегия, которая использует котирование для создания спреда на рынке. Она создает две дочерние стратегии котирования: одну для покупки и одну для продажи.

## Основные компоненты

// Основные компоненты
public class MqSpreadStrategy : Strategy
{
    private MarketQuotingStrategy _strategyBuy;
    private MarketQuotingStrategy _strategySell;
}

## Методы

### OnStarted

Вызывается при запуске стратегии:

- Подписывается на изменение рыночного времени
- Инициализирует первоначальное создание стратегий котирования

// Метод OnStarted
protected override void OnStarted(DateTimeOffset time)
{
    Connector.MarketTimeChanged += Connector_MarketTimeChanged;
    Connector_MarketTimeChanged(new TimeSpan());
    base.OnStarted(time);
}

### Connector_MarketTimeChanged

Метод, вызываемый при изменении рыночного времени:

- Проверяет текущую позицию и состояние существующих стратегий котирования
- Создает новые стратегии котирования для покупки и продажи

// Метод Connector_MarketTimeChanged
private void Connector_MarketTimeChanged(TimeSpan obj)
{
    if (Position != 0) return;
    if (_strategyBuy != null && _strategyBuy.ProcessState != ProcessStates.Stopped) return;
    if (_strategySell != null && _strategySell.ProcessState != ProcessStates.Stopped) return;

    _strategyBuy = new MarketQuotingStrategy(Sides.Buy, Volume)
    {
        Name = "buy " + CurrentTime,
        Volume = 1,
        PriceType = MarketPriceTypes.Following,
        IsSupportAtomicReRegister = false
    };
    ChildStrategies.Add(_strategyBuy);

    _strategySell = new MarketQuotingStrategy(Sides.Sell, Volume)
    {
        Name = "sell " + CurrentTime,
        Volume = 1,
        PriceType = MarketPriceTypes.Following,
        IsSupportAtomicReRegister = false
    };
    ChildStrategies.Add(_strategySell);
}

## Логика работы

- Стратегия реагирует на изменения рыночного времени
- При каждом изменении времени, если нет открытой позиции и предыдущие стратегии котирования остановлены, создаются новые стратегии
- Создаются две стратегии котирования:
  1. Стратегия покупки (Buy)
  2. Стратегия продажи (Sell)
- Обе стратегии настроены на котирование с использованием рыночных цен (Following)

## Особенности

- Использует [MarketQuotingStrategy](xref:StockSharp.Algo.Strategies.Quoting.MarketQuotingStrategy) для создания котировок
- Создает спред на рынке, выставляя одновременно заявки на покупку и продажу
- Реагирует на изменения рыночного времени для обновления стратегий
- Не поддерживает атомарное перерегистрирование заявок
- Использует текущее время для именования стратегий, что помогает в их идентификации