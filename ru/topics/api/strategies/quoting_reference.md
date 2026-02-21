# Справочник котирования

## Обзор

Данный раздел содержит полный справочник архитектуры и компонентов системы котирования в StockSharp. Описаны все доступные поведения котирования, типы действий и интерфейсы. Для базового введения см. [Алгоритм Котирования](quoting.md).

## Архитектура

### QuotingStrategy (устаревший)

Класс [QuotingStrategy](xref:StockSharp.Algo.Strategies.Quoting.QuotingStrategy) является устаревшим. Вместо него рекомендуется использовать [QuotingProcessor](xref:StockSharp.Algo.Strategies.Quoting.QuotingProcessor).

Основные параметры устаревшего класса:

- `QuotingSide` -- направление котирования (Buy/Sell)
- `QuotingVolume` -- объем котирования
- `TimeOut` -- таймаут выполнения
- `UseBidAsk` -- использование цен стакана
- `UseLastTradePrice` -- использование цены последней сделки

### QuotingEngine

[QuotingEngine](xref:StockSharp.Algo.Strategies.Quoting.QuotingEngine) -- функциональное ядро системы котирования. Вычисляет оставшийся объем и таймауты, возвращает рекомендации по действиям без побочных эффектов.

### QuotingBehaviorAlgo

[QuotingBehaviorAlgo](xref:StockSharp.Algo.Strategies.Quoting.QuotingBehaviorAlgo) -- реализация интерфейса `IPositionModifyAlgo` для алгоритмического управления позицией.

Основные методы:

| Метод | Описание |
|-------|----------|
| `UpdateMarketData(time, price, volume)` | Обновление рыночных данных |
| `UpdateOrderBook(depth)` | Обновление стакана |
| `GetNextAction()` | Получение следующего действия котирования |

Поддерживает режимы VWAP и TWAP.

### QuotingProcessor

[QuotingProcessor](xref:StockSharp.Algo.Strategies.Quoting.QuotingProcessor) -- основной процессор для исполнения котирования в рамках стратегии. Управляет жизненным циклом заявок: размещение, модификация, отмена.

## Типы действий QuotingAction

Процессор возвращает действия типа [QuotingAction](xref:StockSharp.Algo.Strategies.Quoting.QuotingAction):

| Тип | Описание |
|-----|----------|
| `None(reason)` | Действие не требуется |
| `PlaceOrder(price, volume)` | Разместить новую заявку |
| `ModifyOrder(price, volume)` | Модифицировать существующую заявку |
| `CancelOrder()` | Отменить заявку |
| `Finish(success, reason)` | Завершить котирование |

## Поведения котирования

StockSharp предоставляет 10 типов поведений котирования, каждый из которых реализует интерфейс [IQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.IQuotingBehavior):

| # | Класс | Описание | Ключевые параметры |
|---|-------|---------|-------------------|
| 1 | [BestByPriceQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.BestByPriceQuotingBehavior) | По лучшей цене из стакана | BestPriceOffset |
| 2 | [LastTradeQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.LastTradeQuotingBehavior) | По цене последней сделки | BestPriceOffset |
| 3 | [LimitQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.LimitQuotingBehavior) | Фиксированная лимитная цена | LimitPrice |
| 4 | [MarketQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.MarketQuotingBehavior) | Рыночная цена с отступом | PriceOffset, PriceType (Following/Opposite/Middle) |
| 5 | [VolatilityQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.VolatilityQuotingBehavior) | По волатильности опциона (Black-Scholes) | IVRange, Model |
| 6 | [TheorPriceQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.TheorPriceQuotingBehavior) | По теоретической цене опциона | TheorPriceOffset |
| 7 | [BestByVolumeQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.BestByVolumeQuotingBehavior) | По кумулятивному объему в стакане | VolumeExchange |
| 8 | [LevelQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.LevelQuotingBehavior) | По уровню глубины стакана | Level (Range\<int\>), OwnLevel |
| 9 | [VWAPQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.VWAPQuotingBehavior) | VWAP -- средневзвешенная по объему | BestPriceOffset |
| 10 | [TWAPQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.TWAPQuotingBehavior) | TWAP -- средневзвешенная по времени | TimeInterval, PriceBufferSize (по умолчанию 10) |

## Интерфейс IQuotingBehavior

Интерфейс [IQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.IQuotingBehavior) определяет два ключевых метода:

### CalculateBestPrice

Вычисляет лучшую цену для выставления заявки:

```cs
decimal? CalculateBestPrice(
	Security security,
	IMarketDataProvider provider,
	Sides quotingDirection,
	decimal? bestBid,
	decimal? bestAsk,
	decimal? lastTradePrice,
	decimal? lastTradeVolume,
	IEnumerable<QuoteChange> bids,
	IEnumerable<QuoteChange> asks);
```

### NeedQuoting

Определяет, нужно ли обновить текущую заявку:

```cs
decimal? NeedQuoting(
	Security security,
	IMarketDataProvider provider,
	DateTimeOffset currentTime,
	decimal? currentPrice,
	decimal? currentVolume,
	decimal? newVolume,
	decimal? bestPrice);
```

Возвращает `null`, если обновление не требуется, или новую цену для выставления заявки.

## Примеры использования

### Рыночное котирование с отступом

```cs
var behavior = new MarketQuotingBehavior
{
	PriceOffset = new Unit(2, UnitTypes.Absolute),
	PriceType = MarketPriceTypes.Following,
	BestPriceOffset = new Unit(0.5m),
};
```

### VWAP-котирование

```cs
var behavior = new VWAPQuotingBehavior
{
	BestPriceOffset = new Unit(1),
};
```

### Котирование опционов по волатильности

```cs
var behavior = new VolatilityQuotingBehavior
{
	IVRange = new Range<decimal>(0.2m, 0.3m),
	Model = new BlackScholes(option, connector),
};
```

### Полный пример с процессором

```cs
// Выбираем поведение котирования
var behavior = new BestByPriceQuotingBehavior
{
	BestPriceOffset = new Unit(0.01m),
};

// Создаем процессор
var processor = new QuotingProcessor(
	behavior,
	Security,
	Portfolio,
	Sides.Buy,
	volume: 10,
	maxOrderVolume: 10,
	timeout: TimeSpan.FromMinutes(5),
	subscriptionProvider: this,
	ruleContainer: this,
	transactionProvider: this,
	timeProvider: this,
	marketDataProvider: this,
	isTradingAllowed: IsFormedAndOnlineAndAllowTrading,
	useBidAsk: true,
	useLastTradePrice: true)
{
	Parent = this,
};

processor.Finished += isOk =>
{
	this.AddInfoLog($"Котирование завершено: {isOk}");
	processor?.Dispose();
};

processor.Start();
```

## См. также

- [Алгоритм Котирования](quoting.md)
- [Стратегия котирования](samples/mq.md)
- [Котирование по волатильности](../options/volatility_trading.md)
