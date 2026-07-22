# Configuración del conector ProBit Global

Indique los parámetros de conexión de ProBit Global en la configuración del conector.

## Parámetros de conexión

- `Key` - identificador de cliente OAuth emitido en las credenciales de API de ProBit.
- `Secret` - secreto de cliente OAuth.
- `RestEndpoint` - dirección de la API REST.
- `AuthEndpoint` - dirección del punto de acceso de tokens OAuth.
- `WebSocketEndpoint` - dirección del servidor WebSocket.

Los datos públicos de mercado funcionan sin credenciales. La negociación, los saldos, el historial de órdenes y los canales WebSocket privados requieren `Key` y `Secret`.

Para una compra a mercado, establezca el importe en la divisa cotizada en `ProBitOrderCondition.QuoteAmount`.

## Documentación oficial de la API

- [Documentación de la API de ProBit Global](https://docs-en.probit.com/)
- [Credenciales de API de ProBit Global](https://www.probit.com/en-us/my-page/api-management/api-credential)
