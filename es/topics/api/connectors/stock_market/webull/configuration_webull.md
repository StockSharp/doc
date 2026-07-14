# Configuración del conector Webull

Cree una aplicación en [Webull OpenAPI](https://developer.webull.com/apis/docs/) y obtenga sus credenciales.

- **Clave** (`Key`) — la clave de la aplicación Webull OpenAPI.
- **Secreto** (`Secret`) — el secreto de la aplicación.
- **Token de acceso** (`Token`) — un token de acceso opcional.
- **Cuenta** (`Account`) — el identificador de la cuenta de negociación. Si se omite, el conector solicita automáticamente la lista de cuentas.
- **Modo de demostración** (`IsDemo`) — uso del entorno de pruebas de Webull.

Los parámetros `Token` y `Account` pueden omitirse si no son necesarios para su aplicación y cuenta de Webull.
