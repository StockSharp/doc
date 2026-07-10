# 异步订单操作

[Connector](xref:StockSharp.Algo.Connector) 类提供了所有订单操作的异步版本。异步方法可以避免阻塞调用线程，并通过 `CancellationToken` 支持取消。

## 方法

### RegisterOrderAsync

新订单的异步注册：

```cs
public async ValueTask RegisterOrderAsync(Order order, CancellationToken cancellationToken = default)
```

该方法会验证订单（检查数量，自动确定订单类型——限价还是市价），初始化交易，并将注册命令发送到适配器。如果发生错误，将生成注册错误事件。

同步对应方法 [RegisterOrder](xref:StockSharp.BusinessEntities.ITransactionProvider.RegisterOrder(StockSharp.BusinessEntities.Order)) 内部会调用 `RegisterOrderAsync`。

### CancelOrderAsync

异步取消现有订单：

```cs
public async ValueTask CancelOrderAsync(Order order, CancellationToken cancellationToken = default)
```

该方法会为取消操作创建一个新的交易标识符，并将撤单命令发送给适配器。该订单必须已经预先注册。

同步对应方法：[CancelOrder](xref:StockSharp.BusinessEntities.ITransactionProvider.CancelOrder(StockSharp.BusinessEntities.Order))。

### EditOrderAsync

对活动订单进行异步编辑（在不取消的情况下修改价格和/或数量）：

```cs
public async ValueTask EditOrderAsync(Order order, Order changes, CancellationToken cancellationToken = default)
```

参数：
- `order` -- 要编辑的原始订单。
- `changes` -- 一个包含新字段值（价格、数量等）的 [Order](xref:StockSharp.BusinessEntities.Order) 对象。

在调用之前，建议先检查是否支持编辑：

```cs
if (connector.IsOrderEditable(order) == true)
{
    var changes = order.CreateOrder();
    changes.Price = newPrice;
    await connector.EditOrderAsync(order, changes);
}
```

同步对应方法：[EditOrder](xref:StockSharp.BusinessEntities.ITransactionProvider.EditOrder(StockSharp.BusinessEntities.Order,StockSharp.BusinessEntities.Order))。

### ReRegisterOrderAsync

订单的异步重新注册（在一次操作中取消旧订单并注册新订单）：

```cs
public async ValueTask ReRegisterOrderAsync(Order oldOrder, Order newOrder, CancellationToken cancellationToken = default)
```

当交易所不支持订单编辑，但支持原子替换时使用。可以通过 `IsOrderReplaceable` 检查是否支持：

```cs
if (connector.IsOrderReplaceable(order) == true)
{
    var newOrder = order.CreateOrder();
    newOrder.Price = newPrice;
    await connector.ReRegisterOrderAsync(order, newOrder);
}
```

## 何时使用异步方法

**使用异步方法**，当：
- 代码在 `async` 上下文中执行（例如在 ASP.NET 处理程序、跨平台应用程序中）。
- 你需要通过 `CancellationToken` 支持操作取消。
- 你需要避免阻塞 UI 线程。

**使用同步方法**，当：
- 代码在策略（`Strategy`）内部运行，策略内部会自行管理线程。
- 一个不需要异步的简单脚本或控制台应用程序。

同步方法（`RegisterOrder`、`CancelOrder`、`EditOrder`）内部通过 `AsyncHelper.Run` 调用其异步对应方法，因此它们在功能上完全等效。

## 示例

```cs
private readonly Connector _connector = new();

public async Task PlaceAndManageOrderAsync(Security security, Portfolio portfolio, CancellationToken cancellationToken)
{
    // 创建订单
    var order = new Order
    {
        Security = security,
        Portfolio = portfolio,
        Direction = Sides.Buy,
        Volume = 1,
        Price = security.BestBid?.Price ?? 100m,
        Type = OrderTypes.Limit,
    };

    // 异步注册
    await _connector.RegisterOrderAsync(order, cancellationToken);

    // ... 等待市场条件变化 ...

// 异步修改价格（如果支持）
    if (_connector.IsOrderEditable(order) == true)
    {
        var changes = order.CreateOrder();
        changes.Price = order.Price - 0.01m;
        await _connector.EditOrderAsync(order, changes, cancellationToken);
    }

    // 异步撤销
    await _connector.CancelOrderAsync(order, cancellationToken);
}
```

## 另请参阅

[订单](../orders_management.md)
