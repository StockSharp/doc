# Configuración del conector Bit2Me

Los datos públicos de mercado están disponibles sin credenciales. Indique una clave API y un secreto para usar operaciones de cuenta y trading.

## Parámetros de conexión

- `Key` — clave API de Bit2Me.
- `Secret` — secreto API de Bit2Me.
- `RestEndpoint` — dirección de la API REST. Valor de producción: `https://gateway.bit2me.com`.
- `WebSocketEndpoint` — dirección WebSocket. Valor de producción: `wss://ws.bit2me.com/v1/trading`.

El conector admite órdenes de mercado, limitadas y stop-limit. Las suscripciones públicas WebSocket entregan operaciones y actualizaciones completas del libro de nivel 2; las velas se descargan mediante REST.

## Documentación oficial de la API

- [API de Bit2Me](https://api.bit2me.com/)
- [Ejemplos de trading de Bit2Me](https://github.com/bit2me-devs/trading-spot-samples)
