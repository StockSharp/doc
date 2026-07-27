# Настройки коннектора: Rupeezy

Перед подключением к Rupeezy задайте перечисленные ниже свойства адаптера. Список проверен по реализации [RupeezyMessageAdapter](xref:StockSharp.Rupeezy.RupeezyMessageAdapter).

## Основные настройки

Эти параметры показываются первыми в редакторе подключения.

- `ApplicationId` (`string`)
- `ApiKey` (`SecureString`)
- `AuthCode` (`SecureString`)
- `Token` (`SecureString`)

## Дополнительные настройки

Эти свойства управляют адресами, фильтрами, ограничениями и другими параметрами, зависящими от провайдера.

- `PortfolioName` (`string`)
- `DefaultProduct` (`RupeezyProducts`)
- `ReconnectAttempts` (`int`)
- `Address` (`Uri`)
- `MasterAddress` (`Uri`)
- `WebSocketAddress` (`Uri`)
- `PollingInterval` (`TimeSpan`)

## См. также

[Графическое конфигурирование](graphical_configuration_rupeezy.md)

[Инициализация адаптера](adapter_initialization_rupeezy.md)
