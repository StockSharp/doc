# Настройки коннектора: Quiver Quantitative

Перед подключением к Quiver Quantitative задайте перечисленные ниже свойства адаптера. Список проверен по реализации [QuiverQuantMessageAdapter](xref:StockSharp.QuiverQuant.QuiverQuantMessageAdapter).

## Основные настройки

Эти параметры показываются первыми в редакторе подключения.

- `Token` (`SecureString`)
- `Address` (`Uri`)

## Дополнительные настройки

Эти свойства управляют адресами, фильтрами, ограничениями и другими параметрами, зависящими от провайдера.

- `PageSize` (`int`)
- `MaxPages` (`int`)
- `DatasetLimit` (`int`)
- `NewsLimit` (`int`)
- `LimitInsiderCodes` (`bool`)
- `MostRecentInstitutional` (`bool`)
- `IncludeNewFunds` (`bool`)
- `CorporateDonorCycle` (`string`)

## См. также

[Графическое конфигурирование](graphical_configuration_quiver_quant.md)

[Инициализация адаптера](adapter_initialization_quiver_quant.md)
