# Sistema de alertas

## Descripción general

Las estrategias en StockSharp tienen un sistema de alertas integrado que permite enviar notificaciones de distintos tipos: ventanas popup, señales sonoras, entradas de log y mensajes de Telegram. Las alertas son útiles para informar al trader sobre eventos importantes: entradas en posición, rupturas de niveles, errores y otras señales de trading.

Durante las pruebas históricas, las alertas que no sean de tipo `Log` se omiten automáticamente para evitar interferencias durante las pruebas.

## Tipos de alertas

La enumeración `AlertNotifications` define los tipos disponibles:

| Tipo | Descripción |
|------|-------------|
| `Sound` | Señal sonora |
| `Popup` | Ventana popup |
| `Log` | Entrada en archivo de log |
| `Telegram` | Mensaje de Telegram |

## Métodos

### Alert

Método base para enviar una alerta con un tipo, título y mensaje especificados:

```csharp
// Con título y mensaje
Alert(AlertNotifications type, string caption, string message);

// Con título automático (usa el nombre de la estrategia)
Alert(AlertNotifications type, string message);
```

### AlertPopup

Envía una notificación popup. El título es el nombre de la estrategia:

```csharp
AlertPopup(string message);
```

### AlertSound

Envía una notificación sonora:

```csharp
AlertSound(string message);
```

### AlertLog

Envía una notificación al log. Este tipo también funciona durante las pruebas históricas:

```csharp
AlertLog(string message);
```

## Configuración del servicio de alertas

Para que las alertas funcionen, el servicio `IAlertNotificationService` debe estar registrado en el entorno de la estrategia. Esto se hace mediante el método de extensión:

```csharp
strategy.SetAlertService(alertService);
```

El servicio actual se puede obtener mediante:

```csharp
var service = strategy.GetAlertService();
```

En aplicaciones gráficas (Designer, terminal), el servicio normalmente se registra automáticamente.

## Ejemplo de uso

```csharp
public class AlertStrategy : Strategy
{
    private readonly StrategyParam<DataType> _candleType;
    private readonly StrategyParam<decimal> _priceLevel;

    public DataType CandleType
    {
        get => _candleType.Value;
        set => _candleType.Value = value;
    }

    public decimal PriceLevel
    {
        get => _priceLevel.Value;
        set => _priceLevel.Value = value;
    }

    public AlertStrategy()
    {
        _candleType = Param(nameof(CandleType), TimeSpan.FromMinutes(5).TimeFrame());
        _priceLevel = Param(nameof(PriceLevel), 100m);
    }

    protected override void OnStarted2(DateTime time)
    {
        base.OnStarted2(time);

        var subscription = SubscribeCandles(CandleType);

        subscription
            .Bind(ProcessCandle)
            .Start();

        // Alerta sobre el inicio de la estrategia
        AlertLog("Estrategia iniciada, nivel seguido: " + PriceLevel);
    }

    private void ProcessCandle(ICandleMessage candle)
    {
        if (!IsFormedAndOnlineAndAllowTrading())
            return;

        // El precio cruzó el nivel desde abajo hacia arriba
        if (candle.OpenPrice < PriceLevel && candle.ClosePrice >= PriceLevel)
        {
            AlertPopup("El precio cruzó el nivel " + PriceLevel + " hacia arriba!");
            AlertSound("Ruptura de nivel!");
            BuyMarket();
        }

        // El precio cruzó el nivel desde arriba hacia abajo
        if (candle.OpenPrice > PriceLevel && candle.ClosePrice <= PriceLevel)
        {
            Alert(AlertNotifications.Telegram, "Señal de negociación",
                "El precio rompió el nivel " + PriceLevel + " hacia abajo");
            SellMarket();
        }
    }
}
```

En este ejemplo, la estrategia usa distintos tipos de alerta para distintas situaciones: `AlertPopup` y `AlertSound` para captar inmediatamente la atención del trader, y `Alert` con el tipo `Telegram` para notificación remota.
