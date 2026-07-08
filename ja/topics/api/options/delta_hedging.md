# デルタヘッジ

オプション戦略によってポジションを保護したい場合（たとえば [ボラティリティ取引](volatility_trading.md) のような場合）、デルタによるヘッジ戦略 [DeltaHedgeStrategy](xref:StockSharp.Algo.Strategies.Derivatives.DeltaHedgeStrategy) を使用できます。

## デルタヘッジ

1. [DeltaHedgeStrategy](xref:StockSharp.Algo.Strategies.Derivatives.DeltaHedgeStrategy) の動作例として、SampleOptionQuoting サンプルを変更します（詳細は [ボラティリティ取引](volatility_trading.md) を参照）。
2. [VolatilityQuotingStrategy](xref:StockSharp.Algo.Strategies.Derivatives.VolatilityQuotingStrategy) 戦略は開始せず、代わりに [DeltaHedgeStrategy](xref:StockSharp.Algo.Strategies.Derivatives.DeltaHedgeStrategy) に子戦略として渡します。

   ```cs
   // デルタヘッジ戦略を作成
   var hedge = new DeltaHedgeStrategy
   {
   	Security = option.GetUnderlyingAsset(Connector),
   	Portfolio = Portfolio.SelectedPortfolio,
   	Connector = Connector,
   };
   // 20 コントラクト分のオプションクォーティングを作成
   var quoting = new VolatilityQuotingStrategy(Sides.Buy, 20,
   		new Range<decimal>(ImpliedVolatilityMin.Value ?? 0, ImpliedVolatilityMax.Value ?? 100))
   {
           // 稼働数量は 1 コントラクト
   	Volume = 1,
   	Security = option,
   	Portfolio = Portfolio.SelectedPortfolio,
   	Connector = Connector,
   };
   // クォーティングとヘッジを関連付ける
   hedge.ChildStrategies.Add(quoting);
   // ヘッジを開始
   hedge.Start();
   ```

   [DeltaHedgeStrategy](xref:StockSharp.Algo.Strategies.Derivatives.DeltaHedgeStrategy) は、それぞれのストライクで個別に動作する戦略を子戦略として受け取ります。したがって、[DeltaHedgeStrategy](xref:StockSharp.Algo.Strategies.Derivatives.DeltaHedgeStrategy) はすべての子オプション戦略による合計ポジションを制御します。

3. デルタヘッジの完了:

   ```none
   hedge.Stop();
   ```
