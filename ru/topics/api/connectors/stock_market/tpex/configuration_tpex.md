# Настройки коннектора: TPEx

Перед подключением к TPEx задайте перечисленные ниже свойства адаптера. Список проверен по реализации [TpexMessageAdapter](xref:StockSharp.Tpex.TpexMessageAdapter).

## Основные настройки

Эти параметры показываются первыми в редакторе подключения.

- `Address` (`Uri`)
- `Market` (`TpexMarkets`)

## Дополнительные настройки

Эти свойства управляют адресами, фильтрами, ограничениями и другими параметрами, зависящими от провайдера.

- `IncludeListedDerivatives` (`bool`)
- `IncludeValuations` (`bool`)
- `CacheTimeout` (`TimeSpan`)
- `MaxHistoryMonths` (`int`)

## См. также

[Графическое конфигурирование](graphical_configuration_tpex.md)

[Инициализация адаптера](adapter_initialization_tpex.md)
