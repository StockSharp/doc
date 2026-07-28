# Settrade

**Settrade** 将 StockSharp 连接到面向泰国 SET 股票和 TFEX 衍生品的 Settrade Open API v2。适配器通过标准 StockSharp 消息模型提供受支持的市场数据和交易操作。

## 主要功能

- 已根据源代码核实的适配器功能：实时数据更新、Level 1 行情、订单簿、K 线数据、历史数据请求，以及股票和衍生品账户的资产组合与订单操作。
- 服务商特有的传输、会话和数据格式均封装在标准 StockSharp API 之后。

## 适用场景

可用于监控 SET 和 TFEX 品种、分析行情、订单簿和 K 线、请求历史数据，以及执行受支持的订单流程。

可用品种、数据深度、交易权限、请求限制和服务可用性由 Settrade、经纪商及所连接的账户决定。

## 另请参阅

[连接器配置](settrade/configuration_settrade.md)

[图形化配置](settrade/graphical_configuration_settrade.md)

[适配器初始化](settrade/adapter_initialization_settrade.md)
