# Arbitrage Strategy

## Overview

`ArbitrageStrategy` is an arbitrage strategy between a futures contract and its underlying asset. It monitors spreads between instruments and opens positions when arbitrage opportunities arise.

## Main Components and Properties

// Main components and properties
public class ArbitrageStrategy : Strategy
{
    public Security FutureSecurity { get; set; }
    public Security StockSecurity { get; set; }

    public Portfolio FuturePortfolio { get; set; }
    public Portfolio StockPortfolio { get; set; }

    public decimal StockMultiplicator { get; set; }

    public decimal FutureVolume { get; set; }
    public decimal StockVolume { get; set; }

    public decimal ProfitToExit { get; set; }

    public decimal SpreadToGenerateSignal { get; set; }
}

## Methods

### OnStarted

Called when the strategy starts:

- Initializes instrument identifiers
- Subscribes to order book data for both instruments

// OnStarted method
protected override void OnStarted(DateTimeOffset time)
{
    _futId = FutureSecurity.ToSecurityId();
    _stockId = StockSecurity.ToSecurityId();

    var subFut = this.SubscribeMarketDepth(FutureSecurity);
    var subStock = this.SubscribeMarketDepth(StockSecurity);

    subFut.WhenOrderBookReceived(this).Do(ProcessMarketDepth).Apply(this);
    subStock.WhenOrderBookReceived(this).Do(ProcessMarketDepth).Apply(this);
    base.OnStarted(time);
}

### ProcessMarketDepth

Main method for processing order book data:

- Updates the latest order book data
- Calculates average prices
- Determines the type of arbitrage situation (contango or backwardation)
- Makes decisions about opening or closing positions

// ProcessMarketDepth method
private void ProcessMarketDepth(IOrderBookMessage depth)
{
    // ProcessMarketDepth method code
}

### GenerateOrdersBackwardation and GenerateOrdersContango

Methods for generating orders during backwardation and contango:

// GenerateOrdersBackwardation and GenerateOrdersContango methods
private (Order buy, Order sell) GenerateOrdersBackwardation()
{
    // GenerateOrdersBackwardation method code
}

private (Order sell, Order buy) GenerateOrdersContango()
{
    // GenerateOrdersContango method code
}

### GetAveragePrice

Helper method for calculating the average price from the order book:

// GetAveragePrice method
private static decimal GetAveragePrice(IOrderBookMessage depth, Sides orderDirection, decimal volume)
{
    // GetAveragePrice method code
}

## Working Logic

- The strategy monitors spreads between the futures and the underlying asset
- When the spread exceeds a specified threshold, an arbitrage position is opened
- The position is closed when a specified profit level is reached
- Market orders are used for quick execution

## Features

- Supports working with two instruments and two portfolios
- Takes into account the multiplier for the underlying asset
- Uses rules (IMarketRule) to handle order execution events
- Logs information about the current state and spreads