# Extended settings

Extended settings of [HistoryEmulationConnector](../api/StockSharp.Algo.Testing.HistoryEmulationConnector.html).

- [EmulationSettings.MarketTimeChangedInterval](../api/StockSharp.Algo.Strategies.Testing.EmulationSettings.MarketTimeChangedInterval.html) \- time change event input interval. If trades generators are used, then the trades will be generated with this periodicity. The default is 1 minute.
- [MarketEmulatorSettings.Latency](../api/StockSharp.Algo.Testing.MarketEmulatorSettings.Latency.html) \- The minimum value of the registered orders latency. By default, it is equal to **TimeSpan.Zero**, which means instant acceptance of the registered orders by the exchange. 
- [MarketEmulatorSettings.MatchOnTouch](../api/StockSharp.Algo.Testing.MarketEmulatorSettings.MatchOnTouch.html) \- to execute the orders if the price "touches" the level (an assumption is sometimes too "optimistic" and the mode must be switched off for a realistic testing). If the mode is off, then limit orders will be executed if the price is "passed through them" at least in 1 step. The option is available in all modes except the order log. The default is off.

Even if the strategy is tested on the candles, it is necessary to subscribe to the tick trades:

```cs
		\_connector.SubscribeTrades(security);
		
```

If the strategy needs the order books, it is necessary to subscribe to the order books:

```cs
		\_connector.SubscribeMarketDepth(security);
		
```

If there are no order books, then to check the working ability of strategies that need order books, it is possible to enable the generation:

```cs
var mdGenerator \= new TrendMarketDepthGenerator(connector.GetSecurityId(security));
\_connector.MarketDataAdapter.SendInMessage(new GeneratorMessage
{
    IsSubscribe \= true,
    Generator \= mdGenerator
});
		
```

- The order book update interval [MarketDataGenerator.Interval](../api/StockSharp.Algo.Testing.MarketDataGenerator.Interval.html). The update can not be more often than tick trades come (because the order books are generated before each trade):

  ```cs
  mdGenerator.Interval \= TimeSpan.FromSeconds(1);
  				
  ```
- The order books depth [MarketDepthGenerator.MaxBidsDepth](../api/StockSharp.Algo.Testing.MarketDepthGenerator.MaxBidsDepth.html) and [MarketDepthGenerator.MaxAsksDepth](../api/StockSharp.Algo.Testing.MarketDepthGenerator.MaxAsksDepth.html). The more \- the slower testing:

  ```cs
  mdGenerator.MaxAsksDepth \= 1; 
  mdGenerator.MaxBidsDepth \= 1;
  				
  ```
- Volumes of [MarketDepth.BestBid](../api/StockSharp.BusinessEntities.MarketDepth.BestBid.html) and [MarketDepth.BestAsk](../api/StockSharp.BusinessEntities.MarketDepth.BestAsk.html) are taken from the volume of the trade on which there is generation. The [MarketDepthGenerator.UseTradeVolume](../api/StockSharp.Algo.Testing.MarketDepthGenerator.UseTradeVolume.html) option sets realistic volume level in the order book:

  ```cs
  mdGenerator.UseTradeVolume \= true;
  				
  ```
- The volume level [MarketDataGenerator.MinVolume](../api/StockSharp.Algo.Testing.MarketDataGenerator.MinVolume.html) and [MarketDataGenerator.MaxVolume](../api/StockSharp.Algo.Testing.MarketDataGenerator.MaxVolume.html):

  ```cs
  mdGenerator.MinVolume \= 1;
  mdGenerator.MaxVolume \= 1;
  				
  ```
- The minimum generated spread is equal to [Security.PriceStep](../api/StockSharp.BusinessEntities.Security.PriceStep.html). It should not generate spread between [MarketDepth.BestBid](../api/StockSharp.BusinessEntities.MarketDepth.BestBid.html) and [MarketDepth.BestAsk](../api/StockSharp.BusinessEntities.MarketDepth.BestAsk.html) more than 5 [Security.PriceStep](../api/StockSharp.BusinessEntities.Security.PriceStep.html) (so do not get too wide spread while the generation from candles is performed):

  ```cs
  mdGenerator.MinSpreadStepCount \= 1;
  mdGenerator.MaxSpreadStepCount \= 5;
  				
  ```
