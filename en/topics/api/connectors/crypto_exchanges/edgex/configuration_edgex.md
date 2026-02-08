# Configuration edgeX

To work with the connector, generate **API Key** and **Secret** in the exchange account and specify them in connection settings.

Main settings:

- **Key** and **Secret**.
- **Clearing account** and **Passphrase**.
- **Section**: `Spot` or `Derivatives`.
- **Enable spot**: enables the spot section when API support is available.
- **Demo** mode.
- **Spot REST / Derivatives REST** endpoints.
- **Spot WS / Derivatives public WS / Derivatives private WS** endpoints.

Official API documentation:

- [Authentication](https://edgex-1.gitbook.io/edgex-documentation/developer/api/authentication)
- [Order API](https://edgex-1.gitbook.io/edgex-documentation/developer/api/private-api/order-api)
- [Account API](https://edgex-1.gitbook.io/edgex-documentation/developer/api/private-api/account-api)
- [Private websocket stream](https://edgex-1.gitbook.io/edgex-documentation/developer/api/private-api/private-websocket-stream)
- [Funding API](https://edgex-1.gitbook.io/edgex-documentation/developer/api/public-api/funding-api)
- [Meta-data API](https://edgex-1.gitbook.io/edgex-documentation/developer/api/public-api/meta-data-api)
- [Quote API](https://edgex-1.gitbook.io/edgex-documentation/developer/api/public-api/quote-api)

> [!TIP]
> `Derivatives` is fully implemented. `Spot` should be enabled only when the target API environment supports it.
