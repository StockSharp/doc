# Графическая настройка: LSEG Real-Time

Во всех продуктах StockSharp подключение настраивается в [окне настройки подключений](../../../graphical_user_interface/connection_settings_window.md).

- `AuthenticationMode` - режим или вариант работы коннектора.
- `Address` - адрес сервиса.
- `StandbyAddress` - адрес сервиса.
- `IsHotStandby` - переключатель поведения коннектора.
- `Login` - идентификатор счёта или клиента.
- `Password` - учётные данные для авторизации.
- `ClientId` - идентификатор счёта или клиента.
- `Secret` - учётные данные для авторизации.
- `ApplicationId` - идентификатор счёта или клиента. Значение по умолчанию: `256`.
- `Service` - параметр подключения. Значение по умолчанию: `ELEKTRON_DD`.
- `Region` - параметр подключения. Значение по умолчанию: `us-east-1`.
- `Position` - параметр подключения.
- `Scope` - параметр подключения. Значение по умолчанию: `trapi.streaming.pricing.read`.
- `AuthUrl` - адрес сервиса.
- `DiscoveryUrl` - адрес сервиса. Значение по умолчанию: `https://api.refinitiv.com/streaming/pricing/v1/`.

## См. также

[Коннекторы](../../../connectors.md)

[Графическая настройка](../../graphical_configuration.md)

[Сохранение и загрузка настроек](../../save_and_load_settings.md)

[Создание собственного коннектора](../../creating_own_connector.md)
