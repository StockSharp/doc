# 滑点测量

[S#](../api.md) 通过 [SlippageManager](xref:StockSharp.Algo.Slippage.SlippageManager) 计算滑点。滑点是订单预期执行价格与实际交易价格之间的差额。

## ISlippageManager 接口

[ISlippageManager](xref:StockSharp.Algo.Slippage.ISlippageManager) 接口定义了基础契约：

- **滑点** — 累积的总滑点（小数）。
- **Reset()** — 重置管理器的状态。
- **ProcessMessage(Message)** — 处理一条消息；返回给定执行的滑点或 `null`。

## 运作方式

滑点管理器分三个阶段工作：

### 1. 更新市场价格

当收到 [Level1ChangeMessage](xref:StockSharp.Messages.Level1ChangeMessage) 或 [QuoteChangeMessage](xref:StockSharp.Messages.QuoteChangeMessage) 时，管理者会保存每个交易品种的最佳买价和卖价。

### 2. 保存计划价格

当收到一条 [OrderRegisterMessage](xref:StockSharp.Messages.OrderRegisterMessage) 时，管理者会保存“计划价格”——下单注册时的最佳市场价格：

- 对于买入（`Buy`），使用最佳卖价。
- 对于卖出（`Sell`），使用最佳买价。

### 3. 计算滑点

当收到包含交易的 [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage) 时，经理会计算滑点：

- 买入：`(TradePrice - PlannedPrice) * TradeVolume`
- 出售：`(PlannedPrice - TradePrice) * TradeVolume`

正值表示不利的滑点（价格比预期更差），而负值表示有利的滑点（价格比预期更好）。

## 状态：ISlippageManagerState

[ISlippageManagerState](xref:StockSharp.Algo.Slippage.ISlippageManagerState) 接口存储管理器的内部状态：

- 每个工具的最佳买入/卖出价格（[SecurityId](xref:StockSharp.Messages.SecurityId)）。
- 每笔交易的计划价格和方向（`TransactionId`）。
- 总累计滑点。

默认实现是 [SlippageManagerState](xref:StockSharp.Algo.Slippage.SlippageManagerState)。

## 设置

| 属性 | 默认值 | 描述 |
|----------|:-------:|-------------|
| `CalculateNegative` | `true` | 考虑有利偏移。如果 `false`，负值将被替换为零。 |

## 通过适配器进行集成

[SlippageMessageAdapter](xref:StockSharp.Algo.Slippage.SlippageMessageAdapter) 类封装了一个内部适配器，并自动为所有交易计算滑点。

## 与战略的整合

该策略 ([Strategy](xref:StockSharp.Algo.Strategies.Strategy)) 暴露了 `Slippage` 属性，用于跟踪整体滑点。

## 使用示例

```cs
// 创建带状态存储的管理器
var manager = new SlippageManager(new SlippageManagerState());

// 仅统计不利滑点
manager.CalculateNegative = false;

// Processing market data (updating best prices)
manager.ProcessMessage(level1Msg);
manager.ProcessMessage(quoteChangeMsg);

// Processing order registration (saving the planned price)
manager.ProcessMessage(orderRegisterMsg);

// Processing a trade (calculating slippage)
decimal? slippage = manager.ProcessMessage(executionMsg);
if (slippage != null)
{
    Console.WriteLine($"Slippage: {slippage.Value}");
}

// 累计总滑点
Console.WriteLine($"Total slippage: {manager.Slippage}");
```

## 重置状态

`Reset()` 方法会完全清除内部状态：最佳价格、计划价格和累计滑点：

```cs
manager.Reset();
```
