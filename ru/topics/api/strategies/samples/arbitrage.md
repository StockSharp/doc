# Стратегия арбитража

## Обзор

`ArbitrageStrategy` - это стратегия арбитража между фьючерсом и базовым активом. Она отслеживает спреды между инструментами и открывает позиции при возникновении арбитражных возможностей.

## Основные компоненты и свойства

// Основные компоненты и свойства
public class ArbitrageStrategy : Strategy
{
    public Security FutureSecurity { get; set; }
    public Security StockSecurity { get; set; }

    public Portfolio FuturePortfolio { get; set; }
    public Portfolio StockPortfolio { get; set; }

    public decimal StockMultiplicator { get; set; }

    public decimal FutureVolume { get; set; }
    public decimal StockVolume { get; set; }

    public decimal ProfitToExit { get; set; }

    public decimal SpreadToGenerateSignal { get; set; }
}

## Методы

### OnStarted

Вызывается при запуске стратегии:

- Инициализирует идентификаторы инструментов
- Подписывается на данные стакана для обоих инструментов

// Метод OnStarted
protected override void OnStarted(DateTimeOffset time)
{
    _futId = FutureSecurity.ToSecurityId();
    _stockId = StockSecurity.ToSecurityId();

    var subFut = this.SubscribeMarketDepth(FutureSecurity);
    var subStock = this.SubscribeMarketDepth(StockSecurity);

    subFut.WhenOrderBookReceived(this).Do(ProcessMarketDepth).Apply(this);
    subStock.WhenOrderBookReceived(this).Do(ProcessMarketDepth).Apply(this);
    base.OnStarted(time);
}

### ProcessMarketDepth

Основной метод обработки данных стакана:

- Обновляет последние данные стакана
- Рассчитывает средние цены
- Определяет тип арбитражной ситуации (контанго или бэквордация)
- Принимает решения об открытии или закрытии позиций

// Метод ProcessMarketDepth
private void ProcessMarketDepth(IOrderBookMessage depth)
{
    // Код метода ProcessMarketDepth
}

### GenerateOrdersBackvardation и GenerateOrdersContango

Методы для генерации ордеров при бэквордации и контанго:

// Методы GenerateOrdersBackvardation и GenerateOrdersContango
private (Order buy, Order sell) GenerateOrdersBackvardation()
{
    // Код метода GenerateOrdersBackvardation
}

private (Order sell, Order buy) GenerateOrdersContango()
{
    // Код метода GenerateOrdersContango
}

### GetAveragePrice

Вспомогательный метод для расчета средней цены по стакану:

// Метод GetAveragePrice
private static decimal GetAveragePrice(IOrderBookMessage depth, Sides orderDirection, decimal volume)
{
    // Код метода GetAveragePrice
}

## Логика работы

- Стратегия отслеживает спреды между фьючерсом и базовым активом
- При превышении спредом заданного порога открывается арбитражная позиция
- Позиция закрывается при достижении заданного уровня прибыли
- Используются рыночные ордера для быстрого исполнения

## Особенности

- Поддерживает работу с двумя инструментами и двумя портфелями
- Учитывает мультипликатор для базового актива
- Использует правила (IMarketRule) для обработки событий исполнения ордеров
- Логирует информацию о текущем состоянии и спредах