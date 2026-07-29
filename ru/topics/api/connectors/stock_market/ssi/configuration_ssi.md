# Настройки коннектора: SSI

Перед подключением к SSI задайте перечисленные ниже свойства адаптера. Список проверен по реализации [SSIMessageAdapter](xref:StockSharp.SSI.SSIMessageAdapter).

## Основные настройки

Эти параметры показываются первыми в редакторе подключения.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `ClientId` (`string`)
- `Account` (`string`)

## Дополнительные настройки

Эти свойства управляют аутентификацией и состоянием сессии, адресами провайдера, потоковыми данными и опросом.

- `PrivateKey` (`SecureString`)
- `Otp` (`SecureString`)
- `RestEndpoint` (`string`)
- `StreamingEndpoint` (`string`)
- `PollingInterval` (`TimeSpan`)

## См. также

[Графическое конфигурирование](graphical_configuration_ssi.md)

[Инициализация адаптера](adapter_initialization_ssi.md)
