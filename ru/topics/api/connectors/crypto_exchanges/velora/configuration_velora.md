# Настройки коннектора: Velora

Перед подключением к Velora задайте перечисленные ниже свойства адаптера. Список проверен по реализации [VeloraMessageAdapter](xref:StockSharp.Velora.VeloraMessageAdapter).

## Основные настройки

Эти параметры показываются первыми в редакторе подключения.

- `Partner` (`string`)
- `Chain` (`VeloraChains`)
- `WalletAddress` (`string`)
- `PrivateKey` (`SecureString`)
- `ApiEndpoint` (`string`)
- `RpcEndpoint` (`string`)

## Дополнительные настройки

Эти свойства управляют адресами, фильтрами, ограничениями и другими параметрами, зависящими от провайдера.

- `Markets` (`string`)
- `ProbeVolume` (`decimal`)
- `SlippageTolerance` (`decimal`)
- `PollingInterval` (`TimeSpan`)
- `ReceiptTimeout` (`TimeSpan`)
- `IsAutoApprove` (`bool`)

## См. также

[Графическое конфигурирование](graphical_configuration_velora.md)

[Инициализация адаптера](adapter_initialization_velora.md)
