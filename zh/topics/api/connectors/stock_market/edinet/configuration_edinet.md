# 连接器配置：EDINET

连接 EDINET 前，请配置以下适配器属性。该列表已根据 [EdinetMessageAdapter](xref:StockSharp.Edinet.EdinetMessageAdapter) 的实现进行核对。

## 基本设置

连接编辑器会优先显示这些设置。

- `Token` (`SecureString`)

## 高级设置

这些属性用于控制端点、筛选条件、限制以及提供商特有的其他行为。

- `Address` (`Uri`)
- `CodeListAddress` (`Uri`)
- `ViewerAddress` (`Uri`)
- `DisclosureType` (`EdinetDisclosureTypes`)
- `ListedOnly` (`bool`)
- `IncludeWithdrawn` (`bool`)
- `IncludeUnavailable` (`bool`)
- `DefaultLookupDays` (`int`)
- `MaxDays` (`int`)
- `RequestInterval` (`TimeSpan`)
- `MaxDocumentSizeMb` (`int`)
- `CodeListCacheTimeout` (`TimeSpan`)

## 另请参阅

[图形化配置](graphical_configuration_edinet.md)

[适配器初始化](adapter_initialization_edinet.md)
