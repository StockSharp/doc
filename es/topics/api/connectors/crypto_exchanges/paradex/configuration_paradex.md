# Configuración Paradex

Para trabajar con el conector, especifique sus credenciales API y la configuración de autenticación de Starknet.

Configuración principal:

- **clave** y **secreto**.
- **Cuenta Starknet** y **Clave Starknet**.
- **Section**: `Spot` o `Derivatives`.
- **Habilitar spot**: habilita la sección spot cuando el soporte de la API está disponible.
- Modo **Demo**.
- Endpoints **Spot REST / Derivatives REST**.
- Endpoints **Spot WS / Derivatives WS**.
- **Ruta de autenticación** (valor predeterminado: `/v1/auth`).

Documentación oficial de la API:

- [URLs de API](https://docs.paradex.trade/api/prod/api-urls)
- [Autenticación](https://docs.paradex.trade/api/prod/authentication)
- [API REST](https://docs.paradex.trade/api/prod/rest-api)
- [Crear una nueva orden](https://docs.paradex.trade/api/prod/orders/create-a-new-order)
- [Introducción a WebSocket](https://docs.paradex.trade/api/prod/websocket/introduction)
- [Canales WebSocket](https://docs.paradex.trade/api/prod/websocket/channels)
- [Canal de libro de órdenes](https://docs.paradex.trade/api/prod/websocket/channels/order_book_channel)

> [!TIP]
> Los derivados de Paradex son totalmente compatibles. Habilite `Spot` solo cuando el entorno API de destino confirme soporte spot.
