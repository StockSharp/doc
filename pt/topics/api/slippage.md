# Medição de Slippage

[S#](../api.md) calcula o slippage através do [SlippageManager](xref:StockSharp.Algo.Slippage.SlippageManager). Slippage é a diferença entre o preço de execução esperado de uma ordem e o preço real do negócio.

## Interface ISlippageManager

A interface [ISlippageManager](xref:StockSharp.Algo.Slippage.ISlippageManager) define o contrato base:

- **Slippage** - slippage total acumulado (decimal).
- **Reset()** - reinicia o estado do gestor.
- **ProcessMessage(Message)** - processa uma mensagem; devolve o slippage para a execução indicada ou `null`.

## Como Funciona

O gestor de slippage funciona em três etapas:

### 1. Atualizar Preços de Mercado

Quando é recebido um [Level1ChangeMessage](xref:StockSharp.Messages.Level1ChangeMessage) ou [QuoteChangeMessage](xref:StockSharp.Messages.QuoteChangeMessage), o gestor guarda os melhores preços bid e ask para cada instrumento.

### 2. Guardar o Preço Planeado

Quando é recebida uma [OrderRegisterMessage](xref:StockSharp.Messages.OrderRegisterMessage), o gestor guarda o "preço planeado" - o melhor preço de mercado no momento do registo da ordem:

- Para uma compra (`Buy`), é usado o melhor ask.
- Para uma venda (`Sell`), é usado o melhor bid.

### 3. Calcular Slippage

Quando é recebida uma [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage) com um negócio, o gestor calcula o slippage:

- Para uma compra: `(TradePrice - PlannedPrice) * TradeVolume`
- Para uma venda: `(PlannedPrice - TradePrice) * TradeVolume`

Um valor positivo significa slippage desfavorável (preço pior do que o esperado), enquanto um valor negativo significa slippage favorável (preço melhor do que o esperado).

## Estado: ISlippageManagerState

A interface [ISlippageManagerState](xref:StockSharp.Algo.Slippage.ISlippageManagerState) armazena o estado interno do gestor:

- Melhores preços bid/ask para cada instrumento ([SecurityId](xref:StockSharp.Messages.SecurityId)).
- Preços planeados e direções para cada transação (`TransactionId`).
- Slippage total acumulado.

A implementação predefinida é [SlippageManagerState](xref:StockSharp.Algo.Slippage.SlippageManagerState).

## Definições

| Propriedade | Predefinição | Descrição |
|----------|:-------:|-------------|
| `CalculateNegative` | `true` | Contabilizar slippage favorável. Se `false`, os valores negativos são substituídos por zero. |

## Integração via adaptador

A classe [SlippageMessageAdapter](xref:StockSharp.Algo.Slippage.SlippageMessageAdapter) envolve um adapter interno e calcula automaticamente o slippage para todos os negócios.

## Integração com Estratégia

A estratégia ([Strategy](xref:StockSharp.Algo.Strategies.Strategy)) expõe a propriedade `Slippage` para acompanhar o slippage global.

## Exemplo de Utilização

```cs
// Criar gerenciador com armazenamento de estado
var manager = new SlippageManager(new SlippageManagerState());

// Considerar apenas slippage desfavorável
manager.CalculateNegative = false;

// Processar dados de mercado (atualizar melhores preços)
manager.ProcessMessage(level1Msg);
manager.ProcessMessage(quoteChangeMsg);

// Processar registo de ordem (guardar o preço planeado)
manager.ProcessMessage(orderRegisterMsg);

// Processar uma negociação (calcular slippage)
decimal? slippage = manager.ProcessMessage(executionMsg);
if (slippage != null)
{
    Console.WriteLine($"Slippage: {slippage.Value}");
}

// Slippage acumulado total
Console.WriteLine($"Slippage total: {manager.Slippage}");
```

## Reiniciar o Estado

O método `Reset()` limpa completamente o estado interno: melhores preços, preços planeados e slippage acumulado:

```cs
manager.Reset();
```
