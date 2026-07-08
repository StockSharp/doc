# Cobertura delta

Se pretender proteger posições através de estratégias com opções (por exemplo, como em [Negociação de volatilidade](volatility_trading.md)), pode usar a estratégia de cobertura por delta [DeltaHedgeStrategy](xref:StockSharp.Algo.Strategies.Derivatives.DeltaHedgeStrategy).

## Cobertura delta

1. Como demonstração do funcionamento de [DeltaHedgeStrategy](xref:StockSharp.Algo.Strategies.Derivatives.DeltaHedgeStrategy), o exemplo SampleOptionQuoting é modificado (para detalhes, consulte [Negociação de volatilidade](volatility_trading.md)).
2. A estratégia [VolatilityQuotingStrategy](xref:StockSharp.Algo.Strategies.Derivatives.VolatilityQuotingStrategy) não é iniciada diretamente; em vez disso, é passada como estratégia filha para [DeltaHedgeStrategy](xref:StockSharp.Algo.Strategies.Derivatives.DeltaHedgeStrategy).

   ```cs
   // criar estratégia de hedge delta
   var hedge = new DeltaHedgeStrategy
   {
   	Security = option.GetUnderlyingAsset(Connector),
   	Portfolio = Portfolio.SelectedPortfolio,
   	Connector = Connector,
   };
   // criar cotação de opção para 20 contratos
   var quoting = new VolatilityQuotingStrategy(Sides.Buy, 20,
   		new Range<decimal>(ImpliedVolatilityMin.Value ?? 0, ImpliedVolatilityMax.Value ?? 100))
   {
           // tamanho de trabalho é 1 contrato
   	Volume = 1,
   	Security = option,
   	Portfolio = Portfolio.SelectedPortfolio,
   	Connector = Connector,
   };
   // vincular cotação e hedge
   hedge.ChildStrategies.Add(quoting);
   // iniciar hedge
   hedge.Start();
   ```

   A [DeltaHedgeStrategy](xref:StockSharp.Algo.Strategies.Derivatives.DeltaHedgeStrategy) recebe como estratégias filhas estratégias que trabalham separadamente no respetivo strike. Assim, [DeltaHedgeStrategy](xref:StockSharp.Algo.Strategies.Derivatives.DeltaHedgeStrategy) controla a posição total de todas as estratégias de opções filhas.

3. Concluir a cobertura delta:

   ```none
   hedge.Stop();
   ```
