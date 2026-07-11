# Panel de logs

[LogControl](xref:StockSharp.Xaml.LogControl) - tabla para mostrar los mensajes de log. Los botones de la barra de herramientas permiten filtrar mensajes con distintos niveles de logging.

## LogControl

![panel de registro GUI](../../../../images/gui_logcontrol.png)

Código de ejemplo

```xaml
<Window x:Class="LoggingControls.MainWindow"
		xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
		xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
		xmlns:sx="clr-namespace:StockSharp.Xaml;assembly=StockSharp.Xaml"
		Title="MainWindow" Height="350" Width="525">
	<Grid>
		<sx:LogControl x:Name="LogControl"/>
	</Grid>
</Window>
	  				
```
```cs
// crear una nueva instancia de LogManager
_logManager = new LogManager();
// agregar .NET tracing como fuente de log.
_logManager.Sources.Add(new Ecng.Logging.TraceSource());
// agregar LogControl como listener de log.
_logManager.Listeners.Add(new GuiLogListener(LogControl));
..........................                  
// enviar mensajes de prueba desde TraceSource:
Trace.TraceInformation("Info  test message");
Trace.TraceWarning("Warning test message");
Trace.TraceError("Mensaje de prueba de error");
					
```

