# Einstellungen des ProBit-Global-Konnektors

Geben Sie in den Konnektoreinstellungen die Verbindungsparameter für ProBit Global an.

## Verbindungsparameter

- `Key` - OAuth-Client-ID aus den ProBit-API-Zugangsdaten.
- `Secret` - OAuth-Client-Geheimnis.
- `RestEndpoint` - Adresse der REST-API.
- `AuthEndpoint` - Adresse des OAuth-Token-Endpunkts.
- `WebSocketEndpoint` - Adresse des WebSocket-Servers.

Öffentliche Marktdaten funktionieren ohne Zugangsdaten. Handel, Kontostände, Auftragshistorie und private WebSocket-Kanäle erfordern `Key` und `Secret`.

Legen Sie bei einem Marktkauf den Betrag in der Notierungswährung in `ProBitOrderCondition.QuoteAmount` fest.

## Offizielle API-Dokumentation

- [ProBit-Global-API-Dokumentation](https://docs-en.probit.com/)
- [ProBit-Global-API-Zugangsdaten](https://www.probit.com/en-us/my-page/api-management/api-credential)
