# Sistema de comisiones

[S#](../api.md) implementa un sistema flexible de cálculo de comisiones a través de [CommissionManager](xref:StockSharp.Algo.Commissions.CommissionManager). El gestor acepta mensajes de órdenes y operaciones y calcula las comisiones en función de las reglas configuradas.

## Interfaz ICommissionManager

La interfaz [ICommissionManager](xref:StockSharp.Algo.Commissions.ICommissionManager) define el contrato base:

- **Rules** — una colección de reglas [ICommissionRule](xref:StockSharp.Algo.Commissions.ICommissionRule) para el cálculo de comisiones.
- **Commission** — el monto total acumulado de la comisión (decimal).
- **Reset()** — restablece el estado del gestor y de todas las reglas.
- **Process(Message)** — procesa un mensaje; devuelve la comisión para el mensaje dado o `null`.

## Interfaz ICommissionRule

Cada regla implementa [ICommissionRule](xref:StockSharp.Algo.Commissions.ICommissionRule):

- **Title** — el título de la regla.
- **Value** — el valor de la comisión ([Unit](xref:Ecng.ComponentModel.Unit)), puede ser absoluto o basado en porcentaje.
- **Process(ExecutionMessage)** — calcula la comisión para un mensaje específico.

La clase base [CommissionRule](xref:StockSharp.Algo.Commissions.CommissionRule) contiene un método auxiliar `GetValue(price, volume)`:

- Para valores **absolutos**, devuelve `Value` tal cual.
- Para valores **porcentuales**, calcula `(price * volume * Value) / 100`.

## Tipos de reglas

### Reglas de órdenes

| Clase | Descripción |
|-------|-------------|
| [CommissionOrderRule](xref:StockSharp.Algo.Commissions.CommissionOrderRule) | Comisión por orden (basada en el precio y el volumen de la orden). |
| [CommissionOrderVolumeRule](xref:StockSharp.Algo.Commissions.CommissionOrderVolumeRule) | Comisión basada en el volumen de la orden. Para valores absolutos: `Value * volume`. |
| [CommissionOrderCountRule](xref:StockSharp.Algo.Commissions.CommissionOrderCountRule) | Comisión por cada N órdenes. La propiedad `Count` establece el umbral. |

### Reglas de operaciones

| Clase | Descripción |
|-------|-------------|
| [CommissionTradeRule](xref:StockSharp.Algo.Commissions.CommissionTradeRule) | Comisión por operación (basada en el precio y el volumen de la operación). |
| [CommissionTradeVolumeRule](xref:StockSharp.Algo.Commissions.CommissionTradeVolumeRule) | Comisión basada en el volumen de la operación. |
| [CommissionTradePriceRule](xref:StockSharp.Algo.Commissions.CommissionTradePriceRule) | Comisión: `price * volume * Value`. |
| [CommissionTradeCountRule](xref:StockSharp.Algo.Commissions.CommissionTradeCountRule) | Comisión por cada N operaciones. La propiedad `Count` establece el umbral. |
| [CommissionTurnOverRule](xref:StockSharp.Algo.Commissions.CommissionTurnOverRule) | Comisión por cada umbral de volumen de negociación. La propiedad `TurnOver` establece el umbral. |

### Reglas de filtro

| Clase | Descripción |
|-------|-------------|
| [CommissionSecurityIdRule](xref:StockSharp.Algo.Commissions.CommissionSecurityIdRule) | Comisión solo para un instrumento específico. Propiedad `Security`. |
| [CommissionBoardCodeRule](xref:StockSharp.Algo.Commissions.CommissionBoardCodeRule) | Comisión solo para una plataforma de negociación específica. Propiedad `Board`. |
| [CommissionSecurityTypeRule](xref:StockSharp.Algo.Commissions.CommissionSecurityTypeRule) | Comisión solo para un tipo de instrumento específico. Propiedad `SecurityType`. |

## Integración mediante adaptador

La clase [CommissionMessageAdapter](xref:StockSharp.Algo.Commissions.CommissionMessageAdapter) envuelve un adaptador interno y calcula automáticamente las comisiones en los mensajes [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage) entrantes y salientes. Si un mensaje no tiene establecido el campo `Commission`, el adaptador lo completa a partir del gestor.

## Integración con Strategy

La estrategia ([Strategy](xref:StockSharp.Algo.Strategies.Strategy)) expone la propiedad `Commission`, mediante la cual se puede rastrear la comisión acumulada.

## Ejemplo de uso

```cs
var manager = new CommissionManager();

// Fixed commission of 1.5 per trade
manager.Rules.Add(new CommissionTradeRule { Value = 1.5m });

// 0.1% of turnover for futures
manager.Rules.Add(new CommissionSecurityTypeRule
{
    SecurityType = SecurityTypes.Future,
    Value = new Unit(0.1m, UnitTypes.Percent)
});

// Commission of 50 for every 100 orders
manager.Rules.Add(new CommissionOrderCountRule
{
    Count = 100,
    Value = 50m
});

// Commission of 10 for every 1,000,000 in turnover
manager.Rules.Add(new CommissionTurnOverRule
{
    TurnOver = 1_000_000m,
    Value = 10m
});

// Processing a message
decimal? commission = manager.Process(executionMsg);
if (commission != null)
{
    Console.WriteLine($"Commission for message: {commission.Value}");
}

// Total accumulated commission
Console.WriteLine($"Total commission: {manager.Commission}");
```

## Restablecimiento del estado

El método `Reset()` restablece la comisión total a cero y llama a `Reset()` en cada regla, lo que borra los contadores internos (número de órdenes, volumen de negociación actual, etc.):

```cs
manager.Reset();
```
