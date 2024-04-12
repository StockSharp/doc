# Quoting

Quoting algorithm allows you to control the position of the registered orders in the order book. The necessary of such functionality occurs when you need to quickly open and close positions at favorable prices. Also, due to the quick monitoring of the order book, the quoting allows to realize scalping tools for over small time frames. 

Also, the quoting can emulate market orders on a exchanges, where the [OrderTypes.Market](xref:StockSharp.Messages.OrderTypes.Market) type of orders is not supported. 

## Prerequisites

[Child strategies](child_strategies.md)

To implement the quoting the [S\#](../../api.md) includes the [QuotingStrategy](xref:StockSharp.Algo.Strategies.Quoting.QuotingStrategy) class. This is the base abstract class for all derivative algorithms: 

- [MarketQuotingStrategy](xref:StockSharp.Algo.Strategies.Quoting.MarketQuotingStrategy) – this algorithm monitors the best quote ([Security.BestBid](xref:StockSharp.BusinessEntities.Security.BestBid) for buy or [Security.BestAsk](xref:StockSharp.BusinessEntities.Security.BestAsk) for sell), registering its orders for the same price or a little better, depending on the [MarketQuotingStrategy.PriceOffset](xref:StockSharp.Algo.Strategies.Quoting.MarketQuotingStrategy.PriceOffset) value. Additionally, the [MarketQuotingStrategy](xref:StockSharp.Algo.Strategies.Quoting.MarketQuotingStrategy) includes the [MarketQuotingStrategy.PriceType](xref:StockSharp.Algo.Strategies.Quoting.MarketQuotingStrategy.PriceType), parameter, which controls the position of the order movement in spread: [MarketPriceTypes.Following](xref:StockSharp.Algo.MarketPriceTypes.Following) – the algorithm looks the best quote, [MarketQuotingStrategy.PriceType](xref:StockSharp.Algo.Strategies.Quoting.MarketQuotingStrategy.PriceType), [MarketPriceTypes.Middle](xref:StockSharp.Algo.MarketPriceTypes.Middle) – the algorithm will put the order in the middle of the spread. This parameter affects how soon the order will be matched. 
- [BestByVolumeQuotingStrategy](xref:StockSharp.Algo.Strategies.Quoting.BestByVolumeQuotingStrategy) – looks what total volume has a better price than quoted order and if it exceeds the permitted limit [BestByVolumeQuotingStrategy.VolumeExchange](xref:StockSharp.Algo.Strategies.Quoting.BestByVolumeQuotingStrategy.VolumeExchange), the order is moved to the edge of the spread. 
- [BestByPriceQuotingStrategy](xref:StockSharp.Algo.Strategies.Quoting.BestByPriceQuotingStrategy) – looks how far the quoted order is from the best quote. If the tolerance interval [BestByPriceQuotingStrategy.BestPriceOffset](xref:StockSharp.Algo.Strategies.Quoting.BestByPriceQuotingStrategy.BestPriceOffset) has been exceeded, the order is moved to the edge of the spread. 
- [LastTradeQuotingStrategy](xref:StockSharp.Algo.Strategies.Quoting.LastTradeQuotingStrategy) – is similar to the [MarketQuotingStrategy](xref:StockSharp.Algo.Strategies.Quoting.MarketQuotingStrategy) except that it monitors not order book depth, but the last trade [Security.LastTrade](xref:StockSharp.BusinessEntities.Security.LastTrade). 
- [LevelQuotingStrategy](xref:StockSharp.Algo.Strategies.Quoting.LevelQuotingStrategy) – quoting by the specified level in the order book. 
- [LimitQuotingStrategy](xref:StockSharp.Algo.Strategies.Quoting.LimitQuotingStrategy) – quoting by the limited price. 

## Quoting adding to the SampleSMA

1. It is necessary to enable the export of the order book before starting work, so the moving average algorithm described in the [iteration model](iteration_model.md), section begin to work in association with the strategy:

   ```cs
   if (!_isAaplOrderBookStarted)
   {
   	_connector.SubscribeMarketDepth(aapl);
   	_isAaplOrderBookStarted = true;	
   }
   ```
2. It is necessary to replace the code in the SmaStrategy class from:

   ```cs
   base.RegisterOrder(order);
   ```

   to: 

   ```cs
   var strategy = new MarketQuotingStrategy(direction, volume);
   ChildStrategies.Add(strategy);
   ```

## Next Steps

[Take\-profit and stop\-loss](take_profit_and_stop_loss.md)
