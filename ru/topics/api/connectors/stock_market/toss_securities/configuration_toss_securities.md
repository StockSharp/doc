# Настройки коннектора: Toss Securities

Перед подключением к Toss Securities задайте перечисленные ниже свойства адаптера. Список проверен по реализации [TossSecuritiesMessageAdapter](xref:StockSharp.TossSecurities.TossSecuritiesMessageAdapter).

## Основные настройки

Эти параметры показываются первыми в редакторе подключения.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `AccountSequence` (`long`)
- `PollingInterval` (`TimeSpan`)

## Дополнительные настройки

Эти свойства управляют адресами, фильтрами, ограничениями и другими параметрами, зависящими от провайдера.

- `PortfolioName` (`string`)
- `AccountPollingInterval` (`TimeSpan`)
- `AdjustedCandles` (`bool`)
- `RestAddress` (`Uri`)

## См. также

[Графическое конфигурирование](graphical_configuration_toss_securities.md)

[Инициализация адаптера](adapter_initialization_toss_securities.md)
