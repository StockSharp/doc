# Настройки коннектора: OpenFIGI

Перед подключением к OpenFIGI задайте перечисленные ниже свойства адаптера. Список проверен по реализации [OpenFigiMessageAdapter](xref:StockSharp.OpenFigi.OpenFigiMessageAdapter).

## Основные настройки

Эти параметры показываются первыми в редакторе подключения.

- `Key` (`SecureString`)

## Дополнительные настройки

Эти свойства управляют адресами провайдера, интервалами запросов, фильтрами, параметрами данных и лимитами результатов.

- `RestEndpoint` (`Uri`)
- `RequestInterval` (`TimeSpan`)
- `MaximumPages` (`int`)
- `MaximumResults` (`int`)
- `ExchangeCode` (`string`)
- `MicCode` (`string`)
- `Currency` (`string`)
- `MarketSector` (`string`)
- `SecurityType2` (`string`)
- `IncludeUnlistedEquities` (`bool`)

## См. также

[Графическое конфигурирование](graphical_configuration_openfigi.md)

[Инициализация адаптера](adapter_initialization_openfigi.md)
