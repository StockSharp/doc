# StocksTrader

**StocksTrader** 将 StockSharp 连接到官方 StocksTrader REST API。

该连接器支持模拟和实盘账户发现、账户状态轮询、交易品种搜索，以及最新买价、卖价和最新成交价快照。交易功能涵盖市价单、限价单和止损单、挂单的修改与撤销、未平仓持仓的止损和止盈修改、平仓，以及订单和成交历史。

StocksTrader 没有流式市场数据 API，因此 Level 1 请求会返回最新可用的快照并立即完成，而订单、成交和账户状态则通过轮询获取。

连接之前，请在 StocksTrader 网页终端中创建 Bearer 令牌。

## 另请参阅

[连接器配置](stocks_trader/configuration_stocks_trader.md)

[图形化配置](stocks_trader/graphical_configuration_stocks_trader.md)

[适配器初始化](stocks_trader/adapter_initialization_stocks_trader.md)

[StocksTrader 官方 API 文档](https://api-doc.stockstrader.com/)
