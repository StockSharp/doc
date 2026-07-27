# Настройки коннектора: EXANTE

Перед подключением к EXANTE задайте перечисленные ниже свойства адаптера. Список проверен по реализации [ExanteMessageAdapter](xref:StockSharp.Exante.ExanteMessageAdapter).

## Основные настройки

Эти параметры показываются первыми в редакторе подключения.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `IsDemo` (`bool`)
- `SummaryCurrency` (`string`)
- `PollingInterval` (`TimeSpan`)

## Дополнительные настройки

Эти свойства управляют адресами, фильтрами, ограничениями и другими параметрами, зависящими от провайдера.

- `MaxMarketDepth` (`int`)
- `HistoryRequestSize` (`int`)
- `LiveAddress` (`Uri`)
- `DemoAddress` (`Uri`)

## См. также

[Графическое конфигурирование](graphical_configuration_exante.md)

[Инициализация адаптера](adapter_initialization_exante.md)
