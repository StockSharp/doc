# Настройки коннектора: Ventura

Перед подключением к Ventura задайте перечисленные ниже свойства адаптера. Список проверен по реализации [VenturaMessageAdapter](xref:StockSharp.Ventura.VenturaMessageAdapter).

## Основные настройки

Эти параметры показываются первыми в редакторе подключения.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `Token` (`SecureString`)
- `ClientId` (`string`)

## Дополнительные настройки

Эти свойства управляют адресами, фильтрами, ограничениями и другими параметрами, зависящими от провайдера.

- `RequestToken` (`SecureString`)
- `RefreshToken` (`SecureString`)
- `Pin` (`SecureString`)
- `TotpSecret` (`SecureString`)
- `MacAddress` (`string`)
- `PortfolioName` (`string`)
- `DefaultProduct` (`VenturaProducts`)
- `PollingInterval` (`TimeSpan`)
- `ReconnectAttempts` (`int`)
- `RestAddress` (`Uri`)
- `MarketDataAddress` (`Uri`)
- `OrderStatusAddress` (`Uri`)

## См. также

[Графическое конфигурирование](graphical_configuration_ventura.md)

[Инициализация адаптера](adapter_initialization_ventura.md)
