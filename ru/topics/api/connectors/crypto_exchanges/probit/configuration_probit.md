# Настройки коннектора ProBit Global

В настройках коннектора укажите параметры подключения к ProBit Global.

## Параметры подключения

- `Key` — идентификатор клиента OAuth из учётных данных ProBit API.
- `Secret` — секрет клиента OAuth.
- `RestEndpoint` — адрес REST API.
- `AuthEndpoint` — адрес конечной точки получения токена OAuth.
- `WebSocketEndpoint` — адрес сервера WebSocket.

Публичные рыночные данные доступны без учётных данных. Для торговли, получения балансов, истории заявок и приватных каналов WebSocket требуются `Key` и `Secret`.

Для рыночной покупки задайте сумму в валюте котировки через `ProBitOrderCondition.QuoteAmount`.

## Официальная документация API

- [Документация ProBit Global API](https://docs-en.probit.com/)
- [Учётные данные ProBit Global API](https://www.probit.com/en-us/my-page/api-management/api-credential)
