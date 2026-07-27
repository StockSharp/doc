# Настройки коннектора: Mastertrust

Перед подключением к Mastertrust задайте перечисленные ниже свойства адаптера. Список проверен по реализации [MastertrustMessageAdapter](xref:StockSharp.Mastertrust.MastertrustMessageAdapter).

## Основные настройки

Эти параметры показываются первыми в редакторе подключения.

- `ClientId` (`string`)
- `OAuthClientId` (`string`)
- `OAuthClientSecret` (`SecureString`)
- `AuthorizationCode` (`SecureString`)
- `RedirectUri` (`Uri`)
- `Token` (`SecureString`)

## Дополнительные настройки

Эти свойства управляют адресами, фильтрами, ограничениями и другими параметрами, зависящими от провайдера.

- `PortfolioName` (`string`)
- `DefaultProduct` (`MastertrustProducts`)
- `ReconnectAttempts` (`int`)
- `Address` (`Uri`)
- `MasterAddress` (`Uri`)
- `WebSocketAddress` (`Uri`)
- `PollingInterval` (`TimeSpan`)

## См. также

[Графическое конфигурирование](graphical_configuration_mastertrust.md)

[Инициализация адаптера](adapter_initialization_mastertrust.md)
