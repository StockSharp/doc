# S\#.API

**S\#.API** \- a free library for beginners and professionals in the field of algorithmic trading. S\#.API is oriented to program on [C\#](https://en.wikipedia.org/wiki/C_Sharp_(programming_language)) in the Visual Studio environment, allowing to create absolutely any strategies: from positional strategies with a long timeframe to the high\-frequency strategies (HFT) using direct access (DMA) to stock trading. 

S\#.API is the basis of all our products. On the basis of the library, solutions such as [S\#.Designer](Designer.md), [S\#.Data](Hydra.md) etc., as well as our [S\#.MatLab](MatLab.md) adapter are created. 

S\#.API uses the [Messages](Messages.md) mechanism, which makes it possible to unify the development of adapters, and also allows you to create your [Adapters](Messages_adapters.md) to any external trading system. 

## The benefits and capabilities of the library:

1. **Portability** \- the algorithm does not depend on a broker or exchange API and can work with any connection. For example, it is easy to move from [OpenECry](OEC.md) to [Interactive Brokers](IB.md), or from **Forex** to the stock exchange. Details are in the [Architecture S\#.API](StockSharpArchitecture.md) section. 
2. **Supports many sources:**[Connectors](API_Connectors.md).
3. **Versatility** \- focused on private algorithmic traders, small teams, investment companies, banks. 
4. **Performance** \- the simultaneous execution of hundreds of strategies on any instruments. 
5. **Speed** \- the orders processing in the S\#.API takes no more than a few microseconds. 
6. **Direct connection** – trading through a direct connection to exchanges, as well as support for the [FIX](Fix.md) protocol. 
7. **Realistic backtesting** \- the most accurate [Backtesting\/Emulation](StrategyTesting.md) on ticks and order books, determination of the actual slippage. 
8. **Popularity** \- widely used [C\#](https://en.wikipedia.org/wiki/C_Sharp_(programming_language)) language, popular programming environment Visual Studio. 

## Recommended content

[Architecture S\#.API](StockSharpArchitecture.md)
