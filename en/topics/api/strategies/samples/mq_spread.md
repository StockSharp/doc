# Spread Quoting Strategy

## Overview

`MqSpreadStrategy` is a strategy that uses quoting to create a spread in the market. It creates two child quoting strategies: one for buying and one for selling.

## Main Components

// Main components
public class MqSpreadStrategy : Strategy
{
   private MarketQuotingStrategy _strategyBuy;
   private MarketQuotingStrategy _strategySell;
}

## Methods

### OnStarted

Called when the strategy starts:

- Subscribes to market time changes
- Initializes the initial creation of quoting strategies

// OnStarted method
protected override void OnStarted(DateTimeOffset time)
{
   Connector.MarketTimeChanged += Connector_MarketTimeChanged;
   Connector_MarketTimeChanged(new TimeSpan());
   base.OnStarted(time);
}

### Connector_MarketTimeChanged

Method called when market time changes:

- Checks the current position and state of existing quoting strategies
- Creates new quoting strategies for buying and selling

// Connector_MarketTimeChanged method
private void Connector_MarketTimeChanged(TimeSpan obj)
{
   if (Position != 0) return;
   if (_strategyBuy != null && _strategyBuy.ProcessState != ProcessStates.Stopped) return;
   if (_strategySell != null && _strategySell.ProcessState != ProcessStates.Stopped) return;

   _strategyBuy = new MarketQuotingStrategy(Sides.Buy, Volume)
   {
       Name = "buy " + CurrentTime,
       Volume = 1,
       PriceType = MarketPriceTypes.Following,
       IsSupportAtomicReRegister = false
   };
   ChildStrategies.Add(_strategyBuy);

   _strategySell = new MarketQuotingStrategy(Sides.Sell, Volume)
   {
       Name = "sell " + CurrentTime,
       Volume = 1,
       PriceType = MarketPriceTypes.Following,
       IsSupportAtomicReRegister = false
   };
   ChildStrategies.Add(_strategySell);
}

## Working Logic

- The strategy responds to changes in market time
- With each time change, if there's no open position and previous quoting strategies are stopped, new strategies are created
- Two quoting strategies are created:
 1. Buy strategy
 2. Sell strategy
- Both strategies are set to quote using market prices (Following)

## Features

- Uses [MarketQuotingStrategy](xref:StockSharp.Algo.Strategies.Quoting.MarketQuotingStrategy) to create quotes
- Creates a spread in the market by simultaneously placing buy and sell orders
- Responds to market time changes to update strategies
- Does not support atomic re-registration of orders
- Uses the current time to name strategies, which helps in their identification