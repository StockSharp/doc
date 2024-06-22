# Order Rule Strategy

## Overview

`SimpleOrderRulesStrategy` is a strategy that demonstrates the use of rules for processing order-related events in StockSharp. It subscribes to trades and creates rules for handling order registration.

## Main Components

// Main components
public class SimpleOrderRulesStrategy : Strategy
{
}

## OnStarted Method

Called when the strategy starts:

- Subscribes to trades
- Creates two sets of rules for processing order registration events

// OnStarted method
protected override void OnStarted(DateTimeOffset time)
{
    var sub = this.SubscribeTrades(Security);

    sub.WhenTickTradeReceived(this).Do(() =>
    {
        var order = this.BuyAtMarket(1);
        var ruleReg = order.WhenRegistered(this);
        var ruleRegFailed = order.WhenRegisterFailed(this);

        ruleReg
            .Do(() => this.AddInfoLog("Order №1 Registered"))
            .Once()
            .Apply(this)
            .Exclusive(ruleRegFailed);

        ruleRegFailed
            .Do(() => this.AddInfoLog("Order №1 RegisterFailed"))
            .Once()
            .Apply(this)
            .Exclusive(ruleReg);

        RegisterOrder(order);
    }).Once().Apply(this);

    sub.WhenTickTradeReceived(this).Do(() =>
    {
        var order = this.BuyAtMarket(10000000);
        var ruleReg = order.WhenRegistered(this);
        var ruleRegFailed = order.WhenRegisterFailed(this);

        ruleReg
            .Do(() => this.AddInfoLog("Order №2 Registered"))
            .Once()
            .Apply(this)
            .Exclusive(ruleRegFailed);

        ruleRegFailed
            .Do(() => this.AddInfoLog("Order №2 RegisterFailed"))
            .Once()
            .Apply(this)
            .Exclusive(ruleReg);

        RegisterOrder(order);
    }).Once().Apply(this);

    base.OnStarted(time);
}

## Working Logic

### First Set of Rules

- Creates a market buy order for 1 unit when a tick is received
- Sets up rules for handling successful registration and registration failure
- The rules are mutually exclusive and trigger only once

### Second Set of Rules

- Creates a market buy order for 10,000,000 units when the next tick is received
- Similarly sets up rules for handling successful registration and registration failure
- The rules are also mutually exclusive and trigger only once

## Features

- Demonstrates the creation of rules for handling order registration events
- Uses the mechanism of mutually exclusive rules (`Exclusive`)
- Shows an example of logging information about order events
- Illustrates the use of `Once()` to limit rule triggering
- Creates orders with different volumes to demonstrate various scenarios (successful registration and registration failure)