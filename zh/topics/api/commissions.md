# 佣金系统

[S\#](../api.md) 通过 [CommissionManager](xref:StockSharp.Algo.Commissions.CommissionManager) 提供灵活的佣金计算系统。管理器接收订单和成交消息，并按照已配置的规则计算佣金。

## ICommissionManager 接口

[ICommissionManager](xref:StockSharp.Algo.Commissions.ICommissionManager) 接口定义基础契约：

- **Rules** — 用于计算佣金的 [ICommissionRule](xref:StockSharp.Algo.Commissions.ICommissionRule) 规则集合。
- **Commission** — 累计佣金总额（decimal）。
- **Reset()** — 重置管理器和所有规则的状态。
- **Process(Message)** — 处理消息，并返回该消息的佣金或 `null`。

## ICommissionRule 接口

每条规则都实现 [ICommissionRule](xref:StockSharp.Algo.Commissions.ICommissionRule)：

- **Title** — 规则标题。
- **Value** — 佣金值（[Unit](xref:Ecng.ComponentModel.Unit)），可以是绝对值或百分比。
- **Process(ExecutionMessage)** — 计算特定消息的佣金。

基类 [CommissionRule](xref:StockSharp.Algo.Commissions.CommissionRule) 包含辅助方法 `GetValue(price, volume)`：

- 对于**绝对值**，直接返回 `Value`。
- 对于**百分比值**，计算 `(price * volume * Value) / 100`。

## 规则类型

### 订单规则

| 类 | 说明 |
|-------|-------------|
| [CommissionOrderRule](xref:StockSharp.Algo.Commissions.CommissionOrderRule) | 每笔订单的佣金（基于订单价格和数量）。 |
| [CommissionOrderVolumeRule](xref:StockSharp.Algo.Commissions.CommissionOrderVolumeRule) | 按订单数量计算佣金。对于绝对值：`Value * volume`。 |
| [CommissionOrderCountRule](xref:StockSharp.Algo.Commissions.CommissionOrderCountRule) | 每 N 个订单收取一次佣金。`Count` 属性用于设置阈值。 |

### 成交规则

| 类 | 说明 |
|-------|-------------|
| [CommissionTradeRule](xref:StockSharp.Algo.Commissions.CommissionTradeRule) | 每笔成交的佣金（根据成交价格和数量计算）。 |
| [CommissionTradeVolumeRule](xref:StockSharp.Algo.Commissions.CommissionTradeVolumeRule) | 按成交数量计算佣金。 |
| [CommissionTradePriceRule](xref:StockSharp.Algo.Commissions.CommissionTradePriceRule) | 佣金计算公式：`price * volume * Value`。 |
| [CommissionTradeCountRule](xref:StockSharp.Algo.Commissions.CommissionTradeCountRule) | 每 N 笔成交收取一次佣金。`Count` 属性用于设置阈值。 |
| [CommissionTurnOverRule](xref:StockSharp.Algo.Commissions.CommissionTurnOverRule) | 每达到一个成交额阈值收取佣金。`TurnOver` 属性用于设置阈值。 |

### 过滤规则

| 类 | 说明 |
|-------|-------------|
| [CommissionSecurityIdRule](xref:StockSharp.Algo.Commissions.CommissionSecurityIdRule) | 仅对特定证券收取佣金，由 `Security` 属性指定。 |
| [CommissionBoardCodeRule](xref:StockSharp.Algo.Commissions.CommissionBoardCodeRule) | 仅对特定交易板收取佣金，由 `Board` 属性指定。 |
| [CommissionSecurityTypeRule](xref:StockSharp.Algo.Commissions.CommissionSecurityTypeRule) | 仅对特定证券类型收取佣金，由 `SecurityType` 属性指定。 |

## 通过适配器进行集成

[CommissionMessageAdapter](xref:StockSharp.Algo.Commissions.CommissionMessageAdapter) 类封装内部适配器，并自动计算传入和传出 [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage) 的佣金。如果消息尚未设置 `Commission` 字段，适配器会使用管理器的计算结果填充该字段。

## 与策略集成

[Strategy](xref:StockSharp.Algo.Strategies.Strategy) 提供 `Commission` 属性，可通过该属性跟踪累计佣金。

## 使用示例

```cs
var manager = new CommissionManager();

// Fixed commission of 1.5 per trade
manager.Rules.Add(new CommissionTradeRule { Value = 1.5m });

// 0.1% of turnover for futures
manager.Rules.Add(new CommissionSecurityTypeRule
{
    SecurityType = SecurityTypes.Future,
    Value = new Unit(0.1m, UnitTypes.Percent)
});

// Commission of 50 for every 100 orders
manager.Rules.Add(new CommissionOrderCountRule
{
    Count = 100,
    Value = 50m
});

// Commission of 10 for every 1,000,000 in turnover
manager.Rules.Add(new CommissionTurnOverRule
{
    TurnOver = 1_000_000m,
    Value = 10m
});

// Processing a message
decimal? commission = manager.Process(executionMsg);
if (commission != null)
{
    Console.WriteLine($"Commission for message: {commission.Value}");
}

// Total accumulated commission
Console.WriteLine($"Total commission: {manager.Commission}");
```

## 重置状态

`Reset()` 方法会将佣金总额重置为零，并对每条规则调用 `Reset()`，从而清除内部计数器（订单数量、当前成交额等）：

```cs
manager.Reset();
```
