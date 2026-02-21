# Writing a Connector with AI

A step-by-step guide to creating an exchange connector for StockSharp using AI tools.

## Preparation

### 1. Study the Exchange API

Before starting, prepare:
- REST/WebSocket API documentation for the exchange
- Test API keys (sandbox/testnet)
- List of supported data types (candles, order book, ticks, trades)
- List of supported order types (limit, market, stop)

### 2. Create a Project

```bash
dotnet new classlib -n StockSharp.MyExchange --framework net10.0
cd StockSharp.MyExchange
dotnet add package StockSharp.Messages
dotnet add package StockSharp.Algo
```

### 3. Prepare Context for the AI

Create a `CLAUDE.md` file:

```markdown
# Project Rules — Exchange Connector

- Framework: StockSharp 5.x, .NET 10
- Connector is implemented as a MessageAdapter
- Inherit from AsyncMessageAdapter for async/await
- All HTTP requests via HttpClient with CancellationToken
- WebSocket subscriptions via native client or ClientWebSocket
- Type mapping: exchange types → StockSharp Messages
- Error handling: SendOutError() for connection errors
- All strings in localization resources (or at least const)
```

## Connector Architecture

A connector in StockSharp is a `MessageAdapter` that:
1. Receives incoming messages (requests) from the core
2. Processes them (calls the exchange API)
3. Sends back response messages (results)

```
StockSharp Core → [Message] → MessageAdapter → [HTTP/WS] → Exchange
Exchange → [HTTP/WS] → MessageAdapter → [Message] → StockSharp Core
```

## Step-by-Step Example

### Step 1: Basic Adapter Structure

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

### Step 2: Instrument Lookup

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

### Step 3: Market Data

Prompt:

```
Add market data subscription to the adapter:

1. Candles (MarketDataTypes.CandleTimeFrame):
   - REST: GET /api/v1/klines?symbol={}&interval={}&limit=1000
   - WebSocket: subscribe to channel kline_{symbol}_{interval}
   - Interval mapping: 1m, 5m, 15m, 1h, 4h, 1d

2. Order book (MarketDataTypes.MarketDepth):
   - WebSocket: subscribe to channel depth_{symbol}
   - Parse bids/asks into QuoteChangeMessage

3. Ticks (MarketDataTypes.Trades):
   - WebSocket: subscribe to channel trades_{symbol}
   - Parse into ExecutionMessage with ExecutionTypes.Tick
```

### Step 4: Trading Operations

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

### Step 5: Code Review

After generating each step, ask the AI:

```
Review the generated adapter for StockSharp API compliance:
1. Are all message types handled?
2. Is CancellationToken used correctly?
3. Is HTTP error handling in place?
4. Are SubscriptionFinishedMessage sent after completion?
5. Does WebSocket reconnection work on disconnect?
```

## Key Implementation Details

### MessageAdapter — What to Handle

| Incoming Message | Action | Response Message |
|-----------------|--------|-----------------|
| `ConnectMessage` | Connect to API | `ConnectMessage` (response) |
| `DisconnectMessage` | Disconnect | `DisconnectMessage` (response) |
| `SecurityLookupMessage` | Request instruments | `SecurityMessage` × N |
| `MarketDataMessage` (subscribe) | Subscribe to data | `SubscriptionResponseMessage` |
| `OrderRegisterMessage` | Create order | `ExecutionMessage` |
| `OrderCancelMessage` | Cancel order | `ExecutionMessage` |
| `PortfolioLookupMessage` | Request portfolios | `PortfolioMessage`, `PositionChangeMessage` |

### Async Pattern

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
        // ... request instruments
    }

    // ... other methods
}
```

### Request Signing

Most exchanges require HMAC signing for private requests:

```csharp
private string SignRequest(string payload)
{
    using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(Secret.To<string>()));
    var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
    return Convert.ToHexString(hash).ToLowerInvariant();
}
```

## Connector Review Checklist

### Connection
- [ ] Connect/disconnect works correctly
- [ ] Authentication errors are handled
- [ ] Reconnection works on disconnect

### Instruments
- [ ] Instrument list loads successfully
- [ ] Correctly mapped: SecurityId, PriceStep, VolumeStep
- [ ] `SubscriptionFinishedMessage` is sent

### Market Data
- [ ] Candles: history loading + subscription to new ones
- [ ] Order book: correct depth, updates
- [ ] Ticks: correct time, volume, direction

### Trading
- [ ] Limit orders: creation, cancellation
- [ ] Market orders: creation
- [ ] Order status updates
- [ ] Portfolio balance updates

### General
- [ ] All `CancellationToken` are propagated
- [ ] Errors are logged via `SendOutError()`
- [ ] No resource leaks (WebSocket, HttpClient)
- [ ] Compiles without errors or warnings

## Example Prompts

### Adding a New Data Type

```
Add Level 1 data support (BestBid/BestAsk) to my connector:
- WebSocket channel: ticker_{symbol}
- Parse bid, ask, last, volume
- Send Level1ChangeMessage with fields:
  Level1Fields.BestBidPrice, Level1Fields.BestAskPrice,
  Level1Fields.LastTradePrice, Level1Fields.Volume
```

### Handling Rate Limits

```
Add rate limit handling to my connector:
- API returns headers X-RateLimit-Remaining and X-RateLimit-Reset
- When limit is reached: wait until Reset, log a warning
- Use SemaphoreSlim to limit concurrent requests
```

## Tips

1. **Start read-only** — first implement connection, instruments, and market data. Add trading operations after verification
2. **Use sandbox** — test on the exchange's test environment
3. **Reference existing connectors** — give the AI code from an existing StockSharp connector as a reference
4. **Log everything** — detailed logs are invaluable when debugging a connector
5. **Handle edge cases** — reconnection, instrument changes, non-standard order types
