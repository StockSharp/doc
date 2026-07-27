# Настройки коннектора: Trading Economics

Перед подключением к Trading Economics задайте перечисленные ниже свойства адаптера. Список проверен по реализации [TradingEconomicsMessageAdapter](xref:StockSharp.TradingEconomics.TradingEconomicsMessageAdapter).

## Основные настройки

Эти параметры показываются первыми в редакторе подключения.

- `Token` (`SecureString`)
- `Address` (`Uri`)

## Дополнительные настройки

Эти свойства управляют адресами, фильтрами, ограничениями и другими параметрами, зависящими от провайдера.

- `DefaultMarket` (`string`)
- `DefaultSearch` (`string`)
- `NewsLimit` (`int`)

## См. также

[Графическое конфигурирование](graphical_configuration_trading_economics.md)

[Инициализация адаптера](adapter_initialization_trading_economics.md)
