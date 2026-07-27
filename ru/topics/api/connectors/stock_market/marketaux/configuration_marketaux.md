# Настройки коннектора: Marketaux

Перед подключением к Marketaux задайте перечисленные ниже свойства адаптера. Список проверен по реализации [MarketauxMessageAdapter](xref:StockSharp.Marketaux.MarketauxMessageAdapter).

## Основные настройки

Эти параметры показываются первыми в редакторе подключения.

- `Token` (`SecureString`)
- `Address` (`Uri`)

## Дополнительные настройки

Эти свойства управляют адресами, фильтрами, ограничениями и другими параметрами, зависящими от провайдера.

- `Languages` (`string`)
- `EntityTypes` (`string`)
- `Countries` (`string`)
- `MustHaveEntities` (`bool`)
- `GroupSimilar` (`bool`)
- `NewsPageSize` (`int`)
- `MaxPages` (`int`)
- `DatasetLimit` (`int`)
- `SentimentInterval` (`MarketauxIntervals`)

## См. также

[Графическое конфигурирование](graphical_configuration_marketaux.md)

[Инициализация адаптера](adapter_initialization_marketaux.md)
