# Настройки коннектора: TWSE

Перед подключением к TWSE задайте перечисленные ниже свойства адаптера. Список проверен по реализации [TwseMessageAdapter](xref:StockSharp.Twse.TwseMessageAdapter).

## Основные настройки

Эти параметры показываются первыми в редакторе подключения.

- `Address` (`Uri`)

## Дополнительные настройки

Эти свойства управляют адресами, фильтрами, ограничениями и другими параметрами, зависящими от провайдера.

- `IncludeProfiles` (`bool`)
- `IncludeValuations` (`bool`)
- `CacheTimeout` (`TimeSpan`)

## См. также

[Графическое конфигурирование](graphical_configuration_twse_openapi.md)

[Инициализация адаптера](adapter_initialization_twse_openapi.md)
