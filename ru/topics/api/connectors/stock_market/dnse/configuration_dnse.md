# Настройки коннектора: DNSE

Перед подключением к DNSE задайте перечисленные ниже свойства адаптера. Список проверен по реализации [DnseMessageAdapter](xref:StockSharp.Dnse.DnseMessageAdapter).

## Основные настройки

Эти параметры показываются первыми в редакторе подключения.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `TradingToken` (`SecureString`)
- `Account` (`string`)

## Дополнительные настройки

Эти свойства управляют адресами, фильтрами, ограничениями и другими параметрами, зависящими от провайдера.

- `OtpType` (`DnseOtpTypes`)
- `OneTimePassword` (`SecureString`)
- `RequestEmailOtpOnConnect` (`bool`)
- `DefaultLoanPackageId` (`int`)
- `DefaultBoardId` (`string`)
- `MarketDataPriceMultiplier` (`decimal`)
- `AccountPollingInterval` (`TimeSpan`)
- `LookupLimit` (`int`)
- `ApiVersion` (`string`)
- `DateHeaderName` (`string`)
- `RestAddress` (`Uri`)
- `WebSocketAddress` (`Uri`)

## См. также

[Графическое конфигурирование](graphical_configuration_dnse.md)

[Инициализация адаптера](adapter_initialization_dnse.md)
