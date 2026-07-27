# Настройки коннектора: EDINET

Перед подключением к EDINET задайте перечисленные ниже свойства адаптера. Список проверен по реализации [EdinetMessageAdapter](xref:StockSharp.Edinet.EdinetMessageAdapter).

## Основные настройки

Эти параметры показываются первыми в редакторе подключения.

- `Token` (`SecureString`)

## Дополнительные настройки

Эти свойства управляют адресами, фильтрами, ограничениями и другими параметрами, зависящими от провайдера.

- `Address` (`Uri`)
- `CodeListAddress` (`Uri`)
- `ViewerAddress` (`Uri`)
- `DisclosureType` (`EdinetDisclosureTypes`)
- `ListedOnly` (`bool`)
- `IncludeWithdrawn` (`bool`)
- `IncludeUnavailable` (`bool`)
- `DefaultLookupDays` (`int`)
- `MaxDays` (`int`)
- `RequestInterval` (`TimeSpan`)
- `MaxDocumentSizeMb` (`int`)
- `CodeListCacheTimeout` (`TimeSpan`)

## См. также

[Графическое конфигурирование](graphical_configuration_edinet.md)

[Инициализация адаптера](adapter_initialization_edinet.md)
