# Logging en estrategias

En StockSharp, la clase [Strategy](xref:StockSharp.Algo.Strategies.Strategy) hereda de [BaseLogReceiver](xref:Ecng.Logging.BaseLogReceiver), lo que permite usar herramientas integradas para registrar todas las acciones y eventos que ocurren durante el funcionamiento de una estrategia de trading.

## Niveles de logging

StockSharp admite los siguientes niveles de logging (enumerados en orden de importancia creciente):

1. Verbose - el nivel de logging más detallado para trazas
2. Debug - mensajes para depuración
3. Info - mensajes informativos habituales
4. Warning - advertencias sobre posibles problemas
5. Error - mensajes de error

## Métodos de logging en Strategy

La estrategia proporciona los siguientes métodos para escribir mensajes en el log:

### LogVerbose

El método [LogVerbose](xref:Ecng.Logging.BaseLogReceiver.LogVerbose(System.String,System.Object[])) está diseñado para registrar mensajes detallados de traza:

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);
	
	LogVerbose("Strategy started with parameters: Long SMA={0}, Short SMA={1}", LongSmaLength, ShortSmaLength);
	
	// ...
}
```

### LogDebug

El método [LogDebug](xref:Ecng.Logging.BaseLogReceiver.LogDebug(System.String,System.Object[])) se usa para mensajes de depuración:

```cs
private void ProcessCandle(ICandleMessage candle)
{
	LogDebug("Processing candle: {0}, Open={1}, Close={2}, High={3}, Low={4}, Volume={5}", 
		candle.OpenTime, candle.OpenPrice, candle.ClosePrice, candle.HighPrice, candle.LowPrice, candle.TotalVolume);
	
	// ...
}
```

### LogInfo

El método [LogInfo](xref:Ecng.Logging.BaseLogReceiver.LogInfo(System.String,System.Object[])) se usa para mensajes informativos habituales:

```cs
private void CalculateSignal(decimal shortSma, decimal longSma)
{
	bool isShortGreaterThanLong = shortSma > longSma;
	
	LogInfo("Signal: {0}, Short SMA={1}, Long SMA={2}", 
		isShortGreaterThanLong ? "Buy" : "Sell", shortSma, longSma);
	
	// ...
}
```

### LogWarning

El método [LogWarning](xref:Ecng.Logging.BaseLogReceiver.LogWarning(System.String,System.Object[])) se usa para registrar advertencias:

```cs
public void RegisterOrder(Order order)
{
	if (order.Volume <= 0)
	{
		LogWarning("Intento de registrar una orden con volumen no válido: {0}", order.Volume);
		return;
	}
	
	// ...
}
```

### LogError

El método [LogError](xref:Ecng.Logging.BaseLogReceiver.LogError(System.String,System.Object[])) se usa para registrar mensajes de error:

```cs
try
{
	// Algunas acciones
}
catch (Exception ex)
{
	LogError("Error al realizar la operación: {0}", ex.Message);
	Stop();
}
```

También existe una sobrecarga [LogError](xref:Ecng.Logging.BaseLogReceiver.LogError(System.Exception)) que acepta directamente una excepción:

```cs
try
{
	// Algunas acciones
}
catch (Exception ex)
{
	LogError(ex);
	Stop();
}
```

## Configuración del nivel de logging

La clase [Strategy](xref:StockSharp.Algo.Strategies.Strategy) contiene una propiedad [LogLevel](xref:Ecng.Logging.ILogSource.LogLevel) que determina qué mensajes se escribirán en el log:

```cs
// Establecer el nivel de logging para la estrategia
strategy.LogLevel = LogLevels.Info;
```

Con el nivel de logging seleccionado, solo se registrarán los mensajes de ese nivel y de niveles superiores. Por ejemplo, si se establece `LogLevels.Info`, los mensajes Verbose y Debug se ignorarán.

## Parámetro LogLevel

Para configurar cómodamente el nivel de logging en el constructor de la estrategia, puede agregar un parámetro:

```cs
public class SmaStrategy : Strategy
{
	private readonly StrategyParam<LogLevels> _logLevel;
	
	public SmaStrategy()
	{
		_logLevel = Param(nameof(LogLevel), LogLevels.Info)
					.SetDisplay("Nivel de registro", "Nivel de detalle de los mensajes de registro", "Configuración de registro");
	}
	
	public override LogLevels LogLevel
	{
		get => _logLevel.Value;
		set => _logLevel.Value = value;
	}
	
	// ...
}
```

## Ejemplos de uso en una estrategia real

### Logging del inicio y parada de la estrategia

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);
	
	LogInfo("Strategy {0} started at {1}. Instrument: {2}, Portfolio: {3}", 
		Name, time, Security?.Code, Portfolio?.Name);
	
	// ...
}

protected override void OnStopped()
{
	LogInfo("Strategy {0} stopped. Position: {1}, P&L: {2}", 
		Name, Position, PnL);
	
	base.OnStopped();
}
```

### Logging de operaciones

```cs
protected override void OnNewMyTrade(MyTrade trade)
{
	LogInfo("{0} {1} {2} at price {3}. Volume: {4}",
		trade.Order.Direction == Sides.Buy ? "Bought" : "Sold",
		trade.Order.Security.Code,
		trade.Order.Type,
		trade.Trade.Price,
		trade.Trade.Volume);

	base.OnNewMyTrade(trade);
}
```

### Logging de errores de registro de órdenes

```cs
protected override void OnOrderRegisterFailed(OrderFail fail, bool calcRisk)
{
	LogError("Order registration error {0}: {1}", 
		fail.Order.TransactionId, fail.Error.Message);
	
	base.OnOrderRegisterFailed(fail, calcRisk);
}
```

## Conexión de listeners de log

Para recibir mensajes de una estrategia, conecte listeners mediante [LogManager](xref:Ecng.Logging.LogManager):

```cs
var logManager = new LogManager();

// Escribir en archivo
var fileListener = new FileLogListener("{0}_{1:00}_{2:00}.txt".Put(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day));
logManager.Listeners.Add(fileListener);

// Enviar email
var emailListener = new EmailLogListener("from@stocksharp.com", "to@stocksharp.com");
emailListener.Filters.Add(msg => msg.Level == LogLevels.Error);
logManager.Listeners.Add(emailListener);

// Agregar la estrategia como fuente de log
logManager.Sources.Add(strategy);
```

## Visualización de logs

Los mensajes escritos en el log de la estrategia se pueden ver:

1. En el programa [Designer](../../designer.md), en el panel "Logs"
2. En archivos de log, si [FileLogListener](xref:Ecng.Logging.FileLogListener) está configurado
3. En la interfaz de usuario mediante [LogControl](xref:StockSharp.Xaml.LogControl), si se usa [GuiLogListener](xref:StockSharp.Xaml.GuiLogListener)

## Ver también

[Registro de logs](../logging.md)
[Componente LogControl](../graphical_user_interface/logging/log_panel.md)
