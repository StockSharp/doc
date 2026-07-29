# 连接器配置：SEC EDGAR

连接 SEC EDGAR 前，请配置以下适配器属性。该列表已根据 [SecEdgarMessageAdapter](xref:StockSharp.SecEdgar.SecEdgarMessageAdapter) 的实现进行核对。

## 基本设置

连接编辑器会优先显示这些设置。

- `DataEndpoint` (`Uri`)
- `UserAgent` (`string`)

## 高级设置

这些属性用于控制服务端点、请求间隔、筛选条件、数据选项和结果限制。

- `WebsiteEndpoint` (`Uri`)
- `RequestInterval` (`TimeSpan`)
- `Forms` (`string`)
- `MaximumHistoricalFiles` (`int`)
- `MaximumFacts` (`int`)

## 另请参阅

[图形化配置](graphical_configuration_sec_edgar.md)

[适配器初始化](adapter_initialization_sec_edgar.md)
