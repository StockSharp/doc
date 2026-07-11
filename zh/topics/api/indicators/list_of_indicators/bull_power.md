# 多头力量

**多头力量（多头力量）** 是 Elder-ray 系统中的看涨对应指标。它通过比较K线最高价与
指数移动平均线（EMA）来衡量买家推动价格上涨的力度。

使用 [BullPower](xref:StockSharp.Algo.Indicators.BullPower) 类可以操作该指标。

## 描述

该指标使用的公式为：

`多头力量 = High − EMA`。

- 正值表明看涨压力并支持上涨趋势。
- 数值下降至零或低于零表示多头力量减弱。
- 极端高峰可能预示着调整，尤其是当EMA向下指时。

## 参数

多头力量 从 [ExponentialMovingAverage](xref:StockSharp.Algo.Indicators.ExponentialMovingAverage) 继承其参数：

- **长度** — EMA周期。
- **Alpha**（可选）——平滑系数（如适用）。

## 使用

- 上升的多头力量与上升的 EMA 一起确认了趋势强度。
- 价格创出新高而多头力量读数没有创高，形成看跌背离。
- 将多空能量与价格EMA结合，以评估完整的[埃尔德射线指数](elder_ray.md)结构。

![指标_多头力量](../../../../images/indicator_bull_power.png)

## 另请参阅

[熊势能](bear_power.md)
[长老射线](elder_ray.md)
[指数移动平均线](ema.md)
