# API Documentation

## Overview

StockSharp API, also known as S# API, is a comprehensive Software Development Kit (SDK) designed for creating trading applications similar to [Designer](designer.md), [Terminal](terminal.md), and others. This SDK serves as the architectural foundation for numerous significant projects within the trading community.

## Features

- **Strategy Scripting**: StockSharp API provides a robust scripting mechanism for writing and implementing trading strategies directly in [Designer](designer/strategies/using_csharp.md). Users can develop, test, and deploy trading algorithms using C#.

- **Analytical Tools**: The API is integrated with Hydra, a platform for detailed [market data analysis](hydra/analytics.md). It offers extensive support for data manipulation and storage, enabling complex analytical operations.

- **Custom Application Development**: Besides scripting within existing applications, StockSharp API allows developers to create custom [independent trading solutions](api/examples.md). This feature is crucial for users who require tailor-made functionalities not typically available in standard trading applications.

- **Connectors and Graphical Controls**: The API includes a wide range of [connectors](api/connectors.md) for real-time market data integration from various exchanges. Additionally, it supports the development of customizable [graphical user interfaces](api/graphical_user_interface.md), making it versatile for creating professional trading platforms.

## Architecture

StockSharp API is built with a focus on modularity and [extensibility](api/connectors/creating_own_connector.md). It enables developers to extend its capabilities through plugins and additional modules without altering the core system. This modular architecture makes it an ideal choice for developers looking to build scalable and maintainable trading applications.

## Open Source

The core of the StockSharp API is developed under an open-source paradigm, allowing the community to contribute and ensuring transparency. The source code is available on GitHub, providing both novice and experienced developers with the opportunity to study, modify, and enhance the system according to their specific needs.

## GitHub Repository

The official StockSharp API source code can be found at the GitHub repository:

[StockSharp GitHub Repository](https://github.com/stocksharp/stocksharp)