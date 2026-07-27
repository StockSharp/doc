# Настройки коннектора: HDFC Securities

Перед подключением к HDFC Securities задайте перечисленные ниже свойства адаптера. Список проверен по реализации [HdfcMessageAdapter](xref:StockSharp.HdfcSecurities.HdfcMessageAdapter).

## Основные настройки

Эти параметры показываются первыми в редакторе подключения.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `RequestToken` (`SecureString`)

## Дополнительные настройки

Эти свойства управляют адресами, фильтрами, ограничениями и другими параметрами, зависящими от провайдера.

- `Token` (`SecureString`)
- `PortfolioName` (`string`)
- `DefaultProduct` (`HdfcProducts`)
- `PollingInterval` (`TimeSpan`)
- `ReconnectAttempts` (`int`)
- `RestAddress` (`Uri`)
- `InstrumentAddress` (`Uri`)
- `WebSocketAddress` (`Uri`)

## См. также

[Графическое конфигурирование](graphical_configuration_hdfc_securities.md)

[Инициализация адаптера](adapter_initialization_hdfc_securities.md)
