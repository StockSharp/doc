# Настройки коннектора: OpenMarkets

Получите учётные данные у поставщика и укажите параметры подключения.

- `ClientId` - идентификатор счёта или клиента.
- `ClientSecret` - учётные данные для авторизации.
- `AccountCode` - идентификатор счёта или клиента.
- `IsTest` - переключатель поведения коннектора.
- `DataSource` - параметр подключения. Значение по умолчанию: `OpenMarketsExtensions.DefaultDataSource`.
- `DefaultExchange` - параметр подключения. Значение по умолчанию: `OpenMarketsExtensions.DefaultExchange`.
- `DefaultDestination` - параметр подключения. Значение по умолчанию: `OpenMarketsExtensions.DefaultExchange`.
- `OrderGiver` - параметр подключения.
- `OrderTaker` - параметр подключения.
- `DefaultPriceMultiplier` - числовой параметр коннектора. Значение по умолчанию: `0.01m`.
- `DepthPollingInterval` - временной интервал. Значение по умолчанию: `TimeSpan.FromSeconds(2)`.
