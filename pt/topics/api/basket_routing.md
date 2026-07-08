# Roteamento de Adaptadores

O StockSharp suporta conexões simultâneas a múltiplas bolsas e corretoras. O sistema de roteamento (basket routing) gerencia quais mensagens são direcionadas para quais adaptadores, garantindo uma operação transparente com múltiplas conexões.

## Arquitetura Geral

Ao usar múltiplos adaptadores, o conector cria automaticamente um basket (cesta) que combina todas as conexões. O roteador determina para qual adaptador cada mensagem específica deve ser direcionada -- assinaturas de dados de mercado, transações, requisições de portfólio, etc.

## AdapterRouter

A interface [IAdapterRouter](xref:StockSharp.Algo.IAdapterRouter) define a lógica de roteamento de mensagens entre adaptadores.

### Principais Métodos

| Método | Descrição |
|--------|-------------|
| `GetAdapters` | Retorna uma lista de adaptadores adequados para processar a mensagem fornecida |
| `GetSubscriptionAdaptersAsync` | Determina assincronamente os adaptadores para assinaturas de dados de mercado |
| `GetPortfolioAdapter` | Retorna o adaptador vinculado a um portfólio específico |
| `TryGetOrderAdapter` | Encontra o adaptador através do qual uma ordem foi registrada |
| `SetSecurityAdapter` | Vincula um instrumento a um adaptador específico |
| `SetPortfolioAdapter` | Vincula um portfólio a um adaptador específico |

### Prioridades de Roteamento

O sistema determina o adaptador de destino na seguinte ordem de prioridade:

1. **Especificação explícita** -- se um adaptador é especificado na mensagem através da propriedade `message.Adapter`, esse adaptador é usado.
2. **Vinculação de instrumento** -- mapeamento definido via `SetSecurityAdapter`.
3. **Vinculação de tipo de dado** -- adaptadores registrados para um tipo de mensagem específico.
4. **Filtragem por tipo suportado** -- são selecionados os adaptadores que suportam o tipo de mensagem em questão.

### Configurando o Roteamento

```cs
var router = connector.Adapter.InnerAdapters;

// Vincular o instrumento ao adaptador para receber dados de ticks
router.SetSecurityAdapter(
    secId,
    DataType.Ticks,
    binanceAdapter
);

// Vincular a carteira ao adaptador para transações
router.SetPortfolioAdapter(
    "MyPortfolio",
    interactiveBrokersAdapter
);
```

## Gerenciando Conexões

### Estados de Conexão

Cada adaptador no basket passa pelos estados de conexão padrão:

- **Disconnected** -- desconectado
- **Connecting** -- conexão em andamento
- **Connected** -- conectado
- **Disconnecting** -- desconexão em andamento

O basket agrega os estados de todos os adaptadores aninhados.

### Parâmetros de Agregação

A propriedade `ConnectDisconnectEventOnFirstAdapter` determina quando o basket é considerado conectado:

- `true` -- o evento de conexão dispara quando o **primeiro** adaptador se conecta (padrão). Permite iniciar o trabalho sem esperar por todas as conexões.
- `false` -- o evento dispara somente após **todos** os adaptadores se conectarem.

```cs
// Aguardar a conexão de todos os adaptadores
connector.Adapter.InnerAdapters.ConnectDisconnectEventOnFirstAdapter = false;

connector.Connected += () =>
{
    Console.WriteLine("All adapters connected");
};

connector.Connect();
```

## Assinaturas Pai e Filho

Ao trabalhar com múltiplos adaptadores, uma única assinatura pode ser dividida em múltiplas assinaturas filhas, cada uma direcionada ao seu próprio adaptador. O sistema automaticamente:

- Cria assinaturas filhas para cada adaptador adequado
- Agrega as respostas antes de notificar a assinatura pai
- Trata erros parciais (se um adaptador falhar ao assinar, os demais continuam funcionando)

### Exemplo Multi-Bolsa

```cs
// Adicionar adaptadores
connector.Adapter.InnerAdapters.Add(binanceAdapter);
connector.Adapter.InnerAdapters.Add(bybitAdapter);

connector.Connect();

// Assinatura de ticks -- será roteada automaticamente
// para todos os adaptadores que suportam o instrumento informado
var subscription = new Subscription(DataType.Ticks, security);
connector.Subscribe(subscription);
```

## Fila de Mensagens Pendentes

Se nenhum adaptador estiver conectado quando uma mensagem é enviada, a mensagem é colocada em uma fila pendente (`IPendingMessageState`). Quando um adaptador se conecta, todas as mensagens acumuladas são enviadas automaticamente.

```cs
// Registrar uma ordem antes da conexão -- a ordem será enviada
// automaticamente após a conexão ser estabelecida
connector.RegisterOrder(order);
connector.Connect();
```

## Configurando Múltiplas Conexões

### Configuração Programática

```cs
// Criar adaptadores
var binance = new BinanceMessageAdapter(connector.TransactionIdGenerator)
{
    Key = "<API_KEY>",
    Secret = "<API_SECRET>".Secure(),
};

var ib = new InteractiveBrokersMessageAdapter(connector.TransactionIdGenerator)
{
    Address = InteractiveBrokersMessageAdapter.DefaultAddress,
};

// Adicionar à cesta
connector.Adapter.InnerAdapters.Add(binance);
connector.Adapter.InnerAdapters.Add(ib);

// Configurar roteamento
connector.Adapter.InnerAdapters.SetPortfolioAdapter("BinancePortfolio", binance);
connector.Adapter.InnerAdapters.SetPortfolioAdapter("IBPortfolio", ib);

connector.Connect();
```

### Configuração Gráfica

Para configuração visual de conexões, use o componente de configuração gráfica. Veja a seção [Configuração gráfica](connectors/graphical_configuration.md) para detalhes.

## Rastreamento de Ordens

O roteador rastreia automaticamente qual adaptador foi usado para registrar cada ordem. Ao receber atualizações de ordem (mudanças de estado, negociações), o sistema as roteia através do mesmo adaptador:

```cs
// A ordem será registrada pelo adaptador vinculado à carteira
var order = new Order
{
    Security = security,
    Portfolio = portfolio,
    Side = Sides.Buy,
    Price = price,
    Volume = volume,
};

connector.RegisterOrder(order);

// O cancelamento passará automaticamente pelo mesmo adaptador
connector.CancelOrder(order);
```

## Veja Também

- [Conectores](connectors.md)
- [Configuração gráfica](connectors/graphical_configuration.md)
- [Gerenciamento de Posições](positions.md)
