# Marketaux

**Marketaux 连接器**将 StockSharp 接入专业市场数据与分析服务。 它把提供商特有的数据和操作转换为统一的 StockSharp 消息模型，使应用程序能够在不同场所使用相同的订阅和工作流程。

## 主要功能

- 典型覆盖范围：股票。
- 发现交易品种并获取提供商参考数据。
- 提供商支持的市场、公司、申报、披露和参考数据。
- 适配器支持的市场数据：金融新闻、财务披露。
- 请求历史数据，用于图表、分析和策略回测。
- 通过提供商的流式传输通道进行实时订阅。
- 此适配器用于访问数据，不提供订单路由。
- 提供商特有的传输、会话和数据格式均封装在标准 StockSharp API 之后。

## 适用场景

适用于使用提供商数据驱动图表、市场数据存储、分析、研究和策略测试。

可用交易品种、数据深度、交易权限、请求限制和服务可用性由 Marketaux、API 套餐及所连接账户的权限决定。

## 另请参阅

[连接器配置](marketaux/configuration_marketaux.md)

[图形化配置](marketaux/graphical_configuration_marketaux.md)

[适配器初始化](marketaux/adapter_initialization_marketaux.md)
