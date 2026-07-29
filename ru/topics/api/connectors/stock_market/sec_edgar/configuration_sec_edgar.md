# Настройки коннектора: SEC EDGAR

Перед подключением к SEC EDGAR задайте перечисленные ниже свойства адаптера. Список проверен по реализации [SecEdgarMessageAdapter](xref:StockSharp.SecEdgar.SecEdgarMessageAdapter).

## Основные настройки

Эти параметры показываются первыми в редакторе подключения.

- `DataEndpoint` (`Uri`)
- `UserAgent` (`string`)

## Дополнительные настройки

Эти свойства управляют адресами провайдера, интервалами запросов, фильтрами, параметрами данных и лимитами результатов.

- `WebsiteEndpoint` (`Uri`)
- `RequestInterval` (`TimeSpan`)
- `Forms` (`string`)
- `MaximumHistoricalFiles` (`int`)
- `MaximumFacts` (`int`)

## См. также

[Графическое конфигурирование](graphical_configuration_sec_edgar.md)

[Инициализация адаптера](adapter_initialization_sec_edgar.md)
