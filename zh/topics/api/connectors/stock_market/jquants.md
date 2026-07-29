# J-Quants

**J-Quants** 将 StockSharp 连接到提供东京 TSE 市场数据的 J-Quants API V2。

## 主要功能

- 已根据源代码核实的功能：搜索 TSE 股票、期货和期权，获取 Level 1 数值、成交数据和历史 K 线。该适配器仅提供市场数据，不支持交易操作。
- 服务商特有的传输、会话和数据格式均封装在标准 StockSharp API 之后。

## 适用场景

可用于查找日本市场品种，并下载行情、成交和 K 线历史数据用于研究分析。

数据覆盖、历史深度、请求限制和可用性由 J-Quants 订阅方案决定。

## 另请参阅

[连接器配置](jquants/configuration_jquants.md)

[图形化配置](jquants/graphical_configuration_jquants.md)

[适配器初始化](jquants/adapter_initialization_jquants.md)
