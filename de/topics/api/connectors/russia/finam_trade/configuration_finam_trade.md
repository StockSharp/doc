# Connector-Konfiguration: Finam Trade API

Konfigurieren Sie vor der Verbindung mit Finam die folgenden Eigenschaften. Die Liste wurde anhand von [FinamTradeMessageAdapter](xref:StockSharp.FinamTrade.FinamTradeMessageAdapter) geprüft.

## Grundeinstellungen

Diese Einstellungen werden im Verbindungseditor zuerst angezeigt.

- `Token` (`SecureString`) — erforderliches Finam-Trade-API-Geheimnis. Der Adapter tauscht es gegen ein kurzlebiges Sitzungstoken aus.
- `AccountId` (`string`) — optionale Kennung des Handelskontos. Ist sie leer, verwendet der Adapter das erste für das Token verfügbare Konto.

## Erweiterte Einstellungen

- `AppId` (`string`) — Anwendungskennung, die beim Erstellen der Sitzung gesendet wird. Der Standardwert ist `StockSharp`.
- `PollingInterval` (`TimeSpan`) — Intervall zum Abrufen von Konto- und Ordermomentaufnahmen. Der Standardwert beträgt 30 Sekunden; Werte unter einer Sekunde werden abgelehnt.
- `LookupLimit` (`int`) — Höchstzahl der Instrumente bei einer uneingeschränkten Suche. Der Standardwert ist `10000` und muss positiv sein.
- `RestAddress` (`string`) — Basisadresse der REST-API. Der Standardwert ist `https://api.finam.ru/`.
- `WebSocketAddress` (`string`) — Adresse der WebSocket-API. Der Standardwert ist `wss://api.finam.ru/ws`.

Ändern Sie die vorausgefüllten Adressen nur, wenn Finam oder ein kompatibles Gateway andere Endpunkte zugewiesen hat.

## Siehe auch

[Grafische Konfiguration](graphical_configuration_finam_trade.md)

[Adapterinitialisierung](adapter_initialization_finam_trade.md)
