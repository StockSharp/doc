# Paradex-Konfiguration

Um mit dem Connector zu arbeiten, geben Sie Ihre API-Zugangsdaten und die Starknet-Authentifizierungseinstellungen an.

Wichtige Einstellungen:

- **Key** und **Secret**.
- **Starknet account** und **Starknet key**.
- **Section**: `Spot` oder `Derivatives`.
- **Enable spot**: aktiviert den Spot-Bereich, wenn die API-Unterstützung verfügbar ist.
- **Demo**-Modus.
- **Spot REST / Derivatives REST**-Endpunkte.
- **Spot WS / Derivatives WS**-Endpunkte.
- **Auth path** (Standard: `/v1/auth`).

Offizielle API-Dokumentation:

- [API-URLs](https://docs.paradex.trade/api/prod/api-urls)
- [Authentifizierung](https://docs.paradex.trade/api/prod/authentication)
- [REST API](https://docs.paradex.trade/api/prod/rest-api)
- [Neuen Auftrag erstellen](https://docs.paradex.trade/api/prod/orders/create-a-new-order)
- [Websocket-Einführung](https://docs.paradex.trade/api/prod/websocket/introduction)
- [Websocket-Kanäle](https://docs.paradex.trade/api/prod/websocket/channels)
- [Order-Book-Kanal](https://docs.paradex.trade/api/prod/websocket/channels/order_book_channel)

> [!TIP]
> Paradex-Derivate werden vollständig unterstützt. Aktivieren Sie `Spot` nur, wenn die Ziel-API-Umgebung die Spot-Unterstützung bestätigt.

