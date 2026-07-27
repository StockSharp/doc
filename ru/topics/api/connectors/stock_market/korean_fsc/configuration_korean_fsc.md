# Настройки коннектора: Korean FSC

Перед подключением к Korean FSC задайте перечисленные ниже свойства адаптера. Список проверен по реализации [KoreanFscMessageAdapter](xref:StockSharp.KoreanFsc.KoreanFscMessageAdapter).

## Основные настройки

Эти параметры показываются первыми в редакторе подключения.

- `Token` (`SecureString`)
- `DataSet` (`KoreanFscDataSets`)

## Дополнительные настройки

Эти свойства управляют адресами, фильтрами, ограничениями и другими параметрами, зависящими от провайдера.

- `Address` (`Uri`)
- `Market` (`KoreanFscMarkets`)
- `ReferenceDate` (`DateTime?`)
- `LatestSearchDays` (`int`)
- `PageSize` (`int`)
- `MaxPages` (`int`)

## См. также

[Графическое конфигурирование](graphical_configuration_korean_fsc.md)

[Инициализация адаптера](adapter_initialization_korean_fsc.md)
