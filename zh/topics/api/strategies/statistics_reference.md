# 统计参考

[StatisticManager](xref:StockSharp.Algo.Statistics.StatisticManager) 管理一组 [IStatisticParameter](xref:StockSharp.Algo.Statistics.IStatisticParameter) 实例。每个参数在策略执行期间跟踪特定指标。所有可用参数都使用 [StatisticParameterRegistry](xref:StockSharp.Algo.Statistics.StatisticParameterRegistry) 创建。

有关使用策略统计的一般概述，请参阅[策略统计](statistics.md)部分。

## 接口

统计系统建立在接口层次结构上。每个接口定义了计算参数的数据来源：

| 接口 | 描述 |
|-----------|-------------|
| [IStatisticParameter](xref:StockSharp.Algo.Statistics.IStatisticParameter) | 基础接口：属性 `Name`、`Type`、`Value`、`DisplayName`、`Description`、`Category`、`Order`；方法 `Reset()` |
| [IPnLStatisticParameter](xref:StockSharp.Algo.Statistics.IPnLStatisticParameter) | 基于盈亏的参数：方法 `Add(marketTime, pnl, commission)` |
| [ITradeStatisticParameter](xref:StockSharp.Algo.Statistics.ITradeStatisticParameter) | 基于交易的参数：方法 `Add(PnLInfo)` |
| [IOrderStatisticParameter](xref:StockSharp.Algo.Statistics.IOrderStatisticParameter) | 基于订单的参数：方法 `New(order)`、`Changed(order)`、`RegisterFailed(fail)`、`CancelFailed(fail)` |
| [IPositionStatisticParameter](xref:StockSharp.Algo.Statistics.IPositionStatisticParameter) | 基于位置的参数：方法 `Add(marketTime, position)` |
| [IRiskFreeRateStatisticParameter](xref:StockSharp.Algo.Statistics.IRiskFreeRateStatisticParameter) | 带有无风险利率的参数: 属性 `RiskFreeRate` |
| [IBeginValueStatisticParameter](xref:StockSharp.Algo.Statistics.IBeginValueStatisticParameter) | 带有初始值的参数：属性 `BeginValue` |

## 损益 (P&L) 参数

该组中的所有参数都实现了 [IPnLStatisticParameter](xref:StockSharp.Algo.Statistics.IPnLStatisticParameter) 接口并继承自 [BasePnLStatisticParameter](xref:StockSharp.Algo.Statistics.BasePnLStatisticParameter`1)。它们在策略的盈亏值每次更新时接收数据。

| 类别 | 描述 | 值类型 |
|-------|-------------|------------|
| [净利润参数](xref:StockSharp.Algo.Statistics.NetProfitParameter) | 整个期间的净利润。设置为当前损益值 | `decimal` |
| [净利润百分比参数](xref:StockSharp.Algo.Statistics.NetProfitPercentParameter) | 净利润占百分比。需要设置 `BeginValue`（初始资本）。公式：`pnl * 100 / BeginValue` | `decimal` |
| [MaxProfitParameter](xref:StockSharp.Algo.Statistics.MaxProfitParameter) | 最大利润（整个期间的最高盈亏值） | `decimal` |
| [MaxProfitPercentParameter](xref:StockSharp.Algo.Statistics.MaxProfitPercentParameter) | 最大利润百分比。需要 `BeginValue`。公式：`MaxProfit * 100 / BeginValue` | `decimal` |
| [MaxProfitDateParameter](xref:StockSharp.Algo.Statistics.MaxProfitDateParameter) | 达到最大利润的日期 | `DateTime` |
| [最大回撤参数](xref:StockSharp.Algo.Statistics.MaxDrawdownParameter) | 最大绝对回撤。股权曲线的峰值和低谷之间的差值 | `decimal` |
| [最大回撤百分比参数](xref:StockSharp.Algo.Statistics.MaxDrawdownPercentParameter) | 最大回撤百分比。公式：`MaxDrawdown * 100 / MaxEquity` | `decimal` |
| [最大回撤日期参数](xref:StockSharp.Algo.Statistics.MaxDrawdownDateParameter) | 最大回撤日期 | `DateTime` |
| [MaxRelativeDrawdownParameter](xref:StockSharp.Algo.Statistics.MaxRelativeDrawdownParameter) | 最大相对回撤。计算方法为回撤与峰值资产的比率 | `decimal` |
| [ReturnParameter](xref:StockSharp.Algo.Statistics.ReturnParameter) | 整个周期的相对回报。从最低点到当前值的最大相对增长 | `decimal` |
| [CommissionParameter](xref:StockSharp.Algo.Statistics.CommissionParameter) | 支付的总佣金。累计所有佣金数值 | `decimal` |
| [平均回撤参数](xref:StockSharp.Algo.Statistics.AverageDrawdownParameter) | 平均回撤。所有已完成和当前回撤的算术平均值 | `decimal` |
| [RecoveryFactorParameter](xref:StockSharp.Algo.Statistics.RecoveryFactorParameter) | 回收系数。公式：`NetProfit / MaxDrawdown` | `decimal` |
| [夏普比率参数](xref:StockSharp.Algo.Statistics.SharpeRatioParameter) | 夏普比率。公式: `(annualized return - risk-free rate) / annualized standard deviation` | `decimal` |
| [SortinoRatioParameter](xref:StockSharp.Algo.Statistics.SortinoRatioParameter) | 索提诺比率。类似于夏普比率，但只考虑下行偏差 | `decimal` |
| [CalmarRatioParameter](xref:StockSharp.Algo.Statistics.CalmarRatioParameter) | Calmar 比率。公式：`NetProfit / MaxDrawdown` | `decimal` |
| [SterlingRatioParameter](xref:StockSharp.Algo.Statistics.SterlingRatioParameter) | 斯特林比率。公式：`NetProfit / AverageDrawdown` | `decimal` |

### 风险系数

[SharpeRatioParameter](xref:StockSharp.Algo.Statistics.SharpeRatioParameter) 和 [SortinoRatioParameter](xref:StockSharp.Algo.Statistics.SortinoRatioParameter) 继承自基类 [RiskAdjustedRatioParameter](xref:StockSharp.Algo.Statistics.RiskAdjustedRatioParameter)，并实现了 [IRiskFreeRateStatisticParameter](xref:StockSharp.Algo.Statistics.IRiskFreeRateStatisticParameter) 接口。

它们支持以下设置：

- **无风险利率** -- 年化无风险利率 (例如， `0.03m` = 3%)
- **期间** -- 返回计算期间（默认 `TimeSpan.FromDays(1)`）

[CalmarRatioParameter](xref:StockSharp.Algo.Statistics.CalmarRatioParameter) 和 [SterlingRatioParameter](xref:StockSharp.Algo.Statistics.SterlingRatioParameter) 依赖于其他参数 (`NetProfitParameter`, `MaxDrawdownParameter`, `AverageDrawdownParameter`)，并且在通过 [StatisticParameterRegistry](xref:StockSharp.Algo.Statistics.StatisticParameterRegistry) 创建时会自动关联。

## 交易参数

此组中的所有参数都实现了 [ITradeStatisticParameter](xref:StockSharp.Algo.Statistics.ITradeStatisticParameter) 接口。它们通过 [PnLInfo](xref:StockSharp.Algo.PnL.PnLInfo) 对象接收每笔已执行交易的数据。

| 类别 | 描述 | 值类型 |
|-------|-------------|------------|
| [TradeCountParameter](xref:StockSharp.Algo.Statistics.TradeCountParameter) | 交易总数（仅计算具有 `ClosedVolume > 0` 的交易） | `int` |
| [WinningTradesParameter](xref:StockSharp.Algo.Statistics.WinningTradesParameter) | 盈利交易次数 (`ClosedVolume > 0` 和 `PnL > 0`) | `int` |
| [亏损交易参数](xref:StockSharp.Algo.Statistics.LossingTradesParameter) | 亏损交易的数量 (`ClosedVolume > 0` 和 `PnL < 0`) | `int` |
| [RoundtripCountParameter](xref:StockSharp.Algo.Statistics.RoundtripCountParameter) | 完成的往返交易次数（与 `ClosedVolume > 0` 进行的平仓交易） | `int` |
| [平均交易利润参数](xref:StockSharp.Algo.Statistics.AverageTradeProfitParameter) | 每笔交易的平均利润。公式：`SumPnL / Count` | `decimal` |
| [AverageWinTradeParameter](xref:StockSharp.Algo.Statistics.AverageWinTradeParameter) | 盈利交易的平均利润。仅考虑具有 `PnL > 0` 的交易 | `decimal` |
| [平均亏损交易参数](xref:StockSharp.Algo.Statistics.AverageLossTradeParameter) | 亏损交易的平均亏损。仅考虑具有 `PnL < 0` 的交易 | `decimal` |
| [ProfitFactorParameter](xref:StockSharp.Algo.Statistics.ProfitFactorParameter) | 利润因子。公式：`GrossProfit / GrossLoss` | `decimal` |
| [期望参数](xref:StockSharp.Algo.Statistics.ExpectancyParameter) | 数学期望。公式：`P(win) * AvgWin + P(loss) * AvgLoss` | `decimal` |
| [每月交易参数](xref:StockSharp.Algo.Statistics.PerMonthTradeParameter) | 每月平均交易次数 | `decimal` |
| [每日交易参数](xref:StockSharp.Algo.Statistics.PerDayTradeParameter) | 每日平均交易次数 | `decimal` |
| [毛利润参数](xref:StockSharp.Algo.Statistics.GrossProfitParameter) | 毛利润。所有盈利交易的损益总和 (`PnL > 0`) | `decimal` |
| [总亏损参数](xref:StockSharp.Algo.Statistics.GrossLossParameter) | 总亏损。所有亏损交易的盈亏总和（`PnL < 0`，数值为负） | `decimal` |

## 位置参数

此组中的参数实现了 [IPositionStatisticParameter](xref:StockSharp.Algo.Statistics.IPositionStatisticParameter) 接口。它们在每次持仓变动时接收数据。

| 类别 | 描述 | 值类型 |
|-------|-------------|------------|
| [MaxLongPositionParameter](xref:StockSharp.Algo.Statistics.MaxLongPositionParameter) | 最大多头持仓。最高正持仓值 | `decimal` |
| [MaxShortPositionParameter](xref:StockSharp.Algo.Statistics.MaxShortPositionParameter) | 最大空头持仓。最高绝对负持仓数值 | `decimal` |

## 订单参数

该组中的所有参数都实现了 [IOrderStatisticParameter](xref:StockSharp.Algo.Statistics.IOrderStatisticParameter) 接口，并继承自 [BaseOrderStatisticParameter](xref:StockSharp.Algo.Statistics.BaseOrderStatisticParameter`1)。它们接收关于订单注册、变更和错误的数据。

| 类别 | 描述 | 值类型 |
|-------|-------------|------------|
| [OrderCountParameter](xref:StockSharp.Algo.Statistics.OrderCountParameter) | 注册订单总数 | `int` |
| [OrderRegisterErrorCountParameter](xref:StockSharp.Algo.Statistics.OrderRegisterErrorCountParameter) | 订单注册错误数量 | `int` |
| [OrderInsufficientFundErrorCountParameter](xref:StockSharp.Algo.Statistics.OrderInsufficientFundErrorCountParameter) | "资金不足" 错误的数量（类型 `InsufficientFundException`） | `int` |
| [OrderCancelErrorCountParameter](xref:StockSharp.Algo.Statistics.OrderCancelErrorCountParameter) | 订单取消错误次数 | `int` |

## 延迟参数

延迟参数也实现了 [IOrderStatisticParameter](xref:StockSharp.Algo.Statistics.IOrderStatisticParameter)，但跟踪订单处理的时间特性。

| 类别 | 描述 | 值类型 |
|-------|-------------|------------|
| [最大延迟注册参数](xref:StockSharp.Algo.Statistics.MaxLatencyRegistrationParameter) | 最大订单注册延迟 (`Order.LatencyRegistration` 属性) | `TimeSpan` |
| [MinLatencyRegistrationParameter](xref:StockSharp.Algo.Statistics.MinLatencyRegistrationParameter) | 最小订单注册延迟 | `TimeSpan` |
| [最大延迟取消参数](xref:StockSharp.Algo.Statistics.MaxLatencyCancellationParameter) | 最大订单取消延迟 (`Order.LatencyCancellation` 属性) | `TimeSpan` |
| [MinLatencyCancellationParameter](xref:StockSharp.Algo.Statistics.MinLatencyCancellationParameter) | 最小订单取消延迟 | `TimeSpan` |

## 使用

### 访问策略统计

```cs
var strategy = new MyStrategy();

// 执行后访问统计信息
foreach (var param in strategy.StatisticManager.Parameters)
{
    Console.WriteLine($"{param.DisplayName}: {param.Value}");
}
```

### 检索特定参数

```cs
// 获取净利润值
var netProfit = strategy.StatisticManager.Parameters
    .OfType<NetProfitParameter>()
    .First();

Console.WriteLine($"Net profit: {netProfit.Value}");
```

### 为系数配置无风险利率

夏普比率和索提诺比率需要设定无风险利率以便正确计算：

```cs
// 为所有系数设置 3% 的无风险利率
foreach (var param in strategy.StatisticManager.Parameters
    .OfType<IRiskFreeRateStatisticParameter>())
{
    param.RiskFreeRate = 0.03m;
}
```

### 配置百分比参数的初始资本

[NetProfitPercentParameter](xref:StockSharp.Algo.Statistics.NetProfitPercentParameter) 和 [MaxProfitPercentParameter](xref:StockSharp.Algo.Statistics.MaxProfitPercentParameter) 参数需要设置初始资金值：

```cs
// 设置用于百分比计算的初始资金
foreach (var param in strategy.StatisticManager.Parameters
    .OfType<IBeginValueStatisticParameter>())
{
    param.BeginValue = 1_000_000m; // 1,000,000
}
```

### 重置统计数据

```cs
// 重置所有统计参数
strategy.StatisticManager.Reset();
```

### 保存与加载状态

所有参数都支持通过 `IPersistable` 接口进行序列化：

```cs
// Saving
var storage = new SettingsStorage();
strategy.StatisticManager.Save(storage);

// Loading
strategy.StatisticManager.Load(storage);
```

## 另请参阅

- [策略统计](statistics.md)
- [统计图形组件](../graphical_user_interface/strategies/statistics.md)
