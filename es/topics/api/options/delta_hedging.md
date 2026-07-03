# Cobertura delta

Si desea proteger posiciones mediante estrategias con opciones (por ejemplo, como [Trading de volatilidad](volatility_trading.md)), puede usar la estrategia de cobertura por delta [DeltaHedgeStrategy](xref:StockSharp.Algo.Strategies.Derivatives.DeltaHedgeStrategy). 

## Cobertura delta

1. Como demostración de cómo funciona [DeltaHedgeStrategy](xref:StockSharp.Algo.Strategies.Derivatives.DeltaHedgeStrategy), se modifica el ejemplo SampleOptionQuoting (para más detalles, véase [Trading de volatilidad](volatility_trading.md)).
2. La estrategia [VolatilityQuotingStrategy](xref:StockSharp.Algo.Strategies.Derivatives.VolatilityQuotingStrategy) no se inicia, sino que se pasa como estrategia hija a [DeltaHedgeStrategy](xref:StockSharp.Algo.Strategies.Derivatives.DeltaHedgeStrategy).

   ```cs
   // crear estrategia de cobertura delta
   var hedge = new DeltaHedgeStrategy
   {
   	Security = option.GetUnderlyingAsset(Connector),
   	Portfolio = Portfolio.SelectedPortfolio,
   	Connector = Connector,
   };
   // crear cotización de opciones para 20 contratos
   var quoting = new VolatilityQuotingStrategy(Sides.Buy, 20,
   		new Range<decimal>(ImpliedVolatilityMin.Value ?? 0, ImpliedVolatilityMax.Value ?? 100))
   {
           // tamaño de trabajo: 1 contrato
   	Volume = 1,
   	Security = option,
   	Portfolio = Portfolio.SelectedPortfolio,
   	Connector = Connector,
   };
   // vincular cotización y cobertura
   hedge.ChildStrategies.Add(quoting);
   // iniciar cobertura
   hedge.Start();
   ```

   [DeltaHedgeStrategy](xref:StockSharp.Algo.Strategies.Derivatives.DeltaHedgeStrategy) toma como estrategias hijas las estrategias que trabajan por separado en su strike. Así, [DeltaHedgeStrategy](xref:StockSharp.Algo.Strategies.Derivatives.DeltaHedgeStrategy) controla la posición total de todas las estrategias hijas de opciones. 

3. Finalización de la cobertura delta: 

   ```none
   hedge.Stop();
   ```
