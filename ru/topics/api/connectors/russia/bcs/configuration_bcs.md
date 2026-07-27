# Настройки коннектора: BCS

Перед подключением к BCS задайте перечисленные ниже свойства адаптера. Список проверен по реализации [BcsMessageAdapter](xref:StockSharp.Bcs.BcsMessageAdapter).

## Основные настройки

Эти параметры показываются первыми в редакторе подключения.

- `Token` (`SecureString`)
- `IsReadOnly` (`bool`)
- `PortfolioName` (`string`)
- `PollingInterval` (`TimeSpan`)

## Дополнительные настройки

Эти свойства управляют адресами, фильтрами, ограничениями и другими параметрами, зависящими от провайдера.

- `RestEndpoint` (`string`)
- `WebSocketEndpoint` (`string`)

## См. также

[Графическое конфигурирование](graphical_configuration_bcs.md)

[Инициализация адаптера](adapter_initialization_bcs.md)
