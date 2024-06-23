# Extended settings

Extended settings of [HistoryEmulationConnector](xref:StockSharp.Algo.Testing.HistoryEmulationConnector).

- [MarketTimeChangedInterval](xref:StockSharp.Algo.Testing.HistoryEmulationConnector.MarketTimeChangedInterval) \- time change event input interval. If trades generators are used, then the trades will be generated with this periodicity. The default is 1 minute.
- [MarketEmulatorSettings.Latency](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.Latency) \- The minimum value of the registered orders latency. By default, it is equal to **TimeSpan.Zero**, which means instant acceptance of the registered orders by the exchange. 
- [MarketEmulatorSettings.MatchOnTouch](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.MatchOnTouch) \- to execute the orders if the price "touches" the level (an assumption is sometimes too "optimistic" and the mode must be switched off for a realistic testing). If the mode is off, then limit orders will be executed if the price is "passed through them" at least in 1 step. The option is available in all modes except the order log. The default is off.

Even if the strategy is tested on the candles, it is necessary to subscribe to the tick trades:

```cs
		_connector.SubscribeTrades(security);
		
```

If the strategy needs the order books, it is necessary to subscribe to the order books:

```cs
		_connector.SubscribeMarketDepth(security);
		
```

If there are no order books, then to check the working ability of strategies that need order books, it is possible to enable the generation:

```cs
var mdGenerator = new TrendMarketDepthGenerator(connector.GetSecurityId(security));
_connector.MarketDataAdapter.SendInMessage(new GeneratorMessage
{
    IsSubscribe = true,
    Generator = mdGenerator
});
		
```

- The order book update interval [MarketDataGenerator.Interval](xref:StockSharp.Algo.Testing.MarketDataGenerator.Interval). The update can not be more often than tick trades come (because the order books are generated before each trade):

  ```cs
  mdGenerator.Interval = TimeSpan.FromSeconds(1);
  				
  ```
- The order books depth [MarketDepthGenerator.MaxBidsDepth](xref:StockSharp.Algo.Testing.MarketDepthGenerator.MaxBidsDepth) and [MarketDepthGenerator.MaxAsksDepth](xref:StockSharp.Algo.Testing.MarketDepthGenerator.MaxAsksDepth). The more \- the slower testing:

  ```cs
  mdGenerator.MaxAsksDepth = 1; 
  mdGenerator.MaxBidsDepth = 1;
  				
  ```
- Volumes of [MarketDepth.BestBid](xref:StockSharp.BusinessEntities.MarketDepth.BestBid) and [MarketDepth.BestAsk](xref:StockSharp.BusinessEntities.MarketDepth.BestAsk) are taken from the volume of the trade on which there is generation. The [MarketDepthGenerator.UseTradeVolume](xref:StockSharp.Algo.Testing.MarketDepthGenerator.UseTradeVolume) option sets realistic volume level in the order book:

  ```cs
  mdGenerator.UseTradeVolume = true;
  				
  ```
- The volume level [MarketDataGenerator.MinVolume](xref:StockSharp.Algo.Testing.MarketDataGenerator.MinVolume) and [MarketDataGenerator.MaxVolume](xref:StockSharp.Algo.Testing.MarketDataGenerator.MaxVolume):

  ```cs
  mdGenerator.MinVolume = 1;
  mdGenerator.MaxVolume = 1;
  				
  ```
- The minimum generated spread is equal to [Security.PriceStep](xref:StockSharp.BusinessEntities.Security.PriceStep). It should not generate spread between [MarketDepth.BestBid](xref:StockSharp.BusinessEntities.MarketDepth.BestBid) and [MarketDepth.BestAsk](xref:StockSharp.BusinessEntities.MarketDepth.BestAsk) more than 5 [Security.PriceStep](xref:StockSharp.BusinessEntities.Security.PriceStep) (so do not get too wide spread while the generation from candles is performed):

  ```cs
  mdGenerator.MinSpreadStepCount = 1;
  mdGenerator.MaxSpreadStepCount = 5;
  				
  ```
