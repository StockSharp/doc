# Configuración del conector: API de negociación de Finam

Configure las siguientes propiedades antes de conectarse a Finam. La lista se ha verificado con [FinamTradeMessageAdapter](xref:StockSharp.FinamTrade.FinamTradeMessageAdapter).

## Configuración básica

El editor de conexiones muestra primero estos parámetros.

- `Token` (`SecureString`) — secreto obligatorio de la API de negociación de Finam. El adaptador lo canjea por un token de sesión de corta duración.
- `AccountId` (`string`) — identificador opcional de la cuenta de negociación. Si se deja vacío, el adaptador utiliza la primera cuenta disponible para el token.

## Configuración avanzada

- `AppId` (`string`) — identificador de la aplicación enviado al crear la sesión. El valor predeterminado es `StockSharp`.
- `PollingInterval` (`TimeSpan`) — intervalo para consultar instantáneas de la cuenta y las órdenes. El valor predeterminado es 30 segundos; no se aceptan valores inferiores a un segundo.
- `LookupLimit` (`int`) — número máximo de instrumentos devueltos por una búsqueda sin restricciones. El valor predeterminado es `10000` y debe ser positivo.
- `RestAddress` (`string`) — dirección base de la API REST. El valor predeterminado es `https://api.finam.ru/`.
- `WebSocketAddress` (`string`) — dirección de la API WebSocket. El valor predeterminado es `wss://api.finam.ru/ws`.

Mantenga las direcciones predefinidas salvo que Finam o una pasarela compatible le haya asignado otros puntos de conexión.

## Véase también

[Configuración gráfica](graphical_configuration_finam_trade.md)

[Inicialización del adaptador](adapter_initialization_finam_trade.md)
