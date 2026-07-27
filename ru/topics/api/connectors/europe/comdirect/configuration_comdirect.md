# Настройки коннектора: comdirect

Перед подключением к comdirect задайте перечисленные ниже свойства адаптера. Список проверен по реализации [ComdirectMessageAdapter](xref:StockSharp.Comdirect.ComdirectMessageAdapter).

## Основные настройки

Эти параметры показываются первыми в редакторе подключения.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `Login` (`string`)
- `Password` (`SecureString`)
- `TanType` (`ComdirectTanTypes`)
- `PollingInterval` (`TimeSpan`)

## Дополнительные настройки

Эти свойства управляют адресами, фильтрами, ограничениями и другими параметрами, зависящими от провайдера.

- `DefaultCurrency` (`string`)
- `Address` (`Uri`)

## См. также

[Графическое конфигурирование](graphical_configuration_comdirect.md)

[Инициализация адаптера](adapter_initialization_comdirect.md)
