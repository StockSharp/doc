# Настройки коннектора: Settrade

Перед подключением к Settrade задайте перечисленные ниже свойства адаптера. Список проверен по реализации [SettradeMessageAdapter](xref:StockSharp.Settrade.SettradeMessageAdapter).

## Основные настройки

Эти параметры показываются первыми в редакторе подключения.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `AppCode` (`string`)
- `BrokerId` (`string`)
- `Account` (`string`)
- `Pin` (`SecureString`)
- `AccountType` (`SettradeAccountTypes`)
- `IsDemo` (`bool`)

## Дополнительные настройки

Эти свойства управляют параметрами входа, рабочими и тестовыми адресами, а также опросом закрытого состояния.

- `LoginParameters` (`string`)
- `RestEndpoint` (`string`)
- `DemoRestEndpoint` (`string`)
- `MarketDataEndpoint` (`string`)
- `DemoMarketDataEndpoint` (`string`)
- `PollingInterval` (`TimeSpan`)

## См. также

[Графическое конфигурирование](graphical_configuration_settrade.md)

[Инициализация адаптера](adapter_initialization_settrade.md)
