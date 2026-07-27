# Настройки коннектора: Nubra

Перед подключением к Nubra задайте перечисленные ниже свойства адаптера. Список проверен по реализации [NubraMessageAdapter](xref:StockSharp.Nubra.NubraMessageAdapter).

## Основные настройки

Эти параметры показываются первыми в редакторе подключения.

- `Token` (`SecureString`)
- `DeviceId` (`string`)
- `IsDemo` (`bool`)

## Дополнительные настройки

Эти свойства управляют адресами, фильтрами, ограничениями и другими параметрами, зависящими от провайдера.

- `Phone` (`string`)
- `Mpin` (`SecureString`)
- `TotpSecret` (`SecureString`)
- `PortfolioName` (`string`)
- `DefaultProduct` (`NubraProducts`)
- `PollingInterval` (`TimeSpan`)
- `ReconnectAttempts` (`int`)
- `RestAddress` (`Uri`)
- `UatRestAddress` (`Uri`)
- `MarketDataAddress` (`Uri`)
- `UatMarketDataAddress` (`Uri`)

## См. также

[Графическое конфигурирование](graphical_configuration_nubra.md)

[Инициализация адаптера](adapter_initialization_nubra.md)
