# Futuros Contínuos por Volume (VolumeContinuousSecurity)

## Visão Geral

A classe `VolumeContinuousSecurity` representa um contrato de futuros contínuo em que a transição (rollover) entre contratos ocorre com base no volume de negociação ou no juro em aberto. Isto difere de `ExpirationContinuousSecurity`, onde a mudança é realizada de acordo com datas de vencimento predefinidas.

Ambas as classes herdam de `ContinuousSecurity`, que por sua vez herda de `BasketSecurity`.

## Diferença em Relação a ExpirationContinuousSecurity

| Característica | ExpirationContinuousSecurity | VolumeContinuousSecurity |
|---|---|---|
| Condição de rollover | Data de vencimento (fixa) | Limiar de volume ou juro em aberto |
| Configuração | Dicionário `SecurityId -> DateTime` | Lista `SecurityId` + `VolumeLevel` |
| Previsibilidade | Mudança por calendário | Mudança por condições de mercado |
| Código do cabaz | `CE` | `CV` |

`ExpirationContinuousSecurity` requer a especificação manual das datas de transição para cada contrato. `VolumeContinuousSecurity` muda automaticamente para o contrato seguinte quando o seu volume de negociação (ou juro em aberto) excede o limiar especificado.

## Propriedades Principais

```csharp
public class VolumeContinuousSecurity : ContinuousSecurity
{
    // Lista de instrumentos internos (contratos), ordenada pela sequência de rollover
    public SynchronizedList<SecurityId> InnerSecurities { get; }

    // Utilizar juro em aberto em vez de volume para determinar o rollover
    public bool IsOpenInterest { get; set; }

    // Limiar de volume no qual ocorre a mudança para o contrato seguinte
    public Unit VolumeLevel { get; set; }
}
```

A propriedade `VolumeLevel` tem o tipo `Unit`, que permite especificar valores absolutos e percentuais.

## Exemplo de Utilização

```csharp
using StockSharp.Algo;
using StockSharp.Messages;

// Criar um futuro contínuo baseado em volume
var continuous = new VolumeContinuousSecurity
{
    Id = "ES-CONT@CME",
    Board = ExchangeBoard.Cme,
};

// Adicionar contratos pela ordem de rollover
continuous.InnerSecurities.AddRange(new[]
{
    "ES-3.26@CME".ToSecurityId(),
    "ES-6.26@CME".ToSecurityId(),
    "ES-9.26@CME".ToSecurityId(),
});

// Definir limiar de volume para mudança
continuous.VolumeLevel = new Unit(10000);

// Ou utilizar juro em aberto
continuous.IsOpenInterest = true;
continuous.VolumeLevel = new Unit(50000);
```

## Exemplo com ExpirationContinuousSecurity para Comparação

```csharp
using StockSharp.Algo;
using StockSharp.Messages;

// Futuro contínuo baseado em vencimento
var expContinuous = new ExpirationContinuousSecurity
{
    Id = "ES-CONT-EXP@CME",
    Board = ExchangeBoard.Cme,
};

// Especificar datas de transição exactas para cada contrato
expContinuous.ExpirationJumps.Add(
    "ES-3.26@CME".ToSecurityId(),
    new DateTime(2026, 3, 15)
);
expContinuous.ExpirationJumps.Add(
    "ES-6.26@CME".ToSecurityId(),
    new DateTime(2026, 6, 15)
);
```

## Quando Utilizar

`VolumeContinuousSecurity` é adequado para situações em que:

- As datas exactas de rollover não são conhecidas antecipadamente
- É necessária mudança baseada na liquidez (volume de negociação ou juro em aberto)
- É necessária uma transição mais adaptativa que responda às condições de mercado

`ExpirationContinuousSecurity` é preferível quando as datas de vencimento são conhecidas antecipadamente e é necessário um rollover determinístico.
