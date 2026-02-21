# Patterns

**Pattern** (from English: pattern — model, sample) — in technical analysis, refers to stable recurring combinations of price data, volume, or indicators. Pattern analysis is based on one of the axioms of technical analysis: "history repeats itself" — it is believed that recurring data combinations lead to similar results.

Patterns are also called "**templates**" or "**figures**" of technical analysis.

Patterns are conventionally divided into:

- Indeterminate (can lead to both continuation and change of the current trend).
- Continuation patterns of the current trend.
- Patterns of existing trend reversal.

## Using Patterns

### In Designer

[Designer](../designer.md) has built-in preset candlestick patterns that can be used in your trading strategy. Patterns are called through the [indicator](../designer/strategies/using_visual_designer/elements/common/indicator.md) cube with the subsequent selection of the corresponding value. The pattern itself is selected from the drop-down list in the window on the right.

![IndicatorPatternCommon](../../images/indicatorpatterncommon00.png)

It is also possible to edit existing and add your own custom patterns. To do this, you need to click on the button ![Designer edit button](../../images/designer_creating_repository_of_historical_data_01.png) after which the pattern editing window will be shown.

![IndicatorPatternCommon01](../../images/indicatorpatterncommon01.png)

To create your own pattern, click the ![DesignerPlusButton](../../images/designer_panel_circuits_01_button.png) button at the top of the window. Clicking the ![DesignerDeleteButton](../../images/designer_delete_button.png) button deletes the pattern.

### In Terminal

In [Terminal](../terminal.md), patterns are added to the chart like any other indicator. To do this, simply right-click on the chart and select the appropriate indicator from the list of available ones.

### In StockSharp API

When using [S#](../api.md) (or when creating [strategies from code](../designer/strategies/using_code.md) in Designer), working with patterns is done as with any other indicator. Example of use:

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

## Pattern Description Format

When editing a pattern, each line represents a separate candle. The topmost line is the current candle, accordingly, the second line is one candle back, the third and subsequent lines are minus 2 and more candles.

The editor uses the following parameters:
- O - opening price,
- H - high,
- L - low,
- C - closing price,
- V - volume,
- OI - open interest,
- B - candle body,
- LEN - length of the candle (from high to low),
- BS - lower shadow of the candle,
- TS - upper shadow of the candle.

With parameters, it is possible to use the following indices (references) to the desired values. For example, for the closing price:
- C: closing price of the current candle,
- C1: closing price of the 1st candle after the current one,
- C2: closing price of the 2nd candle after the current one,
- pC: closing price of the previous candle,
- pC1: closing price of the candle before the previous one,
All references must be within the range of the current pattern. For example, the range of the 3 Black Crows pattern consists of the current and two previous candles, so referring to the third previous candle is not allowed.

For additional verification of parameters in correlation, the expression && is used, representing a logical AND.

When describing a pattern, it is also possible to use the following functions: abs, acos, asin, atan, ceiling, cos, exp, floor, log, log10, max, min, pow, round, sign, sin, sqrt, tan, truncate. More about using functions is explained in the description of the [formula](../designer/strategies/using_visual_designer/elements/common/formula.md) cube.

When using [ExpressionCandlePattern](xref:StockSharp.Algo.Candles.Patterns.ExpressionCandlePattern) in code, formulas are created by the same rules as described above and use the same variables.

## Standard Patterns

For quick creation of patterns based on existing ones, you can use the section at the bottom of the pattern editor window. Clicking the ![DesignerPlusButton](../../images/designer_panel_circuits_01_button.png) button at the bottom of the window adds the logic of the pattern selected from the drop-down list opposite to the editing window. The ![DesignerDeleteButton](../../images/designer_delete_button.png) button at the bottom of the window deletes the selected line in the editing window.

## Advanced Features

- [ComplexCandlePattern](xref:StockSharp.Algo.Candles.Patterns.ComplexCandlePattern) — allows combining multiple patterns into a single composite pattern for more complex analysis.
- [ICandlePatternProvider](xref:StockSharp.Algo.Candles.Patterns.ICandlePatternProvider) — pattern provider interface that allows loading and saving custom patterns.