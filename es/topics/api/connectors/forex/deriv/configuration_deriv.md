# Configuración del conector: Deriv

Cree un token de Deriv e indique los parámetros de conexión.

- `Token` - token de acceso personal o de OAuth. Obligatorio para las operaciones protegidas.
- `AppId` - identificador de aplicación que se envía con las solicitudes REST autenticadas.
- `AccountId` - identificador de la cuenta de opciones. Opcional cuando solo una cuenta activa coincide con el modo seleccionado.
- `IsDemo` - selecciona la cuenta de demostración. Valor predeterminado: `true`.
- `RestAddress` - punto de conexión REST. Valor predeterminado: `https://api.derivws.com`.
- `PublicWebSocketAddress` - punto de conexión WebSocket público de opciones. Valor predeterminado: `wss://api.derivws.com/trading/v1/options/ws/public`.

Las sesiones públicas de datos de mercado funcionan sin token, mientras que los contratos, los saldos y las transacciones requieren el token y el identificador de aplicación. Las suscripciones se restauran automáticamente mediante una nueva dirección WebSocket de un solo uso.

Los parámetros del contrato se transmiten a través de [DerivOrderCondition](xref:StockSharp.Deriv.DerivOrderCondition): el tipo de contrato, si el importe es una apuesta o un pago, la divisa del contrato, la duración, las barreras y los precios de protección de stop-loss y take-profit.

## Véase también

[Documentación oficial de la API de Deriv](https://developers.deriv.com/docs/)
