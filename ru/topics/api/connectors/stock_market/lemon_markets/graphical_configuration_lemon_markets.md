# Графическая настройка: lemon.markets

Во всех продуктах StockSharp подключение настраивается в [окне настройки подключений](../../../graphical_user_interface/connection_settings_window.md).

- `ApiKey` - учётные данные для авторизации.
- `IsDemo` - переключатель поведения коннектора. Значение по умолчанию: `true`.
- `AccountId` - идентификатор счёта или клиента.
- `SecuritiesAccountId` - идентификатор счёта или клиента.
- `DataPrivacyPrincipal` - параметр подключения.
- `DataPrivacyJustification` - параметр подключения. Значение по умолчанию: `app_usage-stocksharp`.
- `PersonId` - идентификатор счёта или клиента.
- `DefaultFeeAmount` - размер комиссии, используемый при отсутствии значения от сервиса.
- `IsAppropriatenessConsentAccepted` - переключатель поведения коннектора.
- `PollingInterval` - временной интервал. Значение по умолчанию: `TimeSpan.FromSeconds(10)`.

## См. также

[Коннекторы](../../../connectors.md)

[Графическая настройка](../../graphical_configuration.md)

[Сохранение и загрузка настроек](../../save_and_load_settings.md)

[Создание собственного коннектора](../../creating_own_connector.md)
