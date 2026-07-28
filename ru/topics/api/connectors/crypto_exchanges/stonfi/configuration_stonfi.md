# Настройки коннектора: STON.fi

Перед подключением к STON.fi задайте перечисленные ниже свойства адаптера. Список проверен по реализации [StonFiMessageAdapter](xref:StockSharp.StonFi.StonFiMessageAdapter).

## Основные настройки

Эти параметры показываются первыми в редакторе подключения.

- `ApiEndpoint` (`string`)
- `TonCenterEndpoint` (`string`)
- `TonCenterApiKey` (`SecureString`)
- `WalletAddress` (`string`)
- `Mnemonic` (`SecureString`)

## Дополнительные настройки

Эти свойства управляют параметрами кошелька, выбором пулов, ограничениями, опросом, историей и выполнением транзакций.

- `WalletSubwalletId` (`uint`)
- `WalletRevision` (`int`)
- `Pools` (`string`)
- `PoolLimit` (`int`)
- `ProbeVolume` (`decimal`)
- `SlippageTolerance` (`decimal`)
- `PollingInterval` (`TimeSpan`)
- `HistoryBlockLimit` (`int`)
- `PrivatePollingInterval` (`TimeSpan`)
- `TransactionTimeout` (`TimeSpan`)

## См. также

[Графическое конфигурирование](graphical_configuration_stonfi.md)

[Инициализация адаптера](adapter_initialization_stonfi.md)
