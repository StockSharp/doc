# SimFin

**SimFin** 将 StockSharp 连接到提供公司、价格和基本面数据的 SimFin Web API v3。

## 主要功能

- 已根据源代码核实的功能：公司搜索、日频 Level 1 价格、日 K 线，以及通过 `SimFinDataTypes.Fundamentals` 提供的结构化基本面数据。该适配器仅提供历史数据，不支持交易。
- 服务商特有的传输、会话和数据格式均封装在标准 StockSharp API 之后。

## 适用场景

可用于下载受支持公司的日价格、财务报表、派生值和可选财务比率。

公司覆盖、历史、字段、请求限制和可用性取决于 SimFin 订阅方案。

## 另请参阅

[连接器配置](simfin/configuration_simfin.md)

[图形化配置](simfin/graphical_configuration_simfin.md)

[适配器初始化](simfin/adapter_initialization_simfin.md)
