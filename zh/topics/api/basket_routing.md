# 适配器路由

StockSharp 支持同时连接多个交易所和经纪商。路由系统（篮子路由）负责决定将不同消息发送到哪个适配器，从而使多个连接能够以透明方式协同工作。

## 总体架构

使用多个适配器时，连接器会自动创建一个组合所有连接的篮子。路由器负责确定每条具体消息应发送到哪个适配器，包括市场数据订阅、事务、投资组合请求等。

## AdapterRouter

[IAdapterRouter](xref:StockSharp.Algo.IAdapterRouter) 接口定义适配器之间的消息路由逻辑。

### 主要方法

| 方法 | 说明 |
|--------|-------------|
| `GetAdapters` | 返回适合处理指定消息的适配器列表 |
| `GetSubscriptionAdaptersAsync` | 异步确定用于市场数据订阅的适配器 |
| `GetPortfolioAdapter` | 返回与特定投资组合绑定的适配器 |
| `TryGetOrderAdapter` | 查找注册指定订单时使用的适配器 |
| `SetSecurityAdapter` | 将交易品种绑定到特定适配器 |
| `SetPortfolioAdapter` | 将投资组合绑定到特定适配器 |

### 路由优先级

系统按照以下优先级确定目标适配器：

1. **显式指定** -- 如果消息通过 `message.Adapter` 属性指定了适配器，则使用该适配器。
2. **交易品种绑定** -- 使用 `SetSecurityAdapter` 设置的映射。
3. **数据类型绑定** -- 为特定消息类型注册的适配器。
4. **支持类型筛选** -- 选择支持指定消息类型的适配器。

### 配置路由

```cs
var router = connector.Adapter.InnerAdapters;

// 将交易品种绑定到适配器以接收 tick 数据
router.SetSecurityAdapter(
    secId,
    DataType.Ticks,
    binanceAdapter
);

// 将投资组合绑定到适配器以执行交易操作
router.SetPortfolioAdapter(
    "MyPortfolio",
    interactiveBrokersAdapter
);
```

## 管理连接

### 连接状态

篮子中的每个适配器都会经历标准连接状态：

- **Disconnected** -- 已断开连接
- **Connecting** -- 正在连接
- **Connected** -- 已连接
- **Disconnecting** -- 正在断开连接

篮子会聚合所有内部适配器的状态。

### 聚合参数

`ConnectDisconnectEventOnFirstAdapter` 属性决定何时将篮子视为已连接：

- `true` -- **第一个**适配器连接后即触发连接事件（默认值），无需等待所有连接完成即可开始工作。
- `false` -- 只有在**所有**适配器均已连接后才触发事件。

```cs
// 等待所有适配器连接
connector.Adapter.InnerAdapters.ConnectDisconnectEventOnFirstAdapter = false;

connector.Connected += () =>
{
    Console.WriteLine("All adapters connected");
};

connector.Connect();
```

## 父订阅与子订阅

使用多个适配器时，一个订阅可以拆分成多个子订阅，每个子订阅分别发送到对应的适配器。系统会自动：

- 为每个合适的适配器创建子订阅
- 在通知父订阅前聚合响应
- 处理部分错误（如果一个适配器订阅失败，其他适配器仍会继续工作）

### 多交易所示例

```cs
// 添加适配器
connector.Adapter.InnerAdapters.Add(binanceAdapter);
connector.Adapter.InnerAdapters.Add(bybitAdapter);

connector.Connect();

// 订阅 tick 数据 -- 将自动路由
// 到所有支持该交易品种的适配器
var subscription = new Subscription(DataType.Ticks, security);
connector.Subscribe(subscription);
```

## 待处理消息队列

发送消息时如果没有任何适配器已连接，该消息会进入待处理队列（`IPendingMessageState`）。适配器连接后，所有积压消息都会自动发送。

```cs
// 连接前注册订单 -- 订单将被发送
// 在连接建立后自动执行
connector.RegisterOrder(order);
connector.Connect();
```

## 配置多个连接

### 使用代码配置

```cs
// 创建适配器
var binance = new BinanceMessageAdapter(connector.TransactionIdGenerator)
{
    Key = "<API_KEY>",
    Secret = "<API_SECRET>".Secure(),
};

var ib = new InteractiveBrokersMessageAdapter(connector.TransactionIdGenerator)
{
    Address = InteractiveBrokersMessageAdapter.DefaultAddress,
};

// 添加到篮子
connector.Adapter.InnerAdapters.Add(binance);
connector.Adapter.InnerAdapters.Add(ib);

// 配置路由
connector.Adapter.InnerAdapters.SetPortfolioAdapter("BinancePortfolio", binance);
connector.Adapter.InnerAdapters.SetPortfolioAdapter("IBPortfolio", ib);

connector.Connect();
```

### 图形化配置

要以可视化方式配置连接，请使用图形化配置组件。详细信息请参阅[图形化配置](connectors/graphical_configuration.md)章节。

## 订单跟踪

路由器会自动跟踪每个订单是通过哪个适配器注册的。收到订单更新（状态变化、成交）时，系统会通过同一个适配器进行路由：

```cs
// 订单将通过绑定到投资组合的适配器注册
var order = new Order
{
    Security = security,
    Portfolio = portfolio,
    Side = Sides.Buy,
    Price = price,
    Volume = volume,
};

connector.RegisterOrder(order);

// 撤单将自动通过同一适配器执行
connector.CancelOrder(order);
```

## 另请参阅

- [连接器](connectors.md)
- [图形化配置](connectors/graphical_configuration.md)
- [持仓管理](positions.md)
