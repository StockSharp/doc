# Настройки коннектора Bit2Me

Публичные рыночные данные доступны без учётных данных. Для работы со счётом и торговых операций укажите API-ключ и секрет.

## Параметры подключения

- `Key` — API-ключ Bit2Me.
- `Secret` — секрет API Bit2Me.
- `RestEndpoint` — адрес REST API. Адрес по умолчанию: `https://gateway.bit2me.com`.
- `WebSocketEndpoint` — адрес WebSocket. Адрес по умолчанию: `wss://ws.bit2me.com/v1/trading`.

Коннектор поддерживает рыночные, лимитные и стоп-лимитные заявки. Публичные подписки WebSocket передают сделки и полные обновления стакана Level 2; свечи загружаются через REST.

## Официальная документация API

- [Bit2Me API](https://api.bit2me.com/)
- [Примеры торговли Bit2Me](https://github.com/bit2me-devs/trading-spot-samples)
