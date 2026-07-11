# 盈亏管理

[S#](../api.md) 通过 [PnLManager](xref:StockSharp.Algo.PnL.PnLManager) 实现损益（PnL）计算。该管理器处理一系列消息（交易、市场数据），并计算已实现和未实现的利润。

## IPnLManager 接口

[IPnLManager](xref:StockSharp.Algo.PnL.IPnLManager) 接口定义了基础契约：

- **已实现盈亏** — 已实现的利润/亏损（小数）。在平仓时累计。
- **未实现盈亏** — 未实现的利润/亏损（小数）。根据当前市场价格重新计算。
- **Reset()** — 重置管理器的状态。
- **UpdateSecurity(Level1ChangeMessage)** — 更新工具参数（价格步长、步进价格、手数倍数）。
- **ProcessMessage(Message, ICollection\<PortfolioPnLManager\>)** — 处理一条消息；当一个持仓被关闭时返回 [PnLInfo](xref:StockSharp.Algo.PnL.PnLInfo)，否则返回 `null`。

## 建筑学

PnL 系统有三级层次结构：

```
PnLManager
  └── PortfolioPnLManager（按投资组合名称）
        └── PnLQueue（按 SecurityId）
```

- [PnLManager](xref:StockSharp.Algo.PnL.PnLManager) — 顶层，管理一个组合经理字典。
- [PortfolioPnLManager](xref:StockSharp.Algo.PnL.PortfolioPnLManager) — 特定投资组合的PnL管理器，按工具管理队列。
- [PnLQueue](xref:StockSharp.Algo.PnL.PnLQueue) — 用于单一工具匹配交易的先进先出队列。

### PnL队列 — 计算队列

[PnLQueue](xref:StockSharp.Algo.PnL.PnLQueue) 负责匹配开仓和平仓交易：

- **PriceStep** — 工具价格步长。
- **StepPrice** — 步长价格（期货用）。
- **杠杆** — 杠杆。
- **LotMultiplier** — 手数乘数。

利润乘数的计算公式如下：

```
Multiplier = (StepPrice / PriceStep) * Leverage * LotMultiplier
```

对于普通股票（`StepPrice` 未设置的情况），乘数等于 `1 * Leverage * LotMultiplier`。

## PnL信息 — 交易处理结果

[PnLInfo](xref:StockSharp.Algo.PnL.PnLInfo) 类包含平仓结果：

- **服务器时间** — 交易时间。
- **已平仓量** — 已平仓持仓的交易量。
- **PnL** — 来自此交易的已实现利润。

例如，如果持仓是+2，而来了一个-5合约的交易，那么`ClosedVolume = 2`（持仓中的2个合约被平仓）。

## 配置数据源

[PnLManager](xref:StockSharp.Algo.PnL.PnLManager) 允许您为未实现利润计算选择市场数据来源：

| 属性 | 默认值 | 描述 |
|----------|:-------:|-------------|
| `UseTick` | `true` | 使用逐笔交易。 |
| `UseOrderBook` | `false` | 使用订单簿（最佳买/卖价）。 |
| `UseLevel1` | `false` | 使用 Level1 数据。 |
| `UseOrderLog` | `false` | 使用订单日志。 |
| `UseCandles` | `true` | 使用K线（收盘价）。|

## 通过适配器进行集成

[PnLMessageAdapter](xref:StockSharp.Algo.PnL.PnLMessageAdapter) 类封装了一个内部适配器，并自动处理所有用于盈亏计算的消息。

## 与战略的整合

该策略 ([Strategy](xref:StockSharp.Algo.Strategies.Strategy)) 提供：

- `PnLManager` 属性——管理器实例。
- `PnL` 属性 — 总利润 (`RealizedPnL + UnrealizedPnL`)。
- `PnLChanged` 事件 — 利润变动通知。
- `PnLReceived2` 事件 — 当收到新的损益数据时的通知。

## 使用示例

```cs
var pnlManager = new PnLManager
{
    UseTick = true,
    UseOrderBook = true,
    UseCandles = true
};

// 处理消息
var info = pnlManager.ProcessMessage(executionMsg);
if (info != null)
{
    Console.WriteLine($"已平仓: {info.ClosedVolume}, PnL: {info.PnL}");
}

// 总盈亏
var realizedPnL = pnlManager.RealizedPnL;
var unrealizedPnL = pnlManager.UnrealizedPnL;
var totalPnL = realizedPnL + unrealizedPnL;

Console.WriteLine($"Realized PnL: {realizedPnL}");
Console.WriteLine($"Unrealized PnL: {unrealizedPnL}");
Console.WriteLine($"Total PnL: {totalPnL}");
```

## 重置状态

`Reset()` 方法清除所有投资组合经理、计算队列，并将已实现盈亏重置为零：

```cs
pnlManager.Reset();
```
