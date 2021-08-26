# Delta hedging

If you want to protect positions by option strategies (for example, as [Volatility trading](OptionsQuoting.md)) you can use the [DeltaHedgeStrategy](../api/StockSharp.Algo.Strategies.Derivatives.DeltaHedgeStrategy.html) hedging strategy by delta. 

### Delta hedging

Delta hedging

1. As a demonstration of the [DeltaHedgeStrategy](../api/StockSharp.Algo.Strategies.Derivatives.DeltaHedgeStrategy.html) work the SampleOptionQuoting example is modified (for details see [Volatility trading](OptionsQuoting.md)). 
2. The [VolatilityQuotingStrategy](../api/StockSharp.Algo.Strategies.Derivatives.VolatilityQuotingStrategy.html) strategy does not start, but instead it is passed as a child, for the [DeltaHedgeStrategy](../api/StockSharp.Algo.Strategies.Derivatives.DeltaHedgeStrategy.html) strategy. 

   ```cs
   \/\/ create delta hedge strategy
   var hedge \= new DeltaHedgeStrategy
   {
   	Security \= option.GetUnderlyingAsset(Connector),
   	Portfolio \= Portfolio.SelectedPortfolio,
   	Connector \= Connector,
   };
   \/\/ create option quoting for 20 contracts
   var quoting \= new VolatilityQuotingStrategy(Sides.Buy, 20,
   		new Range\<decimal\>(ImpliedVolatilityMin.Value ?? 0, ImpliedVolatilityMax.Value ?? 100))
   {
           \/\/ working size is 1 contract
   	Volume \= 1,
   	Security \= option,
   	Portfolio \= Portfolio.SelectedPortfolio,
   	Connector \= Connector,
   };
   \/\/ link quoting and hending
   hedge.ChildStrategies.Add(quoting);
   \/\/ start henging
   hedge.Start();
   ```

   The [DeltaHedgeStrategy](../api/StockSharp.Algo.Strategies.Derivatives.DeltaHedgeStrategy.html) takes strategies, working separately on their strike, as [child strategies](StrategyChilds.md). Thus, [DeltaHedgeStrategy](../api/StockSharp.Algo.Strategies.Derivatives.DeltaHedgeStrategy.html) controls the total position by all child option strategies. 
3. Completing the delta hedging: 

   ```none
   hedge.Stop();
   ```

## Recommended content
