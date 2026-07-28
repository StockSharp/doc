# Настройки коннектора: Birdeye

Перед подключением к Birdeye задайте перечисленные ниже свойства адаптера. Список проверен по реализации [BirdeyeMessageAdapter](xref:StockSharp.Birdeye.BirdeyeMessageAdapter).

## Основные настройки

Эти параметры показываются первыми в редакторе подключения.

- `Token` (`SecureString`)
- `RestEndpoint` (`string`)
- `WebSocketEndpoint` (`string`)
- `WebSocketOrigin` (`string`)
- `Chain` (`string`)
- `TokenAddress` (`string`)
- `StreamingEnabled` (`bool`)
- `PriceInUsd` (`bool`)

## Дополнительные настройки

Эти свойства управляют адресами, фильтрами, ограничениями и другими параметрами, зависящими от провайдера.

- `MinimumLiquidity` (`decimal`)
- `RequestInterval` (`TimeSpan`)
- `PollingInterval` (`TimeSpan`)
- `MaximumItems` (`int`)
- `HistoryLimit` (`int`)

## См. также

[Графическое конфигурирование](graphical_configuration_birdeye.md)

[Инициализация адаптера](adapter_initialization_birdeye.md)
