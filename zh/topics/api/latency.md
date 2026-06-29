# 延迟测量

[S\#](../api.md) 通过 [LatencyManager](xref:StockSharp.Algo.Latency.LatencyManager) 测量订单注册和取消的延迟。该管理器确定从发送订单到收到交易所确认所经过的时间。

## ILatencyManager 接口

[ILatencyManager](xref:StockSharp.Algo.Latency.ILatencyManager) 接口定义了基础契约：

- **LatencyRegistration** — 所有订单的总注册延迟时间 (TimeSpan)。
- **延迟取消** — 所有订单的总取消延迟（时间跨度）。
- **Reset()** — 重置管理器的状态。
- **ProcessMessage(Message)** — 处理一条消息；返回给定操作的延迟或 `null`。

## 运作方式

延迟管理器按照“请求-响应”原则运行：

### 1. 订单注册

当收到 [OrderRegisterMessage](xref:StockSharp.Messages.OrderRegisterMessage) 时，管理者会保存这对 (`TransactionId`, `LocalTime`) —— 即订单发送的时刻。

### 2. 订单取消

当收到 [OrderCancelMessage](xref:StockSharp.Messages.OrderCancelMessage) 时，管理器会保存这一对 (`TransactionId`, `LocalTime`) —— 即取消发送的时刻。

### 3. 订购替换

当收到一个 [OrderReplaceMessage](xref:StockSharp.Messages.OrderReplaceMessage) 时，管理器会同时登记一个取消（旧订单的）和一个注册（新订单的）。

### 4. 确认

当收到包含订单信息的 [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage)（不处于 `Pending` 状态且不为 `Failed`）时，管理器会计算延迟：

```
Latency = ExecutionMessage.LocalTime - StoredLocalTime
```

结果根据操作类型被加到`LatencyRegistration`或`LatencyCancellation`。

## 状态：ILatencyManagerState

[ILatencyManagerState](xref:StockSharp.Algo.Latency.ILatencyManagerState) 接口存储管理器的内部状态：

- 待处理的注册：`AddRegistration(transactionId, localTime)` / `TryGetAndRemoveRegistration(transactionId, out localTime)`
- 待取消的订单：`AddCancellation(transactionId, localTime)` / `TryGetAndRemoveCancellation(transactionId, out localTime)`
- 累计延迟：`LatencyRegistration`, `LatencyCancellation`
- 添加方法：`AddLatencyRegistration(TimeSpan)`，`AddLatencyCancellation(TimeSpan)`

默认实现是 [LatencyManagerState](xref:StockSharp.Algo.Latency.LatencyManagerState)。

## 错误处理

如果一个订单失败（`OrderState == Failed`），延迟不会被计算——该记录会被直接从状态存储中移除。

## 通过适配器进行集成

[LatencyMessageAdapter](xref:StockSharp.Algo.Latency.LatencyMessageAdapter) 类封装了一个内部适配器，并自动测量所有订单操作的延迟。

## 与战略的整合

该策略 ([Strategy](xref:StockSharp.Algo.Strategies.Strategy)) 公开了 `Latency` 属性以跟踪延迟。

## 使用示例

```cs
// Creating a manager with a state store
var manager = new LatencyManager(new LatencyManagerState());

// Processing order registration (saving the send time)
manager.ProcessMessage(orderRegisterMsg);

// Processing confirmation (calculating latency)
TimeSpan? latency = manager.ProcessMessage(executionMsg);
if (latency != null)
{
    Console.WriteLine($"Latency: {latency.Value.TotalMilliseconds} ms");
}

// Total latencies
Console.WriteLine($"Registration latency: {manager.LatencyRegistration.TotalMilliseconds} ms");
Console.WriteLine($"Cancellation latency: {manager.LatencyCancellation.TotalMilliseconds} ms");
```

## 重置状态

`Reset()` 方法清除所有待处理记录并将累计延迟重置为零：

```cs
manager.Reset();
```
