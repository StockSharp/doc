# Referencia de estadísticas

El [StatisticManager](xref:StockSharp.Algo.Statistics.StatisticManager) gestiona una colección de instancias de [IStatisticParameter](xref:StockSharp.Algo.Statistics.IStatisticParameter). Cada parámetro sigue una métrica específica durante la ejecución de la estrategia. Todos los parámetros disponibles se crean mediante [StatisticParameterRegistry](xref:StockSharp.Algo.Statistics.StatisticParameterRegistry).

Para una descripción general del trabajo con estadísticas de estrategia, consulte la sección [Estadísticas](statistics.md).

## Interfaces

El sistema de estadísticas se basa en una jerarquía de interfaces. Cada interfaz define la fuente de datos para calcular el parámetro:

| Interfaz | Descripción |
|-----------|-------------|
| [IStatisticParameter](xref:StockSharp.Algo.Statistics.IStatisticParameter) | Interfaz base: propiedades `Name`, `Type`, `Value`, `DisplayName`, `Description`, `Category`, `Order`; método `Reset()` |
| [IPnLStatisticParameter](xref:StockSharp.Algo.Statistics.IPnLStatisticParameter) | Parámetros basados en beneficios/pérdidas: método `Add(marketTime, pnl, commission)` |
| [ITradeStatisticParameter](xref:StockSharp.Algo.Statistics.ITradeStatisticParameter) | Parámetros basados en operaciones: método `Add(PnLInfo)` |
| [IOrderStatisticParameter](xref:StockSharp.Algo.Statistics.IOrderStatisticParameter) | Parámetros basados en órdenes: métodos `New(order)`, `Changed(order)`, `RegisterFailed(fail)`, `CancelFailed(fail)` |
| [IPositionStatisticParameter](xref:StockSharp.Algo.Statistics.IPositionStatisticParameter) | Parámetros basados en posiciones: método `Add(marketTime, position)` |
| [IRiskFreeRateStatisticParameter](xref:StockSharp.Algo.Statistics.IRiskFreeRateStatisticParameter) | Parámetros con tasa libre de riesgo: propiedad `RiskFreeRate` |
| [IBeginValueStatisticParameter](xref:StockSharp.Algo.Statistics.IBeginValueStatisticParameter) | Parámetros con valor inicial: propiedad `BeginValue` |

## Parámetros de beneficios y pérdidas (P&L)

Todos los parámetros de este grupo implementan la interfaz [IPnLStatisticParameter](xref:StockSharp.Algo.Statistics.IPnLStatisticParameter) y heredan de [BasePnLStatisticParameter](xref:StockSharp.Algo.Statistics.BasePnLStatisticParameter`1). Reciben datos en cada actualización del valor P&L de la estrategia.

| Clase | Descripción | Tipo de valor |
|-------|-------------|---------------|
| [NetProfitParameter](xref:StockSharp.Algo.Statistics.NetProfitParameter) | Beneficio neto de todo el periodo. Se establece igual al valor P&L actual | `decimal` |
| [NetProfitPercentParameter](xref:StockSharp.Algo.Statistics.NetProfitPercentParameter) | Beneficio neto como porcentaje. Requiere establecer `BeginValue` (capital inicial). Fórmula: `pnl * 100 / BeginValue` | `decimal` |
| [MaxProfitParameter](xref:StockSharp.Algo.Statistics.MaxProfitParameter) | Beneficio máximo (valor P&L más alto de todo el periodo) | `decimal` |
| [MaxProfitPercentParameter](xref:StockSharp.Algo.Statistics.MaxProfitPercentParameter) | Beneficio máximo como porcentaje. Requiere `BeginValue`. Fórmula: `MaxProfit * 100 / BeginValue` | `decimal` |
| [MaxProfitDateParameter](xref:StockSharp.Algo.Statistics.MaxProfitDateParameter) | Fecha en que se alcanzó el beneficio máximo | `DateTime` |
| [MaxDrawdownParameter](xref:StockSharp.Algo.Statistics.MaxDrawdownParameter) | Drawdown absoluto máximo. Diferencia entre el pico y el valle de la curva de equity | `decimal` |
| [MaxDrawdownPercentParameter](xref:StockSharp.Algo.Statistics.MaxDrawdownPercentParameter) | Drawdown máximo como porcentaje. Fórmula: `MaxDrawdown * 100 / MaxEquity` | `decimal` |
| [MaxDrawdownDateParameter](xref:StockSharp.Algo.Statistics.MaxDrawdownDateParameter) | Fecha del drawdown máximo | `DateTime` |
| [MaxRelativeDrawdownParameter](xref:StockSharp.Algo.Statistics.MaxRelativeDrawdownParameter) | Drawdown relativo máximo. Calculado como la relación entre drawdown y valor máximo de equity | `decimal` |
| [ReturnParameter](xref:StockSharp.Algo.Statistics.ReturnParameter) | Rentabilidad relativa de todo el periodo. Crecimiento máximo desde el valle hasta el valor actual en términos relativos | `decimal` |
| [CommissionParameter](xref:StockSharp.Algo.Statistics.CommissionParameter) | Comisión total pagada. Acumula todos los valores de comisión | `decimal` |
| [AverageDrawdownParameter](xref:StockSharp.Algo.Statistics.AverageDrawdownParameter) | Drawdown medio. Media aritmética de todos los drawdowns completados y actuales | `decimal` |
| [RecoveryFactorParameter](xref:StockSharp.Algo.Statistics.RecoveryFactorParameter) | Factor de recuperación. Fórmula: `NetProfit / MaxDrawdown` | `decimal` |
| [SharpeRatioParameter](xref:StockSharp.Algo.Statistics.SharpeRatioParameter) | Ratio Sharpe. Fórmula: `(rentabilidad anualizada - tasa libre de riesgo) / desviación estándar anualizada` | `decimal` |
| [SortinoRatioParameter](xref:StockSharp.Algo.Statistics.SortinoRatioParameter) | Ratio Sortino. Similar a Sharpe, pero solo considera desviaciones negativas | `decimal` |
| [CalmarRatioParameter](xref:StockSharp.Algo.Statistics.CalmarRatioParameter) | Ratio Calmar. Fórmula: `NetProfit / MaxDrawdown` | `decimal` |
| [SterlingRatioParameter](xref:StockSharp.Algo.Statistics.SterlingRatioParameter) | Ratio Sterling. Fórmula: `NetProfit / AverageDrawdown` | `decimal` |

### Coeficientes de riesgo

[SharpeRatioParameter](xref:StockSharp.Algo.Statistics.SharpeRatioParameter) y [SortinoRatioParameter](xref:StockSharp.Algo.Statistics.SortinoRatioParameter) heredan de la clase base [RiskAdjustedRatioParameter](xref:StockSharp.Algo.Statistics.RiskAdjustedRatioParameter) e implementan la interfaz [IRiskFreeRateStatisticParameter](xref:StockSharp.Algo.Statistics.IRiskFreeRateStatisticParameter).

Admiten los siguientes ajustes:

- **RiskFreeRate** -- tasa anual libre de riesgo (por ejemplo, `0.03m` = 3%)
- **Period** -- periodo de cálculo de rentabilidad (valor predeterminado `TimeSpan.FromDays(1)`)

[CalmarRatioParameter](xref:StockSharp.Algo.Statistics.CalmarRatioParameter) y [SterlingRatioParameter](xref:StockSharp.Algo.Statistics.SterlingRatioParameter) dependen de otros parámetros (`NetProfitParameter`, `MaxDrawdownParameter`, `AverageDrawdownParameter`) y se enlazan automáticamente cuando se crean mediante [StatisticParameterRegistry](xref:StockSharp.Algo.Statistics.StatisticParameterRegistry).

## Parámetros de operaciones

Todos los parámetros de este grupo implementan la interfaz [ITradeStatisticParameter](xref:StockSharp.Algo.Statistics.ITradeStatisticParameter). Reciben datos mediante el objeto [PnLInfo](xref:StockSharp.Algo.PnL.PnLInfo) para cada operación ejecutada.

| Clase | Descripción | Tipo de valor |
|-------|-------------|---------------|
| [TradeCountParameter](xref:StockSharp.Algo.Statistics.TradeCountParameter) | Número total de operaciones (solo se cuentan operaciones con `ClosedVolume > 0`) | `int` |
| [WinningTradesParameter](xref:StockSharp.Algo.Statistics.WinningTradesParameter) | Número de operaciones rentables (`ClosedVolume > 0` y `PnL > 0`) | `int` |
| [LossingTradesParameter](xref:StockSharp.Algo.Statistics.LossingTradesParameter) | Número de operaciones perdedoras (`ClosedVolume > 0` y `PnL < 0`) | `int` |
| [RoundtripCountParameter](xref:StockSharp.Algo.Statistics.RoundtripCountParameter) | Número de round-trips completados (operaciones de cierre con `ClosedVolume > 0`) | `int` |
| [AverageTradeProfitParameter](xref:StockSharp.Algo.Statistics.AverageTradeProfitParameter) | Beneficio medio por operación. Fórmula: `SumPnL / Count` | `decimal` |
| [AverageWinTradeParameter](xref:StockSharp.Algo.Statistics.AverageWinTradeParameter) | Beneficio medio de operaciones rentables. Solo se consideran operaciones con `PnL > 0` | `decimal` |
| [AverageLossTradeParameter](xref:StockSharp.Algo.Statistics.AverageLossTradeParameter) | Pérdida media de operaciones perdedoras. Solo se consideran operaciones con `PnL < 0` | `decimal` |
| [ProfitFactorParameter](xref:StockSharp.Algo.Statistics.ProfitFactorParameter) | Factor de beneficio. Fórmula: `GrossProfit / GrossLoss` | `decimal` |
| [ExpectancyParameter](xref:StockSharp.Algo.Statistics.ExpectancyParameter) | Esperanza matemática. Fórmula: `P(win) * AvgWin + P(loss) * AvgLoss` | `decimal` |
| [PerMonthTradeParameter](xref:StockSharp.Algo.Statistics.PerMonthTradeParameter) | Número medio de operaciones por mes | `decimal` |
| [PerDayTradeParameter](xref:StockSharp.Algo.Statistics.PerDayTradeParameter) | Número medio de operaciones por día | `decimal` |
| [GrossProfitParameter](xref:StockSharp.Algo.Statistics.GrossProfitParameter) | Beneficio bruto. Suma del P&L de todas las operaciones rentables (`PnL > 0`) | `decimal` |
| [GrossLossParameter](xref:StockSharp.Algo.Statistics.GrossLossParameter) | Pérdida bruta. Suma del P&L de todas las operaciones perdedoras (`PnL < 0`, el valor es negativo) | `decimal` |

## Parámetros de posición

Los parámetros de este grupo implementan la interfaz [IPositionStatisticParameter](xref:StockSharp.Algo.Statistics.IPositionStatisticParameter). Reciben datos en cada cambio de posición.

| Clase | Descripción | Tipo de valor |
|-------|-------------|---------------|
| [MaxLongPositionParameter](xref:StockSharp.Algo.Statistics.MaxLongPositionParameter) | Posición larga máxima. Valor positivo más alto de posición | `decimal` |
| [MaxShortPositionParameter](xref:StockSharp.Algo.Statistics.MaxShortPositionParameter) | Posición corta máxima. Valor negativo absoluto más alto de posición | `decimal` |

## Parámetros de órdenes

Todos los parámetros de este grupo implementan la interfaz [IOrderStatisticParameter](xref:StockSharp.Algo.Statistics.IOrderStatisticParameter) y heredan de [BaseOrderStatisticParameter](xref:StockSharp.Algo.Statistics.BaseOrderStatisticParameter`1). Reciben datos sobre registro, cambios y errores de órdenes.

| Clase | Descripción | Tipo de valor |
|-------|-------------|---------------|
| [OrderCountParameter](xref:StockSharp.Algo.Statistics.OrderCountParameter) | Número total de órdenes registradas | `int` |
| [OrderRegisterErrorCountParameter](xref:StockSharp.Algo.Statistics.OrderRegisterErrorCountParameter) | Número de errores de registro de órdenes | `int` |
| [OrderInsufficientFundErrorCountParameter](xref:StockSharp.Algo.Statistics.OrderInsufficientFundErrorCountParameter) | Número de errores de "fondos insuficientes" (tipo `InsufficientFundException`) | `int` |
| [OrderCancelErrorCountParameter](xref:StockSharp.Algo.Statistics.OrderCancelErrorCountParameter) | Número de errores de cancelación de órdenes | `int` |

## Parámetros de latencia

Los parámetros de latencia también implementan [IOrderStatisticParameter](xref:StockSharp.Algo.Statistics.IOrderStatisticParameter), pero siguen las características temporales del procesamiento de órdenes.

| Clase | Descripción | Tipo de valor |
|-------|-------------|---------------|
| [MaxLatencyRegistrationParameter](xref:StockSharp.Algo.Statistics.MaxLatencyRegistrationParameter) | Latencia máxima de registro de órdenes (propiedad `Order.LatencyRegistration`) | `TimeSpan` |
| [MinLatencyRegistrationParameter](xref:StockSharp.Algo.Statistics.MinLatencyRegistrationParameter) | Latencia mínima de registro de órdenes | `TimeSpan` |
| [MaxLatencyCancellationParameter](xref:StockSharp.Algo.Statistics.MaxLatencyCancellationParameter) | Latencia máxima de cancelación de órdenes (propiedad `Order.LatencyCancellation`) | `TimeSpan` |
| [MinLatencyCancellationParameter](xref:StockSharp.Algo.Statistics.MinLatencyCancellationParameter) | Latencia mínima de cancelación de órdenes | `TimeSpan` |

## Uso

### Acceder a estadísticas de estrategia

```cs
var strategy = new MyStrategy();

// Acceder a estadísticas después de la ejecución
foreach (var param in strategy.StatisticManager.Parameters)
{
    Console.WriteLine($"{param.DisplayName}: {param.Value}");
}
```

### Recuperar un parámetro específico

```cs
// Obtener el valor de beneficio neto
var netProfit = strategy.StatisticManager.Parameters
    .OfType<NetProfitParameter>()
    .First();

Console.WriteLine($"Net profit: {netProfit.Value}");
```

### Configurar la tasa libre de riesgo para coeficientes

Los ratios Sharpe y Sortino requieren establecer la tasa libre de riesgo para el cálculo correcto:

```cs
// Establecer una tasa libre de riesgo del 3% para todos los coeficientes
foreach (var param in strategy.StatisticManager.Parameters
    .OfType<IRiskFreeRateStatisticParameter>())
{
    param.RiskFreeRate = 0.03m;
}
```

### Configurar capital inicial para parámetros porcentuales

Los parámetros [NetProfitPercentParameter](xref:StockSharp.Algo.Statistics.NetProfitPercentParameter) y [MaxProfitPercentParameter](xref:StockSharp.Algo.Statistics.MaxProfitPercentParameter) requieren establecer el valor de capital inicial:

```cs
// Establecer capital inicial para cálculos porcentuales
foreach (var param in strategy.StatisticManager.Parameters
    .OfType<IBeginValueStatisticParameter>())
{
    param.BeginValue = 1_000_000m; // 1,000,000
}
```

### Restablecer estadísticas

```cs
// Restablecer todos los parámetros estadísticos
strategy.StatisticManager.Reset();
```

### Guardar y cargar estado

Todos los parámetros admiten serialización mediante la interfaz `IPersistable`:

```cs
// Guardado
var storage = new SettingsStorage();
strategy.StatisticManager.Save(storage);

// Carga
strategy.StatisticManager.Load(storage);
```

## Ver también

- [Estadísticas de estrategia](statistics.md)
- [Componente gráfico de estadísticas](../graphical_user_interface/strategies/statistics.md)
