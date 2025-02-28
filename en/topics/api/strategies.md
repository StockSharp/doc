# Strategies in StockSharp

## Introduction

StockSharp provides a powerful infrastructure for creating, testing, and running trading strategies. The foundation for developing algorithmic trading strategies is the base class [Strategy](xref:StockSharp.Algo.Strategies.Strategy), which provides a set of standard functions and abstractions for working with market data, executing trading operations, and analyzing results.

## Navigation

### Strategy Basics

- [Market Data Subscriptions in Strategies](strategies/subscriptions.md) - Detailed guide on using market data subscriptions in strategies. Explains creating and configuring subscriptions, managing their lifecycle, and monitoring their state.

- [Indicators in Strategies](strategies/indicators.md) - Information about working with technical analysis indicators in strategies. Covers adding indicators to a strategy, controlling their formation, and using them in trading logic.

- [Trading Operations in Strategies](strategies/trading_operations.md) - Guide to performing trading operations in strategies. Describes methods for creating and sending orders, closing positions, and monitoring their status.

- [Position Protection](strategies/take_profit_and_stop_loss.md) - Description of mechanisms for protecting open positions using Take Profit and Stop Loss. Examines local and server approaches to position protection.

- [Strategy Parameters](strategies/parameters.md) - Guide to working with strategy parameters through [StrategyParam\<T\>](xref:StockSharp.Algo.Strategies.StrategyParam`1). Describes how to create configurable parameters, set their display in GUI, and use them in optimization.

- [Logging in Strategies](strategies/logging.md) - Guide to using the logging mechanism in strategies for tracking and debugging algorithm performance.

### Advanced Features

- [Strategy Platform Compatibility](strategies/compatibility.md) - Recommendations for creating strategies compatible with various StockSharp platforms: [Designer](../designer.md), [Shell](../shell.md), [Runner](../runner.md), and cloud testing.

- [High-Level APIs in Strategies](strategies/high_level_api.md) - Description of high-level methods to simplify working with subscriptions, indicators, charts, and position protection. Explains how to write cleaner code by focusing on trading logic.

- [Working with Charts in Strategies](strategies/chart.md) - Guide to visualizing strategy data on a chart. Explains how to access the chart, create areas, add elements, and render data.

- [Saving and Loading Settings](strategies/settings_saving_and_loading.md) - Description of the mechanism for saving and loading strategy settings through [Strategy.Save](xref:StockSharp.Algo.Strategies.Strategy.Save(Ecng.Serialization.SettingsStorage)) and [Strategy.Load](xref:StockSharp.Algo.Strategies.Strategy.Load(Ecng.Serialization.SettingsStorage)) methods.

- [State Loading](strategies/orders_and_trades_loading.md) - Guide to loading previously executed orders and trades into a strategy, for example, when restarting a strategy during a trading session.

- [Price Rounding](strategies/shrink_price.md) - Guide to correctly rounding prices in strategies using the [ShrinkPrice](xref:StockSharp.BusinessEntities.EntitiesExtensions.ShrinkPrice(StockSharp.BusinessEntities.Security,System.Decimal)) method.

- [Unit Type](strategies/unit_type.md) - Description of the [Unit](xref:StockSharp.Messages.Unit) data type to simplify arithmetic operations on quantities like percentages, points, or pips.

- [Event Model](strategies/event_model.md) - Explanation of the strategy event model based on [IMarketRule](xref:StockSharp.Algo.IMarketRule). Covers creating rules to react to market events, combining conditions, and managing rule lifecycles.

## Getting Started with Strategy Development

To begin developing your own strategy, it is recommended to:

1. Familiarize yourself with the basics of working with strategies to understand the general principles of strategies in StockSharp.

2. Study the [Market Data Subscriptions in Strategies](strategies/subscriptions.md) section to understand the mechanism of receiving and processing market data.

3. Review the [Indicators in Strategies](strategies/indicators.md) section to understand working with technical analysis indicators.

4. Explore the [Trading Operations in Strategies](strategies/trading_operations.md) section to understand trading operation mechanisms.

5. Examine the [Strategy Parameters](strategies/parameters.md) section to learn about strategy configuration mechanisms.

6. Get acquainted with the [High-Level APIs in Strategies](strategies/high_level_api.md) section to simplify strategy code using built-in high-level functions.

## Strategy Testing

StockSharp provides various methods for testing strategies:

- **Historical Data Testing** - Allows evaluating strategy effectiveness on historical data.
- **Parameter Optimization** - Helps find optimal strategy parameter values.
- **Virtual Account Testing** - Allows checking strategy performance in real-time mode without risking real funds.

Detailed descriptions of testing methods and strategy performance evaluation can be found in the [Testing](../api/testing.md) section.
