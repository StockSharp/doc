# 连接器配置：CoinGlass

连接 CoinGlass 前，请配置以下适配器属性。该列表已根据 [CoinGlassMessageAdapter](xref:StockSharp.CoinGlass.CoinGlassMessageAdapter) 的实现进行核对。

## 基本设置

连接编辑器会优先显示这些设置。

- `Token` (`SecureString`)
- `RestEndpoint` (`string`)
- `MarketType` (`CoinGlassMarketTypes`)
- `CandleMetric` (`CoinGlassCandleMetrics`)
- `Exchange` (`string`)
- `Symbol` (`string`)

## 高级设置

这些属性用于控制端点、筛选条件、限制以及提供商特有的其他行为。

- `RequestInterval` (`TimeSpan`)
- `PollingInterval` (`TimeSpan`)
- `MaximumItems` (`int`)
- `HistoryLimit` (`int`)

## 另请参阅

[图形化配置](graphical_configuration_coinglass.md)

[适配器初始化](adapter_initialization_coinglass.md)
