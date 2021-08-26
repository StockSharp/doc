# Quoting

Quoting algorithm allows you to control the position of the registered orders in the order book. The necessary of such functionality occurs when you need to quickly open and close positions at favorable prices. Also, due to the quick monitoring of the order book, the quoting allows to realize scalping tools for over small time frames. 

Also, the quoting can emulate market orders on a exchanges, where the [OrderTypes.Market](../api/StockSharp.Messages.OrderTypes.Market.html) type of orders is not supported. 

### Prerequisites

[Child strategies](StrategyChilds.md)

To implement the quoting the [S\#](StockSharpAbout.md) includes the [QuotingStrategy](../api/StockSharp.Algo.Strategies.Quoting.QuotingStrategy.html) class. This is the base abstract class for all derivative algorithms: 

- [MarketQuotingStrategy](../api/StockSharp.Algo.Strategies.Quoting.MarketQuotingStrategy.html)

   – this algorithm monitors the best quote (

  [Security.BestBid](../api/StockSharp.BusinessEntities.Security.BestBid.html)

   for buy or 

  [Security.BestAsk](../api/StockSharp.BusinessEntities.Security.BestAsk.html)

   for sell), registering its orders for the same price or a little better, depending on the 

  [MarketQuotingStrategy.PriceOffset](../api/StockSharp.Algo.Strategies.Quoting.MarketQuotingStrategy.PriceOffset.html)

   value. Additionally, the 

  [MarketQuotingStrategy](../api/StockSharp.Algo.Strategies.Quoting.MarketQuotingStrategy.html)

   includes the 

  [MarketQuotingStrategy.PriceType](../api/StockSharp.Algo.Strategies.Quoting.MarketQuotingStrategy.PriceType.html)

  , parameter, which controls the position of the order movement in spread: 

  [MarketPriceTypes.Following](../api/StockSharp.Algo.MarketPriceTypes.Following.html)

   – the algorithm looks the best quote, 

  [MarketQuotingStrategy.PriceType](../api/StockSharp.Algo.Strategies.Quoting.MarketQuotingStrategy.PriceType.html)

  , 

  [MarketPriceTypes.Middle](../api/StockSharp.Algo.MarketPriceTypes.Middle.html)

   – the algorithm will put the order in the middle of the spread. This parameter affects how soon the order will be matched. 
- [BestByVolumeQuotingStrategy](../api/StockSharp.Algo.Strategies.Quoting.BestByVolumeQuotingStrategy.html)

   – looks what total volume has a better price than quoted order and if it exceeds the permitted limit 

  [VolumeExchange](../api/StockSharp.Algo.Strategies.Quoting.BestByVolumeQuotingStrategy.VolumeExchange.html)

  , the order is moved to the edge of the spread. 
- [BestByPriceQuotingStrategy](../api/StockSharp.Algo.Strategies.Quoting.BestByPriceQuotingStrategy.html)

   – looks how far the quoted order is from the best quote. If the tolerance interval 

  [BestPriceOffset](../api/StockSharp.Algo.Strategies.Quoting.BestByPriceQuotingStrategy.BestPriceOffset.html)

   has been exceeded, the order is moved to the edge of the spread. 
- [LastTradeQuotingStrategy](../api/StockSharp.Algo.Strategies.Quoting.LastTradeQuotingStrategy.html)

   – is similar to the 

  [MarketQuotingStrategy](../api/StockSharp.Algo.Strategies.Quoting.MarketQuotingStrategy.html)

   except that it monitors not order book depth, but the last trade 

  [Security.LastTrade](../api/StockSharp.BusinessEntities.Security.LastTrade.html)

  . 
- [LevelQuotingStrategy](../api/StockSharp.Algo.Strategies.Quoting.LevelQuotingStrategy.html)

   – quoting by the specified level in the order book. 
- [LimitQuotingStrategy](../api/StockSharp.Algo.Strategies.Quoting.LimitQuotingStrategy.html)

   – quoting by the limited price. 

### Quoting adding to the SampleSMA

Quoting adding to the SampleSMA

1. It is necessary to enable the export of the order book before starting work, so the moving average algorithm described in the [iteration model](StrategyCreate.md), section begin to work in association with the strategy:

   ```cs
   if (\!\_isAaplOrderBookStarted)
   {
   	\_connector.SubscribeMarketDepth(aapl);
   	\_isAaplOrderBookStarted \= true;	
   }
   ```
2. It is necessary to replace the code in the SmaStrategy class from:

   ```cs
   base.RegisterOrder(order);
   ```

   to: 

   ```cs
   var strategy \= new MarketQuotingStrategy(direction, volume);
   ChildStrategies.Add(strategy);
   ```

### Next Steps

[Take\-profit and stop\-loss](StrategyProtective.md)

## Recommended content
