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
- Connector is implemented as a MessageAdapter
- Inherit from AsyncMessageAdapter for async/await
- All HTTP requests via HttpClient with CancellationToken
- WebSocket subscriptions via native client or ClientWebSocket
- Type mapping: exchange types → StockSharp Messages
- Error handling: SendOutError() for connection errors
- All strings in localization resources (or at least const)
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
Create a basic MessageAdapter for a cryptocurrency exchange connector
called MyExchange using StockSharp:
- Inherit from AsyncMessageAdapter
- Implement connect/disconnect (ConnectMessage, DisconnectMessage)
- Add settings: ApiKey, Secret, demo mode
- Use HttpClient for REST API
- Base API URL: https://api.myexchange.com/v1
```

### Schritt 2: Instrumentensuche

Prompt:

```
Add SecurityLookupMessage handling to the adapter:
- Request GET /api/v1/symbols returns a JSON list of instruments
- Each instrument has: symbol, baseAsset, quoteAsset,
  minQty, maxQty, tickSize, status
- Mapping: symbol → SecurityId, baseAsset/quoteAsset → name,
  tickSize → SecurityMessage.PriceStep
- Send SecurityMessage for each instrument
- Send SubscriptionFinishedMessage at the end
```

### Schritt 3: Marktdaten

Prompt:

```
Add market data subscription to the adapter:

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
Add trading operation support to the adapter:

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
Review the generated adapter for StockSharp API compliance:
1. Are all message types handled?
2. Is CancellationToken used correctly?
3. Is HTTP error handling in place?
4. Are SubscriptionFinishedMessage sent after completion?
5. Does WebSocket reconnection work on disconnect?
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
Add Level 1 data support (BestBid/BestAsk) to my connector:
- WebSocket channel: ticker_{symbol}
- Parse bid, ask, last, volume
- Send Level1ChangeMessage with fields:
  Level1Fields.BestBidPrice, Level1Fields.BestAskPrice,
  Level1Fields.LastTradePrice, Level1Fields.Volume
```

### Umgang mit Rate Limits

```
Add rate limit handling to my connector:
- API returns headers X-RateLimit-Remaining and X-RateLimit-Reset
- When limit is reached: wait until Reset, log a warning
- Use SemaphoreSlim to limit concurrent requests
```

## Tipps

1. **Nur lesend beginnen** — implementieren Sie zuerst Verbindung, Instrumente und Marktdaten. Fügen Sie Handelsoperationen erst nach der Überprüfung hinzu
2. **Sandbox verwenden** — testen Sie in der Testumgebung der Börse
3. **Auf bestehende Connectors verweisen** — geben Sie der KI Code aus einem bestehenden StockSharp-Connector als Referenz
4. **Alles protokollieren** — detaillierte Logs sind beim Debuggen eines Connectors unbezahlbar
5. **Randfälle behandeln** — Wiederverbindung, Instrumentenänderungen, nicht standardmäßige Ordertypen
