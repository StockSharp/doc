# Futuros continuos por volumen (VolumeContinuousSecurity)

## Descripción general

La clase `VolumeContinuousSecurity` representa un contrato de futuros continuo donde la transición (rollover) entre contratos ocurre en función del volumen de negociación o del interés abierto. Esto difiere de `ExpirationContinuousSecurity`, donde el cambio se realiza según fechas de vencimiento predefinidas.

Ambas clases heredan de `ContinuousSecurity`, que a su vez hereda de `BasketSecurity`.

## Diferencia con ExpirationContinuousSecurity

| Característica | ExpirationContinuousSecurity | VolumeContinuousSecurity |
|---|---|---|
| Condición de rollover | Fecha de vencimiento (fija) | Umbral de volumen o interés abierto |
| Configuración | Diccionario `SecurityId -> DateTime` | Lista de `SecurityId` + `VolumeLevel` |
| Previsibilidad | Cambio por calendario | Cambio por condiciones de mercado |
| Código de cesta | `CE` | `CV` |

`ExpirationContinuousSecurity` requiere especificar manualmente las fechas de transición para cada contrato. `VolumeContinuousSecurity` cambia automáticamente al siguiente contrato cuando su volumen de negociación (o interés abierto) supera el umbral especificado.

## Propiedades principales

```csharp
public class VolumeContinuousSecurity : ContinuousSecurity
{
    // Lista de instrumentos internos (contratos), ordenados por secuencia de rollover
    public SynchronizedList<SecurityId> InnerSecurities { get; }

    // Usar interés abierto en lugar de volumen para determinar el rollover
    public bool IsOpenInterest { get; set; }

    // Umbral de volumen en el que ocurre el cambio al siguiente contrato
    public Unit VolumeLevel { get; set; }
}
```

La propiedad `VolumeLevel` tiene el tipo `Unit`, que permite especificar valores tanto absolutos como porcentuales.

## Ejemplo de uso

```csharp
using StockSharp.Algo;
using StockSharp.Messages;

// Crear un futuro continuo basado en volumen
var continuous = new VolumeContinuousSecurity
{
    Id = "ES-CONT@CME",
    Board = ExchangeBoard.Cme,
};

// Agregar contratos en orden de rollover
continuous.InnerSecurities.AddRange(new[]
{
    "ES-3.26@CME".ToSecurityId(),
    "ES-6.26@CME".ToSecurityId(),
    "ES-9.26@CME".ToSecurityId(),
});

// Establecer el umbral de volumen para el cambio
continuous.VolumeLevel = new Unit(10000);

// O usar interés abierto
continuous.IsOpenInterest = true;
continuous.VolumeLevel = new Unit(50000);
```

## Ejemplo con ExpirationContinuousSecurity para comparación

```csharp
using StockSharp.Algo;
using StockSharp.Messages;

// Futuro continuo basado en vencimiento
var expContinuous = new ExpirationContinuousSecurity
{
    Id = "ES-CONT-EXP@CME",
    Board = ExchangeBoard.Cme,
};

// Especificar fechas exactas de transición para cada contrato
expContinuous.ExpirationJumps.Add(
    "ES-3.26@CME".ToSecurityId(),
    new DateTime(2026, 3, 15)
);
expContinuous.ExpirationJumps.Add(
    "ES-6.26@CME".ToSecurityId(),
    new DateTime(2026, 6, 15)
);
```

## Cuándo usarlo

`VolumeContinuousSecurity` es adecuado para situaciones en las que:

- Las fechas exactas de rollover no se conocen de antemano
- Se requiere cambio basado en liquidez (volumen de negociación o interés abierto)
- Se necesita una transición más adaptativa que responda a las condiciones de mercado

`ExpirationContinuousSecurity` es preferible cuando las fechas de vencimiento se conocen de antemano y se requiere un rollover determinista.
