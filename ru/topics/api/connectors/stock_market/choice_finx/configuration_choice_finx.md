# Настройки коннектора: Choice FinX

Перед подключением к Choice FinX задайте перечисленные ниже свойства адаптера. Список проверен по реализации [ChoiceFinXMessageAdapter](xref:StockSharp.ChoiceFinX.ChoiceFinXMessageAdapter).

## Основные настройки

Эти параметры показываются первыми в редакторе подключения.

- `Token` (`SecureString`)
- `AuthorizationHeader` (`string`)
- `AuthorizationScheme` (`string`)

## Дополнительные настройки

Эти свойства управляют адресами, фильтрами, ограничениями и другими параметрами, зависящими от провайдера.

- `VendorId` (`string`)
- `VendorKey` (`SecureString`)
- `WebSocketToken` (`SecureString`)
- `DefaultProduct` (`ChoiceFinXProducts`)
- `PortfolioName` (`string`)
- `ModeType` (`string`)
- `Mode` (`int?`)
- `DeviceId` (`string`)
- `PriceDivisor` (`decimal`)
- `Address` (`Uri`)
- `WebSocketAddress` (`Uri`)
- `PollingInterval` (`TimeSpan`)

## См. также

[Графическое конфигурирование](graphical_configuration_choice_finx.md)

[Инициализация адаптера](adapter_initialization_choice_finx.md)
