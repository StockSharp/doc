# Gestão de Lucros e Perdas

[S#](../api.md) implementa o cálculo de lucros e perdas (PnL) através do [PnLManager](xref:StockSharp.Algo.PnL.PnLManager). O gestor processa um fluxo de mensagens (negócios, dados de mercado) e calcula o lucro realizado e não realizado.

## Interface IPnLManager

A interface [IPnLManager](xref:StockSharp.Algo.PnL.IPnLManager) define o contrato base:

- **RealizedPnL** - lucro/perda realizado (decimal). Acumulado quando posições são fechadas.
- **UnrealizedPnL** - lucro/perda não realizado (decimal). Recalculado com base nos preços de mercado atuais.
- **Reset()** - reinicia o estado do gestor.
- **UpdateSecurity(Level1ChangeMessage)** - atualiza parâmetros do instrumento (passo de preço, preço do passo, multiplicador de lote).
- **ProcessMessage(Message, ICollection\<PortfolioPnLManager\>)** - processa uma mensagem; devolve [PnLInfo](xref:StockSharp.Algo.PnL.PnLInfo) quando uma posição é fechada, caso contrário `null`.

## Arquitetura

O sistema de PnL tem uma hierarquia de três níveis:

```
PnLManager
  └── PortfolioPnLManager (por nome de portefólio)
        └── PnLQueue (por SecurityId)
```

- [PnLManager](xref:StockSharp.Algo.PnL.PnLManager) - nível superior, gere um dicionário de gestores de portefólio.
- [PortfolioPnLManager](xref:StockSharp.Algo.PnL.PortfolioPnLManager) - gestor de PnL para um portefólio específico, gere filas por instrumento.
- [PnLQueue](xref:StockSharp.Algo.PnL.PnLQueue) - fila FIFO para correspondência de negócios num único instrumento.

### PnLQueue - Fila de Cálculo

[PnLQueue](xref:StockSharp.Algo.PnL.PnLQueue) é responsável por fazer a correspondência entre negócios de abertura e de fecho:

- **PriceStep** - passo de preço do instrumento.
- **StepPrice** - custo do passo de preço (para futuros).
- **Leverage** - alavancagem.
- **LotMultiplier** - multiplicador de lote.

O multiplicador de lucro é calculado usando a fórmula:

```
Multiplier = (StepPrice / PriceStep) * Leverage * LotMultiplier
```

Para ações normais (em que `StepPrice` não está definido), o multiplicador é igual a `1 * Leverage * LotMultiplier`.

## PnLInfo - Resultado do Processamento de Negócios

A classe [PnLInfo](xref:StockSharp.Algo.PnL.PnLInfo) contém o resultado do fecho de uma posição:

- **ServerTime** - hora do negócio.
- **ClosedVolume** - volume da posição fechada.
- **PnL** - lucro realizado deste negócio.

Por exemplo, se a posição era +2 e chegou um negócio de -5 contratos, então `ClosedVolume = 2` (foram fechados 2 contratos da posição).

## Configurar Fontes de Dados

[PnLManager](xref:StockSharp.Algo.PnL.PnLManager) permite selecionar fontes de dados de mercado para o cálculo do lucro não realizado:

| Propriedade | Predefinição | Descrição |
|----------|:-------:|-------------|
| `UseTick` | `true` | Usar negócios tick. |
| `UseOrderBook` | `false` | Usar livro de ordens (melhor bid/ask). |
| `UseLevel1` | `false` | Usar dados Level1. |
| `UseOrderLog` | `false` | Usar log de ordens. |
| `UseCandles` | `true` | Usar candles (preço de fecho). |

## Integração via Adapter

A classe [PnLMessageAdapter](xref:StockSharp.Algo.PnL.PnLMessageAdapter) envolve um adapter interno e processa automaticamente todas as mensagens para o cálculo de PnL.

## Integração com Estratégia

A estratégia ([Strategy](xref:StockSharp.Algo.Strategies.Strategy)) fornece:

- Propriedade `PnLManager` - a instância do gestor.
- Propriedade `PnL` - lucro total (`RealizedPnL + UnrealizedPnL`).
- Evento `PnLChanged` - notificação de alterações no lucro.
- Evento `PnLReceived2` - notificação quando são recebidos novos dados de PnL.

## Exemplo de Utilização

```cs
var pnlManager = new PnLManager
{
    UseTick = true,
    UseOrderBook = true,
    UseCandles = true
};

// Processing messages
var info = pnlManager.ProcessMessage(executionMsg);
if (info != null)
{
    Console.WriteLine($"Closed: {info.ClosedVolume}, PnL: {info.PnL}");
}

// Total profit/loss
var realizedPnL = pnlManager.RealizedPnL;
var unrealizedPnL = pnlManager.UnrealizedPnL;
var totalPnL = realizedPnL + unrealizedPnL;

Console.WriteLine($"Realized PnL: {realizedPnL}");
Console.WriteLine($"Unrealized PnL: {unrealizedPnL}");
Console.WriteLine($"Total PnL: {totalPnL}");
```

## Reiniciar o Estado

O método `Reset()` limpa todos os gestores de portefólio, filas de cálculo e reinicia o PnL realizado para zero:

```cs
pnlManager.Reset();
```
