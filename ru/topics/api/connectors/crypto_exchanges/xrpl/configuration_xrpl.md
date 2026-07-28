# Настройки коннектора: XRPL DEX

Перед подключением к XRPL DEX задайте перечисленные ниже свойства адаптера. Список проверен по реализации [XrplMessageAdapter](xref:StockSharp.Xrpl.XrplMessageAdapter).

## Основные настройки

Эти параметры показываются первыми в редакторе подключения.

- `RpcEndpoint` (`string`)
- `StreamingEndpoint` (`string`)
- `Account` (`string`)
- `Seed` (`SecureString`)

## Дополнительные настройки

Эти свойства управляют выбором рынков, глубиной стакана, историей, комиссиями, опросом и защитой транзакций.

- `Markets` (`string`)
- `DomainId` (`string`)
- `OrderBookDepth` (`int`)
- `HistoryLedgerLimit` (`int`)
- `FeeMultiplier` (`decimal`)
- `LastLedgerOffset` (`int`)
- `MarketOrderProtection` (`decimal`)
- `PollingInterval` (`TimeSpan`)

## См. также

[Графическое конфигурирование](graphical_configuration_xrpl.md)

[Инициализация адаптера](adapter_initialization_xrpl.md)
