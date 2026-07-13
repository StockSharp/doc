# 高级订阅

## 概览

`Strategy` 类提供了一组高级市场数据订阅方法：`SubscribeCandles`、`SubscribeTicks`、`SubscribeLevel1` 和 `SubscribeOrderBook`。这些方法返回一个 `ISubscriptionHandler<T>` 对象，该对象允许以方便的链式风格绑定数据处理器和指标。

与手动创建 `Subscription` 对象并调用 `Subscribe()` 不同，高级方法：

- 自动使用正确的参数创建订阅
- 提供一个用于绑定处理程序的已键入 `ISubscriptionHandler<T>`
- 通过 `Bind` 方法与指示器系统集成
- 支持自动图表渲染
- 妥善管理订阅生命周期

## 订阅方式

### 订阅K线

订阅K线。接受时间框架或 `DataType`：

```csharp
// 按时间周期订阅
ISubscriptionHandler<ICandleMessage> SubscribeCandles(
    TimeSpan tf,
    bool isFinishedOnly = true,
    Security security = default);

// 按 DataType 订阅（支持所有 K线类型）
ISubscriptionHandler<ICandleMessage> SubscribeCandles(
    DataType dt,
    bool isFinishedOnly = true,
    Security security = default);

// 使用现成的 Subscription 对象订阅
ISubscriptionHandler<ICandleMessage> SubscribeCandles(Subscription subscription);
```

`isFinishedOnly` 参数默认值为 `true` -- 处理程序只接收完成的K线。

### 订阅逐笔成交

订阅逐笔交易：

```csharp
ISubscriptionHandler<ITickTradeMessage> SubscribeTicks(Security security = null);
ISubscriptionHandler<ITickTradeMessage> SubscribeTicks(Subscription subscription);
```

### 订阅等级1

订阅一级市场数据（最佳买/卖价、最近成交以及其他字段）:

```csharp
ISubscriptionHandler<Level1ChangeMessage> SubscribeLevel1(Security security = null);
ISubscriptionHandler<Level1ChangeMessage> SubscribeLevel1(Subscription subscription);
```

### 订阅订单簿

订阅订单簿：

```csharp
ISubscriptionHandler<IOrderBookMessage> SubscribeOrderBook(Security security = null);
ISubscriptionHandler<IOrderBookMessage> SubscribeOrderBook(Subscription subscription);
```

如果未指定 `security` 参数，则使用策略的 `Security`。

## ISubscriptionHandler 接口

`ISubscriptionHandler<T>` 对象提供以下方法：

### 开始 / 停止

开始和停止订阅：

```csharp
handler.Start();   // 调用 Subscribe
handler.Stop();    // 调用 UnSubscribe
```

### 绑定（无指示器）

绑定一个简单的数据处理器：

```csharp
handler.Bind(Action<T> callback);
```

### 绑定（带指示器）

将处理程序绑定到一个或多个指标。指标会自动处理传入的数据，而处理程序接收已经计算好的值：

```csharp
// 一个指标 -- decimal 数值
handler.Bind(IIndicator indicator, Action<T, decimal> callback);

// 两个指标
handler.Bind(IIndicator ind1, IIndicator ind2, Action<T, decimal, decimal> callback);

// 最多八个指标
handler.Bind(ind1, ind2, ind3, ..., callback);

// 指标数组
handler.Bind(IIndicator[] indicators, Action<T, decimal[]> callback);
```

仅当所有指标都返回非空值时，才会调用 `Bind` 的处理程序。

### 绑定为空

类似于 `Bind`，但即使指标返回空值，也会调用处理程序。值表示为 `decimal?`：

```csharp
handler.BindWithEmpty(IIndicator indicator, Action<T, decimal?> callback);
```

### 绑定扩展

提供对完整 `IIndicatorValue` 对象的访问，而不是提取的 `decimal`：

```csharp
handler.BindEx(IIndicator indicator, Action<T, IIndicatorValue> callback, bool allowEmpty = false);
```

## 示例：带指标的策略

```csharp
public class SmaStrategy : Strategy
{
    private readonly StrategyParam<int> _shortPeriod;
    private readonly StrategyParam<int> _longPeriod;
    private readonly StrategyParam<DataType> _candleType;

    public int ShortPeriod
    {
        get => _shortPeriod.Value;
        set => _shortPeriod.Value = value;
    }

    public int LongPeriod
    {
        get => _longPeriod.Value;
        set => _longPeriod.Value = value;
    }

    public DataType CandleType
    {
        get => _candleType.Value;
        set => _candleType.Value = value;
    }

    public SmaStrategy()
    {
        _shortPeriod = Param(nameof(ShortPeriod), 10);
        _longPeriod = Param(nameof(LongPeriod), 20);
        _candleType = Param(nameof(CandleType), TimeSpan.FromMinutes(5).TimeFrame());
    }

    protected override void OnStarted2(DateTime time)
    {
        base.OnStarted2(time);

        var shortSma = new SimpleMovingAverage { Length = ShortPeriod };
        var longSma = new SimpleMovingAverage { Length = LongPeriod };

        var subscription = SubscribeCandles(CandleType);

        // 绑定两个指标 -- 处理程序会被调用
        // 当两个指标都已形成时
        subscription
            .Bind(shortSma, longSma, (candle, shortValue, longValue) =>
            {
                if (!IsFormedAndOnlineAndAllowTrading())
                    return;

                if (shortValue > longValue && Position <= 0)
                    BuyMarket(Volume + Math.Abs(Position));
                else if (shortValue < longValue && Position >= 0)
                    SellMarket(Volume + Math.Abs(Position));
            })
            .Start();

        // 图表设置
        var area = CreateChartArea();
        if (area != null)
        {
            DrawCandles(area, subscription);
            DrawIndicator(area, shortSma);
            DrawIndicator(area, longSma);
            DrawOwnTrades(area);
        }
    }
}
```

## 示例：勾选订阅

```csharp
protected override void OnStarted2(DateTime time)
{
    base.OnStarted2(time);

    SubscribeTicks()
        .Bind(tick =>
        {
            if (!IsFormedAndOnlineAndAllowTrading())
                return;

            this.AddInfoLog("逐笔成交: 价格={0}, 数量={1}", tick.Price, tick.Volume);
        })
        .Start();
}
```

## 示例：订单簿订阅

```csharp
protected override void OnStarted2(DateTime time)
{
    base.OnStarted2(time);

    SubscribeOrderBook()
        .Bind(book =>
        {
            var bestBid = book.GetBestBid();
            var bestAsk = book.GetBestAsk();

            if (bestBid != null && bestAsk != null)
            {
                var spread = bestAsk.Price - bestBid.Price;
                this.AddInfoLog("Spread: {0}", spread);
            }
        })
        .Start();
}
```

## 与手动订阅创建的区别

| 方面 | 手动订阅 | 高级方法 |
|--------|-------------------|-------------------|
| 创建 | `new Subscription(DataType, Security)` | `SubscribeCandles(tf)` |
| 数据处理 | 订阅连接器事件 | `Bind(callback)` |
| 指标 | 手动 `indicator.Process()` 呼叫 | 通过 `Bind(indicator, callback)` 自动 |
| 指标注册 | 手动添加到 `Indicators` | 自动添加到 `Bind` |
| 图表渲染 | 与 `IChart` 的手动集成 | `DrawCandles`, `DrawIndicator` |

在大多数策略中建议使用高级方法，因为它们显著减少了代码量并降低了出错的可能性。
