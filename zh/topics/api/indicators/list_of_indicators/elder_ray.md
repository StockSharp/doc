# 埃尔德射线指数

**埃尔德射线指数**是亚历山大·埃尔德开发的一个综合指标，它将指数移动平均线与多头力量（多头力量
）和空头力量（空头力量）振荡器结合在一起。它可视化买卖双方的平衡，并有助于识别何时一方失去控制。

使用 [ElderRay](xref:StockSharp.Algo.Indicators.ElderRay) 类来访问该指标。

## 组件

该指标返回一个包含以下内容的 [ElderRayValue](xref:StockSharp.Algo.Indicators.ElderRayValue) 结构：

- **EMA** — 收盘价的基准指数移动平均线；
- **多头力量** — K线最高价与 EMA 之间的距离；
- **空头力量** — K线最低价与 EMA 之间的距离。

## 参数

埃尔德射线 继承了 [ExponentialMovingAverage](xref:StockSharp.Algo.Indicators.ExponentialMovingAverage) 的设置：

- **长度** — EMA周期；
- **Alpha** — 平滑系数，直接配置时使用。

## 解释

- **多头力量 > 0** 以及上升的 EMA 确认了上涨趋势。
- **空头力量 < 0** 且 EMA 下降确认下跌趋势。
- 在价格上涨时多头力量减弱或在价格下跌时空头力量增强会形成背离，并警示反转。
- 多头力量或空头力量的零线穿越标志着市场控制权的转变。

交易决策是通过同时分析EMA和两个振荡器来做出的。例如，当出现以下情况时，会出现买入机会
EMA正在上升，熊力从新的低点回升，多头力量突破零点。

![indicator_elder_ray](../../../../images/indicator_elder_ray.png)

## 另请参阅

[多头力量](bull_power.md)
[空头力量](bear_power.md)
[指数移动平均](ema.md)
