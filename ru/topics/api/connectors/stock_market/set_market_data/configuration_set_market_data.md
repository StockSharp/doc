# Настройки коннектора: SET Market Data

Перед подключением к SET Market Data задайте перечисленные ниже свойства адаптера. Список проверен по реализации [SetMarketDataMessageAdapter](xref:StockSharp.SetMarketData.SetMarketDataMessageAdapter).

## Основные настройки

Эти параметры показываются первыми в редакторе подключения.

- `Token` (`SecureString`)

## Дополнительные настройки

Эти свойства управляют адресами, фильтрами, ограничениями и другими параметрами, зависящими от провайдера.

- `Address` (`Uri`)
- `DataMode` (`SetMarketDataModes`)
- `Markets` (`string`)
- `IndexSectors` (`string`)
- `SecurityTypeCodes` (`string`)
- `IncludeOddLots` (`bool`)
- `IncludeIndices` (`bool`)

## См. также

[Графическое конфигурирование](graphical_configuration_set_market_data.md)

[Инициализация адаптера](adapter_initialization_set_market_data.md)
