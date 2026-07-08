# PVI

**正量指标（PVI）** 是由保罗·迪萨特（Paul Dysart）开发的技术指标，侧重于交易量相比前一天增加的日子的价格变化。

要使用该指标，需要使用 [PositiveVolumeIndex](xref:StockSharp.Algo.Indicators.PositiveVolumeIndex) 类。

## 描述

正向成交量指数（PVI）基于这样一种观点：‘大众’（非专业交易者）在高成交量日更为活跃，而‘聪明资金’在低成交量日采取行动。PVI 仅在当天成交量高于前一交易日的成交量时发生变化。

该指标表明，在成交量增加的情况下价格的波动具有重要意义，并且通常反映大众市场的情绪。PVI跟踪这些波动，忽略成交量较低日的价格变化。

PVI通常与互补的负成交量指标（NVI）一起使用，相反，NVI仅考虑成交量下降的日子。

## 计算

正体积指标的计算涉及以下步骤：

1. 设置初始 PVI 值（通常为 1000）：
   ```
   PVI[initial] = 1000
   ```

2. 对于每一个后续时期：
   ```
   If Volume[current] > Volume[previous], then:
       PVI[current] = PVI[previous] * (1 + (Price[current] - Price[previous]) / Price[previous])
   Otherwise:
       PVI[current] = PVI[previous]
   ```

地点：
- 价格 - 价格（通常指收盘价）
- 成交量 - 交易量

换句话说，PVI 仅在交易量增加的日子发生变化，而在交易量减少或保持不变的日子则保持不变。

## 解释

正成交量指数可以解释如下：

1. **趋势分析**:
   - PVI 上升表明“散户”正在买入，这可能预示未来价格上涨
   - PVI下降表明“群众”在抛售，这可能预示未来价格下跌

2. **移动平均交叉**：
   - PVI 通常与其 255 日移动平均线（大约一年的交易时间）进行比较
   - 当PVI高于其255天SMA时，被视为看涨信号
   - 当PVI低于其255天SMA时，被视为看跌信号

3. **分歧**：
   - 看涨背离：价格形成新低，而PVI形成更高的低点
   - 看跌背离：价格形成新高，而PVI形成较低的新高

4. **与 NVI 结合**：
   - 当 PVI 和 NVI 都在上升时，这是一个强烈的看涨信号
   - 当 PVI 和 NVI 都在下降时，这是一个强烈的看跌信号
   - 当PVI上升而NVI下降时，可能表明“大众”在买入，而“聪明资金”在卖出（潜在的看涨情景）
   - 当PVI下降而NVI上升时，可能表明“大众”在卖出，而“聪明资金”在买入（潜在的看跌情景）

5. **长期变化**：
   - PVI 通常被视为长期指标
   - PVI方向的持续变化可能预示市场情绪的重大转变

6. **确认其他指标**：
   - 当与其他技术指标和分析方法结合使用时，PVI 的效果最佳
   - 当其他指标确认时，PVI信号变得更可靠

7. **设置阈值**：
   - 一些交易者为PVI（e.g.，高于或低于移动平均线5%）设定阈值水平
   - 穿越这些门槛可能被认为比简单的交叉更强的信号

![指标_正向成交量指数](../../../../images/indicator_positive_volume_index.png)

## 另请参阅

[OBV](on_balance_volume.md)
[ADL](accumulation_distribution_line.md)
[Chaikin 资金流量](chaikin_money_flow.md)
[力量指数](force_index.md)
[负成交量指数](negative_volume_index.md)
