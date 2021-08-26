# Equity curve

The [S\#](StockSharpAbout.md) provides the possibility to plot equity curves for further analysis of the trading algorithm work. The data for the curve obtained using the [PnLManager](../api/StockSharp.Algo.PnL.PnLManager.html) class. This class calculates the current value, informing the algorithm about the changes through the [Strategy.PnLChanged](../api/StockSharp.Algo.Strategies.Strategy.PnLChanged.html) event. 

To calculate the curve parameters (maximum drawdown, Sharpe ratio, etc.) [StatisticManager](../api/StockSharp.Algo.Statistics.StatisticManager.html) used. These parameters are stored in the [StatisticManager.Parameters](../api/StockSharp.Algo.Statistics.StatisticManager.Parameters.html) property. Each parameter implements [IPnLStatisticParameter](../api/StockSharp.Algo.Statistics.IPnLStatisticParameter.html), [ITradeStatisticParameter](../api/StockSharp.Algo.Statistics.ITradeStatisticParameter.html), [IOrderStatisticParameter](../api/StockSharp.Algo.Statistics.IOrderStatisticParameter.html) or [IPositionStatisticParameter](../api/StockSharp.Algo.Statistics.IPositionStatisticParameter.html) interfaces. If you want a particular calculation parameter, you must implement one of these interfaces and add the parameter to the [StatisticManager.Parameters](../api/StockSharp.Algo.Statistics.StatisticManager.Parameters.html). 

The use of the [Strategy.PnLChanged](../api/StockSharp.Algo.Strategies.Strategy.PnLChanged.html) is shown in the [backtesting](StrategyTestingHistory.md) section in the context of [trading strategies](Strategy.md): 

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

The statistics manager is obtained through the [Strategy.StatisticManager](../api/StockSharp.Algo.Strategies.Strategy.StatisticManager.html) property. 

When the new data occur, the curve is drawn on the special [EquityCurveChart](../api/StockSharp.Xaml.Charting.EquityCurveChart.html) chart. To start drawing a curve on this chart, you must call the [Overload:StockSharp.Xaml.Charting.EquityCurveChart.CreateCurve](../api/Overload:StockSharp.Xaml.Charting.EquityCurveChart.CreateCurve.html) method. The resulting collection is filled with data that is passed during processing the [Strategy.PnLChanged](../api/StockSharp.Algo.Strategies.Strategy.PnLChanged.html) event. 

The [EquityCurveChart](../api/StockSharp.Xaml.Charting.EquityCurveChart.html) chart allows to draw multiple curves to be able to compare their profitability with each other. An example of this approach is shown in the [optimization](StrategyTestingOptimization.md) section. 

The use of the [StatisticParameterGrid](../api/StockSharp.Xaml.StatisticParameterGrid.html) visual panel, which allows to display [StatisticParameterGrid.Parameters](../api/StockSharp.Xaml.StatisticParameterGrid.Parameters.html) parameters, is shown in the [backtesting](StrategyTestingHistory.md) section. 
