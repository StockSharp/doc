# Настройки коннектора: Paytm Money

Перед подключением к Paytm Money задайте перечисленные ниже свойства адаптера. Список проверен по реализации [PaytmMoneyMessageAdapter](xref:StockSharp.PaytmMoney.PaytmMoneyMessageAdapter).

## Основные настройки

Эти параметры показываются первыми в редакторе подключения.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `Token` (`SecureString`)
- `ReadAccessToken` (`SecureString`)
- `PublicAccessToken` (`SecureString`)

## Дополнительные настройки

Эти свойства управляют адресами, фильтрами, ограничениями и другими параметрами, зависящими от провайдера.

- `RequestToken` (`SecureString`)
- `DefaultProduct` (`PaytmMoneyProducts`)
- `PortfolioName` (`string`)
- `Address` (`Uri`)
- `WebSocketAddress` (`Uri`)
- `SecurityMasterFile` (`string`)
- `PollingInterval` (`TimeSpan`)

## См. также

[Графическое конфигурирование](graphical_configuration_paytm_money.md)

[Инициализация адаптера](adapter_initialization_paytm_money.md)
