# CMF

**Chaikin资金流量(CMF)** 是由Mark Chaikin开发的一个技术指标，用于衡量市场在特定时期内的资金流动（积累与分配）强度。

要使用该指标，你需要使用 [ChaikinMoneyFlow](xref:StockSharp.Algo.Indicators.ChaikinMoneyFlow) 类。

## 描述

查金资金流量（CMF）扩展了累积/分配线（A/D线）的概念，关注特定时间段。该指标衡量在指定时间段内作为总交易量百分比表示的资金流量。

CMF 帮助交易者：
- 确定买卖压力的强度
- 识别累积（买入）和分配（卖出）趋势
- 检测价格变动与资金流动之间的背离
- 确认当前趋势或其弱点

CMF 的核心理念是，在强劲的上升趋势中，收盘价应更接近该周期的最高价，而在强劲的下降趋势中，收盘价应更接近该周期的最低价。

## 参数

该指标具有以下参数：
- **长度** - 计算周期（标准值：20-21天）

## 计算

CMF 计算包括以下步骤：

1. 计算每个期间的资金流动乘数：
   ```
   Money Flow Multiplier = ((Close - Low) - (High - Close)) / (High - Low)
   ```

如果（最高价 - 最低价）= 0，则资金流量乘数 = 0。

2. 计算该期间的资金流量
   ```
   Money Flow Volume = Money Flow Multiplier * Volume
   ```

3. 计算查金资金流量：
   ```
   CMF = Sum(Length 周期内 Money Flow Volume) / Sum(Length 周期内 Volume)
   ```

## 解释

CMF 围绕零线波动，通常在 -1 到 +1 的范围内：

- **正 CMF 值**（高于零）：
  - 显示买方压力（累积）
  - 数值越高，买方压力越强
  - 如果持续较长时间，则尤为重要

- **负 CMF 值**（低于零）：
  - 显示卖方压力（分布）
  - 数值越低，卖方压力越大
  - 在负值区的长期停留确认了下降趋势

- **零线穿越**:
  - 从下向上穿越可能表示上升趋势的开始
  - 从上向下的穿越可能预示着下行趋势的开始

- **分歧**：
  - 看涨背离：价格下跌而CMF上升（潜在的向上反转）
  - 看跌背离：价格上涨而CMF下跌（可能的向下反转）

- **极端水平**：
  - 大于 +0.25 的数值可能表示强烈的累积
  - 低于 -0.25 的数值可能表示强分布

![凯钦资金流量指标](../../../../images/indicator_chaikin_money_flow.png)

## 另请参阅

[ADL](accumulation_distribution_line.md)
[OBV](on_balance_volume.md)
[力指数](force_index.md)
[MFI](money_flow_index.md)