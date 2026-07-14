# Configuración del conector Charles Schwab

Cree una aplicación con acceso a la [API de negociación de Charles Schwab](https://developer.schwab.com/products/trader-api--individual) y obtenga un token OAuth.

- **Token de acceso** (`Token`) — un token de acceso OAuth válido.
- **Dirección** (`Address`) — la dirección de la API de negociación. Valor predeterminado: `https://api.schwabapi.com/`.

El conector obtiene automáticamente la dirección WebSocket y los identificadores de la conexión en tiempo real. Renueve el token OAuth según los requisitos de Charles Schwab.
