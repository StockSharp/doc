# Volume Continuous Futures (VolumeContinuousSecurity)

## Overview

The `VolumeContinuousSecurity` class represents a continuous futures contract where the transition (rollover) between contracts occurs based on trading volume or open interest. This differs from `ExpirationContinuousSecurity`, where switching is performed according to predefined expiration dates.

Both classes inherit from `ContinuousSecurity`, which in turn inherits from `BasketSecurity`.

## Difference from ExpirationContinuousSecurity

| Characteristic | ExpirationContinuousSecurity | VolumeContinuousSecurity |
|---|---|---|
| Rollover condition | Expiration date (fixed) | Volume or open interest threshold |
| Configuration | `SecurityId -> DateTime` dictionary | `SecurityId` list + `VolumeLevel` |
| Predictability | Switching by schedule | Switching by market conditions |
| Basket code | `CE` | `CV` |

`ExpirationContinuousSecurity` requires manually specifying transition dates for each contract. `VolumeContinuousSecurity` automatically switches to the next contract when its trading volume (or open interest) exceeds the specified threshold.

## Main Properties

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

The `VolumeLevel` property has the `Unit` type, which allows specifying both absolute and percentage values.

## Usage Example

```csharp
using StockSharp.Algo;
using StockSharp.Messages;

// Create a volume-based continuous futures
var continuous = new VolumeContinuousSecurity
{
    Id = "RTS-CONT@FORTS",
    Board = ExchangeBoard.Forts,
};

// Add contracts in rollover order
continuous.InnerSecurities.AddRange(new[]
{
    "RTS-3.26@FORTS".ToSecurityId(),
    "RTS-6.26@FORTS".ToSecurityId(),
    "RTS-9.26@FORTS".ToSecurityId(),
});

// Set volume threshold for switching
continuous.VolumeLevel = new Unit(10000);

// Or use open interest
continuous.IsOpenInterest = true;
continuous.VolumeLevel = new Unit(50000);
```

## Example with ExpirationContinuousSecurity for Comparison

```csharp
using StockSharp.Algo;
using StockSharp.Messages;

// Expiration-based continuous futures
var expContinuous = new ExpirationContinuousSecurity
{
    Id = "RTS-CONT-EXP@FORTS",
    Board = ExchangeBoard.Forts,
};

// Specify exact transition dates for each contract
expContinuous.ExpirationJumps.Add(
    "RTS-3.26@FORTS".ToSecurityId(),
    new DateTime(2026, 3, 15)
);
expContinuous.ExpirationJumps.Add(
    "RTS-6.26@FORTS".ToSecurityId(),
    new DateTime(2026, 6, 15)
);
```

## When to Use

`VolumeContinuousSecurity` is suitable for situations where:

- Exact rollover dates are not known in advance
- Switching based on liquidity (trading volume or open interest) is required
- A more adaptive transition that responds to market conditions is needed

`ExpirationContinuousSecurity` is preferred when expiration dates are known in advance and deterministic rollover is required.
