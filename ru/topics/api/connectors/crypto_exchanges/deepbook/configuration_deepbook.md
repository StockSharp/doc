# Настройки коннектора: DeepBook

Перед подключением к DeepBook задайте перечисленные ниже свойства адаптера. Список проверен по реализации [DeepBookMessageAdapter](xref:StockSharp.DeepBook.DeepBookMessageAdapter).

## Основные настройки

Эти параметры показываются первыми в редакторе подключения.

- `WalletAddress` (`string`)
- `PrivateKey` (`SecureString`)
- `IndexerEndpoint` (`string`)
- `GrpcEndpoint` (`string`)

## Дополнительные настройки

Эти свойства управляют адресами, фильтрами, ограничениями и другими параметрами, зависящими от провайдера.

- `PackageId` (`string`)
- `ClockObjectId` (`string`)
- `Pools` (`string`)
- `OrderBookDepth` (`int`)
- `HistoryLimit` (`int`)
- `SlippageTolerance` (`decimal`)
- `PollingInterval` (`TimeSpan`)

## См. также

[Графическое конфигурирование](graphical_configuration_deepbook.md)

[Инициализация адаптера](adapter_initialization_deepbook.md)
