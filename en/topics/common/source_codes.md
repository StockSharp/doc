# Source codes

The open-source [S#](../api.md) code is split across several repositories. The [StockSharp core repository](https://github.com/StockSharp/StockSharp) contains the message model, business entities, shared connector abstractions, algorithms, testing tools, and other platform foundations. Provider-specific connector implementations are not stored in the core repository.

All open provider-specific connectors are maintained in [StockSharp\/Connectors](https://github.com/StockSharp/Connectors). Each connector is an independent .NET project, and the repository includes `Connectors.slnx` for building them together.

The standalone browser chart engine and web-terminal chart stack are maintained in [StockSharp\/JS-Charts](https://github.com/StockSharp/JS-Charts). See [JavaScript charts](../api/graphical_user_interface/charts/javascript_charts.md).

[Instructions for using GitHub](https://docs.github.com/en/get-started/start-your-journey/hello-world)

List of components available with source code:

- Common classes for creating your own connections.
- Format of the market data storage.
- Trading simulator.
- History simulator (backtester).
- Indicators (more than 140) of technical analysis.
- Algorithms for calculating profit-loss, slippage, delay.
- Algorithms for building candles of any time frame, as well as non-time-based candles (tick, range, etc.).
- Logging.
- Import and export.

The source codes of all closed components, as well as ready-made programs, are available upon purchase. For more information about the cost of source codes, see [Source Code Cost](https://stocksharp.com/en/store/?groups=22).

## Recommended content

[Installation instruction](../api/setup.md)
