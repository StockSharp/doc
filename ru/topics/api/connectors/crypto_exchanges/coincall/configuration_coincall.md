# Настройки коннектора: Coincall

Перед подключением к Coincall задайте перечисленные ниже свойства адаптера. Список проверен по реализации [CoincallMessageAdapter](xref:StockSharp.Coincall.CoincallMessageAdapter).

## Основные настройки

Эти параметры показываются первыми в редакторе подключения.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `ProductType` (`CoincallProductTypes`)
- `RestEndpoint` (`string`)
- `OptionsWebSocketEndpoint` (`string`)
- `FuturesWebSocketEndpoint` (`string`)
- `RequestValidityWindow` (`TimeSpan`)
- `PrivatePollingInterval` (`TimeSpan`)

## См. также

[Графическое конфигурирование](graphical_configuration_coincall.md)

[Инициализация адаптера](adapter_initialization_coincall.md)
