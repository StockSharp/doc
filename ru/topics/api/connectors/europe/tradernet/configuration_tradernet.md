# Настройки коннектора: Tradernet

Перед подключением к Tradernet задайте перечисленные ниже свойства адаптера. Список проверен по реализации [TradernetMessageAdapter](xref:StockSharp.Tradernet.TradernetMessageAdapter).

## Основные настройки

Эти параметры показываются первыми в редакторе подключения.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `PollingInterval` (`TimeSpan`)

## Дополнительные настройки

Эти свойства управляют адресами, фильтрами, ограничениями и другими параметрами, зависящими от провайдера.

- `Address` (`Uri`)
- `WebSocketAddress` (`Uri`)
- `MaxMarketDepth` (`int`)
- `SecuritiesPageSize` (`int`)

## См. также

[Графическое конфигурирование](graphical_configuration_tradernet.md)

[Инициализация адаптера](adapter_initialization_tradernet.md)
