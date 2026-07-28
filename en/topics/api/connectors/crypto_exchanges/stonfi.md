# STON.fi

**STON.fi** connects StockSharp to the STON.fi automated market maker on TON. The adapter exposes supported market data and transaction operations through the standard StockSharp message model.

## Key capabilities

- Adapter capabilities verified in the source code: real-time data updates, Level 1 quotes, trade ticks, candle data, historical data requests, portfolio and order operations.
- Provider-specific transport, sessions, and data formats are hidden behind the standard StockSharp API.

## Typical use

Use this connector to monitor STON.fi pools, analyze executable quotes, trades, and candles, request history, and run supported swap workflows.

Available pools, data depth, transaction permissions, request limits, and service availability are controlled by STON.fi, TON services, and the connected wallet.

## See also

[Connector configuration](stonfi/configuration_stonfi.md)

[Graphical configuration](stonfi/graphical_configuration_stonfi.md)

[Adapter initialization](stonfi/adapter_initialization_stonfi.md)
