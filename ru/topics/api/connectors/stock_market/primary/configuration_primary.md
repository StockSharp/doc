# Настройки коннектора: Primary

Перед подключением к Primary задайте перечисленные ниже свойства адаптера. Список проверен по реализации [PrimaryMessageAdapter](xref:StockSharp.Primary.PrimaryMessageAdapter).

## Основные настройки

Эти параметры показываются первыми в редакторе подключения.

- `Login` (`string`)
- `Password` (`SecureString`)
- `IsDemo` (`bool`)
- `Account` (`string`)

## Дополнительные настройки

Эти свойства управляют адресами, фильтрами, ограничениями и другими параметрами, зависящими от провайдера.

- `Token` (`SecureString`)
- `Proprietary` (`string`)
- `DefaultMarket` (`string`)
- `MarketDataLevel` (`int`)
- `AccountPollingInterval` (`TimeSpan`)
- `LookupLimit` (`int`)
- `RestAddress` (`Uri`)
- `SandboxRestAddress` (`Uri`)
- `WebSocketAddress` (`Uri`)
- `SandboxWebSocketAddress` (`Uri`)

## См. также

[Графическое конфигурирование](graphical_configuration_primary.md)

[Инициализация адаптера](adapter_initialization_primary.md)
