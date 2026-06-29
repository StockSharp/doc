# 的

**需求指数（DI）** 是由 James Sibbett 开发的技术指标，用于分析价格与成交量之间的关系，以评估市场中的需求强度和买方压力。

要使用该指标，您需要使用 [DemandIndex](xref:StockSharp.Algo.Indicators.DemandIndex) 类。

## 描述

需求指数（DI）是一个综合的交易量指标，用于评估价格与交易量之间的关系，从而判断买方压力（需求）相对于卖方压力的强弱。该指标基于这样一个假设：价格变动与交易量变动的比率能够比单独观察价格或交易量更准确地评估市场需求。

DI旨在识别以下市场情况：
- 强劲需求（买方压力）
- 需求疲软（卖方压力）
- 价格与成交量之间的不平衡（潜在反转点）
- 确认或否定当前趋势

## 参数

该指标具有以下参数：
- **长度** - 计算周期（默认值：13）

## 计算

需求指数的计算相当复杂，涉及几个阶段：

1. 根据价格变动计算价格组成部分：
   ```
   Price Component = ((High + Low + Close) / 3) - ((Previous High + Previous Low + Previous Close) / 3)
   ```

2. 计算体积分量，同时考虑相对体积变化。

3. 将需求计算为价格和数量组成部分的比率：
   ```
   Raw Demand = Price Component / Volume Component
   ```

4. 平滑所获得的值以减少噪声：
   ```
   Smoothed Demand = EMA(Raw Demand, Length)
   ```

5. 将结果归一化以获得最终指数：
   ```
   Demand Index = 100 * Normalized(Smoothed Demand)
   ```

## 解释

需求指数可以有多种解读方式：

1. **极端水平**：
   - 高正值表示需求强劲（买方压力）
   - 高负值表示需求疲软（卖方压力）

2. **零线交叉**：
   - 从下向上穿越可以被视为看涨信号
   - 从上到下的穿越可以被视为看跌信号

3. **分歧**：
   - 看涨背离：价格创出新低，但DI形成更高的低点
   - 看跌背离：价格创出新高，但DI形成了较低的新高

4. **DI 趋势**：
   - 持续的正DI值确认了上升趋势
   - 持续的负DI值确认了下降趋势

5. **极端值**：
   - 非常高或非常低的数值可能表明市场处于超买或超卖状态

在使用需求指数时，结合其他指标和分析方法最为有效，以筛选出错误信号。

![indicator_demand_index](../../../../images/indicator_demand_index.png)

## 另请参阅

[OBV](on_balance_volume.md)
[ADL](accumulation_distribution_line.md)
[钱流量指标](chaikin_money_flow.md)
[力量平衡](balance_of_power.md)