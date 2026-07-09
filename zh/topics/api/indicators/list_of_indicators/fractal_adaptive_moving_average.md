# FRAMA

**分形自适应移动平均线（FRAMA）** 是由约翰·埃勒斯（John Ehlers）开发的一种技术指标，它根据市场的分形维度调整对价格变化的反应速度。

使用该指标时，需要使用 [FractalAdaptiveMovingAverage](xref:StockSharp.Algo.Indicators.FractalAdaptiveMovingAverage) 类。

## 描述

分形自适应移动平均线（FRAMA）是一种先进的指数移动平均线（EMA）类型，它会根据市场的分形维度自动调整对价格变化的敏感性。该指标由约翰·埃勒斯（John Ehlers）开发，并于2000年10月在《技术分析股票与商品》杂志上介绍。

FRAMA 使用分形几何的概念来分析市场结构。它确定当前市场有多“分形”或混乱，并根据此调整指标的反应速度：

- 在趋势（较少分形）的市场条件下，FRAMA 对价格变化的反应迅速，类似于短期 EMA
- 在横向（更分形）的市场条件下，FRAMA 的反应更慢，类似于长期 EMA

这使得FRAMA能够对显著的价格变动作出更快的反应，并忽略市场噪音，使其相比传统移动平均线更为有效。

## 参数

该指标具有以下参数：
- **长度** - 计算周期（默认值：10-20）

## 计算

FRAMA 计算涉及几个步骤：

1. 根据高低价格长度与周期数的对数比计算分形维数（D）:
   ```
   N1 = High(1...Length/2) - Low(1...Length/2)
   N2 = High(Length/2+1...Length) - Low(Length/2+1...Length)
   N3 = High(1...Length) - Low(1...Length)
   
   D = (log(N1 + N2) - log(N3)) / log(2)
   ```

2. 将分形维数转换为指数平滑的α因子：
   ```
   Smoothing Factor = exp(-4.6 * (D - 1))
   Alpha = Smoothing Factor * Smoothing Factor
   ```

3. 将 alpha 因子应用于当前价格和之前的 FRAMA 值：
   ```
   FRAMA = Alpha * Price + (1 - Alpha) * FRAMA[previous]
   ```

其中：
- 高 - 该期间的最高价格
- 低 - 该期间的最低价格
- log - 自然对数

## 解释

FRAMA 可以类似于其他移动平均线来解释，但需要考虑其自适应特性：

1. **FRAMA 方向**:
   - 向上的FRAMA表明上升趋势
   - 下降的FRAMA表明下降趋势

2. **与价格的交叉**：
   - 当价格从下方穿过FRAMA向上时，可以视为看涨信号
   - 当价格从上方穿过FRAMA向下时，可以视为看跌信号

3. **多个 FRAMA 交叉**:
   - 从下向上穿越短期FRAMA与长期FRAMA可能表示上升趋势的开始
   - 从上到下短期FRAMA与长期FRAMA的交叉可能表示下行趋势的开始

4. **FRAMA 倾斜角**：
   - 陡峭的斜坡角度表明趋势强劲
   - 浅坡角表明趋势较弱
   - 水平移动表示横向趋势

5. **信号过滤**：
   - 由于其自适应特性，FRAMA 比传统移动平均线产生的错误信号更少
   - FRAMA周期越短，指标对价格变化的敏感度就越高

6. **支撑和阻力位**：
   - FRAMA 可以在上升趋势中作为一个动态支撑位
   - FRAMA 可以在下跌趋势中充当动态阻力位

![指标_分形_自适应移动平均](../../../../images/indicator_fractal_adaptive_moving_average.png)

## 另请参阅

[EMA](ema.md)
[考夫曼自适应移动平均线 (KAMA)](kama.md)
[可变指数动态平均 (VIDYA)](vidya.md)