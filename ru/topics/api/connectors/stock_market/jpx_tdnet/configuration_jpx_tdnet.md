# Настройки коннектора: JPX TDnet

Перед подключением к JPX TDnet задайте перечисленные ниже свойства адаптера. Список проверен по реализации [JpxTdnetMessageAdapter](xref:StockSharp.JpxTdnet.JpxTdnetMessageAdapter).

## Основные настройки

Эти параметры показываются первыми в редакторе подключения.

- `Token` (`SecureString`)

## Дополнительные настройки

Эти свойства управляют адресами, фильтрами, ограничениями и другими параметрами, зависящими от провайдера.

- `Address` (`Uri`)
- `ViewerAddress` (`Uri`)
- `IndexMode` (`JpxTdnetIndexModes`)
- `DefaultLookupDays` (`int`)
- `MaxDays` (`int`)
- `SecurityLookupDays` (`int`)
- `RequestInterval` (`TimeSpan`)
- `MaxDocumentSizeMb` (`int`)

## См. также

[Графическое конфигурирование](graphical_configuration_jpx_tdnet.md)

[Инициализация адаптера](adapter_initialization_jpx_tdnet.md)
