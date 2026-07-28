# Настройки коннектора: KyberSwap

Перед подключением к KyberSwap задайте перечисленные ниже свойства адаптера. Список проверен по реализации [KyberSwapMessageAdapter](xref:StockSharp.KyberSwap.KyberSwapMessageAdapter).

## Основные настройки

Эти параметры показываются первыми в редакторе подключения.

- `ClientId` (`string`)
- `Chain` (`KyberSwapChains`)
- `WalletAddress` (`string`)
- `PrivateKey` (`SecureString`)
- `ApiEndpoint` (`string`)
- `RpcEndpoint` (`string`)

## Дополнительные настройки

Эти свойства управляют адресами, фильтрами, ограничениями и другими параметрами, зависящими от провайдера.

- `Markets` (`string`)
- `ProbeVolume` (`decimal`)
- `SlippageTolerance` (`decimal`)
- `PollingInterval` (`TimeSpan`)
- `ReceiptTimeout` (`TimeSpan`)
- `TransactionLifetime` (`TimeSpan`)
- `IsAutoApprove` (`bool`)

## См. также

[Графическое конфигурирование](graphical_configuration_kyber_swap.md)

[Инициализация адаптера](adapter_initialization_kyber_swap.md)
