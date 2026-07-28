# Настройки коннектора: 0x

Перед подключением к 0x задайте перечисленные ниже свойства адаптера. Список проверен по реализации [ZeroXMessageAdapter](xref:StockSharp.ZeroX.ZeroXMessageAdapter).

## Основные настройки

Эти параметры показываются первыми в редакторе подключения.

- `ApiKey` (`SecureString`)
- `Chain` (`ZeroXChains`)
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

[Графическое конфигурирование](graphical_configuration_zero_x.md)

[Инициализация адаптера](adapter_initialization_zero_x.md)
