# Настройки коннектора: Chainflip

Перед подключением к Chainflip задайте перечисленные ниже свойства адаптера. Список проверен по реализации [ChainflipMessageAdapter](xref:StockSharp.Chainflip.ChainflipMessageAdapter).

## Основные настройки

Эти параметры показываются первыми в редакторе подключения.

- `StateRpcEndpoint` (`string`)
- `BackendEndpoint` (`string`)
- `EthereumRpcEndpoint` (`string`)
- `ArbitrumRpcEndpoint` (`string`)
- `WalletAddress` (`string`)
- `PrivateKey` (`SecureString`)

## Дополнительные настройки

Эти свойства управляют целевыми адресами, фильтрами пулов, опросом, глубиной стакана и выполнением транзакций.

- `BitcoinAddress` (`string`)
- `SolanaAddress` (`string`)
- `AssethubAddress` (`string`)
- `PolkadotAddress` (`string`)
- `TronAddress` (`string`)
- `Pools` (`string`)
- `ProbeVolume` (`decimal`)
- `OrderBookDepth` (`int`)
- `PollingInterval` (`TimeSpan`)
- `MaxBlocksPerPoll` (`int`)
- `InitialTickBlocks` (`int`)
- `SlippageTolerance` (`decimal`)
- `RetryDurationBlocks` (`int`)
- `ReceiptTimeout` (`TimeSpan`)
- `IsAutoApprove` (`bool`)

## См. также

[Графическое конфигурирование](graphical_configuration_chainflip.md)

[Инициализация адаптера](adapter_initialization_chainflip.md)
