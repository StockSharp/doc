# Finam Trade API

**Finam Trade API 连接器**将 StockSharp 应用程序接入 Finam 提供的经纪账户和市场数据。它把交易品种、行情、订单、成交和投资组合状态转换为统一的 StockSharp 消息模型。

## 主要功能

- 查询 Finam 提供的股票、债券、货币、基金、期货和期权。
- Level 1 行情、订单簿、市场成交和分时 K 线。
- 历史 K 线请求和实时市场数据订阅。
- 提交市价、限价、止损和止损限价订单，并可撤单。
- 接收订单状态、自有成交、现金余额和持仓更新。
- 自动将 API 密钥换取短期会话令牌。
- 可配置 REST 和 WebSocket 地址，以连接兼容网关和测试环境。

## 适用场景

该连接器适用于需要通过统一 StockSharp 接口访问 Finam 市场数据和交易功能的交易机器人、终端、投资组合监控器和订单管理服务。

连接时需要 Finam Trade API 密钥。可以明确指定交易账户，也可以将账户标识符留空，让连接器使用令牌可访问的第一个账户。交易品种使用 Finam 的 `ticker@MIC` 格式。可用市场、历史深度、实时数据、交易权限和请求限制取决于所连接的账户以及 Finam 的服务条款。

## 另请参阅

[连接器配置](finam_trade/configuration_finam_trade.md)

[图形化配置](finam_trade/graphical_configuration_finam_trade.md)

[适配器初始化](finam_trade/adapter_initialization_finam_trade.md)
