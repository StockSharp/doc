# Statistics Reference

The [StatisticManager](xref:StockSharp.Algo.Statistics.StatisticManager) manages a collection of [IStatisticParameter](xref:StockSharp.Algo.Statistics.IStatisticParameter) instances. Each parameter tracks a specific metric during strategy execution. All available parameters are created using the [StatisticParameterRegistry](xref:StockSharp.Algo.Statistics.StatisticParameterRegistry).

For a general overview of working with strategy statistics, see the [Statistics](statistics.md) section.

## Interfaces

The statistics system is built on a hierarchy of interfaces. Each interface defines the data source for calculating the parameter:

| Interface | Description |
|-----------|-------------|
| [IStatisticParameter](xref:StockSharp.Algo.Statistics.IStatisticParameter) | Base interface: properties `Name`, `Type`, `Value`, `DisplayName`, `Description`, `Category`, `Order`; method `Reset()` |
| [IPnLStatisticParameter](xref:StockSharp.Algo.Statistics.IPnLStatisticParameter) | Parameters based on profit/loss: method `Add(marketTime, pnl, commission)` |
| [ITradeStatisticParameter](xref:StockSharp.Algo.Statistics.ITradeStatisticParameter) | Parameters based on trades: method `Add(PnLInfo)` |
| [IOrderStatisticParameter](xref:StockSharp.Algo.Statistics.IOrderStatisticParameter) | Parameters based on orders: methods `New(order)`, `Changed(order)`, `RegisterFailed(fail)`, `CancelFailed(fail)` |
| [IPositionStatisticParameter](xref:StockSharp.Algo.Statistics.IPositionStatisticParameter) | Parameters based on positions: method `Add(marketTime, position)` |
| [IRiskFreeRateStatisticParameter](xref:StockSharp.Algo.Statistics.IRiskFreeRateStatisticParameter) | Parameters with risk-free rate: property `RiskFreeRate` |
| [IBeginValueStatisticParameter](xref:StockSharp.Algo.Statistics.IBeginValueStatisticParameter) | Parameters with initial value: property `BeginValue` |

## Profit and Loss (P&L) Parameters

All parameters in this group implement the [IPnLStatisticParameter](xref:StockSharp.Algo.Statistics.IPnLStatisticParameter) interface and inherit from [BasePnLStatisticParameter](xref:StockSharp.Algo.Statistics.BasePnLStatisticParameter`1). They receive data on each update of the strategy's P&L value.

| Class | Description | Value Type |
|-------|-------------|------------|
| [NetProfitParameter](xref:StockSharp.Algo.Statistics.NetProfitParameter) | Net profit for the entire period. Set equal to the current P&L value | `decimal` |
| [NetProfitPercentParameter](xref:StockSharp.Algo.Statistics.NetProfitPercentParameter) | Net profit as a percentage. Requires setting `BeginValue` (initial capital). Formula: `pnl * 100 / BeginValue` | `decimal` |
| [MaxProfitParameter](xref:StockSharp.Algo.Statistics.MaxProfitParameter) | Maximum profit (highest P&L value over the entire period) | `decimal` |
| [MaxProfitPercentParameter](xref:StockSharp.Algo.Statistics.MaxProfitPercentParameter) | Maximum profit as a percentage. Requires `BeginValue`. Formula: `MaxProfit * 100 / BeginValue` | `decimal` |
| [MaxProfitDateParameter](xref:StockSharp.Algo.Statistics.MaxProfitDateParameter) | Date when maximum profit was reached | `DateTime` |
| [MaxDrawdownParameter](xref:StockSharp.Algo.Statistics.MaxDrawdownParameter) | Maximum absolute drawdown. Difference between the peak and trough of the equity curve | `decimal` |
| [MaxDrawdownPercentParameter](xref:StockSharp.Algo.Statistics.MaxDrawdownPercentParameter) | Maximum drawdown as a percentage. Formula: `MaxDrawdown * 100 / MaxEquity` | `decimal` |
| [MaxDrawdownDateParameter](xref:StockSharp.Algo.Statistics.MaxDrawdownDateParameter) | Date of maximum drawdown | `DateTime` |
| [MaxRelativeDrawdownParameter](xref:StockSharp.Algo.Statistics.MaxRelativeDrawdownParameter) | Maximum relative drawdown. Calculated as the ratio of drawdown to peak equity value | `decimal` |
| [ReturnParameter](xref:StockSharp.Algo.Statistics.ReturnParameter) | Relative return for the entire period. Maximum growth from trough to current value in relative terms | `decimal` |
| [CommissionParameter](xref:StockSharp.Algo.Statistics.CommissionParameter) | Total commission paid. Accumulates all commission values | `decimal` |
| [AverageDrawdownParameter](xref:StockSharp.Algo.Statistics.AverageDrawdownParameter) | Average drawdown. Arithmetic mean of all completed and current drawdowns | `decimal` |
| [RecoveryFactorParameter](xref:StockSharp.Algo.Statistics.RecoveryFactorParameter) | Recovery factor. Formula: `NetProfit / MaxDrawdown` | `decimal` |
| [SharpeRatioParameter](xref:StockSharp.Algo.Statistics.SharpeRatioParameter) | Sharpe ratio. Formula: `(annualized return - risk-free rate) / annualized standard deviation` | `decimal` |
| [SortinoRatioParameter](xref:StockSharp.Algo.Statistics.SortinoRatioParameter) | Sortino ratio. Similar to Sharpe, but only accounts for downside deviations | `decimal` |
| [CalmarRatioParameter](xref:StockSharp.Algo.Statistics.CalmarRatioParameter) | Calmar ratio. Formula: `NetProfit / MaxDrawdown` | `decimal` |
| [SterlingRatioParameter](xref:StockSharp.Algo.Statistics.SterlingRatioParameter) | Sterling ratio. Formula: `NetProfit / AverageDrawdown` | `decimal` |

### Risk Coefficients

The [SharpeRatioParameter](xref:StockSharp.Algo.Statistics.SharpeRatioParameter) and [SortinoRatioParameter](xref:StockSharp.Algo.Statistics.SortinoRatioParameter) inherit from the base class [RiskAdjustedRatioParameter](xref:StockSharp.Algo.Statistics.RiskAdjustedRatioParameter) and implement the [IRiskFreeRateStatisticParameter](xref:StockSharp.Algo.Statistics.IRiskFreeRateStatisticParameter) interface.

They support the following settings:

- **RiskFreeRate** -- annual risk-free rate (e.g., `0.03m` = 3%)
- **Period** -- return calculation period (default `TimeSpan.FromDays(1)`)

The [CalmarRatioParameter](xref:StockSharp.Algo.Statistics.CalmarRatioParameter) and [SterlingRatioParameter](xref:StockSharp.Algo.Statistics.SterlingRatioParameter) depend on other parameters (`NetProfitParameter`, `MaxDrawdownParameter`, `AverageDrawdownParameter`) and are automatically linked when created via [StatisticParameterRegistry](xref:StockSharp.Algo.Statistics.StatisticParameterRegistry).

## Trade Parameters

All parameters in this group implement the [ITradeStatisticParameter](xref:StockSharp.Algo.Statistics.ITradeStatisticParameter) interface. They receive data through the [PnLInfo](xref:StockSharp.Algo.PnL.PnLInfo) object for each trade executed.

| Class | Description | Value Type |
|-------|-------------|------------|
| [TradeCountParameter](xref:StockSharp.Algo.Statistics.TradeCountParameter) | Total number of trades (only trades with `ClosedVolume > 0` are counted) | `int` |
| [WinningTradesParameter](xref:StockSharp.Algo.Statistics.WinningTradesParameter) | Number of profitable trades (`ClosedVolume > 0` and `PnL > 0`) | `int` |
| [LossingTradesParameter](xref:StockSharp.Algo.Statistics.LossingTradesParameter) | Number of losing trades (`ClosedVolume > 0` and `PnL < 0`) | `int` |
| [RoundtripCountParameter](xref:StockSharp.Algo.Statistics.RoundtripCountParameter) | Number of completed round-trips (closing trades with `ClosedVolume > 0`) | `int` |
| [AverageTradeProfitParameter](xref:StockSharp.Algo.Statistics.AverageTradeProfitParameter) | Average profit per trade. Formula: `SumPnL / Count` | `decimal` |
| [AverageWinTradeParameter](xref:StockSharp.Algo.Statistics.AverageWinTradeParameter) | Average profit of profitable trades. Only trades with `PnL > 0` are considered | `decimal` |
| [AverageLossTradeParameter](xref:StockSharp.Algo.Statistics.AverageLossTradeParameter) | Average loss of losing trades. Only trades with `PnL < 0` are considered | `decimal` |
| [ProfitFactorParameter](xref:StockSharp.Algo.Statistics.ProfitFactorParameter) | Profit factor. Formula: `GrossProfit / GrossLoss` | `decimal` |
| [ExpectancyParameter](xref:StockSharp.Algo.Statistics.ExpectancyParameter) | Mathematical expectancy. Formula: `P(win) * AvgWin + P(loss) * AvgLoss` | `decimal` |
| [PerMonthTradeParameter](xref:StockSharp.Algo.Statistics.PerMonthTradeParameter) | Average number of trades per month | `decimal` |
| [PerDayTradeParameter](xref:StockSharp.Algo.Statistics.PerDayTradeParameter) | Average number of trades per day | `decimal` |
| [GrossProfitParameter](xref:StockSharp.Algo.Statistics.GrossProfitParameter) | Gross profit. Sum of P&L of all profitable trades (`PnL > 0`) | `decimal` |
| [GrossLossParameter](xref:StockSharp.Algo.Statistics.GrossLossParameter) | Gross loss. Sum of P&L of all losing trades (`PnL < 0`, value is negative) | `decimal` |

## Position Parameters

Parameters in this group implement the [IPositionStatisticParameter](xref:StockSharp.Algo.Statistics.IPositionStatisticParameter) interface. They receive data on each position change.

| Class | Description | Value Type |
|-------|-------------|------------|
| [MaxLongPositionParameter](xref:StockSharp.Algo.Statistics.MaxLongPositionParameter) | Maximum long position. Highest positive position value | `decimal` |
| [MaxShortPositionParameter](xref:StockSharp.Algo.Statistics.MaxShortPositionParameter) | Maximum short position. Highest absolute negative position value | `decimal` |

## Order Parameters

All parameters in this group implement the [IOrderStatisticParameter](xref:StockSharp.Algo.Statistics.IOrderStatisticParameter) interface and inherit from [BaseOrderStatisticParameter](xref:StockSharp.Algo.Statistics.BaseOrderStatisticParameter`1). They receive data on order registration, changes, and errors.

| Class | Description | Value Type |
|-------|-------------|------------|
| [OrderCountParameter](xref:StockSharp.Algo.Statistics.OrderCountParameter) | Total number of registered orders | `int` |
| [OrderRegisterErrorCountParameter](xref:StockSharp.Algo.Statistics.OrderRegisterErrorCountParameter) | Number of order registration errors | `int` |
| [OrderInsufficientFundErrorCountParameter](xref:StockSharp.Algo.Statistics.OrderInsufficientFundErrorCountParameter) | Number of "insufficient funds" errors (type `InsufficientFundException`) | `int` |
| [OrderCancelErrorCountParameter](xref:StockSharp.Algo.Statistics.OrderCancelErrorCountParameter) | Number of order cancellation errors | `int` |

## Latency Parameters

Latency parameters also implement [IOrderStatisticParameter](xref:StockSharp.Algo.Statistics.IOrderStatisticParameter), but track the time characteristics of order processing.

| Class | Description | Value Type |
|-------|-------------|------------|
| [MaxLatencyRegistrationParameter](xref:StockSharp.Algo.Statistics.MaxLatencyRegistrationParameter) | Maximum order registration latency (`Order.LatencyRegistration` property) | `TimeSpan` |
| [MinLatencyRegistrationParameter](xref:StockSharp.Algo.Statistics.MinLatencyRegistrationParameter) | Minimum order registration latency | `TimeSpan` |
| [MaxLatencyCancellationParameter](xref:StockSharp.Algo.Statistics.MaxLatencyCancellationParameter) | Maximum order cancellation latency (`Order.LatencyCancellation` property) | `TimeSpan` |
| [MinLatencyCancellationParameter](xref:StockSharp.Algo.Statistics.MinLatencyCancellationParameter) | Minimum order cancellation latency | `TimeSpan` |

## Usage

### Accessing Strategy Statistics

```cs
var strategy = new MyStrategy();

// Access statistics after execution
foreach (var param in strategy.StatisticManager.Parameters)
{
    Console.WriteLine($"{param.DisplayName}: {param.Value}");
}
```

### Retrieving a Specific Parameter

```cs
// Get the net profit value
var netProfit = strategy.StatisticManager.Parameters
    .OfType<NetProfitParameter>()
    .First();

Console.WriteLine($"Net profit: {netProfit.Value}");
```

### Configuring the Risk-Free Rate for Coefficients

The Sharpe and Sortino ratios require setting the risk-free rate for correct calculation:

```cs
// Set a 3% risk-free rate for all coefficients
foreach (var param in strategy.StatisticManager.Parameters
    .OfType<IRiskFreeRateStatisticParameter>())
{
    param.RiskFreeRate = 0.03m;
}
```

### Configuring Initial Capital for Percentage Parameters

The [NetProfitPercentParameter](xref:StockSharp.Algo.Statistics.NetProfitPercentParameter) and [MaxProfitPercentParameter](xref:StockSharp.Algo.Statistics.MaxProfitPercentParameter) parameters require setting the initial capital value:

```cs
// Set initial capital for percentage calculations
foreach (var param in strategy.StatisticManager.Parameters
    .OfType<IBeginValueStatisticParameter>())
{
    param.BeginValue = 1_000_000m; // 1,000,000
}
```

### Resetting Statistics

```cs
// Reset all statistic parameters
strategy.StatisticManager.Reset();
```

### Saving and Loading State

All parameters support serialization via the `IPersistable` interface:

```cs
// Saving
var storage = new SettingsStorage();
strategy.StatisticManager.Save(storage);

// Loading
strategy.StatisticManager.Load(storage);
```

## See Also

- [Strategy Statistics](statistics.md)
- [Statistics Graphical Component](../graphical_user_interface/strategies/statistics.md)
