# Настройки коннектора: CoinGlass

Перед подключением к CoinGlass задайте перечисленные ниже свойства адаптера. Список проверен по реализации [CoinGlassMessageAdapter](xref:StockSharp.CoinGlass.CoinGlassMessageAdapter).

## Основные настройки

Эти параметры показываются первыми в редакторе подключения.

- `Token` (`SecureString`)
- `RestEndpoint` (`string`)
- `MarketType` (`CoinGlassMarketTypes`)
- `CandleMetric` (`CoinGlassCandleMetrics`)
- `Exchange` (`string`)
- `Symbol` (`string`)

## Дополнительные настройки

Эти свойства управляют адресами, фильтрами, ограничениями и другими параметрами, зависящими от провайдера.

- `RequestInterval` (`TimeSpan`)
- `PollingInterval` (`TimeSpan`)
- `MaximumItems` (`int`)
- `HistoryLimit` (`int`)

## См. также

[Графическое конфигурирование](graphical_configuration_coinglass.md)

[Инициализация адаптера](adapter_initialization_coinglass.md)
