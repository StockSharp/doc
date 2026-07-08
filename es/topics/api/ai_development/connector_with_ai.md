# Escribir un conector con IA

Una guía paso a paso para crear un conector de bolsa para StockSharp usando herramientas de IA.

## Preparación

### 1. Estudie la API de la bolsa

Antes de empezar, prepare:
- Documentación de la API REST/WebSocket de la bolsa
- Claves de API de prueba (sandbox/testnet)
- Lista de tipos de datos admitidos (velas, libro de órdenes, ticks, operaciones)
- Lista de tipos de órdenes admitidos (limit, market, stop)

### 2. Cree un proyecto

```bash
dotnet new classlib -n StockSharp.MyExchange --framework net10.0
cd StockSharp.MyExchange
dotnet add package StockSharp.Messages
dotnet add package StockSharp.Algo
```

### 3. Prepare el contexto para la IA

Cree un archivo `CLAUDE.md`:

```markdown
# Reglas del proyecto — conector de exchange

- Framework: StockSharp 5.x, .NET 10
- Connector is implemented as a MessageAdapter
- Inherit from AsyncMessageAdapter for async/await
- All HTTP requests via HttpClient with CancellationToken
- WebSocket subscriptions via native client or ClientWebSocket
- Type mapping: exchange types → StockSharp Messages
- Error handling: SendOutError() for connection errors
- All strings in localization resources (or at least const)
```

## Arquitectura del conector

Un conector en StockSharp es un `MessageAdapter` que:
1. Recibe mensajes entrantes (solicitudes) del núcleo
2. Los procesa (llama a la API de la bolsa)
3. Envía de vuelta mensajes de respuesta (resultados)

```
StockSharp Core → [Message] → MessageAdapter → [HTTP/WS] → Exchange
Exchange → [HTTP/WS] → MessageAdapter → [Message] → StockSharp Core
```

## Ejemplo paso a paso

### Paso 1: Estructura básica del adaptador

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

### Paso 2: Búsqueda de instrumentos

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

### Paso 3: Datos de mercado

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

### Paso 4: Operaciones de trading

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

### Paso 5: Revisión de código

Después de generar cada paso, pregunte a la IA:

```
Review the generated adapter for StockSharp API compliance:
1. Are all message types handled?
2. Is CancellationToken used correctly?
3. Is HTTP error handling in place?
4. Are SubscriptionFinishedMessage sent after completion?
5. Does WebSocket reconnection work on disconnect?
```

## Detalles clave de implementación

### MessageAdapter — qué manejar

| Mensaje entrante | Acción | Mensaje de respuesta |
|-----------------|--------|-----------------|
| `ConnectMessage` | Conectar a la API | `ConnectMessage` (respuesta) |
| `DisconnectMessage` | Desconectar | `DisconnectMessage` (respuesta) |
| `SecurityLookupMessage` | Solicitar instrumentos | `SecurityMessage` × N |
| `MarketDataMessage` (suscripción) | Suscribirse a datos | `SubscriptionResponseMessage` |
| `OrderRegisterMessage` | Crear orden | `ExecutionMessage` |
| `OrderCancelMessage` | Cancelar orden | `ExecutionMessage` |
| `PortfolioLookupMessage` | Solicitar carteras | `PortfolioMessage`, `PositionChangeMessage` |

### Patrón asíncrono

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
        // ... solicitar instrumentos
    }

    // ... otros métodos
}
```

### Firma de solicitudes

La mayoría de las bolsas requieren firma HMAC para solicitudes privadas:

```csharp
private string SignRequest(string payload)
{
    using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(Secret.To<string>()));
    var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
    return Convert.ToHexString(hash).ToLowerInvariant();
}
```

## Lista de verificación de revisión del conector

### Conexión
- [ ] La conexión/desconexión funciona correctamente
- [ ] Los errores de autenticación se manejan
- [ ] La reconexión funciona tras la desconexión

### Instrumentos
- [ ] La lista de instrumentos se carga correctamente
- [ ] Correctamente mapeados: SecurityId, PriceStep, VolumeStep
- [ ] Se envía `SubscriptionFinishedMessage`

### Datos de mercado
- [ ] Velas: carga del histórico + suscripción a nuevas
- [ ] Libro de órdenes: profundidad correcta, actualizaciones
- [ ] Ticks: tiempo, volumen y dirección correctos

### Trading
- [ ] Órdenes límite: creación, cancelación
- [ ] Órdenes de mercado: creación
- [ ] Actualizaciones del estado de las órdenes
- [ ] Actualizaciones del saldo de la cartera

### General
- [ ] Todos los `CancellationToken` se propagan
- [ ] Los errores se registran mediante `SendOutError()`
- [ ] No hay fugas de recursos (WebSocket, HttpClient)
- [ ] Compila sin errores ni advertencias

## Ejemplos de prompts

### Agregar un nuevo tipo de datos

```
Add Level 1 data support (BestBid/BestAsk) to my connector:
- WebSocket channel: ticker_{symbol}
- Parse bid, ask, last, volume
- Send Level1ChangeMessage with fields:
  Level1Fields.BestBidPrice, Level1Fields.BestAskPrice,
  Level1Fields.LastTradePrice, Level1Fields.Volume
```

### Manejo de límites de tasa

```
Add rate limit handling to my connector:
- API returns headers X-RateLimit-Remaining and X-RateLimit-Reset
- When limit is reached: wait until Reset, log a warning
- Use SemaphoreSlim to limit concurrent requests
```

## Consejos

1. **Empiece solo con lectura** — implemente primero la conexión, los instrumentos y los datos de mercado. Añada las operaciones de trading después de verificar
2. **Use el sandbox** — pruebe en el entorno de prueba de la bolsa
3. **Tome como referencia conectores existentes** — proporcione a la IA código de un conector existente de StockSharp como referencia
4. **Registre todo** — los logs detallados son invaluables al depurar un conector
5. **Maneje los casos límite** — reconexión, cambios de instrumentos, tipos de órdenes no estándar
