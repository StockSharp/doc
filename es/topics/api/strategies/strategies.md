# Estrategias

## Descripción general

La clase `Strategy` es la clase base para crear estrategias de trading en StockSharp. Proporciona un conjunto completo de herramientas para suscribirse a datos de mercado, gestionar órdenes y posiciones, calcular estadísticas y generar informes.

Capacidades clave de la clase `Strategy`:

- Suscripción a velas, libros de órdenes, ticks y otros datos de mercado
- Colocación, modificación y cancelación de órdenes
- Gestión de posición objetivo
- Cálculo de PnL, comisión y estadísticas
- Gestión de riesgos
- Sistema de temporizadores y reglas
- Alertas
- Generación de informes

> [!WARNING]
> La funcionalidad de estrategias hijas (`ChildStrategies`) se ha declarado obsoleta y ya no se admite. La propiedad `ChildStrategies` está marcada con el atributo `[Obsolete("Child strategies no longer supported.")]`. Si su código usa estrategias hijas, se recomienda refactorizarlo: ejecute cada estrategia como una instancia independiente.

## Secciones de documentación

- [Gestión de posición objetivo](target_position_management.md) -- gestión declarativa del tamaño de posición mediante `SetTargetPosition`
- [Modos de trading](trading_modes.md) -- restricción de actividad de trading mediante `StrategyTradingModes`
- [Sistema de alertas](alert_system.md) -- envío de notificaciones (popup, sonido, log, Telegram)
- [Sistema de temporizadores](timer_system.md) -- ejecución periódica de acciones
- [Gestión de riesgos](risk_management.md) -- reglas de gestión de riesgos
- [Suscripciones de alto nivel](high_level_subscriptions.md) -- suscripciones simplificadas a datos de mercado
- [Informes de estrategia](reporting.md) -- generación de informes de resultados de trading
- [Funciones avanzadas](advanced_features.md) -- comentarios de órdenes, horarios, tasa libre de riesgo, fuente de indicadores

## Estrategia mínima

```csharp
public class MyStrategy : Strategy
{
    private readonly StrategyParam<DataType> _candleType;

    public DataType CandleType
    {
        get => _candleType.Value;
        set => _candleType.Value = value;
    }

    public MyStrategy()
    {
        _candleType = Param(nameof(CandleType), TimeSpan.FromMinutes(5).TimeFrame());
    }

    protected override void OnStarted2(DateTime time)
    {
        base.OnStarted2(time);

        var subscription = SubscribeCandles(CandleType);

        subscription
            .Bind(ProcessCandle)
            .Start();
    }

    private void ProcessCandle(ICandleMessage candle)
    {
        if (!IsFormedAndOnlineAndAllowTrading())
            return;

        // Lógica de trading
    }
}
```

## Ciclo de vida de la estrategia

1. **Creación** -- constructor, declaración de parámetros mediante `Param<T>`.
2. **Configuración** -- establecimiento de `Security`, `Portfolio`, `Connector` y parámetros.
3. **Inicio** -- llamada a `Start()`, transición al estado `ProcessStates.Started`, invocación de `OnStarted2(DateTime)`.
4. **Ejecución** -- procesamiento de datos de mercado, colocación de órdenes.
5. **Parada** -- llamada a `Stop()`, transición de `ProcessStates.Stopping` a `ProcessStates.Stopped`, invocación de `OnStopped()`.
