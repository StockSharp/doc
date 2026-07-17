# Графическая настройка: OpenMarkets

Во всех продуктах StockSharp подключение настраивается в [окне настройки подключений](../../../graphical_user_interface/connection_settings_window.md).

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

## См. также

[Коннекторы](../../../connectors.md)

[Графическая настройка](../../graphical_configuration.md)

[Сохранение и загрузка настроек](../../save_and_load_settings.md)

[Создание собственного коннектора](../../creating_own_connector.md)
