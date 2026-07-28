# Настройки коннектора: CoinSwitch PRO

Перед подключением к CoinSwitch PRO задайте перечисленные ниже свойства адаптера. Список проверен по реализации [CoinSwitchMessageAdapter](xref:StockSharp.CoinSwitch.CoinSwitchMessageAdapter).

## Основные настройки

Эти параметры показываются первыми в редакторе подключения.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `ProductType` (`CoinSwitchProductTypes`)
- `SpotExchange` (`string`)
- `RestEndpoint` (`string`)
- `HftEndpoint` (`string`)
- `WebSocketEndpoint` (`string`)

## Дополнительные настройки

Эти свойства управляют адресами, фильтрами, ограничениями и другими параметрами, зависящими от провайдера.

- `PollingInterval` (`TimeSpan`)

## См. также

[Графическое конфигурирование](graphical_configuration_coinswitch.md)

[Инициализация адаптера](adapter_initialization_coinswitch.md)
