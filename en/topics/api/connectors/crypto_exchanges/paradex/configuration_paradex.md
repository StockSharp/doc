# Configuration Paradex

To work with the connector, specify your API credentials and Starknet authentication settings.

Main settings:

- **Key** and **Secret**.
- **Starknet account** and **Starknet key**.
- **Section**: `Spot` or `Derivatives`.
- **Enable spot**: enables the spot section when API support is available.
- **Demo** mode.
- **Spot REST / Derivatives REST** endpoints.
- **Spot WS / Derivatives WS** endpoints.
- **Auth path** (default: `/v1/auth`).

Official API documentation:

- [API URLs](https://docs.paradex.trade/api/prod/api-urls)
- [Authentication](https://docs.paradex.trade/api/prod/authentication)
- [REST API](https://docs.paradex.trade/api/prod/rest-api)
- [Create a new order](https://docs.paradex.trade/api/prod/orders/create-a-new-order)
- [Websocket introduction](https://docs.paradex.trade/api/prod/websocket/introduction)
- [Websocket channels](https://docs.paradex.trade/api/prod/websocket/channels)
- [Order book channel](https://docs.paradex.trade/api/prod/websocket/channels/order_book_channel)

> [!TIP]
> Paradex derivatives are fully supported. Enable `Spot` only when target API environment confirms spot support.
