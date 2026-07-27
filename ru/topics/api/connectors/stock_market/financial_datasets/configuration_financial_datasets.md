# Настройки коннектора: Financial Datasets

Перед подключением к Financial Datasets задайте перечисленные ниже свойства адаптера. Список проверен по реализации [FinancialDatasetsMessageAdapter](xref:StockSharp.FinancialDatasets.FinancialDatasetsMessageAdapter).

## Основные настройки

Эти параметры показываются первыми в редакторе подключения.

- `Token` (`SecureString`)
- `Address` (`Uri`)

## Дополнительные настройки

Эти свойства управляют адресами, фильтрами, ограничениями и другими параметрами, зависящими от провайдера.

- `ActiveOnly` (`bool`)
- `FinancialPeriod` (`FinancialDatasetsPeriods`)
- `DataLimit` (`int`)
- `NewsLimit` (`int`)

## См. также

[Графическое конфигурирование](graphical_configuration_financial_datasets.md)

[Инициализация адаптера](adapter_initialization_financial_datasets.md)
