# Настройки коннектора: MarketData.app

Перед подключением к MarketData.app задайте перечисленные ниже свойства адаптера. Список проверен по реализации [MarketDataAppMessageAdapter](xref:StockSharp.MarketDataApp.MarketDataAppMessageAdapter).

## Основные настройки

Эти параметры показываются первыми в редакторе подключения.

- `Token` (`SecureString`)
- `RestEndpoint` (`Uri`)

## Дополнительные настройки

Эти свойства управляют адресами провайдера, интервалами запросов, фильтрами, параметрами данных и лимитами результатов.

- `ExtendedHours` (`bool`)
- `AdjustSplits` (`bool`)
- `MaximumOptionContracts` (`int`)

## См. также

[Графическое конфигурирование](graphical_configuration_marketdataapp.md)

[Инициализация адаптера](adapter_initialization_marketdataapp.md)
