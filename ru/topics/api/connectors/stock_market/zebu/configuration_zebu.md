# Настройки коннектора: Zebu

Перед подключением к Zebu задайте перечисленные ниже свойства адаптера. Список проверен по реализации [ZebuMessageAdapter](xref:StockSharp.Zebu.ZebuMessageAdapter).

## Основные настройки

Эти параметры показываются первыми в редакторе подключения.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `AuthorizationCode` (`SecureString`)
- `UserId` (`string`)

## Дополнительные настройки

Эти свойства управляют адресами, фильтрами, ограничениями и другими параметрами, зависящими от провайдера.

- `RefreshToken` (`SecureString`)
- `Token` (`SecureString`)
- `AccountId` (`string`)
- `TokenExpiresAt` (`DateTime?`)
- `DefaultProduct` (`ShoonyaProducts`)
- `ReconnectAttempts` (`int`)
- `AuthorizationAddress` (`Uri`)
- `RestEndpoint` (`string`)
- `InstrumentEndpointTemplate` (`string`)
- `WebSocketEndpoint` (`string`)

## См. также

[Графическое конфигурирование](graphical_configuration_zebu.md)

[Инициализация адаптера](adapter_initialization_zebu.md)
