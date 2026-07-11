# 空头力量

**空头力量** 是亚历山大·埃尔德（Alexander Elder）的埃尔德射线系统的一部分，用于衡量卖方相对于指数移动平均线（EMA）的力量。它衡量日内最低价低于平均价格的程度，并突出显示空头失去控制的时刻。

使用 [BearPower](xref:StockSharp.Algo.Indicators.BearPower) 类以访问指标。

## 描述

该指标的计算方法是柱低与EMA值的差：

`空头力量 = Low − EMA`。

- 负面读数确认了卖压。
- 数值上升至零或零以上表明空头减弱，并可能出现看涨反转。
- 深谷往往出现在反弹之前，尤其是在恐慌性抛售期间。

## 参数

空头力量 继承了 [ExponentialMovingAverage](xref:StockSharp.Algo.Indicators.ExponentialMovingAverage) 的配置：

- **长度** — EMA周期。
- **Alpha**（可选）——如果EMA以这种方式配置，则为平滑系数。

## 使用

- 当EMA开始上升时，如果熊势能在极低点之后转向上升，要寻找反转信号。
- 穿越零线可能确认当前趋势的变化。
- 将熊力与[多头力量](bull_power.md)以及价格EMA结合起来，构建完整的[埃尔德射线指数](elder_ray.md)指标。

![空头力量 指标图表](../../../../images/indicator_bear_power.png)

## 另请参阅

[多头力量](bull_power.md)
[埃尔德射线指数](elder_ray.md)
[指数移动平均线](ema.md)
