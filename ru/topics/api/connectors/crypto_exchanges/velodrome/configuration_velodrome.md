# Настройки коннектора: Velodrome

Перед подключением к Velodrome задайте перечисленные ниже свойства адаптера. Список проверен по реализации [VelodromeMessageAdapter](xref:StockSharp.Velodrome.VelodromeMessageAdapter).

## Основные настройки

Эти параметры показываются первыми в редакторе подключения.

- `WalletAddress` (`string`)
- `PrivateKey` (`SecureString`)
- `RpcEndpoint` (`string`)
- `WebSocketEndpoint` (`string`)

## Дополнительные настройки

Эти свойства управляют адресами, фильтрами, ограничениями и другими параметрами, зависящими от провайдера.

- `Pools` (`string`)
- `HistoryBlockRange` (`int`)
- `HistoryBlockCount` (`int`)
- `ProbeVolume` (`decimal`)
- `SlippageTolerance` (`decimal`)
- `PollingInterval` (`TimeSpan`)

## См. также

[Графическое конфигурирование](graphical_configuration_velodrome.md)

[Инициализация адаптера](adapter_initialization_velodrome.md)
