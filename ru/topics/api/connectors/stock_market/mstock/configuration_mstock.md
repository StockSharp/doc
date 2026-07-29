# Настройки коннектора: m.Stock

Перед подключением к m.Stock задайте перечисленные ниже свойства адаптера. Список проверен по реализации [MStockMessageAdapter](xref:StockSharp.MStock.MStockMessageAdapter).

## Основные настройки

Эти параметры показываются первыми в редакторе подключения.

- `Key` (`SecureString`)
- `ClientCode` (`string`)

## Дополнительные настройки

Эти свойства управляют аутентификацией и состоянием сессии, адресами провайдера, потоковыми данными и опросом.

- `Password` (`SecureString`)
- `Otp` (`SecureString`)
- `UseTotp` (`bool`)
- `RefreshToken` (`SecureString`)
- `AccessToken` (`SecureString`)
- `RestEndpoint` (`string`)
- `StreamingEndpoint` (`string`)
- `StreamingEnabled` (`bool`)
- `PollingInterval` (`TimeSpan`)

## См. также

[Графическое конфигурирование](graphical_configuration_mstock.md)

[Инициализация адаптера](adapter_initialization_mstock.md)
