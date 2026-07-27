# Настройки коннектора: Jainam

Перед подключением к Jainam задайте перечисленные ниже свойства адаптера. Список проверен по реализации [JainamMessageAdapter](xref:StockSharp.Jainam.JainamMessageAdapter).

## Основные настройки

Эти параметры показываются первыми в редакторе подключения.

- `UserId` (`string`)
- `AppCode` (`string`)
- `ApiSecret` (`SecureString`)
- `AuthCode` (`SecureString`)
- `Token` (`SecureString`)

## Дополнительные настройки

Эти свойства управляют адресами, фильтрами, ограничениями и другими параметрами, зависящими от провайдера.

- `PortfolioName` (`string`)
- `DefaultProduct` (`JainamProducts`)
- `ReconnectAttempts` (`int`)
- `PollingInterval` (`TimeSpan`)
- `RestAddress` (`Uri`)
- `InstrumentAddress` (`string`)
- `WebSocketAddress` (`string`)

## См. также

[Графическое конфигурирование](graphical_configuration_jainam.md)

[Инициализация адаптера](adapter_initialization_jainam.md)
