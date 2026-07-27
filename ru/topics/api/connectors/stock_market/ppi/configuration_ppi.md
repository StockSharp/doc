# Настройки коннектора: PPI

Перед подключением к PPI задайте перечисленные ниже свойства адаптера. Список проверен по реализации [PpiMessageAdapter](xref:StockSharp.Ppi.PpiMessageAdapter).

## Основные настройки

Эти параметры показываются первыми в редакторе подключения.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `AuthorizedClient` (`string`)
- `ClientKey` (`SecureString`)
- `IsDemo` (`bool`)
- `Account` (`string`)

## Дополнительные настройки

Эти свойства управляют адресами, фильтрами, ограничениями и другими параметрами, зависящими от провайдера.

- `Token` (`SecureString`)
- `RefreshToken` (`SecureString`)
- `DefaultMarket` (`string`)
- `DefaultInstrumentType` (`string`)
- `DefaultSettlement` (`string`)
- `AccountPollingInterval` (`TimeSpan`)
- `LookupLimit` (`int`)
- `RestAddress` (`Uri`)
- `SandboxRestAddress` (`Uri`)
- `RealtimeAddress` (`Uri`)
- `SandboxRealtimeAddress` (`Uri`)

## См. также

[Графическое конфигурирование](graphical_configuration_ppi.md)

[Инициализация адаптера](adapter_initialization_ppi.md)
