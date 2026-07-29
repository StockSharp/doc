# Настройки коннектора: IIFL

Перед подключением к IIFL задайте перечисленные ниже свойства адаптера. Список проверен по реализации [IIFLMessageAdapter](xref:StockSharp.IIFL.IIFLMessageAdapter).

## Основные настройки

Эти параметры показываются первыми в редакторе подключения.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `ClientId` (`string`)

## Дополнительные настройки

Эти свойства управляют аутентификацией и состоянием сессии, адресами провайдера, потоковыми данными и опросом.

- `AuthorizationCode` (`string`)
- `SessionToken` (`SecureString`)
- `PortfolioName` (`string`)
- `RestEndpoint` (`string`)
- `BridgeHost` (`string`)
- `BridgePort` (`int`)
- `TokenValidationEndpoint` (`string`)
- `StreamingEnabled` (`bool`)
- `PollingInterval` (`TimeSpan`)

## См. также

[Графическое конфигурирование](graphical_configuration_iifl.md)

[Инициализация адаптера](adapter_initialization_iifl.md)
