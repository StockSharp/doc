# Equity curve

The [S\#](StockSharpAbout.md) provides the possibility to plot equity curves for further analysis of the trading algorithm work. The data for the curve obtained using the [PnLManager](xref:StockSharp.Algo.PnL.PnLManager) class. This class calculates the current value, informing the algorithm about the changes through the [Strategy.PnLChanged](xref:StockSharp.Algo.Strategies.Strategy.PnLChanged) event. 

To calculate the curve parameters (maximum drawdown, Sharpe ratio, etc.) [StatisticManager](xref:StockSharp.Algo.Statistics.StatisticManager) used. These parameters are stored in the [StatisticManager.Parameters](xref:StockSharp.Algo.Statistics.StatisticManager.Parameters) property. Each parameter implements [IPnLStatisticParameter](xref:StockSharp.Algo.Statistics.IPnLStatisticParameter), [ITradeStatisticParameter](xref:StockSharp.Algo.Statistics.ITradeStatisticParameter), [IOrderStatisticParameter](xref:StockSharp.Algo.Statistics.IOrderStatisticParameter) or [IPositionStatisticParameter](xref:StockSharp.Algo.Statistics.IPositionStatisticParameter) interfaces. If you want a particular calculation parameter, you must implement one of these interfaces and add the parameter to the [StatisticManager.Parameters](xref:StockSharp.Algo.Statistics.StatisticManager.Parameters). 

The use of the [Strategy.PnLChanged](xref:StockSharp.Algo.Strategies.Strategy.PnLChanged) is shown in the [backtesting](StrategyTestingHistory.md) section in the context of [trading strategies](Strategy.md): 

```cs
_strategy.PnLChanged += () =>
{
	var data = new EquityData
	{
		Time = _strategy.Trader.MarketTime,
		Value = _strategy.PnL,
	};
	this.GuiAsync(() => _curveItems.Add(data));
};      
      
```

The statistics manager is obtained through the [Strategy.StatisticManager](xref:StockSharp.Algo.Strategies.Strategy.StatisticManager) property. 

When the new data occur, the curve is drawn on the special [EquityCurveChart](xref:StockSharp.Xaml.Charting.EquityCurveChart) chart. To start drawing a curve on this chart, you must call the [EquityCurveChart.CreateCurve](xref:StockSharp.Xaml.Charting.EquityCurveChart.CreateCurve(System.String,System.Windows.Media.Color,System.Windows.Media.Color,StockSharp.Xaml.Charting.LineChartStyles,System.Guid))**(**[System.String](xref:System.String) title, [System.Windows.Media.Color](xref:System.Windows.Media.Color) color, [System.Windows.Media.Color](xref:System.Windows.Media.Color) secondColor, [StockSharp.Xaml.Charting.LineChartStyles](xref:StockSharp.Xaml.Charting.LineChartStyles) style, [System.Guid](xref:System.Guid) id **)** method. The resulting collection is filled with data that is passed during processing the [Strategy.PnLChanged](xref:StockSharp.Algo.Strategies.Strategy.PnLChanged) event. 

The [EquityCurveChart](xref:StockSharp.Xaml.Charting.EquityCurveChart) chart allows to draw multiple curves to be able to compare their profitability with each other. An example of this approach is shown in the [optimization](StrategyTestingOptimization.md) section. 

The use of the [StatisticParameterGrid](xref:StockSharp.Xaml.StatisticParameterGrid) visual panel, which allows to display [StatisticParameterGrid.Parameters](xref:StockSharp.Xaml.StatisticParameterGrid.Parameters) parameters, is shown in the [backtesting](StrategyTestingHistory.md) section. 
