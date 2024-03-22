# Working Time

![Designer Working time 00](../images/Designer_Working_time_00.png)

This block is used to determine the working time for the strategy. For example, to define when trading is taking place for a specific instrument or when the strategy is allowed to trade.
#### Incoming Sockets

- **Any Data** - the block accepts any value but takes the timestamp from it, which is then compared to the block's parameters.
#### Outgoing Sockets

- **Flag** - a flag that determines whether the timestamp meets the block's parameters (true) or not (false).
#### Parameters

- **Time From** - the start time of the working time.
- **Time To** - the end time of the working time.

The block can be used to determine when trading is conducted for multiple instruments from different trading platforms.

![Designer Working time 01](../images/Designer_Working_time_01.png)

## See Also

[Is Trading Allowed](Designer_TradeAllowedDiagramElement.html)