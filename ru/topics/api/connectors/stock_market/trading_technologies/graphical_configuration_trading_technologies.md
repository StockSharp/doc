# Графическая настройка: Торговые технологии

Во всех продуктах StockSharp подключение настраивается в [окне настройки подключений](../../../graphical_user_interface/connection_settings_window.md).

- `SdkPath` - путь к локальному файлу или каталогу.
- `AppSecretKey` - учётные данные для авторизации.
- `Environment` - режим или вариант работы коннектора. Значение по умолчанию: `TradingTechnologiesEnvironments.ProdSim`.
- `InitializationTimeout` - временной интервал. Значение по умолчанию: `5000`.
- `MarketDepth` - количество запрашиваемых уровней стакана. Значение по умолчанию: `20`.
- `IsBinaryProtocol` - переключатель поведения коннектора. Значение по умолчанию: `true`.
- `IsOptionsEnabled` - переключатель поведения коннектора. Значение по умолчанию: `true`.

## См. также

[Коннекторы](../../../connectors.md)

[Графическая настройка](../../graphical_configuration.md)

[Сохранение и загрузка настроек](../../save_and_load_settings.md)

[Создание собственного коннектора](../../creating_own_connector.md)
