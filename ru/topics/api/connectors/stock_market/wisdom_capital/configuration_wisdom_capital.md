# Настройки коннектора: Wisdom Capital

Перед подключением к Wisdom Capital задайте перечисленные ниже свойства адаптера. Список проверен по реализации [WisdomCapitalMessageAdapter](xref:StockSharp.WisdomCapital.WisdomCapitalMessageAdapter).

## Основные настройки

Эти параметры показываются первыми в редакторе подключения.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `MarketDataKey` (`SecureString`)
- `MarketDataSecret` (`SecureString`)

## Дополнительные настройки

Эти свойства управляют адресами, фильтрами, ограничениями и другими параметрами, зависящими от провайдера.

- `Token` (`SecureString`)
- `UserId` (`string`)
- `MarketDataToken` (`SecureString`)
- `MarketDataUserId` (`string`)
- `PortfolioName` (`string`)
- `DefaultProduct` (`WisdomCapitalProducts`)
- `Source` (`string`)
- `PollingInterval` (`TimeSpan`)
- `ReconnectAttempts` (`int`)
- `EngineIoVersion` (`int`)
- `RestAddress` (`Uri`)

## См. также

[Графическое конфигурирование](graphical_configuration_wisdom_capital.md)

[Инициализация адаптера](adapter_initialization_wisdom_capital.md)
