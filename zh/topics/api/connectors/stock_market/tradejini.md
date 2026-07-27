# Tradejini

**Tradejini 连接器**将 StockSharp 接入金融市场经纪商或电子交易场所。 它把提供商特有的数据和操作转换为统一的 StockSharp 消息模型，使应用程序能够在不同场所使用相同的订阅和工作流程。

## 主要功能

- 典型覆盖范围：股票、期货、期权、外汇、大宗商品。
- 发现交易品种并获取提供商参考数据。
- 适配器支持的市场数据：K 线。
- 请求历史数据，用于图表、分析和策略回测。
- 提供商支持的订单提交和成交处理流程。
- 投资组合、余额、持仓和成交状态更新。
- 提供商特有的传输、会话和数据格式均封装在标准 StockSharp API 之后。

## 适用场景

适用于需要直接连接提供商的实时策略、交易终端、订单管理服务和监控工具。

可用交易品种、数据深度、交易权限、请求限制和服务可用性由 Tradejini、API 套餐及所连接账户的权限决定。

## 另请参阅

[连接器配置](tradejini/configuration_tradejini.md)

[图形化配置](tradejini/graphical_configuration_tradejini.md)

[适配器初始化](tradejini/adapter_initialization_tradejini.md)
