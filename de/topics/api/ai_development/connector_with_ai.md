# Einen Connector mit KI schreiben

Eine Schritt-für-Schritt-Anleitung zur Erstellung eines Börsen-Connectors für StockSharp mithilfe von KI-Tools.

## Vorbereitung

### 1. Die Börsen-API studieren

Bereiten Sie vor dem Start Folgendes vor:
- REST/WebSocket-API-Dokumentation der Börse
- Test-API-Schlüssel (Sandbox/Testnet)
- Liste der unterstützten Datentypen (Candles, Orderbuch, Ticks, Trades)
- Liste der unterstützten Ordertypen (Limit, Market, Stop)

### 2. Ein Projekt erstellen

```bash
dotnet new classlib -n StockSharp.MyExchange --framework net10.0
cd StockSharp.MyExchange
dotnet add package StockSharp.Messages
dotnet add package StockSharp.Algo
```

### 3. Kontext für die KI vorbereiten

Erstellen Sie eine Datei `CLAUDE.md`:

```markdown
# Projektregeln — Börsen-Connector

- Framework: StockSharp 5.x, .NET 10
- Der Connector wird als MessageAdapter implementiert
- Von AsyncMessageAdapter erben, um async/await zu nutzen
- Alle HTTP-Anfragen über HttpClient mit CancellationToken ausführen
- WebSocket-Abonnements über einen nativen Client oder ClientWebSocket
- Typzuordnung: Börsentypen → StockSharp Messages
- Fehlerbehandlung: SendOutError() bei Verbindungsfehlern
- Alle Zeichenfolgen in Lokalisierungsressourcen ablegen (oder zumindest als const)
```

## Connector-Architektur

Ein Connector in StockSharp ist ein `MessageAdapter`, der:
1. Eingehende Nachrichten (Anfragen) vom Kern empfängt
2. Sie verarbeitet (ruft die Börsen-API auf)
3. Antwortnachrichten (Ergebnisse) zurücksendet

```
StockSharp Core → [Message] → MessageAdapter → [HTTP/WS] → Exchange
Exchange → [HTTP/WS] → MessageAdapter → [Message] → StockSharp Core
```

## Schritt-für-Schritt-Beispiel

### Schritt 1: Grundlegende Adapter-Struktur

Prompt:

```
Erstelle mit StockSharp einen einfachen MessageAdapter für einen
Kryptowährungsbörsen-Connector namens MyExchange:
- von AsyncMessageAdapter erben
- Verbindung/Trennung implementieren (ConnectMessage, DisconnectMessage)
- Einstellungen hinzufügen: ApiKey, Secret, Demo-Modus
- HttpClient für die REST-API verwenden
- Basis-API-URL: https://api.myexchange.com/v1
```

### Schritt 2: Instrumentensuche

Prompt:

```
Füge dem Adapter die Verarbeitung von SecurityLookupMessage hinzu:
- Die Anfrage GET /api/v1/symbols gibt eine JSON-Liste von Instrumenten zurück
- Jedes Instrument enthält: symbol, baseAsset, quoteAsset,
  minQty, maxQty, tickSize, status
- Zuordnung: symbol → SecurityId, baseAsset/quoteAsset → name,
  tickSize → SecurityMessage.PriceStep
- Für jedes Instrument SecurityMessage senden
- Am Ende SubscriptionFinishedMessage senden
```

### Schritt 3: Marktdaten

Prompt:

```
Füge dem Adapter Market-Data-Abonnements hinzu:

1. Candles (MarketDataTypes.CandleTimeFrame):
   - REST: GET /api/v1/klines?symbol={}&interval={}&limit=1000
   - WebSocket: subscribe to channel kline_{symbol}_{interval}
   - Interval mapping: 1m, 5m, 15m, 1h, 4h, 1d

2. Orderbuch (MarketDataTypes.MarketDepth):
   - WebSocket: subscribe to channel depth_{symbol}
   - Parse bids/asks into QuoteChangeMessage

3. Ticks (MarketDataTypes.Trades):
   - WebSocket: subscribe to channel trades_{symbol}
   - Parse into ExecutionMessage with ExecutionTypes.Tick
```

### Schritt 4: Handelsoperationen

Prompt:

```
Füge dem Adapter Unterstützung für Handelsoperationen hinzu:

1. Order registration (OrderRegisterMessage):
   - POST /api/v1/order with params: symbol, side, type, quantity, price
   - Return ExecutionMessage with ExecutionTypes.Transaction

2. Order cancellation (OrderCancelMessage):
   - DELETE /api/v1/order/{orderId}
   - Return ExecutionMessage with status OrderStates.Done

3. Portfolio retrieval (PortfolioLookupMessage):
   - GET /api/v1/account
   - Parse balances into PositionChangeMessage

4. WebSocket for order updates:
   - Channel orders_{listenKey}
   - Parse order status updates
```

### Schritt 5: Code-Review

Bitten Sie die KI nach dem Generieren jedes Schritts:

```
Prüfe den generierten Adapter auf Kompatibilität mit der StockSharp-API:
1. Werden alle Nachrichtentypen verarbeitet?
2. Wird CancellationToken korrekt verwendet?
3. Ist die Behandlung von HTTP-Fehlern implementiert?
4. Wird SubscriptionFinishedMessage nach Abschluss gesendet?
5. Funktioniert die WebSocket-Wiederverbindung nach einer Trennung?
```

## Wichtige Implementierungsdetails

### MessageAdapter — Was zu behandeln ist

| Eingehende Nachricht | Aktion | Antwortnachricht |
|-----------------|--------|-----------------|
| `ConnectMessage` | Verbindung zur API herstellen | `ConnectMessage` (Antwort) |
| `DisconnectMessage` | Verbindung trennen | `DisconnectMessage` (Antwort) |
| `SecurityLookupMessage` | Instrumente anfordern | `SecurityMessage` × N |
| `MarketDataMessage` (subscribe) | Daten abonnieren | `SubscriptionResponseMessage` |
| `OrderRegisterMessage` | Order erstellen | `ExecutionMessage` |
| `OrderCancelMessage` | Order stornieren | `ExecutionMessage` |
| `PortfolioLookupMessage` | Portfolios anfordern | `PortfolioMessage`, `PositionChangeMessage` |

### Async-Muster

```csharp
public class MyExchangeAdapter : AsyncMessageAdapter
{
    private HttpClient _httpClient;

    protected override ValueTask OnConnectAsync(ConnectMessage msg, CancellationToken token)
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri("https://api.myexchange.com/v1/");
        _httpClient.DefaultRequestHeaders.Add("X-API-KEY", Key.To<string>());

        SendOutMessage(new ConnectMessage());
        return default;
    }

    protected override ValueTask OnSecurityLookupAsync(SecurityLookupMessage msg, CancellationToken token)
    {
        // ... Instrumente anfordern
    }

    // ... weitere Methoden
}
```

### Signierung von Anfragen

Die meisten Börsen verlangen eine HMAC-Signierung für private Anfragen:

```csharp
private string SignRequest(string payload)
{
    using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(Secret.To<string>()));
    var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
    return Convert.ToHexString(hash).ToLowerInvariant();
}
```

## Checkliste für die Connector-Überprüfung

### Verbindung
- [ ] Verbinden/Trennen funktioniert korrekt
- [ ] Authentifizierungsfehler werden behandelt
- [ ] Wiederverbindung funktioniert nach Trennung

### Instrumente
- [ ] Instrumentenliste wird erfolgreich geladen
- [ ] Korrekt zugeordnet: SecurityId, PriceStep, VolumeStep
- [ ] `SubscriptionFinishedMessage` wird gesendet

### Marktdaten
- [ ] Candles: Verlaufsladen + Abonnement neuer Candles
- [ ] Orderbuch: korrekte Tiefe, Updates
- [ ] Ticks: korrekte Zeit, Volumen, Richtung

### Handel
- [ ] Limit-Orders: Erstellung, Stornierung
- [ ] Market-Orders: Erstellung
- [ ] Order-Status-Updates
- [ ] Portfolio-Salden-Updates

### Allgemein
- [ ] Alle `CancellationToken` werden weitergegeben
- [ ] Fehler werden über `SendOutError()` protokolliert
- [ ] Keine Ressourcenlecks (WebSocket, HttpClient)
- [ ] Kompiliert ohne Fehler oder Warnungen

## Beispiel-Prompts

### Hinzufügen eines neuen Datentyps

```
Füge meinem Connector Unterstützung für Level-1-Daten (BestBid/BestAsk) hinzu:
- WebSocket-Kanal: ticker_{symbol}
- bid, ask, last, volume parsen
- Level1ChangeMessage mit folgenden Feldern senden:
  Level1Fields.BestBidPrice, Level1Fields.BestAskPrice,
  Level1Fields.LastTradePrice, Level1Fields.Volume
```

### Umgang mit Rate Limits

```
Füge meinem Connector die Behandlung von Rate Limits hinzu:
- Die API gibt die Header X-RateLimit-Remaining und X-RateLimit-Reset zurück
- Wenn das Limit erreicht ist: bis Reset warten und eine Warnung protokollieren
- SemaphoreSlim verwenden, um gleichzeitige Anfragen zu begrenzen
```

## Tipps

1. **Nur lesend beginnen** — implementieren Sie zuerst Verbindung, Instrumente und Marktdaten. Fügen Sie Handelsoperationen erst nach der Überprüfung hinzu
2. **Sandbox verwenden** — testen Sie in der Testumgebung der Börse
3. **Auf bestehende Connectors verweisen** — geben Sie der KI Code aus einem bestehenden StockSharp-Connector als Referenz
4. **Alles protokollieren** — detaillierte Logs sind beim Debuggen eines Connectors unbezahlbar
5. **Randfälle behandeln** — Wiederverbindung, Instrumentenänderungen, nicht standardmäßige Ordertypen
