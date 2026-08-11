# API Documentation

## Overview

StockSharp API, also known as S# API, is a software development kit (SDK) for building trading applications such as [Designer](designer.md), [Terminal](terminal.md), and other custom trading tools. It provides the core infrastructure for market data, order routing, strategy execution, testing, and trading UI components.

## Features

- **Strategy scripting**: StockSharp API lets users write and run trading strategies directly in [Designer](designer/strategies/using_code.md). Strategies can be developed, tested, and deployed using C#, F#, or Python.

- **Analytics tools**: The API integrates with Hydra for detailed [market data analysis](hydra/analytics.md). It supports data processing, storage, and custom analytical workflows.

- **Custom application development**: Developers can use StockSharp API to build standalone [trading solutions](api/examples.md) instead of relying only on built-in application scripting.

- **Connectors and desktop controls**: The API includes many [connectors](api/connectors.md) for real-time market data and trading access. It also provides customizable [desktop controls](api/graphical_user_interface.md) for building professional trading platforms.

## Architecture

StockSharp API is built around modularity and [extensibility](api/connectors/creating_own_connector.md). Developers can extend it with plugins and additional modules without changing the core system. This architecture helps build scalable and maintainable trading applications.

## Open Source

The core of StockSharp API is open source. The source code is available on GitHub, so developers can study it, modify it, and contribute improvements.

## GitHub Repository

The official StockSharp API source code is available in the GitHub repository:

[StockSharp GitHub Repository](https://github.com/stocksharp/stocksharp)
