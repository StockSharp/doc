# Настройки коннектора: StockData.org

Перед подключением к StockData.org задайте перечисленные ниже свойства адаптера. Список проверен по реализации [StockDataOrgMessageAdapter](xref:StockSharp.StockDataOrg.StockDataOrgMessageAdapter).

## Основные настройки

Эти параметры показываются первыми в редакторе подключения.

- `Token` (`SecureString`)
- `Address` (`Uri`)

## Дополнительные настройки

Эти свойства управляют адресами, фильтрами, ограничениями и другими параметрами, зависящими от провайдера.

- `ExtendedHours` (`bool`)
- `AdjustedIntraday` (`bool`)
- `NewsLanguage` (`string`)
- `NewsPageSize` (`int`)
- `MaxRequests` (`int`)
- `QuoteTimeZoneId` (`string`)

## См. также

[Графическое конфигурирование](graphical_configuration_stockdata_org.md)

[Инициализация адаптера](adapter_initialization_stockdata_org.md)
