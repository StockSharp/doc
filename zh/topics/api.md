# API 文档

## 概述

StockSharp API 也称为 S# API，是一套完整的软件开发工具包（SDK），用于创建类似 [Designer](designer.md)、[Terminal](terminal.md) 等产品的交易应用程序。许多交易领域的重要项目都以这套 SDK 作为底层架构。

## 功能

- **策略脚本**：StockSharp API 提供完善的脚本机制，可直接在 [Designer](designer/strategies/using_code.md) 中编写和实现交易策略。用户可以使用 C#、F# 或 Python 开发、测试和部署交易算法。

- **分析工具**：API 与 Hydra 集成，支持详细的[市场数据分析](hydra/analytics.md)。它提供丰富的数据处理和存储能力，可用于完成复杂的分析任务。

- **自定义应用程序开发**：除了在现有应用程序中编写脚本，开发人员还可以使用 StockSharp API 创建自定义的[独立交易解决方案](api/examples.md)。对于需要标准交易应用程序通常不具备的定制功能的用户，这一点尤为重要。

- **连接器和桌面控件**：API 包含大量[连接器](api/connectors.md)，可接入不同交易所的实时市场数据。此外，它还提供可定制的[桌面控件](api/graphical_user_interface.md)，适合用于构建专业交易平台。

## 架构

StockSharp API 的设计重点是模块化和[可扩展性](api/connectors/creating_own_connector.md)。开发人员无需修改核心系统，即可通过插件和附加模块扩展其功能。这种模块化架构便于构建可扩展、易维护的交易应用程序。

## 开源

StockSharp API 的核心采用开源方式开发，既允许社区参与贡献，也保证了项目的透明度。源代码托管在 GitHub 上，无论是初学者还是经验丰富的开发人员，都可以根据自己的具体需求研究、修改和改进系统。

## GitHub 仓库

StockSharp API 的官方源代码位于以下 GitHub 仓库：

[StockSharp GitHub 仓库](https://github.com/stocksharp/stocksharp)
