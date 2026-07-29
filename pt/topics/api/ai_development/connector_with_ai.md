# Escrevendo um Conector com IA

Um guia passo a passo para criar um conector de bolsa para o StockSharp usando ferramentas de IA.

## Preparação

### 1. Estude a API da bolsa

Antes de começar, prepare:
- Documentação da API REST/WebSocket da bolsa
- Chaves de API de teste (sandbox/testnet)
- Lista de tipos de dados suportados (velas, livro de ofertas, ticks, negociações)
- Lista de tipos de ordem suportados (limit, market, stop)

### 2. Crie um Projeto

```bash
dotnet new classlib -n StockSharp.MyExchange --framework net10.0
cd StockSharp.MyExchange
dotnet add package StockSharp.Messages
dotnet add package StockSharp.Algo
```

### 3. Prepare o Contexto para a IA

Crie um ficheiro `CLAUDE.md`:

```markdown
# Regras do projeto — conector de bolsa

- Plataforma: StockSharp 5.x, .NET 10
- O conector é implementado como um MessageAdapter
- Herdar de AsyncMessageAdapter para async/await
- Todos os pedidos HTTP via HttpClient com CancellationToken
- Subscrições WebSocket via cliente nativo ou ClientWebSocket
- Mapeamento de tipos: tipos da bolsa → StockSharp Messages
- Tratamento de erros: SendOutError() para erros de ligação
- Todas as strings em recursos de localização (ou pelo menos como const)
```

## Arquitetura do Conector

Um conector no StockSharp é um `MessageAdapter` que:
1. Recebe mensagens de entrada (requisições) do núcleo
2. Processa-as (chama a API da bolsa)
3. Envia de volta mensagens de resposta (resultados)

```
StockSharp Core → [Message] → MessageAdapter → [HTTP/WS] → bolsa
bolsa → [HTTP/WS] → MessageAdapter → [Message] → StockSharp Core
```

## Exemplo Passo a Passo

### Passo 1: Estrutura Básica do Adaptador

Prompt:

```
Crie com StockSharp um MessageAdapter básico para um conector de bolsa
de criptomoedas chamado MyExchange:
- herdar de AsyncMessageAdapter
- implementar ligação/desligação (ConnectMessage, DisconnectMessage)
- adicionar definições: ApiKey, Secret, modo demo
- usar HttpClient para a REST API
- URL base da API: https://api.myexchange.com/v1
```

### Passo 2: Busca de Instrumentos

Prompt:

```
Adicione ao adaptador o tratamento de SecurityLookupMessage:
- O pedido GET /api/v1/symbols devolve uma lista JSON de instrumentos
- Cada instrumento contém: symbol, baseAsset, quoteAsset,
  minQty, maxQty, tickSize, status
- Mapeamento: symbol → SecurityId, baseAsset/quoteAsset → name,
  tickSize → SecurityMessage.PriceStep
- Enviar SecurityMessage para cada instrumento
- Enviar SubscriptionFinishedMessage no final
```

### Passo 3: Dados de Mercado

Prompt:

```
Adicione ao adaptador a subscrição de dados de mercado:

1. Velas (MarketDataTypes.CandleTimeFrame):
   - REST: GET /api/v1/klines?symbol={}&interval={}&limit=1000
   - WebSocket: subscrever o canal kline_{symbol}_{interval}
   - Mapeamento de intervalos: 1m, 5m, 15m, 1h, 4h, 1d

2. Livro de ordens (MarketDataTypes.MarketDepth):
   - WebSocket: subscrever o canal depth_{symbol}
   - Converter bids/asks em QuoteChangeMessage

3. Ticks (MarketDataTypes.Trades):
   - WebSocket: subscrever o canal trades_{symbol}
   - Converter em ExecutionMessage com ExecutionTypes.Tick
```

### Passo 4: Operações de Negociação

Prompt:

```
Adicione ao adaptador suporte para operações de negociação:

1. Registo de ordem (OrderRegisterMessage):
   - POST /api/v1/order com parâmetros: symbol, side, type, quantity, price
   - Devolver ExecutionMessage com ExecutionTypes.Transaction

2. Cancelamento de ordem (OrderCancelMessage):
   - DELETE /api/v1/order/{orderId}
   - Devolver ExecutionMessage com estado OrderStates.Done

3. Obtenção de carteira (PortfolioLookupMessage):
   - GET /api/v1/account
   - Converter saldos em PositionChangeMessage

4. WebSocket para atualizações de ordens:
   - Canal orders_{listenKey}
   - Processar atualizações do estado das ordens
```

### Passo 5: Revisão de Código

Após gerar cada etapa, pergunte à IA:

```
Reveja o adaptador gerado quanto à conformidade com a API StockSharp:
1. Todos os tipos de mensagens são tratados?
2. CancellationToken é usado corretamente?
3. O tratamento de erros HTTP está implementado?
4. SubscriptionFinishedMessage é enviado após a conclusão?
5. A nova ligação WebSocket funciona após uma desligação?
```

## Detalhes-Chave da Implementação

### MessageAdapter — O Que Tratar

| Mensagem de Entrada | Ação | Mensagem de Resposta |
|-----------------|--------|-----------------|
| `ConnectMessage` | Ligar à API | `ConnectMessage` (resposta) |
| `DisconnectMessage` | Desligar | `DisconnectMessage` (resposta) |
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

### Ligação
- [ ] Ligar/desligar funciona corretamente
- [ ] Erros de autenticação são tratados
- [ ] O restabelecimento da ligação funciona após uma desligação

### Instrumentos
- [ ] Lista de instrumentos carrega com sucesso
- [ ] Mapeamento correto: SecurityId, PriceStep, VolumeStep
- [ ] `SubscriptionFinishedMessage` é enviada

### Dados de Mercado
- [ ] Velas: carregamento de histórico + assinatura de novos
- [ ] Livro de ofertas: profundidade correta, atualizações
- [ ] Ticks: horário, volume e direção corretos

### Negociação
- [ ] Ordens limit: criação, cancelamento
- [ ] Ordens market: criação
- [ ] Atualizações do estado da ordem
- [ ] Atualizações de saldo do portfólio

### Geral
- [ ] Todos os `CancellationToken` são propagados
- [ ] Erros são registrados via `SendOutError()`
- [ ] Sem vazamentos de recursos (WebSocket, HttpClient)
- [ ] Compila sem erros ou avisos

## Exemplos de Prompts

### Adicionando um Novo Tipo de Dado

```
Adicione ao meu conector suporte para dados de Nível 1 (BestBid/BestAsk):
- Canal WebSocket: ticker_{symbol}
- Fazer parse de bid, ask, last, volume
- Enviar Level1ChangeMessage com os campos:
  Level1Fields.BestBidPrice, Level1Fields.BestAskPrice,
  Level1Fields.LastTradePrice, Level1Fields.Volume
```

### Tratamento de Limites de Taxa

```
Adicione ao meu conector o tratamento de limites de frequência:
- A API devolve os cabeçalhos X-RateLimit-Remaining e X-RateLimit-Reset
- Quando o limite for atingido: aguardar até Reset e registar um aviso
- Usar SemaphoreSlim para limitar pedidos concorrentes
```

## Dicas

1. **Comece somente leitura** — primeiro implemente a ligação, os instrumentos e os dados de mercado. Adicione operações de negociação após a verificação
2. **Use o sandbox** — teste no ambiente de testes da bolsa
3. **Referencie conectores existentes** — forneça à IA código de um conector StockSharp existente como referência
4. **Registe tudo** — registos detalhados são inestimáveis ao depurar um conector
5. **Trate casos extremos** — restabelecimento da ligação, mudanças de instrumentos, tipos de ordem não padronizados
