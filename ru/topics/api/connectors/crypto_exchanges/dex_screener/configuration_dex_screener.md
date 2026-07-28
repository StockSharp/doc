# Настройки коннектора: DEX Screener

Перед подключением к DEX Screener задайте перечисленные ниже свойства адаптера. Список проверен по реализации [DexScreenerMessageAdapter](xref:StockSharp.DexScreener.DexScreenerMessageAdapter).

## Основные настройки

Эти параметры показываются первыми в редакторе подключения.

- `RestEndpoint` (`string`)
- `ChainId` (`string`)
- `TokenAddress` (`string`)
- `SearchQuery` (`string`)
- `PriceInUsd` (`bool`)

## Дополнительные настройки

Эти свойства управляют адресами, фильтрами, ограничениями и другими параметрами, зависящими от провайдера.

- `RequestInterval` (`TimeSpan`)
- `PollingInterval` (`TimeSpan`)
- `MaximumItems` (`int`)

## См. также

[Графическое конфигурирование](graphical_configuration_dex_screener.md)

[Инициализация адаптера](adapter_initialization_dex_screener.md)
