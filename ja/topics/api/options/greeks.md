# グリークス

[ブラック-ショールズモデル](https://en.wikipedia.org/wiki/Black–Scholes_model) の公式は、基本的な「グリークス」であるデルタ、ガンマ、ベガ、シータ、ローを計算するために [S#](../../api.md) で実装されています。[ボラティリティ取引](volatility_trading.md) および [デルタヘッジ](delta_hedging.md) 戦略は、この公式に基づいて実装されています。また、[S#](../../api.md) ではオプションプレミアムと [IV](https://en.wikipedia.org/wiki/Implied_volatility) を計算できます。

次のコードは、「グリークス」を計算する [BlackScholes](xref:StockSharp.Algo.Derivatives.BlackScholes) クラスのメソッドを示しています。

```cs
var bs = new BlackScholes(option, _connector, _connector);
DateTimeOffset currentTime = DateTimeOffset.Now;
decimal delta = bs.Delta(currentTime);
decimal gamma = bs.Gamma(currentTime);
decimal vega = bs.Vega(currentTime);
decimal theta = bs.Theta(currentTime);
decimal rho = bs.Rho(currentTime);
decimal iv = bs.ImpliedVolatility(currentTime, premium);  // premium はオプションコントラクトのプレミアム
```

さらに、インストールパッケージには OptionCalculator サンプルが含まれており、このサンプルではすべての「グリークス」が計算され、[OptionDesk](xref:StockSharp.Xaml.OptionDesk) グラフィックコンポーネントを使用して可視化されます。[グラフィックコンポーネント](graphic_components.md) を参照してください。
