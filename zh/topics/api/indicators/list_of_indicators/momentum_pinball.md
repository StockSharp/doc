# MP

**动量弹球 (MP)** 是一种技术指标，用于分析价格动量及其变化，以识别市场中的潜在反转点和趋势强度。

使用该指标时，需要使用 [MomentumPinball](xref:StockSharp.Algo.Indicators.MomentumPinball) 类。

## 描述

动量弹球（MP）指示器是一种专门的振荡器，用于追踪价格动量并识别潜在的反转点。“弹球”这一名称反映了该指示器能够识别价格像弹球机中的球一样，在极端位置弹跳的能力。

议员分析当前动量与历史极端之间的关系，判断市场何时达到超买或超卖状态。该指标还帮助识别动能开始减弱的时刻，这可能是趋势反转的前奏。

主要思想是极端动量值通常不稳定，达到极端后通常会出现修正或反转。MP 通过跟踪动量本身及其变化，帮助可视化这一过程。

## 参数

该指标具有以下参数：
- **Length** - 计算周期（默认值：14）

## 计算

动量弹球指标的计算包括以下步骤：

1. 将基础动量计算为当前价格与 N 期前价格的差值：
   ```
   Momentum = Price[current] - Price[current - Length]
   ```

2. 确定指定时期的历史最大和最小动量：
   ```
   Max_Momentum = Length 周期内 Maximum(Momentum)
   Min_Momentum = Length 周期内 Minimum(Momentum)
   ```

3. 将当前动量相对于历史极值进行归一化：
   ```
   Normalized_Momentum = (Momentum - Min_Momentum) / (Max_Momentum - Min_Momentum)
   ```

4. 计算归一化动量的变化率：
   ```
   Momentum_Change = Normalized_Momentum[current] - Normalized_Momentum[current - 1]
   ```

5. 最终 MP 计算作为归一化动量及其变化的组合：
   ```
   MP = Normalized_Momentum + Momentum_Change
   ```

其中：
- 价格 - 价格（通常指收盘价）
- 长度 - 计算周期
- 动量 - 基础动量
- Normalized_Momentum - 归一化动量
- 动量变化 - 动量变化

## 解释

动量弹球指标可以解释如下：

1. **极端水平**：
   - 大于0.8的数值表示市场超买状况
   - 低于0.2的数值表示市场超卖状态
   - 当 MP 达到这些水平时，反转或修正的概率会增加

2. **分歧**：
   - 看涨背离：价格创出新低，而MP形成更高的低点
   - 看跌背离：价格创出新高，而MP形成更低的高点
   - 背离通常出现在重要趋势反转之前

3. **中心线交叉**：
   - MP从下向上穿过0.5水平可以被视为看涨信号
   - MP从上向下穿过0.5水平可以被视为看跌信号

4. **从极端反弹**：
   - 从超买或超卖水平的MP反转可以产生市场入场信号
   - 当这种反转伴随着背离时，会形成特别强的信号

5. **趋势分析**:
   - 持续的 MP 值高于 0.5 确认了上升趋势
   - 持续的 MP 值低于 0.5 确认了下降趋势
   - MP 在 0.5 水平附近的波动表明横向趋势或不确定性

6. **动量强度**：
   - MP陡峭的斜坡表示强劲的动量
   - MP斜率浅表明动能较弱
   - 货币供应量增长放缓或下降可能预示趋势反转

7. **与其他指标结合**：
   - MP通常与趋势指标结合使用
   - 例如，移动平均线可以用来确定趋势方向，而MP可以用来确定进出点

![MP 指标图表](../../../../images/indicator_momentum_pinball.png)

## 另请参阅

[动量](momentum.md)
[相对强弱指数](rsi.md)
[随机振荡器](stochastic_oscillator.md)
[优振荡器](pretty_good_oscillator.md)
