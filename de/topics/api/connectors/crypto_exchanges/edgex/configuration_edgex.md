# Konfiguration edgeX

Um mit dem Connector zu arbeiten, generieren Sie **API Key** und **Secret** im Börsenkonto und geben Sie diese in den Verbindungseinstellungen an.

Wichtige Einstellungen:

- **Key** und **Secret**.
- **Clearing account** und **Passphrase**.
- **Section**: `Spot` oder `Derivatives`.
- **Enable spot**: aktiviert den Spot-Bereich, sofern API-Unterstützung verfügbar ist.
- **Demo**-Modus.
- **Spot REST / Derivatives REST**-Endpunkte.
- **Spot WS / Derivatives public WS / Derivatives private WS**-Endpunkte.

Offizielle API-Dokumentation:

- [Authentication](https://edgex-1.gitbook.io/edgex-documentation/developer/api/authentication)
- [Order API](https://edgex-1.gitbook.io/edgex-documentation/developer/api/private-api/order-api)
- [Account API](https://edgex-1.gitbook.io/edgex-documentation/developer/api/private-api/account-api)
- [Private websocket stream](https://edgex-1.gitbook.io/edgex-documentation/developer/api/private-api/private-websocket-stream)
- [Funding API](https://edgex-1.gitbook.io/edgex-documentation/developer/api/public-api/funding-api)
- [Meta-data API](https://edgex-1.gitbook.io/edgex-documentation/developer/api/public-api/meta-data-api)
- [Quote API](https://edgex-1.gitbook.io/edgex-documentation/developer/api/public-api/quote-api)

> [!TIP]
> `Derivatives` ist vollständig implementiert. `Spot` sollte nur aktiviert werden, wenn die Ziel-API-Umgebung dies unterstützt.
