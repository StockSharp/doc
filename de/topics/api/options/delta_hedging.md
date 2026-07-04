# Delta-Hedging

Wenn Sie Positionen durch Optionsstrategien absichern möchten (zum Beispiel wie bei [Volatility trading](volatility_trading.md)), können Sie die [DeltaHedgeStrategy](xref:StockSharp.Algo.Strategies.Derivatives.DeltaHedgeStrategy) verwenden, eine Hedging-Strategie nach Delta.

## Delta-Hedging

1. Zur Demonstration der Funktionsweise von [DeltaHedgeStrategy](xref:StockSharp.Algo.Strategies.Derivatives.DeltaHedgeStrategy) wird das Beispiel SampleOptionQuoting angepasst (Details siehe [Volatility trading](volatility_trading.md)).
2. Die Strategie [VolatilityQuotingStrategy](xref:StockSharp.Algo.Strategies.Derivatives.VolatilityQuotingStrategy) wird nicht gestartet, sondern als untergeordnete Strategie an [DeltaHedgeStrategy](xref:StockSharp.Algo.Strategies.Derivatives.DeltaHedgeStrategy) übergeben.

   ```cs
   // Delta-Hedge-Strategie erstellen
   var hedge = new DeltaHedgeStrategy
   {
   	Security = option.GetUnderlyingAsset(Connector),
   	Portfolio = Portfolio.SelectedPortfolio,
   	Connector = Connector,
   };
   // Optionsquoting für 20 Kontrakte erstellen
   var quoting = new VolatilityQuotingStrategy(Sides.Buy, 20,
   		new Range<decimal>(ImpliedVolatilityMin.Value ?? 0, ImpliedVolatilityMax.Value ?? 100))
   {
           // Arbeitsgröße ist 1 Kontrakt
   	Volume = 1,
   	Security = option,
   	Portfolio = Portfolio.SelectedPortfolio,
   	Connector = Connector,
   };
   // Quoting und Hedging verknüpfen
   hedge.ChildStrategies.Add(quoting);
   // Hedging starten
   hedge.Start();
   ```

   [DeltaHedgeStrategy](xref:StockSharp.Algo.Strategies.Derivatives.DeltaHedgeStrategy) übernimmt Strategien, die separat auf ihrem Strike arbeiten, als untergeordnete Strategien. Dadurch kontrolliert [DeltaHedgeStrategy](xref:StockSharp.Algo.Strategies.Derivatives.DeltaHedgeStrategy) die Gesamtposition über alle untergeordneten Optionsstrategien.

3. Beenden des Delta-Hedging:

   ```none
   hedge.Stop();
   ```

