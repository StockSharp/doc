# Настройки коннектора: MasterLink

Перед подключением к MasterLink задайте перечисленные ниже свойства адаптера. Список проверен по реализации [MasterLinkMessageAdapter](xref:StockSharp.MasterLink.MasterLinkMessageAdapter).

## Основные настройки

Эти параметры показываются первыми в редакторе подключения.

- `Login` (`string`)
- `Password` (`SecureString`)
- `CertificatePath` (`string`)
- `CertificatePassword` (`SecureString`)
- `NodePath` (`string`)
- `GatewayDirectory` (`string`)

## Дополнительные настройки

Эти свойства управляют адресами, фильтрами, ограничениями и другими параметрами, зависящими от провайдера.

- `Account` (`string`)
- `RegisterApiAuth` (`bool`)
- `MarketDataMode` (`MasterLinkMarketDataModes`)
- `AdjustedCandles` (`bool`)
- `AccountPollingInterval` (`TimeSpan`)
- `MaxLookupResults` (`int`)

## См. также

[Графическое конфигурирование](graphical_configuration_masterlink.md)

[Инициализация адаптера](adapter_initialization_masterlink.md)
