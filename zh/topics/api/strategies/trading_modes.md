# 策略交易模式

## 概览

`TradingMode` 属性允许限制策略的交易活动而不完全停止它。这对于风险管理非常有用——例如，禁止开新持仓，同时只允许平已有持仓，或者完全阻止提交订单。

该模式使用 `StrategyTradingModes` 枚举进行设置，并且可以在策略运行时更改。

## 策略交易模式 枚举

| 值 | 描述 |
|-------|-------------|
| `Full` | 完全交易权限。订单无限制。默认值。 |
| `Disabled` | 完全禁止交易。所有下单尝试都会被拒绝。 |
| `CancelOrdersOnly` | 仅允许取消订单。禁止新订单和修改现有订单。 |
| `ReducePositionOnly` | 只允许减少当前持仓的订单。禁止开立新持仓和增加现有持仓。 |
| `LongOnly` | 仅允许做多头持仓。卖出仅允许用来平掉已持有的多头持仓（卖出数量不得超过当前持仓）。禁止开立空头持仓。 |

## 设置模式

```csharp
// 创建策略时
var strategy = new MyStrategy();
strategy.TradingMode = StrategyTradingModes.ReducePositionOnly;

// 运行期间动态变更
strategy.TradingMode = StrategyTradingModes.Disabled;
```

## 模式检查逻辑

在尝试注册订单时，策略会检查当前模式：

- **`Disabled`** -- 该订单被拒绝，理由是“禁止交易”。
- **`ReducePositionOnly`** —— 如果当前持仓为零、订单方向与持仓方向相同，或者订单数量超过持仓绝对值，该订单将被拒绝。
- **`LongOnly`** —— 如果当前持仓非正或卖出量超过当前持仓，则卖出订单被拒绝。
- **`Full`** -- 无限制。
- **`CancelOrdersOnly`** -- 只允许取消订单。

## IsFormedAndOnlineAndAllowTrading 方法

`IsFormedAndOnlineAndAllowTrading` 扩展方法检查策略是否已形成（`IsFormed`）、是否处于在线状态（`IsOnline`），以及交易模式是否允许所需的操作：

```csharp
// 检查完整交易权限（默认）
if (!IsFormedAndOnlineAndAllowTrading())
    return;

// 只检查撤单权限
if (!IsFormedAndOnlineAndAllowTrading(StrategyTradingModes.CancelOrdersOnly))
    CancelActiveOrders();

// 检查减仓权限
if (!IsFormedAndOnlineAndAllowTrading(StrategyTradingModes.ReducePositionOnly))
    return;
```

当使用 `required` 参数调用时的权限逻辑：

| 当前交易模式 \ 必需 | `Full` | `CancelOrdersOnly` | `ReducePositionOnly` |
|-------------------------------|--------|---------------------|---------------------|
| `Full` | 是 | 是 | 是 |
| `Disabled` | 不 | 不 | 不 |
| `CancelOrdersOnly` | 不 | 是 | 不 |
| `ReducePositionOnly` | 不 | 是 | 是 |
| `LongOnly` | 不 | 是 | 是 |

## 使用示例

```csharp
public class TradingModeStrategy : Strategy
{
    private readonly StrategyParam<DataType> _candleType;

    public DataType CandleType
    {
        get => _candleType.Value;
        set => _candleType.Value = value;
    }

    public TradingModeStrategy()
    {
        _candleType = Param(nameof(CandleType), TimeSpan.FromMinutes(5).TimeFrame());
    }

    protected override void OnStarted2(DateTime time)
    {
        base.OnStarted2(time);

        var subscription = SubscribeCandles(CandleType);

        subscription
            .Bind(ProcessCandle)
            .Start();
    }

    private void ProcessCandle(ICandleMessage candle)
    {
        // 检查策略是否已准备好进行完整交易
        if (!IsFormedAndOnlineAndAllowTrading())
            return;

        if (candle.ClosePrice > candle.OpenPrice)
        {
            BuyMarket(Volume);
        }
        else if (candle.ClosePrice < candle.OpenPrice)
        {
            SellMarket(Volume);
        }
    }
}

// 以限制模式启动策略 -- 仅允许多头持仓
var strategy = new TradingModeStrategy();
strategy.TradingMode = StrategyTradingModes.LongOnly;
strategy.Start();

// 稍后 -- 切换到平仓模式
strategy.TradingMode = StrategyTradingModes.ReducePositionOnly;

// 完整交易块
strategy.TradingMode = StrategyTradingModes.Disabled;
```

在这个例子中，该策略最初以`LongOnly`模式运行，该模式仅允许买入和平多仓。当市场条件发生变化时，模式可以切换到`ReducePositionOnly`以逐步平仓，然后切换到`Disabled`以完全停止交易活动。
