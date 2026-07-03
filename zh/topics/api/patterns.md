# 模式

**模式**（来自英文：pattern — 模型，样本）——在技术分析中，指价格数据、成交量或指标的稳定重复组合。模式分析基于技术分析的一个公理：“历史会重演”——认为重复出现的数据组合会导致类似的结果。

模式也被称为技术分析的“**模板**”或“**形态**”。

模式通常分为：

- 不确定（可能导致当前趋势的延续或改变）。
- 当前趋势的延续模式。
- 现有趋势反转的模式。

## 使用模式

### 在 Designer

[Designer](../designer.md) 内置了可用于交易策略的预设K线图形。可以通过 [indicator](../designer/strategies/using_visual_designer/elements/common/indicator.md) 立方体调用这些图形，然后选择相应的值。图形本身可以在右侧窗口的下拉列表中选择。

![IndicatorPatternCommon](../../images/indicatorpatterncommon00.png)

也可以编辑已有图形并添加自定义图形。为此，需要点击 ![Designer 编辑按钮](../../images/designer_creating_repository_of_historical_data_01.png)，之后将显示图形编辑窗口。

![IndicatorPatternCommon01](../../images/indicatorpatterncommon01.png)

要创建您自己的图案，请点击窗口顶部的 ![DesignerPlusButton](../../images/designer_panel_circuits_01_button.png) 按钮。点击 ![DesignerDeleteButton](../../images/designer_delete_button.png) 按钮可以删除图案。

### 在 Terminal

在 [Terminal](../terminal.md) 中，模式像其他指标一样被添加到图表中。要做到这一点，只需右键点击图表，然后从可用指标列表中选择相应的指标。

### 在 StockSharp API 中

在使用 [S#](../api.md)（或在 Designer 中从代码创建 [策略](../designer/strategies/using_code.md)）时，处理模式与使用其他指标相同。使用示例：

```cs
// Creating a pattern indicator
var patternIndicator = new CandlePatternIndicator
{
	// Setting the desired pattern
	Pattern = new ExpressionCandlePattern("My pattern", new[]
	{
		new CandleExpressionCondition(Paths.FileSystem, "C > O"), // Current candle is rising
		new CandleExpressionCondition(Paths.FileSystem, "pC < pO") // Previous candle is falling
	})
};

// Adding the indicator to the collection
Indicators.Add(patternIndicator);

// Processing a candle
var result = patternIndicator.Process(candle);

// Checking the result
if (result.GetValue<bool>())
{
	// Pattern detected, perform necessary actions
}
```

## 模式描述格式

在编辑图案时，每一行代表一根独立的K线。最上面的一行是当前K线，相应地，第二行是一根K线之前，第三行及之后的行是减去 2 根及更多的K线。

编辑器使用以下参数：
- O - 开盘价，
- H - 高
- L - 低
- C - 收盘价，
- V - 体积,
- OI - 未平仓合约
- B - K线本体,
- LEN - K线的长度（从最高到最低）,
- BS - K线的下影线,
- TS - K线的上影线。

使用参数时，可以使用以下索引（引用）来获取所需的值。例如，对于收盘价：
- C：当前K线的收盘价，
- C1：当前K线之后第一根K线的收盘价，
- C2：当前K线之后第二根K线的收盘价，
- pC：前一根K线的收盘价，
- pC1：前一个K线之前K线的收盘价，
所有引用必须在当前形态的范围内。例如，三只乌鸦形态的范围包括当前K线和前两根K线，因此不允许引用第三根之前的K线。

为了对相关参数进行额外验证，使用表达式 &&，表示逻辑与。

在描述一个形态时，也可以使用以下函数：abs, acos, asin, atan, ceiling, cos, exp, floor, log, log10, max, min, pow, round, sign, sin, sqrt, tan, truncate。关于函数使用的更多内容，可以在[公式](../designer/strategies/using_visual_designer/elements/common/formula.md)模块的描述中查看。

在代码中使用[ExpressionCandlePattern](xref:StockSharp.Algo.Candles.Patterns.ExpressionCandlePattern)时，公式的创建规则与上述相同，并使用相同的变量。

## 标准模式

为了快速基于现有模式创建新模式，您可以使用模式编辑器窗口底部的部分。点击窗口底部的 ![DesignerPlusButton](../../images/designer_panel_circuits_01_button.png) 按钮，会在编辑窗口对面的下拉列表中选择的模式逻辑添加到编辑窗口中。窗口底部的 ![DesignerDeleteButton](../../images/designer_delete_button.png) 按钮会删除编辑窗口中选定的行。

## 高级功能

- [ComplexCandlePattern](xref:StockSharp.Algo.Candles.Patterns.ComplexCandlePattern) — 允许将多个模式组合成单一的复合模式，以进行更复杂的分析。
- [ICandlePatternProvider](xref:StockSharp.Algo.Candles.Patterns.ICandlePatternProvider) — 一个模式提供接口，允许加载和保存自定义模式。