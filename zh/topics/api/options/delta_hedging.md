# Delta对冲

如果你想通过期权策略（例如，[波动率交易](volatility_trading.md)）来保护持仓，你可以使用按Delta进行的[Delta对冲策略](xref:StockSharp.Algo.Strategies.Derivatives.DeltaHedgeStrategy)进行对冲。

## Delta对冲

1. 为了演示[DeltaHedgeStrategy](xref:StockSharp.Algo.Strategies.Derivatives.DeltaHedgeStrategy)的工作原理，修改了SampleOptionQuoting示例（详情请参见[波动率交易](volatility_trading.md)）。
2. [VolatilityQuotingStrategy](xref:StockSharp.Algo.Strategies.Derivatives.VolatilityQuotingStrategy) 策略不会启动，而是作为子策略传递给 [DeltaHedgeStrategy](xref:StockSharp.Algo.Strategies.Derivatives.DeltaHedgeStrategy)。

   ```cs
   // 创建 delta 对冲策略
   var hedge = new DeltaHedgeStrategy
   {
   	Security = option.GetUnderlyingAsset(Connector),
   	Portfolio = Portfolio.SelectedPortfolio,
   	Connector = Connector,
   };
   // 为 20 份合约创建期权报价
   var quoting = new VolatilityQuotingStrategy(Sides.Buy, 20,
   		new Range<decimal>(ImpliedVolatilityMin.Value ?? 0, ImpliedVolatilityMax.Value ?? 100))
   {
           // 工作数量为 1 份合约
   	Volume = 1,
   	Security = option,
   	Portfolio = Portfolio.SelectedPortfolio,
   	Connector = Connector,
   };
   // 关联报价和对冲
   hedge.ChildStrategies.Add(quoting);
   // 启动对冲
   hedge.Start();
   ```

   [DeltaHedgeStrategy](xref:StockSharp.Algo.Strategies.Derivatives.DeltaHedgeStrategy) 将策略作为子策略分别按其行权价进行操作。因此，[DeltaHedgeStrategy](xref:StockSharp.Algo.Strategies.Derivatives.DeltaHedgeStrategy) 通过所有子期权策略控制总持仓。

3. 完成德尔塔对冲：

   ```none
   hedge.Stop();
   ```
