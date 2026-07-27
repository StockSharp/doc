# Настройки коннектора: Directa

Перед подключением к Directa задайте перечисленные ниже свойства адаптера. Список проверен по реализации [DirectaMessageAdapter](xref:StockSharp.Directa.DirectaMessageAdapter).

## Основные настройки

Эти параметры показываются первыми в редакторе подключения.

- `Address` (`EndPoint`)
- `DataAddress` (`EndPoint`)
- `HistoryAddress` (`EndPoint`)

## Дополнительные настройки

Эти свойства управляют адресами, фильтрами, ограничениями и другими параметрами, зависящими от провайдера.

- `RequestTimeout` (`TimeSpan`)
- `AutoConfirmOrders` (`bool`)
- `MaxMarketDepth` (`int`)
- `TimeZoneId` (`string`)

## См. также

[Графическое конфигурирование](graphical_configuration_directa.md)

[Инициализация адаптера](adapter_initialization_directa.md)
