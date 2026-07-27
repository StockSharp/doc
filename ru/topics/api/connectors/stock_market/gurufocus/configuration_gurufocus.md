# Настройки коннектора: GuruFocus

Перед подключением к GuruFocus задайте перечисленные ниже свойства адаптера. Список проверен по реализации [GuruFocusMessageAdapter](xref:StockSharp.GuruFocus.GuruFocusMessageAdapter).

## Основные настройки

Эти параметры показываются первыми в редакторе подключения.

- `Token` (`SecureString`)
- `Address` (`Uri`)

## Дополнительные настройки

Эти свойства управляют адресами, фильтрами, ограничениями и другими параметрами, зависящими от провайдера.

- `RegionCode` (`string`)
- `PageSize` (`int`)
- `MaxLookupPages` (`int`)
- `DatasetLimit` (`int`)
- `NewsLimit` (`int`)
- `FilingFormType` (`string`)
- `GuruTradeActions` (`string`)

## См. также

[Графическое конфигурирование](graphical_configuration_gurufocus.md)

[Инициализация адаптера](adapter_initialization_gurufocus.md)
