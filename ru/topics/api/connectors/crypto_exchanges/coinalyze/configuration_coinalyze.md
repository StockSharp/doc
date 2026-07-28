# Настройки коннектора: Coinalyze

Перед подключением к Coinalyze задайте перечисленные ниже свойства адаптера. Список проверен по реализации [CoinalyzeMessageAdapter](xref:StockSharp.Coinalyze.CoinalyzeMessageAdapter).

## Основные настройки

Эти параметры показываются первыми в редакторе подключения.

- `Token` (`SecureString`)
- `RestEndpoint` (`string`)
- `MarketType` (`CoinalyzeMarketTypes`)
- `CandleMetric` (`CoinalyzeCandleMetrics`)
- `Exchange` (`string`)
- `ConvertToUsd` (`bool`)

## Дополнительные настройки

Эти свойства управляют адресами, фильтрами, ограничениями и другими параметрами, зависящими от провайдера.

- `RequestInterval` (`TimeSpan`)
- `MaximumItems` (`int`)
- `HistoryLimit` (`int`)

## См. также

[Графическое конфигурирование](graphical_configuration_coinalyze.md)

[Инициализация адаптера](adapter_initialization_coinalyze.md)
