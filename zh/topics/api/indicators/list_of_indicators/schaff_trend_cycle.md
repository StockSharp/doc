# STC

**Schaff 趋势周期 (STC)** 是由 Doug Schaff 开发的一种动量指标。STC 基于这样的假设：市场周期更频繁地在超买和超卖状态之间移动，而不是处于真正的趋势中。

要使用该指标，您需要使用 [SchaffTrendCycle](xref:StockSharp.Algo.Indicators.SchaffTrendCycle) 类。

## 描述

Schaff 趋势周期结合了随机振荡器、MACD 和周期分析的优点。该指标能够比传统指标如 MACD 或随机指标更快地对趋势变化作出反应。

STC 在 0 到 100 之间波动：
- 数值超过75通常表示超买状态
- 低于25的数值表示超卖状态
- 突破50水平可能预示着趋势变化

主要指标信号：
- 当STC从下向上穿过25水平（退出超卖区）时买入
- 当STC从上向下穿过75水平（退出超买区）时卖出

## 参数

- **长度** - 计算指标的主要周期。

## 计算

STC 计算分为几个步骤进行：

1. 计算 MACD：
   ```
   MACD = EMA(Close, Fast) - EMA(Close, Slow)
   Signal = EMA(MACD, Signal)
   ```
其中 Fast、Slow 和 Signal 通常分别为 23、50 和 10。

2. 基于MACD计算随机振荡器：
   ```
   Stoch_K = 100 * ((MACD - Lowest(MACD, Length)) / (Highest(MACD, Length) - Lowest(MACD, Length)))
   Stoch_D = EMA(Stoch_K, 3)
   ```

3. 重复随机计算以获得 STC：
   ```
   STC = 100 * ((Stoch_D - Lowest(Stoch_D, Length)) / (Highest(Stoch_D, Length) - Lowest(Stoch_D, Length)))
   ```

结果是一个比经典随机指标更平滑、比MACD对趋势变化反应更快的振荡器。

![IndicatorSchaffTrendCycle](../../../../images/indicator_schaff_trend_cycle.png)

## 另请参阅

[MACD](macd.md)
[随机指标](stochastic_oscillator.md)