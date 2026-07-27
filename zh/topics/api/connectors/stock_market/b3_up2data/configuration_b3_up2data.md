# 连接器配置：B3 UP2DATA

连接 B3 UP2DATA 前，请配置以下适配器属性。该列表已根据 [B3Up2DataMessageAdapter](xref:StockSharp.B3Up2Data.B3Up2DataMessageAdapter) 的实现进行核对。

## 基本设置

连接编辑器会优先显示这些设置。

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `CertificatePath` (`string`)
- `CertificatePassword` (`SecureString`)
- `Address` (`Uri`)

## 高级设置

这些属性用于控制端点、筛选条件、限制以及提供商特有的其他行为。

- `SasUri` (`SecureString`)
- `ChannelName` (`string`)
- `FileFormat` (`B3Up2DataFileFormats`)
- `BlobPrefix` (`string`)
- `LookbackDays` (`int`)
- `PageSize` (`int`)
- `MaxPages` (`int`)
- `MaxRawFiles` (`int`)

## 另请参阅

[图形化配置](graphical_configuration_b3_up2data.md)

[适配器初始化](adapter_initialization_b3_up2data.md)
