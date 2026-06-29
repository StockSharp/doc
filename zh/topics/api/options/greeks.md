# 希腊人

[Black–Scholes 模型](https://en.wikipedia.org/wiki/Black–Scholes_model) 的公式在 [S#](../../api.md) 中实现，用于计算基本的“希腊字母”：delta、gamma、vega、theta 和 rho。基于此公式实现了 [波动率交易](volatility_trading.md) 和 [Delta 对冲](delta_hedging.md) 策略。[S#](../../api.md) 还允许你计算期权溢价和 [隐含波动率](https://en.wikipedia.org/wiki/Implied_volatility)。

以下代码展示了用于计算“希腊字母”的 [BlackScholes](xref:StockSharp.Algo.Derivatives.BlackScholes) 类方法。

```cs
var bs = new BlackScholes(option, _connector, _connector);
DateTimeOffset currentTime = DateTimeOffset.Now;
decimal delta = bs.Delta(currentTime);
decimal gamma = bs.Gamma(currentTime);
decimal vega = bs.Vega(currentTime);
decimal theta = bs.Theta(currentTime);
decimal rho = bs.Rho(currentTime);
decimal iv = bs.ImpliedVolatility(currentTime, premium);  // premium is premium of the option contract
```

此外，安装包中包括 OptionCalculator 示例，其中所有的“希腊字母”都使用 [OptionDesk](xref:StockSharp.Xaml.OptionDesk) 图形组件进行计算和可视化。请参阅 [图形组件](graphic_components.md)。
