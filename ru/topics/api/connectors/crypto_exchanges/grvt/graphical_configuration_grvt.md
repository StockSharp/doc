# Графическое конфигурирование GRVT

Для всех продуктов [S\#](../../../../api.md) графическая настройка подключения выполняется в экранной форме [Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md):

- `Key` - ключ API для аутентификации.
- `Secret` - секретный ключ API для аутентификации.
- `SubAccountId` - идентификатор торгового субсчета.
- `Environment` - среда сервиса: основная или тестовая сеть.
- `EdgeEndpoint` - адрес API аутентификации.
- `MarketDataEndpoint` - адрес API рыночных данных.
- `TradingEndpoint` - адрес торгового API.
- `MarketWebSocketEndpoint` - адрес WebSocket для рыночных данных.
- `TradingWebSocketEndpoint` - адрес торгового WebSocket.
- `SnapshotInterval` - интервал снимков рыночных данных в миллисекундах.
- `MarketDepth` - запрашиваемое количество уровней стакана.

## См. также

[Коннекторы](../../../connectors.md)

[Графическое конфигурирование](../../graphical_configuration.md)

[Создание собственного коннектора](../../creating_own_connector.md)

[Сохранение и загрузка настроек](../../save_and_load_settings.md)
