# Extended log panel

[Monitor](xref:StockSharp.Xaml.Monitor) \- is the visual element where [LogControl](log_panel.md) is used in conjunction with the **TreeView** hierarchical tree, in which log sources are shown. Initially, the component was designed to monitor the trading strategies. Therefore, by default, the "tree" includes the **Strategy** node. At the same time other sources can be used with this component.

![GUI Monitor](../../../../images/gui_monitor.png)

Sample code

```xaml
<Window x:Class="LoggingControls.MainWindow"
		xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
		xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
		xmlns:sx="clr-namespace:StockSharp.Xaml;assembly=StockSharp.Xaml"
		Title="MainWindow" Height="350" Width="525">
	<Grid>
		<sx:Monitor x:Name="Monitor" />
	</Grid>
</Window>
				
```
```cs
// creating a new instance of LogManager
_logManager = new LogManager();
// adding .NET tracing as a log source.
_logManager.Sources.Add(new Ecng.Logging.TraceSource());
// adding Monitor as a log listener.
_logManager.Listeners.Add(new GuiLogListener(Monitor));
					
```
