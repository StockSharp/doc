# Настройки коннектора: Finage

Перед подключением к Finage задайте перечисленные ниже свойства адаптера. Список проверен по реализации [FinageMessageAdapter](xref:StockSharp.Finage.FinageMessageAdapter).

## Основные настройки

Эти параметры показываются первыми в редакторе подключения.

- `ApiKey` (`SecureString`)

## Дополнительные настройки

Эти свойства управляют доступом REST и WebSocket, адресами, параметрами рыночных данных, фильтрами символов и лимитами.

- `StreamingToken` (`SecureString`)
- `RestEndpoint` (`Uri`)
- `StreamingEndpoint` (`Uri`)
- `RequestInterval` (`TimeSpan`)
- `Symbols` (`string`)
- `MaximumSecurities` (`int`)

## См. также

[Графическое конфигурирование](graphical_configuration_finage.md)

[Инициализация адаптера](adapter_initialization_finage.md)
