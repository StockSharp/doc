# Backtesting\/Emulation

Strategies written by using [Strategy](xref:StockSharp.Algo.Strategies.Strategy) can be tested in three modes: 

1. [Historical data](StrategyTestingHistory.md)

    testing. With this kind of data both a market analysis to find patterns and the strategy parameters optimization may be carried out. 
2. [Random data](StrategyTestingEmulation.md)

    testing. A convenient tool for initial strategy testing to identify errors in the algorithms. Or for automated tests that run on a schedule. 
3. [Simulator](StrategyTestingRealTime.md)

    testing based on data received from a real connection to the trading system (for example, from 

   [OpenECry](OEC.md)

   ), but without the actual orders registration (execution is emulated based on incoming order books). 

When using all three modes the maximum emphasis is focused on the fact that the strategy code, written by using [Strategy](xref:StockSharp.Algo.Strategies.Strategy) is not changed when switching from real trading to testing and back. This is achieved through the implementation of the [IConnector](xref:StockSharp.BusinessEntities.IConnector) main interface, which is a gateway to the trading system. How the interface is used – this has been shown already in the [StockSharp Architecture](StockSharpArchitecture.md) section. In testing mode not real trading system, but the emulation will act as the trading system (depending on the selected mode). Therefore, the strategy code will never know about it \- whether it is trading with a real exchange, or it is the emulation. 

## Recommended content
