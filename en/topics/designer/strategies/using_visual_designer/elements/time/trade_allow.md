# Is Trading Allowed

![Designer TradeAllowedDiagramElement 00](../../../../../../images/designer_tradealloweddiagramelement_00.png)

This block is used to check if trading is currently allowed. The following conditions are checked:

- All strategy subscriptions to market data must be in [Online](../../../../../api/market_data/subscriptions.md) state (receiving real-time data).
- All indicators must be [formed](../../../../../api/indicators.md).
- In the case of [live trading](../../../../live_execution/getting_started.md), the incoming trigger value must have a timestamp greater than the strategy's start time.

### Incoming Sockets


- **Trigger** - the signal that determines the moment when the check should be performed.

### Outgoing Sockets


- **Flag** - a flag that determines whether the trading session is active.

## See Also

[Current Time](current_time.md)