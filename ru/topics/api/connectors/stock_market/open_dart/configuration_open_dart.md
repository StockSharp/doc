# Настройки коннектора: Open DART

Перед подключением к Open DART задайте перечисленные ниже свойства адаптера. Список проверен по реализации [OpenDartMessageAdapter](xref:StockSharp.OpenDart.OpenDartMessageAdapter).

## Основные настройки

Эти параметры показываются первыми в редакторе подключения.

- `Token` (`SecureString`)

## Дополнительные настройки

Эти свойства управляют адресами, фильтрами, ограничениями и другими параметрами, зависящими от провайдера.

- `Address` (`Uri`)
- `DisclosureAddress` (`Uri`)
- `DisclosureType` (`OpenDartDisclosureTypes`)
- `CorporationClass` (`OpenDartCorporationClasses`)
- `FinalReportsOnly` (`bool`)
- `BusinessYear` (`int?`)
- `ReportType` (`OpenDartReportTypes`)
- `FinancialSearchYears` (`int`)
- `MaxPages` (`int`)

## См. также

[Графическое конфигурирование](graphical_configuration_open_dart.md)

[Инициализация адаптера](adapter_initialization_open_dart.md)
