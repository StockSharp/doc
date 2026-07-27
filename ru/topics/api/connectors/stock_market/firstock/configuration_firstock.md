# Настройки коннектора: Firstock

Перед подключением к Firstock задайте перечисленные ниже свойства адаптера. Список проверен по реализации [FirstockMessageAdapter](xref:StockSharp.Firstock.FirstockMessageAdapter).

## Основные настройки

Эти параметры показываются первыми в редакторе подключения.

- `UserId` (`string`)
- `Password` (`SecureString`)
- `OneTimePassword` (`SecureString`)
- `VendorCode` (`string`)
- `ApiKey` (`SecureString`)
- `Token` (`SecureString`)

## Дополнительные настройки

Эти свойства управляют адресами, фильтрами, ограничениями и другими параметрами, зависящими от провайдера.

- `PortfolioName` (`string`)
- `DefaultProduct` (`FirstockProducts`)
- `MarketProtection` (`decimal`)
- `PriceDivisor` (`decimal`)
- `ReconnectAttempts` (`int`)
- `Address` (`Uri`)
- `SymbolsAddress` (`Uri`)
- `WebSocketAddress` (`Uri`)
- `PollingInterval` (`TimeSpan`)

## См. также

[Графическое конфигурирование](graphical_configuration_firstock.md)

[Инициализация адаптера](adapter_initialization_firstock.md)
