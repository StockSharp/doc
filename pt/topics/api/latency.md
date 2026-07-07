# Medição de Latência

[S#](../api.md) mede a latência de registo e cancelamento de ordens através do [LatencyManager](xref:StockSharp.Algo.Latency.LatencyManager). O gestor determina quanto tempo passa entre o envio de uma ordem e a recepção da confirmação da bolsa.

## Interface ILatencyManager

A interface [ILatencyManager](xref:StockSharp.Algo.Latency.ILatencyManager) define o contrato base:

- **LatencyRegistration** — latência total de registo em todas as ordens (TimeSpan).
- **LatencyCancellation** — latência total de cancelamento em todas as ordens (TimeSpan).
- **Reset()** — repõe o estado do gestor.
- **ProcessMessage(Message)** — processa uma mensagem; devolve a latência para a operação indicada ou `null`.

## Como Funciona

O gestor de latência opera segundo o princípio "pedido-resposta":

### 1. Registo de Ordem

Quando é recebida uma [OrderRegisterMessage](xref:StockSharp.Messages.OrderRegisterMessage), o gestor guarda o par (`TransactionId`, `LocalTime`) — o momento em que a ordem foi enviada.

### 2. Cancelamento de Ordem

Quando é recebida uma [OrderCancelMessage](xref:StockSharp.Messages.OrderCancelMessage), o gestor guarda o par (`TransactionId`, `LocalTime`) — o momento em que o cancelamento foi enviado.

### 3. Substituição de Ordem

Quando é recebida uma [OrderReplaceMessage](xref:StockSharp.Messages.OrderReplaceMessage), o gestor regista simultaneamente um cancelamento (da ordem antiga) e um registo (da nova ordem).

### 4. Confirmação

Quando é recebida uma [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage) com informação de ordem (não no estado `Pending` e não `Failed`), o gestor calcula a latência:

```
Latency = ExecutionMessage.LocalTime - StoredLocalTime
```

O resultado é adicionado a `LatencyRegistration` ou `LatencyCancellation`, dependendo do tipo de operação.

## Estado: ILatencyManagerState

A interface [ILatencyManagerState](xref:StockSharp.Algo.Latency.ILatencyManagerState) armazena o estado interno do gestor:

- Registos pendentes: `AddRegistration(transactionId, localTime)` / `TryGetAndRemoveRegistration(transactionId, out localTime)`
- Cancelamentos pendentes: `AddCancellation(transactionId, localTime)` / `TryGetAndRemoveCancellation(transactionId, out localTime)`
- Latências acumuladas: `LatencyRegistration`, `LatencyCancellation`
- Métodos de adição: `AddLatencyRegistration(TimeSpan)`, `AddLatencyCancellation(TimeSpan)`

A implementação predefinida é [LatencyManagerState](xref:StockSharp.Algo.Latency.LatencyManagerState).

## Tratamento de Erros

Se uma ordem falhar (`OrderState == Failed`), a latência não é contabilizada — o registo é simplesmente removido do armazenamento de estado.

## Integração via Adaptador

A classe [LatencyMessageAdapter](xref:StockSharp.Algo.Latency.LatencyMessageAdapter) envolve um adaptador interno e mede automaticamente a latência de todas as operações de ordens.

## Integração com Estratégia

A estratégia ([Strategy](xref:StockSharp.Algo.Strategies.Strategy)) expõe a propriedade `Latency` para acompanhar a latência.

## Exemplo de Utilização

```cs
// Criar um gestor com um armazenamento de estado
var manager = new LatencyManager(new LatencyManagerState());

// Processar registo de ordem (guardar a hora de envio)
manager.ProcessMessage(orderRegisterMsg);

// Processar confirmação (calcular latência)
TimeSpan? latency = manager.ProcessMessage(executionMsg);
if (latency != null)
{
    Console.WriteLine($"Latency: {latency.Value.TotalMilliseconds} ms");
}

// Latências totais
Console.WriteLine($"Registration latency: {manager.LatencyRegistration.TotalMilliseconds} ms");
Console.WriteLine($"Cancellation latency: {manager.LatencyCancellation.TotalMilliseconds} ms");
```

## Repor o Estado

O método `Reset()` limpa todos os registos pendentes e repõe as latências acumuladas a zero:

```cs
manager.Reset();
```
