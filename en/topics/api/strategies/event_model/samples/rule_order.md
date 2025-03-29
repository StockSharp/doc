# Rule for Orders

## Overview

`SimpleOrderRulesStrategy` is a strategy that demonstrates the use of rules for processing events related to orders in StockSharp. It subscribes to trades and creates rules for processing order registration events.

## Main Components

```cs
// Main components
public class SimpleOrderRulesStrategy : Strategy
{
}
```

## OnStarted Method

Called when the strategy starts:

- Creates a subscription to ticks
- Creates two sets of rules for processing order registration events

```cs
// OnStarted method
protected override void OnStarted(DateTimeOffset time)
{
    var sub = new Subscription(DataType.Ticks, Security);

    sub.WhenTickTradeReceived(this).Do(() =>
    {
        var order = CreateOrder(Sides.Buy, default, 1);

        var ruleReg = order.WhenRegistered(this);
        var ruleRegFailed = order.WhenRegisterFailed(this);

        ruleReg
            .Do(() => LogInfo("Order №1 Registered"))
            .Once()
            .Apply(this)
            .Exclusive(ruleRegFailed);

        ruleRegFailed
            .Do(() => LogInfo("Order №1 RegisterFailed"))
            .Once()
            .Apply(this)
            .Exclusive(ruleReg);

        RegisterOrder(order);
    }).Once().Apply(this);

    sub.WhenTickTradeReceived(this).Do(() =>
    {
        var order = CreateOrder(Sides.Buy, default, 10000000);

        var ruleReg = order.WhenRegistered(this);
        var ruleRegFailed = order.WhenRegisterFailed(this);

        ruleReg
            .Do(() => LogInfo("Order №2 Registered"))
            .Once()
            .Apply(this)
            .Exclusive(ruleRegFailed);

        ruleRegFailed
            .Do(() => LogInfo("Order №2 RegisterFailed"))
            .Once()
            .Apply(this)
            .Exclusive(ruleReg);

        RegisterOrder(order);
    }).Once().Apply(this);

    // Sending request for subscribe to market data.
    Subscribe(sub);

    base.OnStarted(time);
}
```

## Logic

### First Set of Rules

- When a tick is received, creates an order to buy 1 unit
- The order is created using the `CreateOrder` method, specifying direction, price (default = market), and volume
- Sets rules for processing successful registration and registration errors
- The rules are mutually exclusive and trigger only once

### Second Set of Rules

- When the next tick is received, creates an order to buy 10,000,000 units
- Similarly sets rules for processing successful registration and registration errors
- The rules are also mutually exclusive and trigger only once

## Features

- Demonstrates creating rules for processing order registration events
- Uses the mechanism of mutually exclusive rules (`Exclusive`)
- Shows an example of logging information about order events using the `LogInfo` method
- Illustrates the use of `Once()` to limit rule triggering
- Creates orders with different volumes to demonstrate various scenarios (successful registration and registration error)