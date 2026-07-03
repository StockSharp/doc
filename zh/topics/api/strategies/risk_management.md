# 风险管理

## 概览

StockSharp 中的每个策略都内置了 `RiskManager`，可实现交易风险的自动控制。当触发规则时，管理者可以平仓、取消活动订单，或完全停止策略交易。

风险管理器处理通过策略的所有交易信息，当规则条件满足时，它会自动执行指定操作，而无需在策略逻辑中添加任何额外代码。

## 策略属性

### 风险管理者

`IRiskManager` 对象用于管理规则列表。当策略初始化时，它会自动创建：

```csharp
// Access the risk manager
IRiskManager manager = strategy.RiskManager;
```

### 风险规则

一个用于读取和设置规则列表的方便属性：

```csharp
// Read current rules
IEnumerable<IRiskRule> rules = strategy.RiskRules;

// Set new rules
strategy.RiskRules = new IRiskRule[]
{
    new RiskPnLRule { PnL = -1000m, Action = RiskActions.StopTrading },
    new RiskPositionSizeRule { Position = 100m, Action = RiskActions.ClosePositions },
};
```

## 触发操作

`RiskActions` 枚举定义了可能的操作：

| 动作 | 描述 |
|--------|-------------|
| `ClosePositions` | 使用市价单关闭所有未平仓持仓 |
| `StopTrading` | 阻止策略交易 |
| `CancelOrders` | 取消所有活动订单 |

当规则被触发时，该策略会记录一条警告，其中包含规则名称、其参数以及执行的操作。

## 可用规则

### 风险盈亏规则 -- 盈利/亏损控制

当达到指定的盈亏水平时触发。正值控制利润，负值控制亏损：

```csharp
// Stop trading on a loss exceeding 5000
new RiskPnLRule
{
    PnL = -5000m,
    Action = RiskActions.StopTrading
}
```

### 风险持仓规模规则 -- 持仓规模控制

当达到指定的持仓规模时触发：

```csharp
// Cancel orders when position >= 100
new RiskPositionSizeRule
{
    Position = 100m,
    Action = RiskActions.CancelOrders
}
```

### RiskOrderFreqRule -- 订单频率控制

当指定时间间隔内的订单数量超过限制时触发：

```csharp
// Stop on more than 50 orders per minute
new RiskOrderFreqRule
{
    Count = 50,
    Interval = TimeSpan.FromMinutes(1),
    Action = RiskActions.StopTrading
}
```

### 风险订单量规则 -- 订单量控制

当单个订单的数量超出时触发。

### 风险订单价格规则 -- 订单价格控制

当订单价格超过设定的边界时触发。

### 风险交易量规则 -- 交易量控制

当单笔交易的交易量被超过时触发。

### 风险交易价格规则 -- 交易价格控制

当交易价格超过设定的边界时触发。

### 风险交易频率规则 -- 交易频率控制

当在该时间间隔内的交易数量被超过时触发。

### 风险持仓时间规则 -- 持仓时间控制

当一个持仓持有时间超过既定时间时触发。

### 风险佣金规则 -- 佣金控制

当总佣金超出时触发。

### 风险滑点规则 -- 滑点控制

当滑点水平被超过时触发。

### RiskErrorRule -- 错误控制

当累计一定数量的错误时触发。

### RiskOrderErrorRule -- 订单注册错误控制

当订单注册错误累积时触发。

## 使用示例

```csharp
public class RiskAwareStrategy : Strategy
{
    private readonly StrategyParam<DataType> _candleType;

    public DataType CandleType
    {
        get => _candleType.Value;
        set => _candleType.Value = value;
    }

    public RiskAwareStrategy()
    {
        _candleType = Param(nameof(CandleType), TimeSpan.FromMinutes(5).TimeFrame());
    }

    protected override void OnStarted2(DateTime time)
    {
        base.OnStarted2(time);

        // Configure risk management rules
        RiskRules = new IRiskRule[]
        {
            // Close positions on a loss exceeding 10000
            new RiskPnLRule
            {
                PnL = -10000m,
                Action = RiskActions.ClosePositions
            },

            // Stop trading when position exceeds 500 contracts
            new RiskPositionSizeRule
            {
                Position = 500m,
                Action = RiskActions.StopTrading
            },

            // Cancel orders when frequency exceeds 100 per minute
            new RiskOrderFreqRule
            {
                Count = 100,
                Interval = TimeSpan.FromMinutes(1),
                Action = RiskActions.CancelOrders
            },
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

        // Trading logic
        if (candle.OpenPrice < candle.ClosePrice && Position <= 0)
            BuyMarket(Volume + Math.Abs(Position));
        else if (candle.OpenPrice > candle.ClosePrice && Position >= 0)
            SellMarket(Volume + Math.Abs(Position));
    }
}
```

在这个例子中，当策略启动时会设置风险管理规则。当亏损达到10000时，持仓将被自动平仓。当持仓量超过500合约时，交易将被阻止。当订单下单过于频繁时，活动订单将被取消。所有这些检查都是自动执行的，无需在`ProcessCandle`方法中编写任何额外代码。
