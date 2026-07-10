# Настройки коннектора Paradex

Для работы с коннектором укажите API данные и параметры аутентификации Starknet.

Основные настройки:

- **Ключ** и **Секрет**.
- **Аккаунт Starknet** и **Ключ Starknet**.
- **Секция**: `Spot` или `Derivatives`.
- **Включить спот**: включает секцию spot, если она поддерживается API.
- режим **демонстрации**.
- адреса **Spot REST / Derivatives REST**.
- адреса **Spot WS / Derivatives WS**.
- **Путь аутентификации** (по умолчанию: `/v1/auth`).

Официальная документация API:

- [URL API](https://docs.paradex.trade/api/prod/api-urls)
- [Аутентификация](https://docs.paradex.trade/api/prod/authentication)
- [REST API](https://docs.paradex.trade/api/prod/rest-api)
- [Создание новой заявки](https://docs.paradex.trade/api/prod/orders/create-a-new-order)
- [Введение в WebSocket](https://docs.paradex.trade/api/prod/websocket/introduction)
- [Каналы WebSocket](https://docs.paradex.trade/api/prod/websocket/channels)
- [Канал книги заявок](https://docs.paradex.trade/api/prod/websocket/channels/order_book_channel)

> [!TIP]
> В текущей реализации полностью поддержан `Derivatives`. `Spot` включайте только при подтвержденной поддержке в целевой API среде.
