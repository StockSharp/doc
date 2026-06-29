# Delta对冲

如果你想通过期权策略（例如，[波动率交易](volatility_trading.md)）来保护头寸，你可以使用按Delta进行的[Delta对冲策略](xref:StockSharp.Algo.Strategies.Derivatives.DeltaHedgeStrategy)进行对冲。

## Delta对冲

1. 为了演示[DeltaHedgeStrategy](xref:StockSharp.Algo.Strategies.Derivatives.DeltaHedgeStrategy)的工作原理，修改了SampleOptionQuoting示例（详情请参见[波动率交易](volatility_trading.md)）。
2. [VolatilityQuotingStrategy](xref:StockSharp.Algo.Strategies.Derivatives.VolatilityQuotingStrategy) 策略不会启动，而是作为子策略传递给 [DeltaHedgeStrategy](xref:StockSharp.Algo.Strategies.Derivatives.DeltaHedgeStrategy)。

   ```cs
   // create delta hedge strategy
   var hedge = new DeltaHedgeStrategy
   {
   	Security = option.GetUnderlyingAsset(Connector),
   	Portfolio = Portfolio.SelectedPortfolio,
   	Connector = Connector,
   };
   // create option quoting for 20 contracts
   var quoting = new VolatilityQuotingStrategy(Sides.Buy, 20,
   		new Range<decimal>(ImpliedVolatilityMin.Value ?? 0, ImpliedVolatilityMax.Value ?? 100))
   {
           // working size is 1 contract
   	Volume = 1,
   	Security = option,
   	Portfolio = Portfolio.SelectedPortfolio,
   	Connector = Connector,
   };
   // link quoting and hedging
   hedge.ChildStrategies.Add(quoting);
   // start hedging
   hedge.Start();
   ```

   [DeltaHedgeStrategy](xref:StockSharp.Algo.Strategies.Derivatives.DeltaHedgeStrategy) 将策略作为子策略分别按其行权价进行操作。因此，[DeltaHedgeStrategy](xref:StockSharp.Algo.Strategies.Derivatives.DeltaHedgeStrategy) 通过所有子期权策略控制总头寸。

3. 完成德尔塔对冲：

   ```none
   hedge.Stop();
   ```
