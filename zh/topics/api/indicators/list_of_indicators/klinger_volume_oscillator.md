# KVO

**克林格成交量振荡器（KVO）** 是由斯蒂芬·克林格开发的一个技术指标，它利用成交量和价格来识别市场的长期趋势和短期反转。

使用该指标时，需要使用 [KlingerVolumeOscillator](xref:StockSharp.Algo.Indicators.KlingerVolumeOscillator) 类。

## 描述

克林格量能振荡器（KVO）由斯蒂芬·克林格（Stephen Klinger）创建，用于衡量成交量与价格之间的背离。该指标基于价格变动由成交量确认的概念。KVO旨在确定趋势方向，同时还判断其强度和潜在的反转点。

KVO 将价格信息与成交量结合，使用一个考虑价格变动方向和幅度以及交易量的成交量力量指标。然后，它对这股资金流应用两个不同周期的指数移动平均线（EMA），并计算它们之间的差值。

该指标是一个在零线以上和以下波动的振荡器。KVO的正值表示买方控制市场，而负值表示卖方占优势。

## 参数

该指标具有以下参数：
- **短期** - 用于计算短期EMA的周期（默认值：34）
- **LongPeriod** - 用于计算长EMA的周期（默认值：55）

## 计算

克林格体积振荡器的计算涉及几个步骤：

1. 确定每个时期的趋势：
   ```
   Trend = +1, 如果 (High + Low + Close) > (High[previous] + Low[previous] + Close[previous])
   Trend = -1, 否则
   ```

2. 计算成交量指标：
   ```
   成交量力度 = Volume * Trend * abs(2 * ((Close - Low) - (High - Close)) / (High - Low))
   ```
如果（最高价 - 最低价）为零，成交量力将设为成交量乘以趋势。

3. 计算两个周期的EMA：
   ```
   短期 EMA = EMA(成交量力度, ShortPeriod)
   长期 EMA = EMA(成交量力度, LongPeriod)
   ```

4. 最终KVO计算：
   ```
   KVO = 短期 EMA - 长期 EMA
   ```

5. 计算信号线（可选）：
   ```
   信号线 = EMA(KVO, 13)
   ```

其中：
- 高、低、收盘 - 最高价、最低价和收盘价
- 成交量 - 交易量
- EMA - 指数移动平均
- ShortPeriod - 短期EMA的周期
- LongPeriod - 长期EMA的周期

## 解释

克林格成交量振荡器可以解释如下：

1. **零线交叉**：
   - KVO 从下向上穿过零线可以被视为看涨信号
   - KVO 从上向下穿过零线可以被视为看跌信号

2. **信号线交叉**：
   - KVO从下方向上穿越信号线可以被视为看涨的入场信号
   - KVO从上方向下穿过信号线可以被视为看跌的入场信号

3. **分歧**：
   - 看涨背离：价格形成新低，而KVO形成更高的低点
   - 看跌背离：价格形成新高，而KVO形成较低的高点

4. **趋势确认**:
   - 正的KVO值确认了上升趋势
   - 负KVO值确认了下降趋势

5. **趋势强度**：
   - KVO值的增加（无论正负）都表示当前趋势的加强
   - KVO 值下降表示当前趋势减弱

6. **潜在反转**：
   - 极端的KVO值可能表明市场处于超买或超卖状态，并可能出现反转
   - KVO 上升或下降的放缓可能预示趋势反转

7. **成交量和价格**：
   - KVO 允许评估价格和成交量变化的一致性
   - 顺势方向的强成交量会导致更极端的KVO值

![克林格成交量振荡器](../../../../images/indicator_klinger_volume_oscillator.png)

## 另请参阅

[OBV](on_balance_volume.md)
[Chaikin 资金流量](chaikin_money_flow.md)
[累积/派发线](accumulation_distribution_line.md)
[力量指数](force_index.md)
