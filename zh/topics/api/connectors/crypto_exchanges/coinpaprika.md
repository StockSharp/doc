# CoinPaprika

**CoinPaprika** 将 StockSharp 连接到 CoinPaprika 加密货币市场数据服务。适配器通过标准 StockSharp 消息模型提供服务商数据。

## 主要功能

- 已根据源代码核实的适配器功能：实时数据更新、Level 1 行情、K 线数据、历史数据请求。
- 服务商特有的传输、会话和数据格式均封装在标准 StockSharp API 之后。
- 此适配器用于访问数据，不提供订单路由。

## 适用场景

适用于使用服务商数据驱动图表、市场数据存储、分析、研究和策略测试。

可用交易品种、数据深度、交易权限、请求限制和服务可用性由 CoinPaprika、API 套餐及所连接账户的权限决定。

## 另请参阅

[连接器配置](coinpaprika/configuration_coinpaprika.md)

[图形化配置](coinpaprika/graphical_configuration_coinpaprika.md)

[适配器初始化](coinpaprika/adapter_initialization_coinpaprika.md)
