# Котирование

Алгоритм котирования позволяет контролировать позицию выставленных заявок в стакане. Необходимость в такой функциональности возникает тогда, когда необходимо быстро открывать и закрывать позиции по выгодным ценам. Также, благодаря быстрому мониторингу стакана, котирование позволяет реализовывать скальперские приводы на сверх малых тайм\-фреймах. 

Также, котирование позволяет эмулировать рыночные заявки на бирже [ФОРТС](https://moex.com/ru/derivatives/), где тип заявок [OrderTypes.Market](../api/StockSharp.Messages.OrderTypes.Market.html) не поддерживается. 

### Предварительные условия

[Дочерние стратегии](StrategyChilds.md)

Для реализации котирования в [S\#](StockSharpAbout.md) входит класс [QuotingStrategy](../api/StockSharp.Algo.Strategies.Quoting.QuotingStrategy.html). Это базовый абстрактный класс для всех производных алгоритмов: 

- [MarketQuotingStrategy](../api/StockSharp.Algo.Strategies.Quoting.MarketQuotingStrategy.html)

   – данный алгоритм мониторит лучшую котировку (

  [Security.BestBid](../api/StockSharp.BusinessEntities.Security.BestBid.html)

   для покупки или 

  [Security.BestAsk](../api/StockSharp.BusinessEntities.Security.BestAsk.html)

   для продажи), выставляя свои заявки по этим же ценам, или чуть лучше, в зависимости от значения 

  [MarketQuotingStrategy.PriceOffset](../api/StockSharp.Algo.Strategies.Quoting.MarketQuotingStrategy.PriceOffset.html)

  . Дополнительно, в 

  [MarketQuotingStrategy](../api/StockSharp.Algo.Strategies.Quoting.MarketQuotingStrategy.html)

   входит параметр 

  [MarketQuotingStrategy.PriceType](../api/StockSharp.Algo.Strategies.Quoting.MarketQuotingStrategy.PriceType.html)

  , который контролирует положение передвижения заявки в спреде: 

  [MarketPriceTypes.Following](../api/StockSharp.Algo.MarketPriceTypes.Following.html)

   – алгоритм смотрит лучшую котировку, 

  [MarketPriceTypes.Opposite](../api/StockSharp.Algo.MarketPriceTypes.Opposite.html)

   – лучшую противоположную котировку и 

  [MarketPriceTypes.Middle](../api/StockSharp.Algo.MarketPriceTypes.Middle.html)

   – алгоритм будет ставить заявку в середину спреда. Данный параметр влияет на то, как скоро будет удовлетворена заявка. 
- [BestByVolumeQuotingStrategy](../api/StockSharp.Algo.Strategies.Quoting.BestByVolumeQuotingStrategy.html)

   – смотрит, какой объем стоит перед котируемой заявкой, и если он превышает допустимую норму 

  [VolumeExchange](../api/StockSharp.Algo.Strategies.Quoting.BestByVolumeQuotingStrategy.VolumeExchange.html)

  , то заявка передвигается на край спреда. 
- [BestByPriceQuotingStrategy](../api/StockSharp.Algo.Strategies.Quoting.BestByPriceQuotingStrategy.html)

   – смотрит, насколько далеко котируемая заявка ушла от лучшей котировки. Если был превышен допустимый интервал 

  [BestPriceOffset](../api/StockSharp.Algo.Strategies.Quoting.BestByPriceQuotingStrategy.BestPriceOffset.html)

  , то заявка передвигается на край спреда. 
- [LastTradeQuotingStrategy](../api/StockSharp.Algo.Strategies.Quoting.LastTradeQuotingStrategy.html)

   – аналогичен 

  [MarketQuotingStrategy](../api/StockSharp.Algo.Strategies.Quoting.MarketQuotingStrategy.html)

   за исключением того, что мониторится не стакан, а последняя сделка 

  [Security.LastTrade](../api/StockSharp.BusinessEntities.Security.LastTrade.html)

  . 
- [LevelQuotingStrategy](../api/StockSharp.Algo.Strategies.Quoting.LevelQuotingStrategy.html)

   – котирование по заданному уровню в стакане. 
- [LimitQuotingStrategy](../api/StockSharp.Algo.Strategies.Quoting.LimitQuotingStrategy.html)

   – котирование по лимитированной цене. 

### Добавление в SampleSMA котирование

Добавление в SampleSMA котирование

1. Для того, чтобы алгоритм скользящей средней, описанный в разделе [Итерационная модель](StrategyCreate.md), стал работать совместно с котировщиком, перед началом работы необходимо запустить экспорт стакана:

   ```cs
   if (\!\_isLkohOrderBookStarted)
   {
   	\/\/ для алгоритма котирования необходимо включить экспорт стакана
   	\_connector.SubscribeMarketDepth(lkoh);
   	\_isLkohOrderBookStarted \= true;
   }
   ```
2. Необходимо заменить код в классе SmaStrategy c:

   ```cs
   \/\/ регистрируем ее
   base.RegisterOrder(order);
   ```

   на: 

   ```cs
   var strategy \= new MarketQuotingStrategy(direction, volume);
   ChildStrategies.Add(strategy);
   ```

### Следующие шаги

[Тейк\-профит и стоп\-лосс](StrategyProtective.md)

## См. также
