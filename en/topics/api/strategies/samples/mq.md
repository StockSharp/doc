# Quoting Strategy

## Overview

`MqStrategy` is a strategy that uses quoting to manage market position. It creates a child quoting strategy depending on the current position.

## Main Components

```cs
// Main components
public class MqStrategy : Strategy
{
   private MarketQuotingStrategy _strategy;
}
```

## Methods

### OnStarted

Called when the strategy starts:

- Subscribes to market time changes
- Initializes the initial creation of the quoting strategy

```cs
// OnStarted method
protected override void OnStarted(DateTimeOffset time)
{
   Connector.MarketTimeChanged += Connector_MarketTimeChanged;
   Connector_MarketTimeChanged(default);

   base.OnStarted(time);
}
```

### Connector_MarketTimeChanged

Method called when market time changes:

- Checks the state of the existing quoting strategy
- Creates a new quoting strategy depending on the current position

```cs
// Connector_MarketTimeChanged method
private void Connector_MarketTimeChanged(TimeSpan obj)
{
   if (_strategy != null && _strategy.ProcessState != ProcessStates.Stopped) return;

   if (Position <= 0)
   {
       _strategy = new MarketQuotingStrategy(Sides.Buy, Volume + Math.Abs(Position))
       {
           Name = "buy " + CurrentTime,
           Volume = 1,
           PriceType = MarketPriceTypes.Following,
       };
       ChildStrategies.Add(_strategy);
   }
   else if (Position > 0)
   {
       _strategy = new MarketQuotingStrategy(Sides.Sell, Volume + Math.Abs(Position))
       {
           Name = "sell " + CurrentTime,
           Volume = 1,
           PriceType = MarketPriceTypes.Following,
       };
       ChildStrategies.Add(_strategy);
   }
}
```

## Working Logic

- The strategy responds to changes in market time
- With each time change, if the previous quoting strategy is stopped, a new strategy is created
- The quoting direction depends on the current position:
 - If the position <= 0, a buy strategy is created
 - If the position > 0, a sell strategy is created
- The quoting volume takes into account the current position

## Features

- Uses [MarketQuotingStrategy](xref:StockSharp.Algo.Strategies.Quoting.MarketQuotingStrategy) to create quotes
- Adapts to the current position by changing the quoting direction
- Responds to market time changes to update the strategy
- Uses market quoting (Following) to determine the price
- Uses the current time to name strategies, which helps in their identification