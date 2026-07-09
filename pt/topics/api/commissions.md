# Sistema de Comissões

[S#](../api.md) implementa um sistema flexível de cálculo de comissões por meio do [CommissionManager](xref:StockSharp.Algo.Commissions.CommissionManager). O gerenciador aceita mensagens de ordens e negociações e calcula as comissões com base em regras configuradas.

## Interface ICommissionManager

A interface [ICommissionManager](xref:StockSharp.Algo.Commissions.ICommissionManager) define o contrato base:

- **Rules** — uma coleção de regras [ICommissionRule](xref:StockSharp.Algo.Commissions.ICommissionRule) para o cálculo de comissões.
- **Commission** — o valor total acumulado de comissão (decimal).
- **Reset()** — redefine o estado do gerenciador e de todas as regras.
- **Process(Message)** — processa uma mensagem; retorna a comissão para a mensagem informada ou `null`.

## Interface ICommissionRule

Cada regra implementa [ICommissionRule](xref:StockSharp.Algo.Commissions.ICommissionRule):

- **Title** — o título da regra.
- **Value** — o valor da comissão ([Unit](xref:Ecng.ComponentModel.Unit)), pode ser absoluto ou baseado em percentual.
- **Process(ExecutionMessage)** — calcula a comissão para uma mensagem específica.

A classe base [CommissionRule](xref:StockSharp.Algo.Commissions.CommissionRule) contém um método auxiliar `GetValue(price, volume)`:

- Para valores **absolutos**, retorna `Value` como está.
- Para valores **percentuais**, calcula `(price * volume * Value) / 100`.

## Tipos de Regras

### Regras de Ordens

| Classe | Descrição |
|-------|-------------|
| [CommissionOrderRule](xref:StockSharp.Algo.Commissions.CommissionOrderRule) | Comissão por ordem (com base no preço e no volume da ordem). |
| [CommissionOrderVolumeRule](xref:StockSharp.Algo.Commissions.CommissionOrderVolumeRule) | Comissão baseada no volume da ordem. Para valores absolutos: `Value * volume`. |
| [CommissionOrderCountRule](xref:StockSharp.Algo.Commissions.CommissionOrderCountRule) | Comissão a cada N ordens. A propriedade `Count` define o limite. |

### Regras de Negociações

| Classe | Descrição |
|-------|-------------|
| [CommissionTradeRule](xref:StockSharp.Algo.Commissions.CommissionTradeRule) | Comissão por negociação (com base no preço e no volume da negociação). |
| [CommissionTradeVolumeRule](xref:StockSharp.Algo.Commissions.CommissionTradeVolumeRule) | Comissão baseada no volume da negociação. |
| [CommissionTradePriceRule](xref:StockSharp.Algo.Commissions.CommissionTradePriceRule) | Comissão: `price * volume * Value`. |
| [CommissionTradeCountRule](xref:StockSharp.Algo.Commissions.CommissionTradeCountRule) | Comissão a cada N negociações. A propriedade `Count` define o limite. |
| [CommissionTurnOverRule](xref:StockSharp.Algo.Commissions.CommissionTurnOverRule) | Comissão para cada limite de volume negociado (turnover). A propriedade `TurnOver` define o limite. |

### Regras de Filtro

| Classe | Descrição |
|-------|-------------|
| [CommissionSecurityIdRule](xref:StockSharp.Algo.Commissions.CommissionSecurityIdRule) | Comissão apenas para um instrumento específico. Propriedade `Security`. |
| [CommissionBoardCodeRule](xref:StockSharp.Algo.Commissions.CommissionBoardCodeRule) | Comissão apenas para uma bolsa específica. Propriedade `Board`. |
| [CommissionSecurityTypeRule](xref:StockSharp.Algo.Commissions.CommissionSecurityTypeRule) | Comissão apenas para um tipo específico de instrumento. Propriedade `SecurityType`. |

## Integração via Adaptador

A classe [CommissionMessageAdapter](xref:StockSharp.Algo.Commissions.CommissionMessageAdapter) envolve um adaptador interno e calcula automaticamente as comissões nas mensagens [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage) recebidas e enviadas. Se uma mensagem não tiver o campo `Commission` definido, o adaptador o preenche a partir do gerenciador.

## Integração com a Estratégia

A estratégia ([Strategy](xref:StockSharp.Algo.Strategies.Strategy)) expõe a propriedade `Commission`, por meio da qual você pode acompanhar a comissão acumulada.

## Exemplo de Uso

```cs
var manager = new CommissionManager();

// Comissão fixa de 1,5 por negociação
manager.Rules.Add(new CommissionTradeRule { Value = 1.5m });

// 0,1% do giro para futuros
manager.Rules.Add(new CommissionSecurityTypeRule
{
    SecurityType = SecurityTypes.Future,
    Value = new Unit(0.1m, UnitTypes.Percent)
});

// Comissão de 50 para cada 100 ordens
manager.Rules.Add(new CommissionOrderCountRule
{
    Count = 100,
    Value = 50m
});

// Comissão de 10 para cada 1.000.000 em giro
manager.Rules.Add(new CommissionTurnOverRule
{
    TurnOver = 1_000_000m,
    Value = 10m
});

// Processar mensagem
decimal? commission = manager.Process(executionMsg);
if (commission != null)
{
    Console.WriteLine($"Comissão da mensagem: {commission.Value}");
}

// Comissão acumulada total
Console.WriteLine($"Comissão total: {manager.Commission}");
```

## Redefinindo o Estado

O método `Reset()` redefine a comissão total para zero e chama `Reset()` em cada regra, o que limpa os contadores internos (contagem de ordens, volume negociado atual, etc.):

```cs
manager.Reset();
```
