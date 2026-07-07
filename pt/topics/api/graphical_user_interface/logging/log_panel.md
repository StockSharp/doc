# Painel de log

[LogControl](xref:StockSharp.Xaml.LogControl) - a tabela para apresentar mensagens de log. Os botões da barra de ferramentas permitem filtrar mensagens com diferentes níveis de registo.

## LogControl

![GUI LogControl](../../../../images/gui_logcontrol.png)

Código de exemplo

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
// creating a new instance of LogManager
_logManager = new LogManager();
// adding .NET tracing as a log source.
_logManager.Sources.Add(new Ecng.Logging.TraceSource());
// adding LogControl as a log listener.
_logManager.Listeners.Add(new GuiLogListener(LogControl));
..........................                  
// sending test messages from the TraceSource:
Trace.TraceInformation("Info  test message");
Trace.TraceWarning("Warning test message");
Trace.TraceError("Error test message");
					
```
