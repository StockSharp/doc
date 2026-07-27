# Настройки коннектора: Bigul

Перед подключением к Bigul задайте перечисленные ниже свойства адаптера. Список проверен по реализации [BigulMessageAdapter](xref:StockSharp.Bigul.BigulMessageAdapter).

## Основные настройки

Эти параметры показываются первыми в редакторе подключения.

- `ClientCode` (`string`)
- `ApiKey` (`SecureString`)
- `ApiSecret` (`SecureString`)
- `OneTimePassword` (`SecureString`)
- `Token` (`SecureString`)
- `Source` (`string`)

## Дополнительные настройки

Эти свойства управляют адресами, фильтрами, ограничениями и другими параметрами, зависящими от провайдера.

- `PortfolioName` (`string`)
- `DefaultProduct` (`BigulProducts`)
- `MarketProtection` (`decimal`)
- `ReconnectAttempts` (`int`)
- `Address` (`Uri`)
- `MasterAddress` (`Uri`)
- `WebSocketAddress` (`Uri`)
- `PollingInterval` (`TimeSpan`)

## См. также

[Графическое конфигурирование](graphical_configuration_bigul.md)

[Инициализация адаптера](adapter_initialization_bigul.md)
