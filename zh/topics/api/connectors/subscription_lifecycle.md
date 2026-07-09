# 订阅生命周期

StockSharp 中的订阅会经历特定的生命周期阶段。[ISubscriptionProvider](xref:StockSharp.BusinessEntities.ISubscriptionProvider) 接口提供用于跟踪每个阶段的事件。

## 生命周期事件

### 订阅已开始

```cs
event Action<Subscription> SubscriptionStarted;
```

当订阅已成功启动时调用——适配器已接受请求并开始传输数据。对于历史订阅，这意味着数据加载已开始。对于实时订阅——意味着服务器已接受请求。

### 在线订阅

```cs
event Action<Subscription> SubscriptionOnline;
```

当订阅已切换到实时模式时调用。对于实时订阅，这意味着历史数据回补（如果有的话）已完成，数据现在正在实时到达。这对于策略来说是一个重要信号，表明指标已经“预热”，可以开始交易。

### 订阅已停止

```cs
event Action<Subscription, Exception> SubscriptionStopped;
```

当订阅结束时调用。`Exception` 参数包含停止的原因：
- `null` -- 正常完成（用户已取消订阅或历史数据已完成）。
- 一个异常对象——一个错误（连接丢失、服务器端错误等）。

### 订阅失败

```cs
event Action<Subscription, Exception, bool> SubscriptionFailed;
```

在订阅错误时调用。第三个 `bool` 参数指示这是订阅（`true`）还是退订（`false`）操作。

## 事件顺序

实时订阅的典型顺序：

1. 呼叫 `Subscribe(subscription)`
2. `SubscriptionStarted` -- 订阅已接受
3. 数据到达（K线、订单簿、交易等）
4. `SubscriptionOnline` -- 切换到实时模式
5. 实时持续数据到达
6. 呼叫 `UnSubscribe(subscription)` 或连接丢失
7. `SubscriptionStopped` -- 订阅已结束

对于历史订阅（带指定日期范围）:

1. 呼叫 `Subscribe(subscription)`
2. `SubscriptionStarted` -- 订阅已接受
3. 历史数据到达
4. `SubscriptionStopped` 与 `null` -- 所有数据已接收

## 连接时订阅

[Connector.SubscriptionsOnConnect](xref:StockSharp.Algo.Connector) 属性定义了在连接时自动发送的订阅集合：

```cs
ISet<Subscription> SubscriptionsOnConnect { get; }
```

默认情况下，包括用于交易品种查询、投资组合查询和订单查询的订阅：

```cs
SubscriptionsOnConnect.Add(SecurityLookup);
SubscriptionsOnConnect.Add(PortfolioLookup);
SubscriptionsOnConnect.Add(OrderLookup);
```

您可以添加您自己的订阅，这些订阅将在每次连接时自动启动：

```cs
// 添加自动 Level1 数据订阅
var l1Sub = new Subscription(DataType.Level1, security);
connector.SubscriptionsOnConnect.Add(l1Sub);

// 移除连接时的自动订单查找
connector.SubscriptionsOnConnect.Remove(connector.OrderLookup);
```

## 每个适配器的连接事件

在处理多个连接（多个适配器）时，指示哪个特定适配器已连接或断开的事件非常有用：

### ConnectedEx

```cs
event Action<IMessageAdapter> ConnectedEx;
```

在特定适配器成功连接时被调用。参数是引发该事件的适配器。

### 已断开连接Ex

```cs
event Action<IMessageAdapter> DisconnectedEx;
```

在特定适配器断开连接时调用。

### 连接错误异常

```cs
event Action<IMessageAdapter, Exception> ConnectionErrorEx;
```

在特定适配器发生连接错误时调用。

聚合事件 `Connected`、`Disconnected` 和 `ConnectionError` 也可用，它们在未指定特定适配器的情况下触发。

## 例子

```cs
private readonly Connector _connector = new();

public void SetupSubscriptionTracking()
{
    // 跟踪订阅生命周期
    _connector.SubscriptionStarted += subscription =>
    {
        Console.WriteLine($"订阅已启动: {subscription.DataType}, " +
            $"证券: {subscription.SecurityId}");
    };

    _connector.SubscriptionOnline += subscription =>
    {
        Console.WriteLine($"订阅在线: {subscription.DataType}");
    };

    _connector.SubscriptionStopped += (subscription, error) =>
    {
        if (error == null)
            Console.WriteLine($"订阅已完成: {subscription.DataType}");
        else
            Console.WriteLine($"订阅已中断: {subscription.DataType}, " +
                $"错误: {error.Message}");
    };

    // 跟踪各个适配器连接
    _connector.ConnectedEx += adapter =>
    {
        Console.WriteLine($"适配器已连接: {adapter.Name}");
    };

    _connector.DisconnectedEx += adapter =>
    {
        Console.WriteLine($"适配器已断开: {adapter.Name}");
    };

    _connector.ConnectionErrorEx += (adapter, error) =>
    {
        Console.WriteLine($"适配器连接错误 {adapter.Name}: {error.Message}");
    };

    // 连接
    _connector.Connect();

    // 连接后 -- 创建订阅
    _connector.Connected += () =>
    {
        var subscription = new Subscription(DataType.Ticks, security);
        _connector.Subscribe(subscription);
    };
}
```

## 另请参阅

[连接](../connectors.md)
