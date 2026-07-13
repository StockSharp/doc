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
# Reglas del proyecto — conector de bolsa

- Marco de trabajo: StockSharp 5.x, .NET 10
- El conector se implementa como un MessageAdapter
- Heredar de AsyncMessageAdapter para async/await
- Todas las peticiones HTTP mediante HttpClient con CancellationToken
- Suscripciones WebSocket mediante cliente nativo o ClientWebSocket
- Mapeo de tipos: tipos de la bolsa → StockSharp Messages
- Gestión de errores: SendOutError() para errores de conexión
- Todas las cadenas en recursos de localización (o al menos como const)
```

## Arquitectura del conector

Un conector en StockSharp es un `MessageAdapter` que:
1. Recibe mensajes entrantes (solicitudes) del núcleo
2. Los procesa (llama a la API de la bolsa)
3. Envía de vuelta mensajes de respuesta (resultados)

```
StockSharp Core → [Message] → MessageAdapter → [HTTP/WS] → bolsa
bolsa → [HTTP/WS] → MessageAdapter → [Message] → StockSharp Core
```

## Ejemplo paso a paso

### Paso 1: Estructura básica del adaptador

Prompt:

```
Cree con StockSharp un MessageAdapter básico para un conector de bolsa
de criptomonedas llamado MyExchange:
- heredar de AsyncMessageAdapter
- implementar conexión/desconexión (ConnectMessage, DisconnectMessage)
- añadir ajustes: ApiKey, Secret, modo demo
- usar HttpClient para la REST API
- URL base de la API: https://api.myexchange.com/v1
```

### Paso 2: Búsqueda de instrumentos

Prompt:

```
Añada al adaptador el manejo de SecurityLookupMessage:
- La petición GET /api/v1/symbols devuelve una lista JSON de instrumentos
- Cada instrumento tiene: symbol, baseAsset, quoteAsset,
  minQty, maxQty, tickSize, status
- Mapeo: symbol → SecurityId, baseAsset/quoteAsset → name,
  tickSize → SecurityMessage.PriceStep
- Enviar SecurityMessage para cada instrumento
- Enviar SubscriptionFinishedMessage al final
```

### Paso 3: Datos de mercado

Prompt:

```
Añada al adaptador la suscripción a datos de mercado:

1. Velas (MarketDataTypes.CandleTimeFrame):
   - REST: GET /api/v1/klines?symbol={}&interval={}&limit=1000
   - WebSocket: suscribirse al canal kline_{symbol}_{interval}
   - Mapeo de intervalos: 1m, 5m, 15m, 1h, 4h, 1d

2. Libro de órdenes (MarketDataTypes.MarketDepth):
   - WebSocket: suscribirse al canal depth_{symbol}
   - Convertir bids/asks en QuoteChangeMessage

3. Ticks (MarketDataTypes.Trades):
   - WebSocket: suscribirse al canal trades_{symbol}
   - Convertir en ExecutionMessage con ExecutionTypes.Tick
```

### Paso 4: Operaciones de negociación

Prompt:

```
Añada al adaptador soporte para operaciones de negociación:

1. Registro de orden (OrderRegisterMessage):
   - POST /api/v1/order con parámetros: symbol, side, type, quantity, price
   - Devolver ExecutionMessage con ExecutionTypes.Transaction

2. Cancelación de orden (OrderCancelMessage):
   - DELETE /api/v1/order/{orderId}
   - Devolver ExecutionMessage con estado OrderStates.Done

3. Obtención de cartera (PortfolioLookupMessage):
   - GET /api/v1/account
   - Convertir balances en PositionChangeMessage

4. WebSocket para actualizaciones de órdenes:
   - Canal orders_{listenKey}
   - Procesar actualizaciones del estado de las órdenes
```

### Paso 5: Revisión de código

Después de generar cada paso, pregunte a la IA:

```
Revise el adaptador generado para comprobar su compatibilidad con la API de StockSharp:
1. ¿Se gestionan todos los tipos de mensajes?
2. ¿Se usa CancellationToken correctamente?
3. ¿Está implementada la gestión de errores HTTP?
4. ¿Se envía SubscriptionFinishedMessage al finalizar?
5. ¿Funciona la reconexión WebSocket tras una desconexión?
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

### Negociación
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
Añada a mi conector soporte para datos de Nivel 1 (BestBid/BestAsk):
- Canal WebSocket: ticker_{symbol}
- Parsear bid, ask, last, volume
- Enviar Level1ChangeMessage con los campos:
  Level1Fields.BestBidPrice, Level1Fields.BestAskPrice,
  Level1Fields.LastTradePrice, Level1Fields.Volume
```

### Manejo de límites de tasa

```
Añada a mi conector la gestión de límites de frecuencia:
- La API devuelve las cabeceras X-RateLimit-Remaining y X-RateLimit-Reset
- Cuando se alcance el límite: esperar hasta Reset y registrar una advertencia
- Usar SemaphoreSlim para limitar las peticiones concurrentes
```

## Consejos

1. **Empiece solo con lectura** — implemente primero la conexión, los instrumentos y los datos de mercado. Añada las operaciones de negociación después de verificar
2. **Use el sandbox** — pruebe en el entorno de prueba de la bolsa
3. **Tome como referencia conectores existentes** — proporcione a la IA código de un conector existente de StockSharp como referencia
4. **Registre todo** — los registros detallados son invaluables al depurar un conector
5. **Maneje los casos límite** — reconexión, cambios de instrumentos, tipos de órdenes no estándar
