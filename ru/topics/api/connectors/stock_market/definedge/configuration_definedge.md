# Настройки коннектора: Definedge

Перед подключением к Definedge задайте перечисленные ниже свойства адаптера. Список проверен по реализации [DefinedgeMessageAdapter](xref:StockSharp.Definedge.DefinedgeMessageAdapter).

## Основные настройки

Эти параметры показываются первыми в редакторе подключения.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `Token` (`SecureString`)
- `WebSocketToken` (`SecureString`)
- `UserId` (`string`)
- `AccountId` (`string`)

## Дополнительные настройки

Эти свойства управляют адресами, фильтрами, ограничениями и другими параметрами, зависящими от провайдера.

- `OneTimePassword` (`SecureString`)
- `DefaultProduct` (`DefinedgeProducts`)
- `AlgoId` (`string`)
- `Address` (`Uri`)
- `LoginAddress` (`Uri`)
- `HistoryAddress` (`Uri`)
- `InstrumentMasterAddress` (`Uri`)
- `WebSocketAddress` (`Uri`)
- `PollingInterval` (`TimeSpan`)

## См. также

[Графическое конфигурирование](graphical_configuration_definedge.md)

[Инициализация адаптера](adapter_initialization_definedge.md)
