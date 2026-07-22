# Configuración del conector: FXOpen TickTrader

Cree un token Web API de FXOpen e indique los parámetros de conexión.

- `WebApiId` - identificador del token Web API.
- `Key` - clave Web API.
- `Secret` - secreto Web API.
- `OneTimePassword` - contraseña de un solo uso opcional cuando se requiere autenticación de dos factores.
- `IsDemo` - selecciona el entorno de demostración. Valor predeterminado: `false`.
- `Address` - punto de conexión REST. Valor predeterminado para la cuenta real: `https://ttlivewebapi.fxopen.net`.
- `FeedAddress` - WebSocket de flujo de datos. Valor predeterminado para la cuenta real: `wss://marginalttlivewebapi.fxopen.net/feed`.
- `TradeAddress` - WebSocket de operaciones. Valor predeterminado para la cuenta real: `wss://marginalttlivewebapi.fxopen.net/trade`.

Al activar `IsDemo` se seleccionan los puntos de conexión oficiales de demostración de TickTrader salvo que se haya personalizado una dirección. El identificador, la clave y el secreto son obligatorios para las suscripciones WebSocket y las operaciones protegidas.

## Véase también

[Documentación oficial de la API de FXOpen](https://ticktrader.fxopen.com/api)

[API Web REST de TickTrader](https://ttlivewebapi.fxopen.net/api/doc/index?apiaddress=ttlivewebapi.fxopen.net&apiport=443)
