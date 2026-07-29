# 连接器配置：Finage

连接 Finage 前，请配置以下适配器属性。该列表已根据 [FinageMessageAdapter](xref:StockSharp.Finage.FinageMessageAdapter) 的实现进行核对。

## 基本设置

连接编辑器会优先显示这些设置。

- `ApiKey` (`SecureString`)

## 高级设置

这些属性用于控制 REST 与 WebSocket 访问、服务端点、市场数据选项、交易对筛选和结果限制。

- `StreamingToken` (`SecureString`)
- `RestEndpoint` (`Uri`)
- `StreamingEndpoint` (`Uri`)
- `RequestInterval` (`TimeSpan`)
- `Symbols` (`string`)
- `MaximumSecurities` (`int`)

## 另请参阅

[图形化配置](graphical_configuration_finage.md)

[适配器初始化](adapter_initialization_finage.md)
