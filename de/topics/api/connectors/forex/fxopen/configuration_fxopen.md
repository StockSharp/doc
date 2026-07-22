# Connector-Konfiguration: FXOpen TickTrader

Erstellen Sie ein FXOpen-Web-API-Token und geben Sie die Verbindungsparameter an.

- `WebApiId` - Kennung des Web-API-Tokens.
- `Key` - Web-API-Schlüssel.
- `Secret` - Web-API-Geheimnis.
- `OneTimePassword` - optionales Einmalpasswort bei erforderlicher Zwei-Faktor-Authentifizierung.
- `IsDemo` - wählt die Demo-Umgebung. Standardwert: `false`.
- `Address` - REST-Endpunkt. Live-Standard: `https://ttlivewebapi.fxopen.net`.
- `FeedAddress` - Feed-WebSocket. Live-Standard: `wss://marginalttlivewebapi.fxopen.net/feed`.
- `TradeAddress` - Trade-WebSocket. Live-Standard: `wss://marginalttlivewebapi.fxopen.net/trade`.

Mit `IsDemo` werden die offiziellen marginalen TickTrader-Demo-Endpunkte gewählt, sofern keine Adresse manuell geändert wurde. ID, Schlüssel und Geheimnis sind für WebSocket-Abonnements und private Operationen erforderlich.

## Siehe auch

[Offizielle FXOpen-API-Dokumentation](https://ticktrader.fxopen.com/api)

[TickTrader Web REST API](https://ttlivewebapi.fxopen.net/api/doc/index?apiaddress=ttlivewebapi.fxopen.net&apiport=443)
