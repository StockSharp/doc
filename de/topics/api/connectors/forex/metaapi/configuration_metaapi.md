# Connector-Konfiguration: MetaApi

Erstellen Sie ein MetaApi-Token, stellen Sie das Handelskonto bereit und geben Sie die Verbindungsparameter an.

- `Token` - MetaApi-Zugriffstoken.
- `AccountId` - Kennung des bereitgestellten MetaApi-Kontos.
- `Region` - Region des Kontos. API-Token ermitteln sie automatisch, daher ist sie nur bei kontobezogenen Token ausdrücklich anzugeben.
- `Domain` - MetaApi-Domain. Standardwert: `agiliumtrade.agiliumtrade.ai`.
- `SynchronizationTimeout` - Wartezeit für die serverseitige Terminalsynchronisierung. Standardwert: 2 Minuten; kürzere Werte werden auf 10 Sekunden angehoben.

Die Verbindung gilt erst dann als hergestellt, wenn MetaApi den Terminalzustand als synchronisiert meldet; die Wartezeit muss daher die anfängliche Synchronisierung der Kontohistorie abdecken.

Parameter für ausstehende Aufträge und Schutzpreise werden über [MetaApiOrderCondition](xref:StockSharp.MetaApi.MetaApiOrderCondition) übergeben: der Aktivierungspreis, die Stop-Loss- und Take-Profit-Preise sowie die mit der Position gespeicherte Magic Number, der Kommentar und die Client-Kennung.

## Siehe auch

[Offizielle MetaApi-Dokumentation](https://metaapi.cloud/docs/client/)
