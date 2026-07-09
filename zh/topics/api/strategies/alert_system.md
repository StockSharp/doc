# 警报系统

## 概览

StockSharp 中的策略具有内置警报系统，可发送各种类型的通知：弹出窗口、声音信号、日志记录和 Telegram 消息。警报对于通知交易者重要事件非常有用 —— 持仓进场、价格水平突破、错误以及其他交易信号。

在回测期间，除了 `Log` 类型外的警报会被自动跳过，以避免在测试过程中干扰。

## 警报类型

`AlertNotifications` 枚举定义了可用的类型：

| 类型 | 描述 |
|------|-------------|
| `Sound` | 声音信号 |
| `Popup` | 弹出窗口 |
| `Log` | 日志文件条目 |
| `Telegram` | 电报消息 |

## 方法

### 警报

用于发送具有指定类型、标题和消息的警报的基本方法：

```csharp
// 带标题和消息
Alert(AlertNotifications type, string caption, string message);

// 使用自动标题（使用策略名称）
Alert(AlertNotifications type, string message);
```

### 警报弹出窗口

发送弹出通知。标题是策略名称：

```csharp
AlertPopup(string message);
```

### 警报声

发送声音通知：

```csharp
AlertSound(string message);
```

### 警报日志

向日志发送通知。这种类型在回测期间也有效：

```csharp
AlertLog(string message);
```

## 配置警报服务

为了使警报生效，必须在策略的环境中注册 `IAlertNotificationService` 服务。这可以通过扩展方法完成：

```csharp
strategy.SetAlertService(alertService);
```

当前服务可以通过以下方式获取：

```csharp
var service = strategy.GetAlertService();
```

在图形应用程序（Designer、终端）中，该服务通常会自动注册。

## 使用示例

```csharp
public class AlertStrategy : Strategy
{
    private readonly StrategyParam<DataType> _candleType;
    private readonly StrategyParam<decimal> _priceLevel;

    public DataType CandleType
    {
        get => _candleType.Value;
        set => _candleType.Value = value;
    }

    public decimal PriceLevel
    {
        get => _priceLevel.Value;
        set => _priceLevel.Value = value;
    }

    public AlertStrategy()
    {
        _candleType = Param(nameof(CandleType), TimeSpan.FromMinutes(5).TimeFrame());
        _priceLevel = Param(nameof(PriceLevel), 100m);
    }

    protected override void OnStarted2(DateTime time)
    {
        base.OnStarted2(time);

        var subscription = SubscribeCandles(CandleType);

        subscription
            .Bind(ProcessCandle)
            .Start();

        // 策略启动提醒
        AlertLog("Strategy started, tracked level: " + PriceLevel);
    }

    private void ProcessCandle(ICandleMessage candle)
    {
        if (!IsFormedAndOnlineAndAllowTrading())
            return;

        // 价格自下向上穿越水平
        if (candle.OpenPrice < PriceLevel && candle.ClosePrice >= PriceLevel)
        {
            AlertPopup("Price crossed level " + PriceLevel + " upward!");
            AlertSound("Level breakout!");
            BuyMarket();
        }

        // 价格自上向下穿越水平
        if (candle.OpenPrice > PriceLevel && candle.ClosePrice <= PriceLevel)
        {
            Alert(AlertNotifications.Telegram, "Trading signal",
                "Price broke level " + PriceLevel + " downward");
            SellMarket();
        }
    }
}
```

在此示例中，该策略针对不同情况使用不同的警报类型：`AlertPopup` 和 `AlertSound` 用于立即吸引交易者的注意，`Alert` 配合 `Telegram` 类型用于远程通知。
