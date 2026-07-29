# Настройки коннектора: SimFin

Перед подключением к SimFin задайте перечисленные ниже свойства адаптера. Список проверен по реализации [SimFinMessageAdapter](xref:StockSharp.SimFin.SimFinMessageAdapter).

## Основные настройки

Эти параметры показываются первыми в редакторе подключения.

- `Key` (`SecureString`)

## Дополнительные настройки

Эти свойства управляют адресами провайдера, интервалами запросов, фильтрами, параметрами данных и лимитами результатов.

- `RestEndpoint` (`Uri`)
- `RequestInterval` (`TimeSpan`)
- `StatementTypes` (`string`)
- `Period` (`string`)
- `AsReported` (`bool`)
- `IncludeRatios` (`bool`)
- `MaximumRecords` (`int`)

## См. также

[Графическое конфигурирование](graphical_configuration_simfin.md)

[Инициализация адаптера](adapter_initialization_simfin.md)
