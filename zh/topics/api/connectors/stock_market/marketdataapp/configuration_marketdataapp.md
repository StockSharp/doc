# 连接器配置：MarketData.app

连接 MarketData.app 前，请配置以下适配器属性。该列表已根据 [MarketDataAppMessageAdapter](xref:StockSharp.MarketDataApp.MarketDataAppMessageAdapter) 的实现进行核对。

## 基本设置

连接编辑器会优先显示这些设置。

- `Token` (`SecureString`)
- `RestEndpoint` (`Uri`)

## 高级设置

这些属性用于控制服务端点、请求间隔、筛选条件、数据选项和结果限制。

- `ExtendedHours` (`bool`)
- `AdjustSplits` (`bool`)
- `MaximumOptionContracts` (`int`)

## 另请参阅

[图形化配置](graphical_configuration_marketdataapp.md)

[适配器初始化](adapter_initialization_marketdataapp.md)
