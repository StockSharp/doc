# GAPO

**戈帕拉克里希南波动指数（GAPO）** 是由 Tushar Gopalakrishnan 开发的一个技术指标，用于通过对数刻度衡量市场波动性。

要使用该指标，您需要使用 [GopalakrishnanRangeIndex](xref:StockSharp.Algo.Indicators.GopalakrishnanRangeIndex) 类。

## 描述

Gopalakrishnan 范围指数（GAPO）是一种波动性指标，使用对数刻度来衡量特定时期内的总体价格区间。它由 Tushar Gopalakrishnan 开发，并在《股票与商品技术分析》杂志上发表。

GAPO 通过测量一段时间内最高价与最低价的对数比率来评估极端的市场波动。这种方法使该指标能够更准确地反映波动性增加，尤其是在价格剧烈波动的时期。

GAPO 指标特别适用于：
- 识别高波动性和低波动性时期
- 在极端行情之后检测潜在的反转点
- 调整其他基于波动率的指标的参数
- 根据当前市场情况调整交易策略

## 参数

该指标具有以下参数：
- **长度** - 计算周期（默认值：10）

## 计算

Gopalakrishnan 范围指数的计算非常简单：

```
GAPO = log(N) * log(Highest High - Lowest Low)
```

地点：
- log - 自然对数
- N - 期数（长度）
- 最高高点 - 在长度周期内的最高高点
- 最低低点 - 在指定周期内的最低低点

## 解释

戈帕拉克里希南指数可以解释如下：

1. **绝对值**：
   - 高 GAPO 值表示高波动期
   - 低 GAPO 值表示波动性低的时期
   - 极高的数值可能表明市场可能过度扩张并存在潜在的回调

2. **GAPO 趋势**：
   - GAPO 值增加表示波动性增加
   - GAPO 值下降表示波动性下降
   - 一个急剧的GAPO跳跃可能预示着新趋势运动的开始

3. **相对等级**：
   - 将当前的GAPO值与其历史水平进行比较可以评估相对波动性
   - 超过历史范围第95百分位的数值可能表明极端波动
   - 低于历史范围第5百分位的数值可能表示异常低的波动性

4. **交易策略**：
   - 在高波动时期（高GAPO值），增加止损和目标利润的幅度可能是适当的
   - 在低波动性时期（低 GAPO 值），区间交易策略可能更合适
   - 极端的 GAPO 值可以作为寻找反转点的相反指标

5. **与其他指标的结合**：
   - GAPO 可以用来过滤来自其他指标的信号
   - 在高波动期间，趋势指标信号可能更可靠
   - 在低波动性期间，振荡器信号可能更有效

![indicator_gopalakrishnan_range_index](../../../../images/indicator_gopalakrishnan_range_index.png)

## 另请参阅

[平均真实波幅](atr.md)
[波动指数](choppiness_index.md)
[真实波幅](true_range.md)