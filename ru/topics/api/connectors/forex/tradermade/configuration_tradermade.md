# Настройки коннектора: TraderMade

Перед подключением к TraderMade задайте перечисленные ниже свойства адаптера. Список проверен по реализации [TraderMadeMessageAdapter](xref:StockSharp.TraderMade.TraderMadeMessageAdapter).

## Основные настройки

Эти параметры показываются первыми в редакторе подключения.

- `RestKey` (`SecureString`)

## Дополнительные настройки

Эти свойства управляют доступом REST и WebSocket, адресами, параметрами рыночных данных, фильтрами символов и лимитами.

- `StreamingKey` (`SecureString`)
- `RestEndpoint` (`Uri`)
- `StreamingEndpoint` (`Uri`)
- `RequestInterval` (`TimeSpan`)
- `EnableLadder` (`bool`)
- `Weekend` (`bool`)
- `QuoteCurrencies` (`string`)
- `Symbols` (`string`)
- `MaximumSecurities` (`int`)

## См. также

[Графическое конфигурирование](graphical_configuration_tradermade.md)

[Инициализация адаптера](adapter_initialization_tradermade.md)
