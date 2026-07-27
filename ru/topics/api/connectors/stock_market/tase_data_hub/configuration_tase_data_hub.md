# Настройки коннектора: TASE Data Hub

Перед подключением к TASE Data Hub задайте перечисленные ниже свойства адаптера. Список проверен по реализации [TaseDataHubMessageAdapter](xref:StockSharp.TaseDataHub.TaseDataHubMessageAdapter).

## Основные настройки

Эти параметры показываются первыми в редакторе подключения.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)

## Дополнительные настройки

Эти свойства управляют адресами, фильтрами, ограничениями и другими параметрами, зависящими от провайдера.

- `Address` (`Uri`)
- `Scope` (`string`)
- `SecurityLookupDays` (`int`)
- `ReferenceCacheTimeout` (`TimeSpan`)

## См. также

[Графическое конфигурирование](graphical_configuration_tase_data_hub.md)

[Инициализация адаптера](adapter_initialization_tase_data_hub.md)
