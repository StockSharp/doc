# Sistema de temporizadores

## Descripción general

Las estrategias en StockSharp admiten un sistema de temporizadores integrado que permite ejecutar acciones en intervalos de tiempo especificados. Los temporizadores se basan en el mecanismo `WhenIntervalElapsed` del conector y funcionan correctamente tanto en trading real como durante las pruebas históricas (usando el tiempo virtual del emulador).

Los temporizadores son cómodos para:

- Comprobaciones periódicas de condiciones de mercado
- Rebalanceo de cartera a intervalos fijos
- Cierre de posiciones por tiempo
- Supervisión del estado de la estrategia

## Interfaz ITimerHandler

El temporizador se gestiona mediante la interfaz `ITimerHandler`, que proporciona los siguientes miembros:

| Miembro | Descripción |
|--------|-------------|
| `Start()` | Inicia el temporizador. Devuelve `ITimerHandler` para encadenar métodos |
| `Stop()` | Detiene el temporizador. Devuelve `ITimerHandler` para encadenar métodos |
| `Interval` | Intervalo de activación del temporizador (lectura y escritura) |
| `IsStarted` | Indica si el temporizador está en ejecución |
| `Dispose()` | Libera recursos y detiene el temporizador |

## Métodos de creación de temporizadores

### CreateTimer

Crea un temporizador pero no lo inicia. El inicio se realiza mediante una llamada separada a `Start()`:

```csharp
ITimerHandler CreateTimer(TimeSpan interval, Action callback);
```

### StartTimer

Crea e inicia inmediatamente un temporizador. Equivale a `CreateTimer(...).Start()`:

```csharp
ITimerHandler StartTimer(TimeSpan interval, Action callback);
```

El intervalo debe ser un valor positivo (`> TimeSpan.Zero`); de lo contrario, se lanzará una excepción.

## Gestión de temporizadores

Un temporizador se puede detener y reiniciar:

```csharp
var timer = CreateTimer(TimeSpan.FromMinutes(1), MyCallback);

// Iniciar
timer.Start();

// Detener
timer.Stop();

// Cambiar intervalo
timer.Interval = TimeSpan.FromMinutes(5);

// Iniciar de nuevo
timer.Start();

// Liberar recursos
timer.Dispose();
```

## Ejemplo de uso

```csharp
public class TimerStrategy : Strategy
{
    private ITimerHandler _checkTimer;
    private ITimerHandler _closeTimer;

    private readonly StrategyParam<TimeSpan> _checkInterval;
    private readonly StrategyParam<TimeSpan> _maxHoldTime;

    public TimeSpan CheckInterval
    {
        get => _checkInterval.Value;
        set => _checkInterval.Value = value;
    }

    public TimeSpan MaxHoldTime
    {
        get => _maxHoldTime.Value;
        set => _maxHoldTime.Value = value;
    }

    public TimerStrategy()
    {
        _checkInterval = Param(nameof(CheckInterval), TimeSpan.FromMinutes(1));
        _maxHoldTime = Param(nameof(MaxHoldTime), TimeSpan.FromHours(4));
    }

    protected override void OnStarted2(DateTime time)
    {
        base.OnStarted2(time);

        // Temporizador para comprobaciones periódicas de condiciones de mercado
        _checkTimer = StartTimer(CheckInterval, OnCheckTimer);

        // Temporizador para cierre forzado de posición (creado pero no iniciado)
        _closeTimer = CreateTimer(MaxHoldTime, OnCloseTimer);

        var subscription = SubscribeCandles(TimeSpan.FromMinutes(5));

        subscription
            .Bind(ProcessCandle)
            .Start();
    }

    private void OnCheckTimer()
    {
        if (!IsFormedAndOnlineAndAllowTrading())
            return;

        this.AddInfoLog("Comprobando condiciones de mercado en {0}", CurrentTime);

        // Comprobación periódica del estado de la posición
        if (Position != 0)
        {
            this.AddInfoLog("Posición actual: {0}", Position);
        }
    }

    private void OnCloseTimer()
    {
        if (Position != 0)
        {
            this.AddInfoLog("Tiempo de mantenimiento de posición expirado, cerrando");
            ClosePosition();

            // Detener el temporizador de cierre después de que se active
            _closeTimer.Stop();
        }
    }

    private void ProcessCandle(ICandleMessage candle)
    {
        if (!IsFormedAndOnlineAndAllowTrading())
            return;

        if (candle.OpenPrice < candle.ClosePrice && Position == 0)
        {
            BuyMarket();

            // Iniciar el temporizador de cierre forzado
            _closeTimer.Start();
        }
    }
}
```

En este ejemplo se usan dos temporizadores: uno para comprobar periódicamente el estado (`StartTimer` -- creado e iniciado inmediatamente), y otro para limitar el tiempo de mantenimiento de la posición (`CreateTimer` -- creado pero iniciado solo al entrar en una posición).
