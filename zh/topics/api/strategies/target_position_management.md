# 目标职位管理

## 概览

目标仓位管理系统允许策略声明性地指定所需的仓位大小，而平台会自动下达必要的订单以达到该水平。你无需手动计算交易的数量和方向，只需调用 `SetTargetPosition(10)` —— 管理器将决定是买入还是卖出，以及交易的数量。

关键组件是 `PositionTargetManager` 类，它会自动：

- 计算当前位置与目标位置之间的差值
- 确定订单的方向和数量
- 处理订单执行、取消和错误
- 支持在失败时重试

## 战略方法

### 设置目标位置

设置目标位置。提供两种调用方式：

```csharp
// For the strategy's main security and portfolio
SetTargetPosition(decimal target);

// For an arbitrary security and portfolio
SetTargetPosition(Security security, Portfolio portfolio, decimal target);
```

当 `target` 大于当前仓位时，经理将下买单。当小于时——下卖单。如果仓位已等于目标仓位（考虑到 `PositionTolerance`），则不采取任何行动。

### 取消目标位置

取消先前设置的目标位置并停止所有相关的活动订单：

```csharp
// For the strategy's main security and portfolio
CancelTargetPosition();

// For an arbitrary security and portfolio
CancelTargetPosition(Security security, Portfolio portfolio);
```

### 获取目标位置

返回当前目标位置值，如果未设置目标，则返回`null`：

```csharp
decimal? target = GetTargetPosition();
decimal? target = GetTargetPosition(security, portfolio);
```

## 目标位置管理器 属性

`TargetPositionManager` 属性提供对 `PositionTargetManager` 对象的直接访问，以进行微调：

```csharp
// Maximum number of retry attempts on order error (default is 3)
TargetPositionManager.MaxRetries = 5;

// Tolerance for determining whether the target position is reached
TargetPositionManager.PositionTolerance = 0.01m;

// Order type (default is Market)
TargetPositionManager.OrderType = OrderTypes.Market;
```

经理生成以下事件：

- `TargetReached` -- 目标位置已到达
- `Error` -- 执行订单时发生错误
- `OrderRegistered` -- 经理已注册了一个订单

## TargetAlgoFactory 属性

`TargetAlgoFactory` 属性允许设置位置变动算法的工厂。默认使用 `MarketOrderAlgo`，它会创建市价单：

```csharp
// Use a custom algorithm instead of market orders
TargetAlgoFactory = (side, volume) => new MyCustomAlgo(side, volume);
```

## 使用示例

```csharp
public class TargetPositionStrategy : Strategy
{
    private readonly StrategyParam<DataType> _candleType;

    public DataType CandleType
    {
        get => _candleType.Value;
        set => _candleType.Value = value;
    }

    public TargetPositionStrategy()
    {
        _candleType = Param(nameof(CandleType), TimeSpan.FromMinutes(5).TimeFrame());
    }

    protected override void OnStarted2(DateTime time)
    {
        base.OnStarted2(time);

        // Configure the target position manager
        TargetPositionManager.MaxRetries = 5;
        TargetPositionManager.TargetReached += (sec, pf) =>
        {
            this.AddInfoLog("Target position reached: {0}, {1}", sec, pf);
        };

        var subscription = SubscribeCandles(CandleType);

        subscription
            .Bind(ProcessCandle)
            .Start();
    }

    private void ProcessCandle(ICandleMessage candle)
    {
        if (!IsFormedAndOnlineAndAllowTrading())
            return;

        if (candle.OpenPrice < candle.ClosePrice)
        {
            // Bullish candle -- set target position for buying
            SetTargetPosition(Volume);
        }
        else if (candle.OpenPrice > candle.ClosePrice)
        {
            // Bearish candle -- set target position for selling
            SetTargetPosition(-Volume);
        }
    }
}
```

在这个例子中，该策略不处理手动的交易量和方向计算。它只是声明所需的仓位大小，`PositionTargetManager` 处理所有的下单工作。
