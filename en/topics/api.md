# API

**API** \- a free library for beginners and professionals in the field of algorithmic trading. API is oriented to program on [C\#](https://en.wikipedia.org/wiki/C_Sharp_(programming_language)) in the Visual Studio environment, allowing to create absolutely any strategies: from positional strategies with a long timeframe to the high\-frequency strategies (HFT) using direct access (DMA) to stock trading. 

API is the basis of all our products. On the basis of the library, solutions such as [Designer](designer.md), [Hydra](hydra.md) etc., as well as our [MatLab](matlab.md) adapter are created. 

API uses the [Messages](api/messages.md) mechanism, which makes it possible to unify the development of adapters, and also allows you to create your [Adapters](api/messages/adapters.md) to any external trading system. 

## The benefits and capabilities of the library:

1. **Portability** \- the algorithm does not depend on a broker or exchange API and can work with any connection. For example, it is easy to move from [OpenECry](api/connectors/stock_market/openecry.md) to [Interactive Brokers](api/connectors/stock_market/interactive_brokers.md), or from **Forex** to the stock exchange.
2. **Supports many sources:**[Connectors](api/connectors.md).
3. **Versatility** \- focused on private algorithmic traders, small teams, investment companies, banks. 
4. **Performance** \- the simultaneous execution of hundreds of strategies on any instruments. 
5. **Speed** \- the orders processing in the API takes no more than a few microseconds. 
6. **Direct connection** – trading through a direct connection to exchanges, as well as support for the [FIX](api/connectors/common/fix_protocol.md) protocol. 
7. **Realistic backtesting** \- the most accurate [Backtesting\/Emulation](api/testing.md) on ticks and order books, determination of the actual slippage. 
8. **Popularity** \- widely used [C\#](https://en.wikipedia.org/wiki/C_Sharp_(programming_language)) language, popular programming environment Visual Studio. 

## Recommended content

[Setup API](api/setup.md)
