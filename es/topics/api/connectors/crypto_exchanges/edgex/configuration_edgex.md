# Configuración edgeX

Para trabajar con el conector, genere **API Key** y **Secret** en la cuenta de la bolsa y especifíquelos en la configuración de conexión.

Configuración principal:

- **Key** y **Secret**.
- **Clearing account** y **Passphrase**.
- **Section**: `Spot` o `Derivatives`.
- **Enable spot**: habilita la sección spot cuando el soporte de la API está disponible.
- Modo **Demo**.
- Endpoints **Spot REST / Derivatives REST**.
- Endpoints **Spot WS / Derivatives public WS / Derivatives private WS**.

Documentación oficial de la API:

- [Authentication](https://edgex-1.gitbook.io/edgex-documentation/developer/api/authentication)
- [Order API](https://edgex-1.gitbook.io/edgex-documentation/developer/api/private-api/order-api)
- [Account API](https://edgex-1.gitbook.io/edgex-documentation/developer/api/private-api/account-api)
- [Private websocket stream](https://edgex-1.gitbook.io/edgex-documentation/developer/api/private-api/private-websocket-stream)
- [Funding API](https://edgex-1.gitbook.io/edgex-documentation/developer/api/public-api/funding-api)
- [Meta-data API](https://edgex-1.gitbook.io/edgex-documentation/developer/api/public-api/meta-data-api)
- [Quote API](https://edgex-1.gitbook.io/edgex-documentation/developer/api/public-api/quote-api)

> [!TIP]
> `Derivatives` está completamente implementado. `Spot` debe habilitarse solo cuando el entorno de API de destino lo admite.
