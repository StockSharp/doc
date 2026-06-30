# 成交量连续期货（VolumeContinuousSecurity）

## 概览

这`VolumeContinuousSecurity`该类表示一种连续期货合约，其中合约之间的转换（展期）是基于交易量或未平仓合约数进行的。这不同于`ExpirationContinuousSecurity`，其中切换根据预定的到期日期进行。

两个类都继承自`ContinuousSecurity`，而它又继承自`BasketSecurity`.

## 与 ExpirationContinuousSecurity 的差异

| 特征 | 持续到期安全性 | 持续量安全性 |
|---|---|---|
| 展期条件 | 到期日（固定） | 成交量或未平仓合约数量阈值 |
| 配置 | `SecurityId -> DateTime` 字典 | `SecurityId` 列表 + `VolumeLevel` |
| 可预测性 | 按计划切换 | 按市场条件切换 |
| 篮子代码 | `CE` | `CV` |

`ExpirationContinuousSecurity` 需要手动为每个合约指定转换日期。`VolumeContinuousSecurity` 当其交易量（或持仓量）超过指定阈值时，会自动切换到下一个合约。

## 主要属性

```csharp
public class VolumeContinuousSecurity : ContinuousSecurity
{
    // List of inner securities (contracts), ordered by rollover sequence
    public SynchronizedList<SecurityId> InnerSecurities { get; }

    // Use open interest instead of volume for rollover determination
    public bool IsOpenInterest { get; set; }

    // Volume threshold at which switching to the next contract occurs
    public Unit VolumeLevel { get; set; }
}
```

`VolumeLevel` 属性具有 `Unit` 类型，它允许指定绝对值和百分比值。

## 使用示例

```csharp
using StockSharp.Algo;
using StockSharp.Messages;

// Create a volume-based continuous futures
var continuous = new VolumeContinuousSecurity
{
    Id = "ES-CONT@CME",
    Board = ExchangeBoard.Cme,
};

// Add contracts in rollover order
continuous.InnerSecurities.AddRange(new[]
{
    "ES-3.26@CME".ToSecurityId(),
    "ES-6.26@CME".ToSecurityId(),
    "ES-9.26@CME".ToSecurityId(),
});

// Set volume threshold for switching
continuous.VolumeLevel = new Unit(10000);

// Or use open interest
continuous.IsOpenInterest = true;
continuous.VolumeLevel = new Unit(50000);
```

## 用于比较的到期连续证券示例

```csharp
using StockSharp.Algo;
using StockSharp.Messages;

// Expiration-based continuous futures
var expContinuous = new ExpirationContinuousSecurity
{
    Id = "ES-CONT-EXP@CME",
    Board = ExchangeBoard.Cme,
};

// Specify exact transition dates for each contract
expContinuous.ExpirationJumps.Add(
    "ES-3.26@CME".ToSecurityId(),
    new DateTime(2026, 3, 15)
);
expContinuous.ExpirationJumps.Add(
    "ES-6.26@CME".ToSecurityId(),
    new DateTime(2026, 6, 15)
);
```

## 何时使用

`VolumeContinuousSecurity` 适用于以下情况：

- 确切的结转日期事先未知
- 需要根据流动性（交易量或未平仓合约）进行切换
- 需要一种能够根据市场条件做出反应的更具适应性的过渡

当已知到期日期并且需要确定性回转时，优先使用 `ExpirationContinuousSecurity`。
