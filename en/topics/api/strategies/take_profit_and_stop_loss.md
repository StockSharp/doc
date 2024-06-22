# Position Protection

## Introduction

This modification of the SMA strategy implements a mechanism for protecting open positions using a local protective controller. This approach allows for flexible risk management and automatic closing of positions when certain conditions are met.

## Key Components of Position Protection

### Protective Controller

The strategy uses two key objects for position protection:

```cs
// Declaration of protective controllers
private readonly ProtectiveController _protectiveController = new();
private IProtectivePositionController _posController;

// This code initializes the main protective controller and creates a placeholder for
// a specific position controller. ProtectiveController manages all positions,
// while IProtectivePositionController is responsible for a specific position.
```

- `_protectiveController`: The main controller managing protection for all positions.
- `_posController`: Controller for a specific position.

### Protection Initialization

When opening a new position or modifying an existing one, the protective controller is initialized:

```cs
// Initialization of the protective controller for a new position
this.WhenNewMyTrade()
    .Do(t =>
    {
        // ... (other code)

        if (TakeValue.IsSet() || StopValue.IsSet())
        {
            _posController ??= _protectiveController.GetController(
                security.ToSecurityId(),
                portfolio.Name,
                new LocalProtectiveBehaviourFactory(security.PriceStep, security.Decimals),
                TakeValue, StopValue, true, default, default, true);
        }

        var info = _posController?.Update(t.Trade.Price, t.GetPosition());

        if (info is not null)
            ActiveProtection(info.Value);
    })
    .Apply(this);

// This code creates and initializes a protective controller for a new position
// upon receiving information about a new trade. It also updates the information
// about the position in the controller and activates protection if necessary.
```

This creates a controller for a specific position with given take-profit and stop-loss parameters.

### Updating Position Information

```cs
var info = _posController?.Update(t.Trade.Price, t.GetPosition());

if (info is not null)
   ActiveProtection(info.Value);
```

This allows the controller to track the current state of the position and adjust protective orders as necessary.

### Checking Activation Conditions for Protection

In the method processing new data (e.g., when receiving a new candle), the conditions for activating protective orders are checked:

```cs
// Checking protection activation conditions in the ProcessCandle method
var info = _posController?.TryActivate(candle.ClosePrice, CurrentTime);

if (info is not null)
    ActiveProtection(info.Value);

// This code checks if a protective order needs to be activated based on
// the current price (in this case, the candle's closing price) and time.
// If the conditions are met, the ActiveProtection method is called.
```

Here, the candle's closing price is used as the current price, but it can be any relevant price value (e.g., the price of the last trade or the current spread in the order book).

### Activating a Protective Order

If the conditions for activating a protective order are met, the corresponding logic is triggered:

```cs
// Method for activating a protective order
private void ActiveProtection((bool isTake, Sides side, decimal price, decimal volume, OrderCondition condition) info)
{
    // sending a protective (position-closing) order as a regular order
    RegisterOrder(this.CreateOrder(info.side, info.price, info.volume));
}

// This method creates and registers an order to close the position
// based on the information received from the protective controller.
```

This method creates and registers an order to close the position according to the parameters returned by the protective controller.

## Comparison with Server-Side Stop Orders

### Advantages of Server-Side Stop Orders

1. Stop orders (stop loss and take profit) are sent directly to the broker.
2. The broker independently monitors the achievement of stop conditions.
3. When a stop is triggered, the broker automatically places a market or limit order.

### Advantages of the Local Approach

1. **Flexibility**: Ability to implement complex protection logic unavailable in standard server-side stops.
2. **Confidentiality**: Information about stop levels is not transmitted to the broker, which can be important in some markets.
3. **Reaction Speed**: Potentially faster reaction to changing market conditions.
4. **Adaptability**: Ability to dynamically adjust protection levels based on market data or strategy logic.
5. **Independence from broker/exchange implementation**: The local approach works the same regardless of whether the broker or exchange supports all necessary types of protective orders.
6. **Testing on historical data**: Ability to fully test the strategy with position protection on historical data, which is impossible with server-side stops.

### Disadvantages of the Local Approach

1. **Dependence on trading terminal functionality**: If the terminal is disconnected, the protection will not work.
2. **System load**: Requires constant calculations on the client side.
3. **Delays**: Possible delays in placing an order after protection conditions are triggered.

### Disadvantages of Server-Side Stop Orders

1. **Dependence on broker/exchange implementation**: Not all brokers or exchanges support all types of protective orders, which may limit strategy functionality.
2. **Inability to fully test on historical data**: Server-side stops cannot be accurately modeled when testing on historical data, making it difficult to assess the real effectiveness of the strategy.
3. **Limited flexibility**: Usually only basic types of stop orders are available, limiting the possibilities for implementing complex protective mechanisms.

## Conclusion

Using a local protective controller in the SMA strategy allows for effective risk management of open positions. This approach provides flexibility in setting protection parameters and quick reaction to changes in market situations, which is critical for successful trading.