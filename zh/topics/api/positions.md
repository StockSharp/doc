# 持仓管理

StockSharp 提供了一个灵活的持仓管理系统，允许您跟踪持仓的当前状态，基于订单或交易进行计算，并维护持仓的生命周期历史（开仓、平仓、反转）。

## 持仓管理器

[PositionManager](xref:StockSharp.Algo.Positions.PositionManager) 类实现了 [IPositionManager](xref:StockSharp.Algo.Positions.IPositionManager) 接口，并作为根据传入消息计算当前持仓的主要组件。

### 创建管理器

构造函数接受两个参数：

```cs
var state = new PositionManagerState();
var manager = new PositionManager(byOrders: false, state);
```

- `byOrders = true` —— 该持仓是基于订单余额变化计算的。适用于交易系统接收订单状态更新但未接收单笔交易的情况。
- `byOrders = false` -- 该持仓基于交易量计算（推荐模式）。提供对已执行操作的更精确的核算。

### 处理传入消息

`ProcessMessage` 方法接收一个传入消息 ([Message](xref:StockSharp.Messages.Message))，并在持仓发生变化时返回 [PositionChangeMessage](xref:StockSharp.Messages.PositionChangeMessage)，如果持仓未发生变化则返回 `null`：

```cs
var posChange = manager.ProcessMessage(executionMsg);

if (posChange != null)
{
    Console.WriteLine($"持仓: {posChange.CurrentValue}");
}
```

## IPositionManagerState

[IPositionManagerState](xref:StockSharp.Algo.Positions.IPositionManagerState) 接口描述了持仓管理器的内部状态。[PositionManagerState](xref:StockSharp.Algo.Positions.PositionManagerState) 实现存储有关当前订单和持仓的信息。

### 主要方法

| 方法 | 描述 |
|--------|-------------|
| `AddOrGetOrder` | 注册一个新订单或通过 `transactionId` 返回现有订单 |
| `TryGetOrder` | 检索订单参数（交易品种、投资组合、方向、余额） |
| `UpdateOrderBalance` | 在部分执行后更新当前订单余额 |
| `RemoveOrder` | 从跟踪中移除已完成的订单 |
| `UpdatePosition` | 按工具和投资组合更新持仓，返回新值 |
| `Clear` | 重置所有管理器状态 |

### 与状态示例一起工作

```cs
var state = new PositionManagerState();

// 注册订单
state.AddOrGetOrder(
    transactionId: 12345,
    securityId: secId,
    portfolioName: "MyPortfolio",
    side: Sides.Buy,
    volume: 100,
    balance: 100
);

// 部分成交后更新
state.UpdateOrderBalance(12345, newBalance: 60);

// 直接更新仓位
var newPosition = state.UpdatePosition(secId, "MyPortfolio", diff: 40);
Console.WriteLine($"当前持仓: {newPosition}");

// Clear
state.Clear();
```

## 持仓生命周期追踪器

[PositionLifecycleTracker](xref:StockSharp.Algo.Positions.PositionLifecycleTracker) 类跟踪持仓的完整生命周期——从开仓到平仓（往返）。这对于分析单个交易、计算每个持仓的利润以及生成报告非常有用。

### 主要特点

- **历史**：`History` 属性（`IReadOnlyList<ReportPosition>`）包含所有已完成的往返持仓。
- **`RoundTripClosed` 事件**：当一个持仓被平仓（数值变为零）或反转（持仓符号改变）时触发。
- **`ProcessPosition` 方法**：接受一个 [Position](xref:StockSharp.BusinessEntities.Position) 对象并更新内部状态。

### 检测到的状态

| 状态 | 描述 |
|-------|-------------|
| 开仓 | 持仓从零变为非零 |
| 关闭 | 持仓价值为零 |
| 反转 | 持仓方向变化（例如，从多头变为空头） |

### 使用示例

```cs
var tracker = new PositionLifecycleTracker();

tracker.RoundTripClosed += report =>
{
    Console.WriteLine($"往返交易已完成:");
    Console.WriteLine($"  开仓: {report.OpenTime}");
    Console.WriteLine($"  平仓: {report.CloseTime}");
};

// 处理仓位更新
tracker.ProcessPosition(position);

// 查看历史
foreach (var report in tracker.History)
{
    Console.WriteLine($"  {report.OpenTime} -> {report.CloseTime}");
}
```

## 持仓消息适配器

[PositionMessageAdapter](xref:StockSharp.Algo.Positions.PositionMessageAdapter) 类是一个围绕消息适配器的封装，它可以从消息流自动计算持仓。它在内部连接器基础设施中使用。

### 运作方式

```cs
var innerAdapter = connector.Adapter;
var posManager = new PositionManager(byOrders: false, new PositionManagerState());
var posAdapter = new PositionMessageAdapter(innerAdapter, posManager);
```

适配器拦截订单执行和交易消息，调用 `PositionManager.ProcessMessage`，并为上游处理器生成相应的 `PositionChangeMessage` 实例。

## 策略中的持仓

在 [Strategy](xref:StockSharp.Algo.Strategies.Strategy) 类中，可以通过 `Position` 属性访问当前持仓：

```cs
// 主交易品种的当前仓位
decimal currentPosition = Position;

// 平仓
if (Position > 0)
    SellMarket(Math.Abs(Position));
else if (Position < 0)
    BuyMarket(Math.Abs(Position));

// 或通过内置方法
ClosePosition();
```

有关策略中交易操作的更多详细信息，请参阅[交易操作](strategies/trading_operations.md)部分。

## 另请参阅

- [交易操作](strategies/trading_operations.md)
- [持仓保护](strategies/take_profit_and_stop_loss.md)
- [目标持仓管理](strategies/target_position_management.md)
- [报告](strategies/reporting.md)
