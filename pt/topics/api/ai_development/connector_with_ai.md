# Escrevendo um Conector com IA

Um guia passo a passo para criar um conector de bolsa para o StockSharp usando ferramentas de IA.

## Preparação

### 1. Estude a API da Bolsa

Antes de começar, prepare:
- Documentação da API REST/WebSocket da bolsa
- Chaves de API de teste (sandbox/testnet)
- Lista de tipos de dados suportados (candles, livro de ofertas, ticks, negociações)
- Lista de tipos de ordem suportados (limit, market, stop)

### 2. Crie um Projeto

```bash
dotnet new classlib -n StockSharp.MyExchange --framework net10.0
cd StockSharp.MyExchange
dotnet add package StockSharp.Messages
dotnet add package StockSharp.Algo
```

### 3. Prepare o Contexto para a IA

Crie um arquivo `CLAUDE.md`:

```markdown
# Regras do projeto — conector de exchange

- Framework: StockSharp 5.x, .NET 10
- Connector is implemented as a MessageAdapter
- Inherit from AsyncMessageAdapter for async/await
- All HTTP requests via HttpClient with CancellationToken
- WebSocket subscriptions via native client or ClientWebSocket
- Type mapping: exchange types → StockSharp Messages
- Error handling: SendOutError() for connection errors
- All strings in localization resources (or at least const)
```

## Arquitetura do Conector

Um conector no StockSharp é um `MessageAdapter` que:
1. Recebe mensagens de entrada (requisições) do núcleo
2. Processa-as (chama a API da bolsa)
3. Envia de volta mensagens de resposta (resultados)

```
StockSharp Core → [Message] → MessageAdapter → [HTTP/WS] → Exchange
Exchange → [HTTP/WS] → MessageAdapter → [Message] → StockSharp Core
```

## Exemplo Passo a Passo

### Passo 1: Estrutura Básica do Adaptador

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

### Passo 2: Busca de Instrumentos

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

### Passo 3: Dados de Mercado

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

### Passo 4: Operações de Negociação

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

### Passo 5: Revisão de Código

Após gerar cada etapa, pergunte à IA:

```
Review the generated adapter for StockSharp API compliance:
1. Are all message types handled?
2. Is CancellationToken used correctly?
3. Is HTTP error handling in place?
4. Are SubscriptionFinishedMessage sent after completion?
5. Does WebSocket reconnection work on disconnect?
```

## Detalhes-Chave da Implementação

### MessageAdapter — O Que Tratar

| Mensagem de Entrada | Ação | Mensagem de Resposta |
|-----------------|--------|-----------------|
| `ConnectMessage` | Conectar à API | `ConnectMessage` (resposta) |
| `DisconnectMessage` | Desconectar | `DisconnectMessage` (resposta) |
| `SecurityLookupMessage` | Solicitar instrumentos | `SecurityMessage` × N |
| `MarketDataMessage` (subscribe) | Assinar dados | `SubscriptionResponseMessage` |
| `OrderRegisterMessage` | Criar ordem | `ExecutionMessage` |
| `OrderCancelMessage` | Cancelar ordem | `ExecutionMessage` |
| `PortfolioLookupMessage` | Solicitar portfólios | `PortfolioMessage`, `PositionChangeMessage` |

### Padrão Assíncrono

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

    // ... outros métodos
}
```

### Assinatura de Requisições

A maioria das bolsas exige assinatura HMAC para requisições privadas:

```csharp
private string SignRequest(string payload)
{
    using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(Secret.To<string>()));
    var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
    return Convert.ToHexString(hash).ToLowerInvariant();
}
```

## Lista de Verificação de Revisão do Conector

### Conexão
- [ ] Conectar/desconectar funciona corretamente
- [ ] Erros de autenticação são tratados
- [ ] Reconexão funciona após desconexão

### Instrumentos
- [ ] Lista de instrumentos carrega com sucesso
- [ ] Mapeamento correto: SecurityId, PriceStep, VolumeStep
- [ ] `SubscriptionFinishedMessage` é enviada

### Dados de Mercado
- [ ] Candles: carregamento de histórico + assinatura de novos
- [ ] Livro de ofertas: profundidade correta, atualizações
- [ ] Ticks: horário, volume e direção corretos

### Negociação
- [ ] Ordens limit: criação, cancelamento
- [ ] Ordens market: criação
- [ ] Atualizações de status de ordem
- [ ] Atualizações de saldo do portfólio

### Geral
- [ ] Todos os `CancellationToken` são propagados
- [ ] Erros são registrados via `SendOutError()`
- [ ] Sem vazamentos de recursos (WebSocket, HttpClient)
- [ ] Compila sem erros ou avisos

## Exemplos de Prompts

### Adicionando um Novo Tipo de Dado

```
Add Level 1 data support (BestBid/BestAsk) to my connector:
- WebSocket channel: ticker_{symbol}
- Parse bid, ask, last, volume
- Send Level1ChangeMessage with fields:
  Level1Fields.BestBidPrice, Level1Fields.BestAskPrice,
  Level1Fields.LastTradePrice, Level1Fields.Volume
```

### Tratamento de Limites de Taxa

```
Add rate limit handling to my connector:
- API returns headers X-RateLimit-Remaining and X-RateLimit-Reset
- When limit is reached: wait until Reset, log a warning
- Use SemaphoreSlim to limit concurrent requests
```

## Dicas

1. **Comece somente leitura** — primeiro implemente conexão, instrumentos e dados de mercado. Adicione operações de negociação após a verificação
2. **Use o sandbox** — teste no ambiente de testes da bolsa
3. **Referencie conectores existentes** — forneça à IA código de um conector StockSharp existente como referência
4. **Registre tudo** — logs detalhados são inestimáveis ao depurar um conector
5. **Trate casos extremos** — reconexão, mudanças de instrumentos, tipos de ordem não padronizados
