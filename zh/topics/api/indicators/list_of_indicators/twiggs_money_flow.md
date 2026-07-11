# TMF

**Twiggs 资金流向 (TMF)** 是由 Colin Twiggs 开发的一种成交量指标，是 Chaikin 资金流向指标的改进版本。TMF 对市场情绪的变化更为敏感，虚假信号更少。

使用该指标时，需要使用 [TwiggsMoneyFlow](xref:StockSharp.Algo.Indicators.TwiggsMoneyFlow) 类。

## 描述

Twiggs 资金流分析价格与成交量之间的关系，以确定资金流入或流出市场的方向。与传统的成交量指标不同，TMF 通过将数值归一化到 -1 到 +1 之间来消除噪音。

TMF 的主要特征：
- 正值表示资金流入该工具（看涨情绪）
- 负值表示资金从该工具流出（看跌情绪）
- 0的数值表示供需平衡

该指标适用于：
- 确认当前趋势或识别趋势弱点
- 检测价格与资金流的背离
- 识别潜在的市场反转点

## 参数

- **长度** - 指指数移动平均的计算周期，通常使用21作为数值。

## 计算

Twiggs 资金流计算分为几个步骤进行：

1. 计算真实波幅：
   ```
   TR = Max(High - Low, |High - 前一收盘价|, |Low - 前一收盘价|)
   ```

2. 确定Twiggs资金流量成交量（TMFV）：
   ```
   TMFV = Volume * ((Close - Low - (High - Close)) / TR)
   ```
当（高 - 低 = 0）时，TMFV = 0

3. 计算 TMFV 和成交量的指数移动平均：
   ```
   EMA_TMFV = EMA(TMFV, Length)
   EMA_Volume = EMA(Volume, Length)
   ```

4. 最终 TMF 值：
   ```
   TMF = EMA_TMFV / EMA_Volume
   ```

TMF 值范围从 -1（强烈看跌信号）到 +1（强烈看涨信号）。

![IndicatorTwiggsMoneyFlow](../../../../images/indicator_twiggs_money_flow.png)

## 另请参阅

[ADL](accumulation_distribution_line.md)
[资金流动指数](money_flow_index.md)
[能量潮](on_balance_volume.md)