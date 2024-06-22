# Стратегия котирования

## Обзор

`MqStrategy` - это стратегия, которая использует котирование для управления позицией на рынке. Она создает дочернюю стратегию котирования в зависимости от текущей позиции.

## Основные компоненты

// Основные компоненты
public class MqStrategy : Strategy
{
    private MarketQuotingStrategy _strategy;
}

## Методы

### OnStarted

Вызывается при запуске стратегии:

- Подписывается на изменение рыночного времени
- Инициализирует первоначальное создание стратегии котирования

// Метод OnStarted
protected override void OnStarted(DateTimeOffset time)
{
    Connector.MarketTimeChanged += Connector_MarketTimeChanged;
    Connector_MarketTimeChanged(default);

    base.OnStarted(time);
}

### Connector_MarketTimeChanged

Метод, вызываемый при изменении рыночного времени:

- Проверяет состояние существующей стратегии котирования
- Создает новую стратегию котирования в зависимости от текущей позиции

// Метод Connector_MarketTimeChanged
private void Connector_MarketTimeChanged(TimeSpan obj)
{
    if (_strategy != null && _strategy.ProcessState != ProcessStates.Stopped) return;

    if (Position <= 0)
    {
        _strategy = new MarketQuotingStrategy(Sides.Buy, Volume + Math.Abs(Position))
        {
            Name = "buy " + CurrentTime,
            Volume = 1,
            PriceType = MarketPriceTypes.Following,
        };
        ChildStrategies.Add(_strategy);
    }
    else if (Position > 0)
    {
        _strategy = new MarketQuotingStrategy(Sides.Sell, Volume + Math.Abs(Position))
        {
            Name = "sell " + CurrentTime,
            Volume = 1,
            PriceType = MarketPriceTypes.Following,
        };
        ChildStrategies.Add(_strategy);
    }
}

## Логика работы

- Стратегия реагирует на изменения рыночного времени
- При каждом изменении времени, если предыдущая стратегия котирования остановлена, создается новая стратегия
- Направление котирования зависит от текущей позиции:
  - Если позиция <= 0, создается стратегия покупки
  - Если позиция > 0, создается стратегия продажи
- Объем котирования учитывает текущую позицию

## Особенности

- Использует [MarketQuotingStrategy](xref:StockSharp.Algo.Strategies.Quoting.MarketQuotingStrategy) для создания котировок
- Адаптируется к текущей позиции, меняя направление котирования
- Реагирует на изменения рыночного времени для обновления стратегии
- Использует рыночное котирование (Following) для определения цены
- Использует текущее время для именования стратегий, что помогает в их идентификации