# Котирование

Алгоритм котирования позволяет контролировать позицию выставленных заявок в стакане. Необходимость в такой функциональности возникает тогда, когда необходимо быстро открывать и закрывать позиции по выгодным ценам. Также, благодаря быстрому мониторингу стакана, котирование позволяет реализовывать скальперские приводы на сверх малых тайм\-фреймах. 

Также, котирование позволяет эмулировать рыночные заявки на бирже [ФОРТС](https://moex.com/ru/derivatives/), где тип заявок [OrderTypes.Market](xref:StockSharp.Messages.OrderTypes.Market) не поддерживается. 

### Предварительные условия

[Дочерние стратегии](StrategyChilds.md)

Для реализации котирования в [S\#](StockSharpAbout.md) входит класс [QuotingStrategy](xref:StockSharp.Algo.Strategies.Quoting.QuotingStrategy). Это базовый абстрактный класс для всех производных алгоритмов: 

- [MarketQuotingStrategy](xref:StockSharp.Algo.Strategies.Quoting.MarketQuotingStrategy) – данный алгоритм мониторит лучшую котировку ([Security.BestBid](xref:StockSharp.BusinessEntities.Security.BestBid) для покупки или [Security.BestAsk](xref:StockSharp.BusinessEntities.Security.BestAsk) для продажи), выставляя свои заявки по этим же ценам, или чуть лучше, в зависимости от значения [MarketQuotingStrategy.PriceOffset](xref:StockSharp.Algo.Strategies.Quoting.MarketQuotingStrategy.PriceOffset). Дополнительно, в [MarketQuotingStrategy](xref:StockSharp.Algo.Strategies.Quoting.MarketQuotingStrategy) входит параметр [MarketQuotingStrategy.PriceType](xref:StockSharp.Algo.Strategies.Quoting.MarketQuotingStrategy.PriceType), который контролирует положение передвижения заявки в спреде: [MarketPriceTypes.Following](xref:StockSharp.Algo.MarketPriceTypes.Following) – алгоритм смотрит лучшую котировку, [MarketPriceTypes.Opposite](xref:StockSharp.Algo.MarketPriceTypes.Opposite) – лучшую противоположную котировку и [MarketPriceTypes.Middle](xref:StockSharp.Algo.MarketPriceTypes.Middle) – алгоритм будет ставить заявку в середину спреда. Данный параметр влияет на то, как скоро будет удовлетворена заявка. 
- [BestByVolumeQuotingStrategy](xref:StockSharp.Algo.Strategies.Quoting.BestByVolumeQuotingStrategy) – смотрит, какой объем стоит перед котируемой заявкой, и если он превышает допустимую норму [VolumeExchange](xref:StockSharp.Algo.Strategies.Quoting.BestByVolumeQuotingStrategy.VolumeExchange), то заявка передвигается на край спреда. 
- [BestByPriceQuotingStrategy](xref:StockSharp.Algo.Strategies.Quoting.BestByPriceQuotingStrategy) – смотрит, насколько далеко котируемая заявка ушла от лучшей котировки. Если был превышен допустимый интервал [BestPriceOffset](xref:StockSharp.Algo.Strategies.Quoting.BestByPriceQuotingStrategy.BestPriceOffset), то заявка передвигается на край спреда. 
- [LastTradeQuotingStrategy](xref:StockSharp.Algo.Strategies.Quoting.LastTradeQuotingStrategy) – аналогичен [MarketQuotingStrategy](xref:StockSharp.Algo.Strategies.Quoting.MarketQuotingStrategy) за исключением того, что мониторится не стакан, а последняя сделка [Security.LastTrade](xref:StockSharp.BusinessEntities.Security.LastTrade). 
- [LevelQuotingStrategy](xref:StockSharp.Algo.Strategies.Quoting.LevelQuotingStrategy) – котирование по заданному уровню в стакане. 
- [LimitQuotingStrategy](xref:StockSharp.Algo.Strategies.Quoting.LimitQuotingStrategy) – котирование по лимитированной цене. 

### Добавление в SampleSMA котирование

Добавление в SampleSMA котирование

1. Для того, чтобы алгоритм скользящей средней, описанный в разделе [Итерационная модель](StrategyCreate.md), стал работать совместно с котировщиком, перед началом работы необходимо запустить экспорт стакана:

   ```cs
   if (!_isLkohOrderBookStarted)
   {
   	// для алгоритма котирования необходимо включить экспорт стакана
   	_connector.SubscribeMarketDepth(lkoh);
   	_isLkohOrderBookStarted = true;
   }
   ```
2. Необходимо заменить код в классе SmaStrategy c:

   ```cs
   // регистрируем ее
   base.RegisterOrder(order);
   ```

   на: 

   ```cs
   var strategy = new MarketQuotingStrategy(direction, volume);
   ChildStrategies.Add(strategy);
   ```

### Следующие шаги

[Тейк\-профит и стоп\-лосс](StrategyProtective.md)

## См. также
