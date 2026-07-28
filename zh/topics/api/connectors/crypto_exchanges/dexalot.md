# Dexalot

**Dexalot** 将 StockSharp 连接到运行在区块链上的 Dexalot 中央限价订单簿。适配器通过标准 StockSharp 消息模型提供受支持的市场数据和交易操作。

## 主要功能

- 已根据源代码核实的适配器功能：实时数据更新、Level 1 行情、成交数据、订单簿、K 线数据、历史数据请求、资产组合与订单操作。
- 服务商特有的传输、会话和数据格式均封装在标准 StockSharp API 之后。

## 适用场景

可用于监控 Dexalot 市场、分析行情、成交、订单簿和 K 线、请求历史数据，以及提交受支持的订单。

可用交易对、数据深度、交易权限、请求限制和服务可用性由 Dexalot 及所连接的钱包决定。

## 另请参阅

[连接器配置](dexalot/configuration_dexalot.md)

[图形化配置](dexalot/graphical_configuration_dexalot.md)

[适配器初始化](dexalot/adapter_initialization_dexalot.md)
