# T3MA

**T3 移动平均线 (T3MA)** 是由 Tim Tillson 开发的一种高级移动平均线。T3 表示经过三次平滑的指数移动平均线 (EMA) 并带有体积因子，使其比传统移动平均线更平滑且不易产生虚假信号。

要使用该指标，需要使用 [T3MovingAverage](xref:StockSharp.Algo.Indicators.T3MovingAverage) 类。

## 描述

T3移动平均线的开发是为了消除传统移动平均线的缺点，例如滞后和错误信号。通过多次平滑处理和可调的体量因子，T3MA提供了一条更平滑的曲线，更准确地跟随价格趋势。

T3MA的主要优点：
- 比普通移动平均线滞后更少
- 曲线更平滑，虚假信号更少
- 由于可调节的容量因子，可适应各种市场条件

T3MA 可用于：
- 确定趋势方向
- 当价格穿过指标线时寻找入场和出场点
- 基于不同周期的多条T3MA交叉构建交易系统

## 参数

- **VolumeFactor** - 决定平滑度的体积因子（通常取值在 0 到 1 之间，推荐值为 0.7）。
- **长度** - 计算周期，类似于普通移动平均中的周期。

## 计算

T3 移动平均的计算分为几个步骤进行：

1. 计算六个周期相同的连续指数移动平均线：
   ```
   EMA1 = EMA(Price, Length)
   EMA2 = EMA(EMA1, Length)
   EMA3 = EMA(EMA2, Length)
   EMA4 = EMA(EMA3, Length)
   EMA5 = EMA(EMA4, Length)
   EMA6 = EMA(EMA5, Length)
   ```

2. 根据获得的EMA和成交量因子计算T3：
   ```
   c1 = -VolumeFactor^3
   c2 = 3 * VolumeFactor^2 + 3 * VolumeFactor^3
   c3 = -6 * VolumeFactor^2 - 3 * VolumeFactor - 3 * VolumeFactor^3
   c4 = 1 + 3 * VolumeFactor + VolumeFactor^3 + 3 * VolumeFactor^2
   
   T3 = c1 * EMA6 + c2 * EMA5 + c3 * EMA4 + c4 * EMA3
   ```

当 VolumeFactor = 0 时，T3 等同于 EMA3（三重 EMA）。当 VolumeFactor = 1 时，T3 达到最大平滑。

![IndicatorT3MovingAverage](../../../../images/indicator_t3_moving_average.png)

## 另请参阅

[EMA](ema.md)
[双指数移动平均](dema.md)
[TEMA](tema.md)
