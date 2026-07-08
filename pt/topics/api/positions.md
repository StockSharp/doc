# Gestão de Posições

StockSharp fornece um sistema flexível de gestão de posições que permite acompanhar o estado atual das posições, calculá-las com base em ordens ou negócios, e manter um histórico do ciclo de vida (abertura, fecho, reversões).

## PositionManager

A classe [PositionManager](xref:StockSharp.Algo.Positions.PositionManager) implementa a interface [IPositionManager](xref:StockSharp.Algo.Positions.IPositionManager) e serve como componente principal para calcular posições atuais com base nas mensagens recebidas.

### Criar o Gestor

O construtor aceita dois parâmetros:

```cs
var state = new PositionManagerState();
var manager = new PositionManager(byOrders: false, state);
```

- `byOrders = true` -- a posição é calculada com base nas alterações do saldo das ordens. Adequado quando o sistema de negociação recebe atualizações do estado das ordens, mas não negócios individuais.
- `byOrders = false` -- a posição é calculada com base nos volumes dos negócios (modo recomendado). Fornece uma contabilização mais precisa das operações executadas.

### Processar Mensagens

O método `ProcessMessage` aceita uma mensagem de entrada ([Message](xref:StockSharp.Messages.Message)) e devolve uma [PositionChangeMessage](xref:StockSharp.Messages.PositionChangeMessage) quando a posição muda, ou `null` se a posição não tiver mudado:

```cs
var posChange = manager.ProcessMessage(executionMsg);

if (posChange != null)
{
    Console.WriteLine($"Position: {posChange.CurrentValue}");
}
```

## IPositionManagerState

A interface [IPositionManagerState](xref:StockSharp.Algo.Positions.IPositionManagerState) descreve o estado interno do gestor de posições. A implementação [PositionManagerState](xref:StockSharp.Algo.Positions.PositionManagerState) armazena informações sobre ordens e posições atuais.

### Métodos Principais

| Método | Descrição |
|--------|-------------|
| `AddOrGetOrder` | Regista uma nova ordem ou devolve uma existente por `transactionId` |
| `TryGetOrder` | Obtém parâmetros da ordem (instrumento, portefólio, direção, saldo) |
| `UpdateOrderBalance` | Atualiza o saldo atual da ordem após execução parcial |
| `RemoveOrder` | Remove uma ordem concluída do acompanhamento |
| `UpdatePosition` | Atualiza a posição por instrumento e portefólio, devolve o novo valor |
| `Clear` | Reinicia todo o estado do gestor |

### Exemplo de Trabalho com Estado

```cs
var state = new PositionManagerState();

// Registrar uma ordem
state.AddOrGetOrder(
    transactionId: 12345,
    securityId: secId,
    portfolioName: "MyPortfolio",
    side: Sides.Buy,
    volume: 100,
    balance: 100
);

// Atualizar após execução parcial
state.UpdateOrderBalance(12345, newBalance: 60);

// Atualizar posição diretamente
var newPosition = state.UpdatePosition(secId, "MyPortfolio", diff: 40);
Console.WriteLine($"Current position: {newPosition}");

// Clear
state.Clear();
```

## PositionLifecycleTracker

A classe [PositionLifecycleTracker](xref:StockSharp.Algo.Positions.PositionLifecycleTracker) acompanha o ciclo de vida completo das posições -- desde a abertura até ao fecho (round-trip). Isto é útil para analisar negócios individuais, calcular o lucro de cada posição e gerar relatórios.

### Características Principais

- **History**: a propriedade `History` (`IReadOnlyList<ReportPosition>`) contém todas as posições round-trip concluídas.
- Evento **`RoundTripClosed`**: é acionado quando uma posição é fechada (valor chegou a zero) ou revertida (sinal da posição mudou).
- Método **`ProcessPosition`**: aceita um objeto [Position](xref:StockSharp.BusinessEntities.Position) e atualiza o estado interno.

### Estados Detetados

| Estado | Descrição |
|-------|-------------|
| Opening | A posição transita de zero para um valor diferente de zero |
| Closing | O valor da posição chega a zero |
| Reversal | O sinal da posição muda (por exemplo, de longa para curta) |

### Exemplo de Utilização

```cs
var tracker = new PositionLifecycleTracker();

tracker.RoundTripClosed += report =>
{
    Console.WriteLine($"Round-trip completed:");
    Console.WriteLine($"  Opened: {report.OpenTime}");
    Console.WriteLine($"  Closed: {report.CloseTime}");
};

// Processar atualizações de posição
tracker.ProcessPosition(position);

// Ver histórico
foreach (var report in tracker.History)
{
    Console.WriteLine($"  {report.OpenTime} -> {report.CloseTime}");
}
```

## PositionMessageAdapter

A classe [PositionMessageAdapter](xref:StockSharp.Algo.Positions.PositionMessageAdapter) é um wrapper em torno de um adapter de mensagens que calcula automaticamente posições a partir do fluxo de mensagens. É usada dentro da infraestrutura interna do conector.

### Como Funciona

```cs
var innerAdapter = connector.Adapter;
var posManager = new PositionManager(byOrders: false, new PositionManagerState());
var posAdapter = new PositionMessageAdapter(innerAdapter, posManager);
```

O adapter intercepta mensagens de execução de ordens e de negócios, chama `PositionManager.ProcessMessage` e gera instâncias `PositionChangeMessage` correspondentes para handlers a montante.

## Posições em Estratégias

Na classe [Strategy](xref:StockSharp.Algo.Strategies.Strategy), a posição atual é acedida através da propriedade `Position`:

```cs
// Posição atual do instrumento principal
decimal currentPosition = Position;

// Fechar posição
if (Position > 0)
    SellMarket(Math.Abs(Position));
else if (Position < 0)
    BuyMarket(Math.Abs(Position));

// Ou por meio de um método integrado
ClosePosition();
```

Para mais detalhes sobre operações de negociação em estratégias, consulte a secção [Operações de Negociação](strategies/trading_operations.md).

## Ver Também

- [Operações de Negociação](strategies/trading_operations.md)
- [Proteção de Posição](strategies/take_profit_and_stop_loss.md)
- [Gestão da Posição-Alvo](strategies/target_position_management.md)
- [Relatórios](strategies/reporting.md)
