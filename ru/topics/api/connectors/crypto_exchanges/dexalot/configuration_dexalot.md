# Настройки коннектора: Dexalot

Перед подключением к Dexalot задайте перечисленные ниже свойства адаптера. Список проверен по реализации [DexalotMessageAdapter](xref:StockSharp.Dexalot.DexalotMessageAdapter).

## Основные настройки

Эти параметры показываются первыми в редакторе подключения.

- `RestEndpoint` (`string`)
- `WebSocketEndpoint` (`string`)
- `RpcEndpoint` (`string`)
- `WalletAddress` (`string`)
- `PrivateKey` (`SecureString`)

## Дополнительные настройки

Эти свойства управляют адресами контрактов, фильтрами пар, глубиной стакана, опросом и выполнением транзакций.

- `TradePairsAddress` (`string`)
- `PortfolioAddress` (`string`)
- `Pairs` (`string`)
- `OrderBookDepth` (`int`)
- `PrivatePollingInterval` (`TimeSpan`)
- `ReceiptTimeout` (`TimeSpan`)
- `SelfTradePrevention` (`DexalotSelfTradePrevention`)

## См. также

[Графическое конфигурирование](graphical_configuration_dexalot.md)

[Инициализация адаптера](adapter_initialization_dexalot.md)
