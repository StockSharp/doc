# Настройки коннектора: Tradejini

Перед подключением к Tradejini задайте перечисленные ниже свойства адаптера. Список проверен по реализации [TradejiniMessageAdapter](xref:StockSharp.Tradejini.TradejiniMessageAdapter).

## Основные настройки

Эти параметры показываются первыми в редакторе подключения.

- `ApiKey` (`SecureString`)
- `Password` (`SecureString`)
- `TwoFactorCode` (`SecureString`)
- `TwoFactorType` (`TradejiniTwoFactorTypes`)
- `Token` (`SecureString`)

## Дополнительные настройки

Эти свойства управляют адресами, фильтрами, ограничениями и другими параметрами, зависящими от провайдера.

- `PortfolioName` (`string`)
- `DefaultProduct` (`TradejiniProducts`)
- `Address` (`Uri`)
- `PollingInterval` (`TimeSpan`)

## См. также

[Графическое конфигурирование](graphical_configuration_tradejini.md)

[Инициализация адаптера](adapter_initialization_tradejini.md)
