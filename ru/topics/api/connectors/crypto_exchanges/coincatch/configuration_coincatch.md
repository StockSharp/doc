# Настройки коннектора: CoinCatch

Перед подключением к CoinCatch задайте перечисленные ниже свойства адаптера. Список проверен по реализации [CoinCatchMessageAdapter](xref:StockSharp.CoinCatch.CoinCatchMessageAdapter).

## Основные настройки

Эти параметры показываются первыми в редакторе подключения.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `Passphrase` (`SecureString`)
- `ProductType` (`CoinCatchProductTypes`)
- `RestEndpoint` (`string`)
- `PublicWebSocketEndpoint` (`string`)
- `PrivateWebSocketEndpoint` (`string`)
- `PollingInterval` (`TimeSpan`)

## См. также

[Графическое конфигурирование](graphical_configuration_coincatch.md)

[Инициализация адаптера](adapter_initialization_coincatch.md)
