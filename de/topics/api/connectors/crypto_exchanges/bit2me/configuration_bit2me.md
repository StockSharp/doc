# Einstellungen des Bit2Me-Konnektors

Öffentliche Marktdaten sind ohne Zugangsdaten verfügbar. Für Konto- und Handelsoperationen werden API-Schlüssel und Secret benötigt.

## Verbindungsparameter

- `Key` — Bit2Me-API-Schlüssel.
- `Secret` — Bit2Me-API-Secret.
- `RestEndpoint` — REST-API-Adresse. Produktionsstandard: `https://gateway.bit2me.com`.
- `WebSocketEndpoint` — WebSocket-Adresse. Produktionsstandard: `wss://ws.bit2me.com/v1/trading`.

Der Konnektor unterstützt Market-, Limit- und Stop-Limit-Orders. Öffentliche WebSocket-Abonnements liefern Trades und vollständige Level-2-Orderbuch-Aktualisierungen; Kerzen werden über REST geladen.

## Offizielle API-Dokumentation

- [Bit2Me API](https://api.bit2me.com/)
- [Bit2Me-Handelsbeispiele](https://github.com/bit2me-devs/trading-spot-samples)
