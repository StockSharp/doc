# Настройки коннектора: GLEIF

Перед подключением к GLEIF задайте перечисленные ниже свойства адаптера. Список проверен по реализации [GleifMessageAdapter](xref:StockSharp.Gleif.GleifMessageAdapter).

## Основные настройки

Эти параметры показываются первыми в редакторе подключения.

- `Address` (`Uri`)

## Дополнительные настройки

Эти свойства управляют адресами, фильтрами, ограничениями и другими параметрами, зависящими от провайдера.

- `ActiveOnly` (`bool`)
- `ExpandIsins` (`bool`)
- `PageSize` (`int`)
- `MaxPages` (`int`)

## См. также

[Графическое конфигурирование](graphical_configuration_gleif.md)

[Инициализация адаптера](adapter_initialization_gleif.md)
