# 连接器配置：MetaApi

创建 MetaApi 令牌、部署交易账户并填写连接参数。

- `Token` - MetaApi 访问令牌。
- `AccountId` - 已部署的 MetaApi 账户的标识符。
- `Region` - 账户所在区域。API 令牌会自动解析该值，因此只有账户范围的令牌才需要显式设置。
- `Domain` - MetaApi 域名。默认值为 `agiliumtrade.agiliumtrade.ai`。
- `SynchronizationTimeout` - 等待服务器端终端同步的时长。默认值为 2 分钟，更短的值会被提升到 10 秒。

只有当 MetaApi 报告终端状态已同步后，连接才算完成，因此超时时间必须足以覆盖账户历史的初次同步。

挂单和保护性参数通过 [MetaApiOrderCondition](xref:StockSharp.MetaApi.MetaApiOrderCondition) 传递：激活价格、止损和止盈价格，以及随持仓一起保存的魔术号、备注和客户端标识符。

## 另请参阅

[MetaApi 官方文档](https://metaapi.cloud/docs/client/)
