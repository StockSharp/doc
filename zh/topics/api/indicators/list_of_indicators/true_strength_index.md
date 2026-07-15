# 真实强度指数

**真实强度指数（TSI）**是一种由 William Blau 创建的动量振荡器。它对连续收盘价之间的差异
进行了双重平滑处理，有助于在比许多经典
振荡器更少的滞后情况下识别趋势和转折点。

使用 [TrueStrengthIndex](xref:StockSharp.Algo.Indicators.TrueStrengthIndex) 类来访问该指标。

## 计算

1. 计算价格变动 `m = Close − PreviousClose`。
2. 对 `m` 和 `|m|` 应用周期为 `Length1` 和 `Length2` 的两个指数移动平均线。
3. 计算双重平滑动量与双重平滑绝对动量的比率：
`TSI = 100 × EMA(EMA(m, Length1), Length2) / EMA(EMA(|m|, Length1), Length2)`。
4. 可以选择通过对TSI取周期为**信号**的EMA来导出信号线。

## 参数

- **周期1** — 第一个平滑周期。
- **周期2** — 第二个平滑周期。
- **信号** — 信号线的周期（可选）。

## 解释

- **TSI > 0** — 看涨动能。
- **TSI < 0** — 看跌动能。
- **信号线交叉** 提供交易入场点。
- **TSI 与价格之间的背离** 警示潜在的反转。

由于双重平滑和归一化，该指标能够滤除噪声，同时与简单的
动量计算相比仍保持响应性。

![真实强度指数 指标图表](../../../../images/indicator_true_strength_index.png)

## 另请参阅

[动量](momentum.md)
[MACD](macd.md)
[相对强弱指数](rsi.md)
