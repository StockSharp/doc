# Настройки коннектора: CoinPaprika

Перед подключением к CoinPaprika задайте перечисленные ниже свойства адаптера. Список проверен по реализации [CoinPaprikaMessageAdapter](xref:StockSharp.CoinPaprika.CoinPaprikaMessageAdapter).

## Основные настройки

Эти параметры показываются первыми в редакторе подключения.

- `Token` (`SecureString`)
- `RestEndpoint` (`string`)
- `QuoteCurrency` (`string`)
- `ExchangeId` (`string`)

## Дополнительные настройки

Эти свойства управляют адресами, фильтрами, ограничениями и другими параметрами, зависящими от провайдера.

- `RequestInterval` (`TimeSpan`)
- `PollingInterval` (`TimeSpan`)
- `MaximumItems` (`int`)
- `HistoryLimit` (`int`)

## См. также

[Графическое конфигурирование](graphical_configuration_coinpaprika.md)

[Инициализация адаптера](adapter_initialization_coinpaprika.md)
