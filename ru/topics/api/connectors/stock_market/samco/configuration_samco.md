# Настройки коннектора: Samco

Перед подключением к Samco задайте перечисленные ниже свойства адаптера. Список проверен по реализации [SamcoMessageAdapter](xref:StockSharp.Samco.SamcoMessageAdapter).

## Основные настройки

Эти параметры показываются первыми в редакторе подключения.

- `Key` (`SecureString`)

## Дополнительные настройки

Эти свойства управляют аутентификацией и состоянием сессии, адресами провайдера, потоковыми данными и опросом.

- `Secret` (`SecureString`)
- `SessionToken` (`SecureString`)
- `RestEndpoint` (`string`)
- `InstrumentEndpoint` (`string`)
- `StreamingEndpoint` (`string`)
- `StreamingEnabled` (`bool`)
- `PollingInterval` (`TimeSpan`)

## См. также

[Графическое конфигурирование](graphical_configuration_samco.md)

[Инициализация адаптера](adapter_initialization_samco.md)
