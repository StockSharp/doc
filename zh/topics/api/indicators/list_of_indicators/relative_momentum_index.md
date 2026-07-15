# RMI

**相对动量指数（RMI）** 是对传统 RSI 指标的一个改进，由 Roger Altman 提出。与经典 RSI 计算一段时间内价格上涨和下跌比例不同，RMI 考虑了选定动量周期内的相对价格变化。

使用该指标时，需要使用 [RelativeMomentumIndex](xref:StockSharp.Algo.Indicators.RelativeMomentumIndex) 类。

## 描述

相对动量指数（RMI）通过增加一个动量周期参数改进了经典的RSI。这使得交易者可以在不改变主要计算周期的情况下调整指标的敏感性。

像RSI一样，RMI在0到100之间波动：
- 高于70的数值通常表示市场超买
- 低于30的数值表示市场超卖
- 位于50的中心线作为确定主要市场走势方向的参考点

RMI 对于识别潜在的趋势反转点以及确认当前趋势的强度特别有用。

## 参数

- **动量周期** - 动量周期，定义价格比较的时间滞后。
- **长度** - 用于计算指标的主要周期（类似于 RSI 中的周期）。

## 计算

RMI 计算分几个步骤进行：

1. 将动量计算为当前价格与 n 期前价格的差值：
   ```
   Momentum = Price(current) - Price(current - MomentumPeriod)
   ```

2. 将动量分为正（U）和负（D）:
   ```
   如果 Momentum > 0，则 U = Momentum，D = 0
   如果 Momentum < 0，则 U = 0，D = |Momentum|
   ```

3. 计算指定期间正负动量的平均值：
   ```
   AverageU = SMA(U, Length)
   AverageD = SMA(D, Length)
   ```

4. 计算相对强度：
   ```
   RS = AverageU / AverageD
   ```

5. 转换为相对动量指数：
   ```
   RMI = 100 - (100 / (1 + RS))
   ```

![RMI 指标图表](../../../../images/indicator_relative_momentum_index.png)

## 另请参阅

[相对强弱指数](rsi.md)
[动量](momentum.md)
