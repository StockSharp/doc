# Настройки коннектора: InvertirOnline

Перед подключением к InvertirOnline задайте перечисленные ниже свойства адаптера. Список проверен по реализации [InvertirOnlineMessageAdapter](xref:StockSharp.InvertirOnline.InvertirOnlineMessageAdapter).

## Основные настройки

Эти параметры показываются первыми в редакторе подключения.

- `Login` (`string`)
- `Password` (`SecureString`)
- `IsDemo` (`bool`)
- `PortfolioName` (`string`)

## Дополнительные настройки

Эти свойства управляют адресами, фильтрами, ограничениями и другими параметрами, зависящими от провайдера.

- `Token` (`SecureString`)
- `RefreshToken` (`SecureString`)
- `DefaultCountry` (`InvertirOnlineCountries`)
- `DefaultMarket` (`string`)
- `DefaultInstrumentType` (`string`)
- `DefaultSettlement` (`InvertirOnlineSettlements`)
- `AdjustedHistory` (`bool`)
- `MarketDataPollingInterval` (`TimeSpan`)
- `AccountPollingInterval` (`TimeSpan`)
- `LookupLimit` (`int`)
- `RestAddress` (`Uri`)
- `SandboxRestAddress` (`Uri`)

## См. также

[Графическое конфигурирование](graphical_configuration_invertironline.md)

[Инициализация адаптера](adapter_initialization_invertironline.md)
