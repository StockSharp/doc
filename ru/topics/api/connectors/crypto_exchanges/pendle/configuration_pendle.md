# Настройки коннектора: Pendle

Перед подключением к Pendle задайте перечисленные ниже свойства адаптера. Список проверен по реализации [PendleMessageAdapter](xref:StockSharp.Pendle.PendleMessageAdapter).

## Основные настройки

Эти параметры показываются первыми в редакторе подключения.

- `Chain` (`PendleChains`)
- `WalletAddress` (`string`)
- `PrivateKey` (`SecureString`)
- `ApiEndpoint` (`string`)
- `RpcEndpoint` (`string`)

## Дополнительные настройки

Эти свойства управляют выбором рынков, ограничениями, опросом и выполнением транзакций.

- `MarketAddresses` (`string`)
- `MaxMarkets` (`int`)
- `ProbeVolume` (`decimal`)
- `SlippageTolerance` (`decimal`)
- `PollingInterval` (`TimeSpan`)
- `HistoryLimit` (`int`)
- `ReceiptTimeout` (`TimeSpan`)
- `IsAutoApprove` (`bool`)

## См. также

[Графическое конфигурирование](graphical_configuration_pendle.md)

[Инициализация адаптера](adapter_initialization_pendle.md)
