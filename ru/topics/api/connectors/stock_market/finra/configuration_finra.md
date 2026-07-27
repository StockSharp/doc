# Настройки коннектора: FINRA

Перед подключением к FINRA задайте перечисленные ниже свойства адаптера. Список проверен по реализации [FinraMessageAdapter](xref:StockSharp.Finra.FinraMessageAdapter).

## Основные настройки

Эти параметры показываются первыми в редакторе подключения.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `IsDemo` (`bool`)
- `DataSet` (`FinraDataSets`)

## Дополнительные настройки

Эти свойства управляют адресами, фильтрами, ограничениями и другими параметрами, зависящими от провайдера.

- `Token` (`SecureString`)
- `WeeklyTierIdentifier` (`string`)
- `WeeklySummaryTypeCode` (`string`)
- `PageSize` (`int`)
- `MaxRecords` (`int`)
- `DataVersion` (`int`)
- `Address` (`Uri`)
- `AuthAddress` (`Uri`)

## См. также

[Графическое конфигурирование](graphical_configuration_finra.md)

[Инициализация адаптера](adapter_initialization_finra.md)
