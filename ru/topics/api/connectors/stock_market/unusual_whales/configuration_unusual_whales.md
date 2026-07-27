# Настройки коннектора: Unusual Whales

Перед подключением к Unusual Whales задайте перечисленные ниже свойства адаптера. Список проверен по реализации [UnusualWhalesMessageAdapter](xref:StockSharp.UnusualWhales.UnusualWhalesMessageAdapter).

## Основные настройки

Эти параметры показываются первыми в редакторе подключения.

- `Token` (`SecureString`)
- `Address` (`Uri`)

## Дополнительные настройки

Эти свойства управляют адресами, фильтрами, ограничениями и другими параметрами, зависящими от провайдера.

- `CandleLimit` (`int`)
- `NewsLimit` (`int`)
- `MaxPages` (`int`)
- `DatasetLimit` (`int`)
- `UnusualFlowOnly` (`bool`)
- `OtmMarketTide` (`bool`)
- `FiveMinuteMarketTide` (`bool`)

## См. также

[Графическое конфигурирование](graphical_configuration_unusual_whales.md)

[Инициализация адаптера](adapter_initialization_unusual_whales.md)
