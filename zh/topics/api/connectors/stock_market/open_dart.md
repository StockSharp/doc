# Open DART

**Open DART 连接器**将 StockSharp 接入金融数据与参考信息服务。 它把提供商特有的数据转换为统一的 StockSharp 消息模型，使应用程序能够在不同数据源中使用相同的订阅和工作流程。

## 主要功能

- 典型覆盖范围：股票、发行人参考信息。
- 发现交易品种并获取提供商参考数据。
- 提供商支持的市场、公司、申报、披露和参考数据。
- 适配器支持的市场数据：Level 1 行情、金融新闻、财务披露。
- 请求历史数据，用于图表、分析和策略回测。
- 此适配器用于访问数据，不提供订单路由。
- 提供商特有的传输、会话和数据格式均封装在标准 StockSharp API 之后。

## 适用场景

适用于交易品种主数据、披露监控、发行人研究、合规流程和历史分析。

可用交易品种、数据深度、交易权限、请求限制和服务可用性由 Open DART、API 套餐及所连接账户的权限决定。

## 另请参阅

[连接器配置](open_dart/configuration_open_dart.md)

[图形化配置](open_dart/graphical_configuration_open_dart.md)

[适配器初始化](open_dart/adapter_initialization_open_dart.md)
