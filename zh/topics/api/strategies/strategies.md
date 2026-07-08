# 策略

## 概览

`Strategy` 类是用于在 StockSharp 中创建交易策略的基类。它提供了一整套用于订阅市场数据、管理订单和持仓、计算统计数据以及生成报告的工具。

`Strategy` 类的主要功能：

- 订阅K线、订单簿、成交数据以及其他市场数据
- 下单、修改和取消订单
- 目标持仓管理
- 损益、佣金和统计计算
- 风险管理
- 计时器和规则系统
- 提醒
- 报告生成

> [!WARNING]
> 子策略功能（`ChildStrategies`）已被声明为过时且不再受支持。`ChildStrategies` 属性已标记为 `[Obsolete("Child strategies no longer supported.")]` 特性。如果您的代码使用子策略，建议进行重构——将每个策略作为独立实例运行。

## 文档部分

- [目标持仓管理](target_position_management.md) -- 通过 `SetTargetPosition` 的声明式持仓管理
- [交易模式](trading_modes.md) -- 通过 `StrategyTradingModes` 限制交易活动
- [警报系统](alert_system.md) -- 发送通知（弹出、声音、日志、Telegram）
- [计时器系统](timer_system.md) -- 定期动作执行
- [风险管理](risk_management.md) -- 风险管理规则
- [高级订阅](high_level_subscriptions.md) -- 简化的市场数据订阅
- [策略报告](reporting.md) -- 生成交易结果报告
- [高级功能](advanced_features.md) -- 订单评论、日程安排、无风险利率、指标来源

## 最小策略

```csharp
public class MyStrategy : Strategy
{
    private readonly StrategyParam<DataType> _candleType;

    public DataType CandleType
    {
        get => _candleType.Value;
        set => _candleType.Value = value;
    }

    public MyStrategy()
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
        if (!IsFormedAndOnlineAndAllowTrading())
            return;

        // 交易逻辑
    }
}
```

## 战略生命周期

1. **创建** -- 构造函数，通过 `Param<T>` 声明参数。
2. **配置** -- 设置 `Security`、`Portfolio`、`Connector` 及参数。
3. **开始** -- 调用 `Start()`，过渡到 `ProcessStates.Started` 状态，调用 `OnStarted2(DateTime)`。
4. **运行** -- 处理市场数据，执行订单。
5. **停止** -- 调用 `Stop()`，通过 `ProcessStates.Stopping` 过渡到 `ProcessStates.Stopped`，调用 `OnStopped()`。
