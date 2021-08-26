# Strategy warm\-up

![Designer Warm up mode 00](~/images/Designer_Warm_up_mode_00.png)

When switching from the strategy design mode to the trading mode, all strategies have a warm\-up mode, as reported by the line above the strategy diagram. In warm\-up mode, order registering is disabled. For example, if you start the strategy in warm\-up mode, then when the signal arrives, the order will not be registered. This is done so that the strategy completely initializes its state without trading on historical data. To enable or disable the warm\-up mode, click the **Trade** button. The warm\-up mode can be set and disconnected from the strategy at any time, no matter whether the strategy is started to trade or not. 

It is recommended that you run the strategy in the following order:

1. Start a strategy in trading in warm\-up mode
2. Wait until all historical data are received, if required for the strategy
3. Select the appropriate time to exit the warm\-up mode
4. Exit warm\-up mode

## Recommended content

[Connections settings](Designer_Connection_settings.md)
