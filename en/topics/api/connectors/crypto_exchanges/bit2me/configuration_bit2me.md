# Bit2Me Connector Settings

Public market data is available without credentials. Specify an API key and secret to use account and trading operations.

## Connection parameters

- `Key` - Bit2Me API key.
- `Secret` - Bit2Me API secret.
- `RestEndpoint` - REST API address. The production default is `https://gateway.bit2me.com`.
- `WebSocketEndpoint` - WebSocket address. The production default is `wss://ws.bit2me.com/v1/trading`.

The connector supports market, limit, and stop-limit orders. Public WebSocket subscriptions provide trades and complete Level 2 order-book updates; candles are downloaded through REST.

## Official API documentation

- [Bit2Me API](https://api.bit2me.com/)
- [Bit2Me trading samples](https://github.com/bit2me-devs/trading-spot-samples)
