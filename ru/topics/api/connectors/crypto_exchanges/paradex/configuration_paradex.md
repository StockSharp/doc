# Настройки коннектора Paradex

Для работы с коннектором укажите API данные и параметры аутентификации Starknet.

Основные настройки:

- **Key** и **Secret**.
- **Starknet account** и **Starknet key**.
- **Section**: `Spot` или `Derivatives`.
- **Enable spot**: включает секцию spot, если она поддерживается API.
- режим **Demo**.
- адреса **Spot REST / Derivatives REST**.
- адреса **Spot WS / Derivatives WS**.
- **Auth path** (по умолчанию: `/v1/auth`).

Официальная документация API:

- [API URLs](https://docs.paradex.trade/api/prod/api-urls)
- [Authentication](https://docs.paradex.trade/api/prod/authentication)
- [REST API](https://docs.paradex.trade/api/prod/rest-api)
- [Create a new order](https://docs.paradex.trade/api/prod/orders/create-a-new-order)
- [Websocket introduction](https://docs.paradex.trade/api/prod/websocket/introduction)
- [Websocket channels](https://docs.paradex.trade/api/prod/websocket/channels)
- [Order book channel](https://docs.paradex.trade/api/prod/websocket/channels/order_book_channel)

> [!TIP]
> В текущей реализации полностью поддержан `Derivatives`. `Spot` включайте только при подтвержденной поддержке в целевой API среде.
