# Настройки коннектора: J-Quants

Перед подключением к J-Quants задайте перечисленные ниже свойства адаптера. Список проверен по реализации [JQuantsMessageAdapter](xref:StockSharp.JQuants.JQuantsMessageAdapter).

## Основные настройки

Эти параметры показываются первыми в редакторе подключения.

- `Key` (`SecureString`)

## Дополнительные настройки

Эти свойства управляют адресами провайдера, интервалами запросов, фильтрами, параметрами данных и лимитами результатов.

- `RestEndpoint` (`string`)
- `RequestInterval` (`TimeSpan`)
- `MaximumPages` (`int`)

## См. также

[Графическое конфигурирование](graphical_configuration_jquants.md)

[Инициализация адаптера](adapter_initialization_jquants.md)
