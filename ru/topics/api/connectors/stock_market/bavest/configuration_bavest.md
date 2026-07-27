# Настройки коннектора: Bavest

Перед подключением к Bavest задайте перечисленные ниже свойства адаптера. Список проверен по реализации [BavestMessageAdapter](xref:StockSharp.Bavest.BavestMessageAdapter).

## Основные настройки

Эти параметры показываются первыми в редакторе подключения.

- `Token` (`SecureString`)
- `Address` (`Uri`)

## Дополнительные настройки

Эти свойства управляют адресами, фильтрами, ограничениями и другими параметрами, зависящими от провайдера.

- `Currency` (`string`)
- `Exchange` (`string`)
- `ExchangeCode` (`string`)
- `FinancialFrequency` (`BavestFinancialFrequencies`)
- `TraceEtfMetrics` (`bool`)
- `ScreenerQuery` (`string`)
- `PageSize` (`int`)
- `MaxPages` (`int`)
- `NewsLimit` (`int`)
- `DatasetLimit` (`int`)

## См. также

[Графическое конфигурирование](graphical_configuration_bavest.md)

[Инициализация адаптера](adapter_initialization_bavest.md)
