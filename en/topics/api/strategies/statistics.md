# Strategy Statistics

## Overview

The StockSharp platform provides a comprehensive system for statistical analysis of trading strategies, which helps traders evaluate effectiveness, optimize parameters, and make informed decisions. The statistics system collects and processes data from various aspects of trading, including orders, trades, positions, and profit/loss indicators.

## Purpose and Benefits

Statistical analysis in trading strategies serves several important functions:

1. **Performance Measurement**: Quantitative assessment of your strategy's success using metrics such as net profit, maximum drawdown, and recovery factor.

2. **Risk Management**: Understanding your strategy's risk profile through metrics such as maximum drawdown percentage and position size statistics.

3. **Optimization**: Finding optimal strategy parameters by comparing statistical indicators across different parameter sets.

4. **Trade Quality Analysis**: Analyzing trade distribution, ratio of profitable to losing trades, average profit per trade.

5. **Operational Metrics**: Tracking operational metrics such as latency statistics and order error rates to identify execution issues.

## Available Statistical Indicators

The [IStatisticManager](xref:StockSharp.Algo.Statistics.IStatisticManager) interface in StockSharp provides access to numerous statistical parameters organized into several categories:

### Profit and Loss Statistics

- Net Profit
- Net Profit (%)
- Maximum Profit
- Maximum Drawdown
- Maximum Drawdown (%)
- Maximum Relative Drawdown
- Recovery Factor

### Trade Statistics

- Number of Profitable Trades
- Number of Losing Trades
- Total Number of Trades
- Average Profit per Trade
- Average Profitable Trade
- Average Losing Trade
- Number of Trades per Month/Day

### Position Statistics

- Maximum Long Position
- Maximum Short Position

### Order Statistics

- Number of Orders
- Number of Order Errors
- Maximum/Minimum Registration Delay
- Maximum/Minimum Cancellation Delay

## Integration with the Strategy Class

The [Strategy](xref:StockSharp.Algo.Strategies.Strategy) class automatically collects and calculates statistics during execution. The statistics manager is available through the `StatisticManager` property, which implements the [IStatisticManager](xref:StockSharp.Algo.Statistics.IStatisticManager) interface.

Key statistical values are also directly represented as properties of the Strategy class:

- `PnL`: Profit and loss value
- `Commission`: Total paid commission
- `Slippage`: Total slippage
- `Latency`: Average order operation latency

## Visualization

StockSharp provides a special graphical component for visualizing strategy statistics called `StatisticParameterGrid`, which is available in the `StockSharp.Xaml` namespace. This grid displays all statistical parameters in a user-friendly format.

For more information about the graphical component, see the documentation on [Statistics](../graphical_user_interface/strategies/statistics.md).

## Usage Example

Here's an example of working with strategy statistics in your code:

```csharp
// Create a strategy
var strategy = new SmaStrategy
{
    // Configure strategy parameters
    Security = security,
    Portfolio = portfolio,
    Volume = 1,
    // Set SMA parameters
    LongSma = 200,
    ShortSma = 50,
};

// Connect the strategy to a chart for visualization
var chart = new ChartPanel();
strategy.SetChart(chart);

// Access the statistics manager
var statisticManager = strategy.StatisticManager;

// Display strategy statistics in the user interface
// Assuming you have a StatisticParameterGrid defined in XAML as 'StatisticsGrid'
StatisticsGrid.Parameters.Clear();
StatisticsGrid.Parameters.AddRange(statisticManager.Parameters);

// Start the strategy
strategy.Start();

// When you need to react to changes in statistics
strategy.PnLChanged += () =>
{
    Console.WriteLine($"Current PnL: {strategy.PnL}");
    
    // You can also access individual statistical parameters
    var netProfit = statisticManager.Parameters
        .OfType<NetProfitParameter>()
        .FirstOrDefault();
        
    if (netProfit != null)
    {
        Console.WriteLine($"Net Profit: {netProfit.Value}");
    }
};

// For tracking position statistics
strategy.PositionChanged += () =>
{
    Console.WriteLine($"Current Position: {strategy.Position}");
};
```

## Custom Statistics

You can also create your own statistical parameters by implementing the appropriate interfaces:

- [IStatisticParameter](xref:StockSharp.Algo.Statistics.IStatisticParameter): Base interface for all statistical parameters
- [IPnLStatisticParameter](xref:StockSharp.Algo.Statistics.IPnLStatisticParameter): For parameters related to profit/loss
- [ITradeStatisticParameter](xref:StockSharp.Algo.Statistics.ITradeStatisticParameter): For parameters related to trades
- [IPositionStatisticParameter](xref:StockSharp.Algo.Statistics.IPositionStatisticParameter): For parameters related to positions
- [IOrderStatisticParameter](xref:StockSharp.Algo.Statistics.IOrderStatisticParameter): For parameters related to orders

Here's a simple example of a custom statistical parameter:

```csharp
[Display(
    ResourceType = typeof(LocalizedStrings),
    Name = "My Custom Indicator",
    Description = "Description of my custom indicator",
    GroupName = "Custom Parameters",
    Order = 1000
)]
public class MyCustomParameter : BasePnLStatisticParameter<decimal>
{
    public MyCustomParameter()
        : base(StatisticParameterTypes.Custom)
    {
    }

    public override void Add(DateTimeOffset marketTime, decimal pnl, decimal? commission)
    {
        // Custom calculation logic
        Value = /* your custom calculation */;
    }
}

// Then add it to your strategy's StatisticManager
strategy.StatisticManager.Parameters.Add(new MyCustomParameter());
```

## Conclusion

The statistical analysis system in StockSharp provides traders with powerful tools for evaluating and optimizing their trading strategies. By using these statistics, you can gain valuable insights into your strategy's performance, identify areas for improvement, and make data-driven decisions to enhance your trading results.