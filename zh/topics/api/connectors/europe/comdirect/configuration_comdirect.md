# 连接器配置：comdirect

连接 comdirect 前，请配置以下适配器属性。该列表已根据 [ComdirectMessageAdapter](xref:StockSharp.Comdirect.ComdirectMessageAdapter) 的实现进行核对。

## 基本设置

连接编辑器会优先显示这些设置。

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `Login` (`string`)
- `Password` (`SecureString`)
- `TanType` (`ComdirectTanTypes`)
- `PollingInterval` (`TimeSpan`)

## 高级设置

这些属性用于控制端点、筛选条件、限制以及提供商特有的其他行为。

- `DefaultCurrency` (`string`)
- `Address` (`Uri`)

## 另请参阅

[图形化配置](graphical_configuration_comdirect.md)

[适配器初始化](adapter_initialization_comdirect.md)
