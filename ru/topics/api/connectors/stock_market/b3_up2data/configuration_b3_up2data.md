# Настройки коннектора: B3 UP2DATA

Перед подключением к B3 UP2DATA задайте перечисленные ниже свойства адаптера. Список проверен по реализации [B3Up2DataMessageAdapter](xref:StockSharp.B3Up2Data.B3Up2DataMessageAdapter).

## Основные настройки

Эти параметры показываются первыми в редакторе подключения.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `CertificatePath` (`string`)
- `CertificatePassword` (`SecureString`)
- `Address` (`Uri`)

## Дополнительные настройки

Эти свойства управляют адресами, фильтрами, ограничениями и другими параметрами, зависящими от провайдера.

- `SasUri` (`SecureString`)
- `ChannelName` (`string`)
- `FileFormat` (`B3Up2DataFileFormats`)
- `BlobPrefix` (`string`)
- `LookbackDays` (`int`)
- `PageSize` (`int`)
- `MaxPages` (`int`)
- `MaxRawFiles` (`int`)

## См. также

[Графическое конфигурирование](graphical_configuration_b3_up2data.md)

[Инициализация адаптера](adapter_initialization_b3_up2data.md)
