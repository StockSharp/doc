# Настройки коннектора: Euronext Web Services

Перед подключением к Euronext Web Services задайте перечисленные ниже свойства адаптера. Список проверен по реализации [EuronextWebServicesMessageAdapter](xref:StockSharp.EuronextWebServices.EuronextWebServicesMessageAdapter).

## Основные настройки

Эти параметры показываются первыми в редакторе подключения.

- `Token` (`SecureString`)

## Дополнительные настройки

Эти свойства управляют адресами, фильтрами, ограничениями и другими параметрами, зависящими от провайдера.

- `Address` (`Uri`)
- `SessionQuality` (`EuronextSessionQualities`)
- `IntradayDepth` (`int`)

## См. также

[Графическое конфигурирование](graphical_configuration_euronext_web_services.md)

[Инициализация адаптера](adapter_initialization_euronext_web_services.md)
