# Настройки коннектора: SEC API

Перед подключением к SEC API задайте перечисленные ниже свойства адаптера. Список проверен по реализации [SecApiMessageAdapter](xref:StockSharp.SecApi.SecApiMessageAdapter).

## Основные настройки

Эти параметры показываются первыми в редакторе подключения.

- `Token` (`SecureString`)
- `Address` (`Uri`)

## Дополнительные настройки

Эти свойства управляют адресами, фильтрами, ограничениями и другими параметрами, зависящими от провайдера.

- `ActiveOnly` (`bool`)
- `DefaultExchange` (`string`)
- `FormTypes` (`string`)
- `ResultLimit` (`int`)

## См. также

[Графическое конфигурирование](graphical_configuration_sec_api.md)

[Инициализация адаптера](adapter_initialization_sec_api.md)
