# Order Book

## Description

The order book (a.k.a. market depth) is information about the current buy and sell orders for a specific security, organized by price levels. In StockSharp, the order book provides data on demand and supply, allowing for real-time market analysis.

## Structure

The [Order Book](xref:StockSharp.Messages.IOrderBookMessage) contains two lists of orders:

- Buy orders, sorted by descending price - [Bids](xref:StockSharp.Messages.IOrderBookMessage.Bids).
- Sell orders, sorted by ascending price - [Asks](xref:StockSharp.Messages.IOrderBookMessage.Asks).

Each order includes a price and volume.

## Usage

Order book data is used for:

- Identifying price levels with maximum order volumes, which may indicate potential support or resistance levels.
- Assessing market liquidity for a security.
- Developing trading strategies based on analysis of changes in the order book.

## Data Retrieval

In StockSharp, subscribing to order book data and receiving updates is done through the corresponding [API methods](OrderBook_Subscription.md).