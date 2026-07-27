# Настройки коннектора: KRX Open API

Перед подключением к KRX Open API задайте перечисленные ниже свойства адаптера. Список проверен по реализации [KrxOpenApiMessageAdapter](xref:StockSharp.KrxOpenApi.KrxOpenApiMessageAdapter).

## Основные настройки

Эти параметры показываются первыми в редакторе подключения.

- `Token` (`SecureString`)
- `IsDemo` (`bool`)
- `DataSet` (`KrxDataSets`)

## Дополнительные настройки

Эти свойства управляют адресами, фильтрами, ограничениями и другими параметрами, зависящими от провайдера.

- `ReferenceDate` (`DateTime?`)
- `LatestSearchDays` (`int`)
- `MaxRequests` (`int`)
- `Address` (`Uri`)
- `SampleAddress` (`Uri`)

## См. также

[Графическое конфигурирование](graphical_configuration_krx_open_api.md)

[Инициализация адаптера](adapter_initialization_krx_open_api.md)
