# APZ

**自适应价格区（APZ）**是由 Lee Leibfarth 开发的技术指标，它创建动态支撑和阻力区，以适应市场波动。

要使用该指标，您需要使用 [AdaptivePriceZone](xref:StockSharp.Algo.Indicators.AdaptivePriceZone) 类。

## 描述

APZ 指标由两条线（上限和下限）组成，形成围绕平均价格的价格区间。该区间会根据当前市场波动性扩张或收缩。当市场变得更具波动性时，区间会扩大；当波动性下降时，区间会收窄。

APZ 特别适用于：
- 识别潜在的支撑和阻力位
- 检测可能的趋势反转点
- 揭示波动性增加和减少的时期
- 基于价格区间突破创建交易系统

## 参数

该指标具有以下参数：
- **周期** - 计算周期（默认值：5）
- **BandPercentage** - 定义波段宽度的范围百分比（默认值：2%）

## 计算

APZ 计算基于指数移动平均线（EMA）和平均真实波幅（ATR）：

1. 首先，计算指定期间的价格指数移动平均（EMA）：
   ```
   EMA = Period 内价格的指数移动平均
   ```

2. 然后使用 ATR 计算波动性：
   ```
   Volatility = Period 内 ATR 的指数移动平均
   ```

3. 上下 APZ 线的计算如下：
   ```
   Upper Line = EMA + (Volatility * BandPercentage)
   Lower Line = EMA - (Volatility * BandPercentage)
   ```

当价格位于上方APZ线之上时，这可以被视为上升趋势。当价格位于下方APZ线之下时，这可能表示下降趋势。当价格在APZ区域内波动时，市场可能处于盘整或横向运动阶段。

![indicator_adaptive_price_zone](../../../../images/indicator_adaptive_price_zone.png)

## 另请参阅

[布林带](bollinger_bands.md)
[凯尔特纳通道](keltner_channels.md)
[唐奇安通道](donchian_channels.md)