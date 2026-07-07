# Referência de Estatísticas

O [StatisticManager](xref:StockSharp.Algo.Statistics.StatisticManager) gere uma coleção de instâncias [IStatisticParameter](xref:StockSharp.Algo.Statistics.IStatisticParameter). Cada parâmetro acompanha uma métrica específica durante a execução da estratégia. Todos os parâmetros disponíveis são criados usando o [StatisticParameterRegistry](xref:StockSharp.Algo.Statistics.StatisticParameterRegistry).

Para uma visão geral do trabalho com estatísticas de estratégia, consulte a secção [Estatísticas](statistics.md).

## Interfaces

O sistema de estatísticas é construído sobre uma hierarquia de interfaces. Cada interface define a fonte de dados para calcular o parâmetro:

| Interface | Descrição |
|-----------|-------------|
| [IStatisticParameter](xref:StockSharp.Algo.Statistics.IStatisticParameter) | Interface base: propriedades `Name`, `Type`, `Value`, `DisplayName`, `Description`, `Category`, `Order`; método `Reset()` |
| [IPnLStatisticParameter](xref:StockSharp.Algo.Statistics.IPnLStatisticParameter) | Parâmetros baseados em lucro/perda: método `Add(marketTime, pnl, commission)` |
| [ITradeStatisticParameter](xref:StockSharp.Algo.Statistics.ITradeStatisticParameter) | Parâmetros baseados em transações: método `Add(PnLInfo)` |
| [IOrderStatisticParameter](xref:StockSharp.Algo.Statistics.IOrderStatisticParameter) | Parâmetros baseados em ordens: métodos `New(order)`, `Changed(order)`, `RegisterFailed(fail)`, `CancelFailed(fail)` |
| [IPositionStatisticParameter](xref:StockSharp.Algo.Statistics.IPositionStatisticParameter) | Parâmetros baseados em posições: método `Add(marketTime, position)` |
| [IRiskFreeRateStatisticParameter](xref:StockSharp.Algo.Statistics.IRiskFreeRateStatisticParameter) | Parâmetros com taxa sem risco: propriedade `RiskFreeRate` |
| [IBeginValueStatisticParameter](xref:StockSharp.Algo.Statistics.IBeginValueStatisticParameter) | Parâmetros com valor inicial: propriedade `BeginValue` |

## Parâmetros de Lucro e Perda (P&L)

Todos os parâmetros deste grupo implementam a interface [IPnLStatisticParameter](xref:StockSharp.Algo.Statistics.IPnLStatisticParameter) e herdam de [BasePnLStatisticParameter](xref:StockSharp.Algo.Statistics.BasePnLStatisticParameter`1). Recebem dados em cada atualização do valor de P&L da estratégia.

| Classe | Descrição | Tipo de Valor |
|-------|-------------|------------|
| [NetProfitParameter](xref:StockSharp.Algo.Statistics.NetProfitParameter) | Lucro líquido de todo o período. Definido igual ao valor atual de P&L | `decimal` |
| [NetProfitPercentParameter](xref:StockSharp.Algo.Statistics.NetProfitPercentParameter) | Lucro líquido em percentagem. Requer definir `BeginValue` (capital inicial). Fórmula: `pnl * 100 / BeginValue` | `decimal` |
| [MaxProfitParameter](xref:StockSharp.Algo.Statistics.MaxProfitParameter) | Lucro máximo (valor de P&L mais alto em todo o período) | `decimal` |
| [MaxProfitPercentParameter](xref:StockSharp.Algo.Statistics.MaxProfitPercentParameter) | Lucro máximo em percentagem. Requer `BeginValue`. Fórmula: `MaxProfit * 100 / BeginValue` | `decimal` |
| [MaxProfitDateParameter](xref:StockSharp.Algo.Statistics.MaxProfitDateParameter) | Data em que o lucro máximo foi atingido | `DateTime` |
| [MaxDrawdownParameter](xref:StockSharp.Algo.Statistics.MaxDrawdownParameter) | Drawdown absoluto máximo. Diferença entre o pico e o vale da curva de capital | `decimal` |
| [MaxDrawdownPercentParameter](xref:StockSharp.Algo.Statistics.MaxDrawdownPercentParameter) | Drawdown máximo em percentagem. Fórmula: `MaxDrawdown * 100 / MaxEquity` | `decimal` |
| [MaxDrawdownDateParameter](xref:StockSharp.Algo.Statistics.MaxDrawdownDateParameter) | Data do drawdown máximo | `DateTime` |
| [MaxRelativeDrawdownParameter](xref:StockSharp.Algo.Statistics.MaxRelativeDrawdownParameter) | Drawdown relativo máximo. Calculado como o rácio entre o drawdown e o valor máximo do capital | `decimal` |
| [ReturnParameter](xref:StockSharp.Algo.Statistics.ReturnParameter) | Retorno relativo de todo o período. Crescimento máximo do vale até ao valor atual em termos relativos | `decimal` |
| [CommissionParameter](xref:StockSharp.Algo.Statistics.CommissionParameter) | Comissão total paga. Acumula todos os valores de comissão | `decimal` |
| [AverageDrawdownParameter](xref:StockSharp.Algo.Statistics.AverageDrawdownParameter) | Drawdown médio. Média aritmética de todos os drawdowns concluídos e atuais | `decimal` |
| [RecoveryFactorParameter](xref:StockSharp.Algo.Statistics.RecoveryFactorParameter) | Fator de recuperação. Fórmula: `NetProfit / MaxDrawdown` | `decimal` |
| [SharpeRatioParameter](xref:StockSharp.Algo.Statistics.SharpeRatioParameter) | Rácio de Sharpe. Fórmula: `(retorno anualizado - taxa sem risco) / desvio padrão anualizado` | `decimal` |
| [SortinoRatioParameter](xref:StockSharp.Algo.Statistics.SortinoRatioParameter) | Rácio de Sortino. Semelhante ao Sharpe, mas considera apenas desvios negativos | `decimal` |
| [CalmarRatioParameter](xref:StockSharp.Algo.Statistics.CalmarRatioParameter) | Rácio de Calmar. Fórmula: `NetProfit / MaxDrawdown` | `decimal` |
| [SterlingRatioParameter](xref:StockSharp.Algo.Statistics.SterlingRatioParameter) | Rácio de Sterling. Fórmula: `NetProfit / AverageDrawdown` | `decimal` |

### Coeficientes de Risco

O [SharpeRatioParameter](xref:StockSharp.Algo.Statistics.SharpeRatioParameter) e o [SortinoRatioParameter](xref:StockSharp.Algo.Statistics.SortinoRatioParameter) herdam da classe base [RiskAdjustedRatioParameter](xref:StockSharp.Algo.Statistics.RiskAdjustedRatioParameter) e implementam a interface [IRiskFreeRateStatisticParameter](xref:StockSharp.Algo.Statistics.IRiskFreeRateStatisticParameter).

Suportam as seguintes definições:

- **RiskFreeRate** -- taxa anual sem risco (por exemplo, `0.03m` = 3%)
- **Period** -- período de cálculo do retorno (predefinição `TimeSpan.FromDays(1)`)

O [CalmarRatioParameter](xref:StockSharp.Algo.Statistics.CalmarRatioParameter) e o [SterlingRatioParameter](xref:StockSharp.Algo.Statistics.SterlingRatioParameter) dependem de outros parâmetros (`NetProfitParameter`, `MaxDrawdownParameter`, `AverageDrawdownParameter`) e são automaticamente associados quando criados através de [StatisticParameterRegistry](xref:StockSharp.Algo.Statistics.StatisticParameterRegistry).

## Parâmetros de Transações

Todos os parâmetros deste grupo implementam a interface [ITradeStatisticParameter](xref:StockSharp.Algo.Statistics.ITradeStatisticParameter). Recebem dados através do objeto [PnLInfo](xref:StockSharp.Algo.PnL.PnLInfo) para cada transação executada.

| Classe | Descrição | Tipo de Valor |
|-------|-------------|------------|
| [TradeCountParameter](xref:StockSharp.Algo.Statistics.TradeCountParameter) | Número total de transações (apenas são contabilizadas transações com `ClosedVolume > 0`) | `int` |
| [WinningTradesParameter](xref:StockSharp.Algo.Statistics.WinningTradesParameter) | Número de transações lucrativas (`ClosedVolume > 0` e `PnL > 0`) | `int` |
| [LossingTradesParameter](xref:StockSharp.Algo.Statistics.LossingTradesParameter) | Número de transações perdedoras (`ClosedVolume > 0` e `PnL < 0`) | `int` |
| [RoundtripCountParameter](xref:StockSharp.Algo.Statistics.RoundtripCountParameter) | Número de round-trips concluídos (transações de fecho com `ClosedVolume > 0`) | `int` |
| [AverageTradeProfitParameter](xref:StockSharp.Algo.Statistics.AverageTradeProfitParameter) | Lucro médio por transação. Fórmula: `SumPnL / Count` | `decimal` |
| [AverageWinTradeParameter](xref:StockSharp.Algo.Statistics.AverageWinTradeParameter) | Lucro médio das transações lucrativas. Apenas são consideradas transações com `PnL > 0` | `decimal` |
| [AverageLossTradeParameter](xref:StockSharp.Algo.Statistics.AverageLossTradeParameter) | Perda média das transações perdedoras. Apenas são consideradas transações com `PnL < 0` | `decimal` |
| [ProfitFactorParameter](xref:StockSharp.Algo.Statistics.ProfitFactorParameter) | Fator de lucro. Fórmula: `GrossProfit / GrossLoss` | `decimal` |
| [ExpectancyParameter](xref:StockSharp.Algo.Statistics.ExpectancyParameter) | Esperança matemática. Fórmula: `P(win) * AvgWin + P(loss) * AvgLoss` | `decimal` |
| [PerMonthTradeParameter](xref:StockSharp.Algo.Statistics.PerMonthTradeParameter) | Número médio de transações por mês | `decimal` |
| [PerDayTradeParameter](xref:StockSharp.Algo.Statistics.PerDayTradeParameter) | Número médio de transações por dia | `decimal` |
| [GrossProfitParameter](xref:StockSharp.Algo.Statistics.GrossProfitParameter) | Lucro bruto. Soma do P&L de todas as transações lucrativas (`PnL > 0`) | `decimal` |
| [GrossLossParameter](xref:StockSharp.Algo.Statistics.GrossLossParameter) | Perda bruta. Soma do P&L de todas as transações perdedoras (`PnL < 0`, valor negativo) | `decimal` |

## Parâmetros de Posição

Os parâmetros deste grupo implementam a interface [IPositionStatisticParameter](xref:StockSharp.Algo.Statistics.IPositionStatisticParameter). Recebem dados em cada alteração de posição.

| Classe | Descrição | Tipo de Valor |
|-------|-------------|------------|
| [MaxLongPositionParameter](xref:StockSharp.Algo.Statistics.MaxLongPositionParameter) | Posição longa máxima. Maior valor de posição positivo | `decimal` |
| [MaxShortPositionParameter](xref:StockSharp.Algo.Statistics.MaxShortPositionParameter) | Posição curta máxima. Maior valor absoluto de posição negativa | `decimal` |

## Parâmetros de Ordens

Todos os parâmetros deste grupo implementam a interface [IOrderStatisticParameter](xref:StockSharp.Algo.Statistics.IOrderStatisticParameter) e herdam de [BaseOrderStatisticParameter](xref:StockSharp.Algo.Statistics.BaseOrderStatisticParameter`1). Recebem dados sobre registo de ordens, alterações e erros.

| Classe | Descrição | Tipo de Valor |
|-------|-------------|------------|
| [OrderCountParameter](xref:StockSharp.Algo.Statistics.OrderCountParameter) | Número total de ordens registadas | `int` |
| [OrderRegisterErrorCountParameter](xref:StockSharp.Algo.Statistics.OrderRegisterErrorCountParameter) | Número de erros de registo de ordens | `int` |
| [OrderInsufficientFundErrorCountParameter](xref:StockSharp.Algo.Statistics.OrderInsufficientFundErrorCountParameter) | Número de erros de "fundos insuficientes" (tipo `InsufficientFundException`) | `int` |
| [OrderCancelErrorCountParameter](xref:StockSharp.Algo.Statistics.OrderCancelErrorCountParameter) | Número de erros de cancelamento de ordens | `int` |

## Parâmetros de Latência

Os parâmetros de latência também implementam [IOrderStatisticParameter](xref:StockSharp.Algo.Statistics.IOrderStatisticParameter), mas acompanham as características temporais do processamento de ordens.

| Classe | Descrição | Tipo de Valor |
|-------|-------------|------------|
| [MaxLatencyRegistrationParameter](xref:StockSharp.Algo.Statistics.MaxLatencyRegistrationParameter) | Latência máxima de registo de ordem (propriedade `Order.LatencyRegistration`) | `TimeSpan` |
| [MinLatencyRegistrationParameter](xref:StockSharp.Algo.Statistics.MinLatencyRegistrationParameter) | Latência mínima de registo de ordem | `TimeSpan` |
| [MaxLatencyCancellationParameter](xref:StockSharp.Algo.Statistics.MaxLatencyCancellationParameter) | Latência máxima de cancelamento de ordem (propriedade `Order.LatencyCancellation`) | `TimeSpan` |
| [MinLatencyCancellationParameter](xref:StockSharp.Algo.Statistics.MinLatencyCancellationParameter) | Latência mínima de cancelamento de ordem | `TimeSpan` |

## Utilização

### Aceder a Estatísticas da Estratégia

```cs
var strategy = new MyStrategy();

// Aceder às estatísticas após a execução
foreach (var param in strategy.StatisticManager.Parameters)
{
    Console.WriteLine($"{param.DisplayName}: {param.Value}");
}
```

### Obter um Parâmetro Específico

```cs
// Obter o valor de lucro líquido
var netProfit = strategy.StatisticManager.Parameters
    .OfType<NetProfitParameter>()
    .First();

Console.WriteLine($"Net profit: {netProfit.Value}");
```

### Configurar a Taxa Sem Risco para Coeficientes

Os rácios de Sharpe e Sortino exigem a definição da taxa sem risco para um cálculo correto:

```cs
// Definir uma taxa sem risco de 3% para todos os coeficientes
foreach (var param in strategy.StatisticManager.Parameters
    .OfType<IRiskFreeRateStatisticParameter>())
{
    param.RiskFreeRate = 0.03m;
}
```

### Configurar Capital Inicial para Parâmetros Percentuais

Os parâmetros [NetProfitPercentParameter](xref:StockSharp.Algo.Statistics.NetProfitPercentParameter) e [MaxProfitPercentParameter](xref:StockSharp.Algo.Statistics.MaxProfitPercentParameter) exigem a definição do valor de capital inicial:

```cs
// Definir capital inicial para cálculos percentuais
foreach (var param in strategy.StatisticManager.Parameters
    .OfType<IBeginValueStatisticParameter>())
{
    param.BeginValue = 1_000_000m; // 1,000,000
}
```

### Repor Estatísticas

```cs
// Repor todos os parâmetros estatísticos
strategy.StatisticManager.Reset();
```

### Guardar e Carregar Estado

Todos os parâmetros suportam serialização através da interface `IPersistable`:

```cs
// Guardar
var storage = new SettingsStorage();
strategy.StatisticManager.Save(storage);

// Carregar
strategy.StatisticManager.Load(storage);
```

## Ver Também

- [Estatísticas da Estratégia](statistics.md)
- [Componente Gráfico de Estatísticas](../graphical_user_interface/strategies/statistics.md)
