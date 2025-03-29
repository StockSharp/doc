# Instruments

In StockSharp, financial instruments are represented by the [Security](xref:StockSharp.BusinessEntities.Security) class, which is a fundamental element for working with trading data. This section covers the main aspects of working with financial instruments within the platform.

## Base Security Class

[Security](xref:StockSharp.BusinessEntities.Security) represents a financial instrument traded on an exchange. An instrument can be a stock, futures contract, option, currency pair, cryptocurrency, and other assets. The class contains all the necessary information for identifying and trading the instrument:

- **Identification information** - code, ISIN, name, instrument class
- **Trading parameters** - price step, lot size, minimum volume
- **Market data** - current values of prices, volumes, order books, etc.
- **Calculated values** - parameters for derivatives, risk calculation, etc.

## Types of Instruments

StockSharp supports working with all major types of financial instruments:

- **Stocks** - equity securities
- **Bonds** - debt securities
- **Futures** - derivative contracts on an underlying asset
- **Options** - contracts giving the right (but not the obligation) to buy or sell an underlying asset
- **Currency pairs** - instruments for trading on the forex market
- **Cryptocurrencies** - digital assets for trading on crypto exchanges
- **ETFs** - exchange-traded funds
- **Indices** - calculated indicators of the state of a market or sector

## Instrument Baskets

In addition to regular instruments, StockSharp implements special classes for working with groups of instruments:

- [IndexSecurity](xref:StockSharp.Algo.IndexSecurity) - an instrument representing an index based on underlying instruments
- [WeightedIndexSecurity](xref:StockSharp.Algo.WeightedIndexSecurity) - an index with weight coefficients for each instrument
- [ContinuousSecurity](xref:StockSharp.Algo.ContinuousSecurity) - a continuous instrument for working with a series of futures contracts

These classes allow you to create composite instruments and work with them in the same way as with regular instruments, receiving aggregated market data, calculating statistics, and executing trading operations.

## Working with Instrument Information

StockSharp provides powerful tools for working with financial instrument information:

- **Instrument search** - by various criteria (code, name, class)
- **Filtering** - selection of instruments according to specified parameters
- **Storage** - saving instrument information to local or remote storage
- **Retrieving exchange information** - loading detailed information from the exchange

## Instrument Identification

Each instrument in StockSharp has a unique identifier [SecurityId](xref:StockSharp.Messages.SecurityId), which is used to unambiguously identify the instrument in the system. The identifier includes:

- **SecurityCode** - exchange code of the instrument
- **BoardCode** - trading venue code
- **Bloomberg/Reuters/ISIN** and other codes - alternative identification methods

## Special Features

- **Continuous futures** - automatic "splicing" of historical data for a series of futures contracts
- **Composite instruments** - creation of virtual instruments based on several real ones
- **Special identifier \*@ALL** - for working with all instruments of a certain class

## See also

[Instrument Identifier](instruments/instrument_identifier.md)

[Identifier \*@ALL](instruments/identifier_@all.md)

[Continuous Futures](instruments/continuous_futures.md)

[Index](instruments/index.md)

[Instrument Search](instruments/instrument_search.md)