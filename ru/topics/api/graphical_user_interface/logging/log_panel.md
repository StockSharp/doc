# Панель логов

[LogControl](xref:StockSharp.Xaml.LogControl) \- таблица для отображения сообщений логов. Кнопки панели инструментов позволяют фильтровать сообщения с разным уровнем логирования.

## LogControl

![GUI LogControl](../../../../images/gui_logcontrol.png)

Пример кода

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
// Создаем LogManager
LogManager _logManager = new LogManager();
// Добавляем источник логов. Используем систему трассировки .NET.
_logManager.Sources.Add(new Ecng.Logging.TraceSource());
// Добавляем "слушателя" логов - GuiLogListener, в конструктор которого передаем ссылку
// на графический элемент.
_logManager.Listeners.Add(new GuiLogListener(LogControl));
..........................                  
// При использовании в качестве источника TraceSource, отладочные сообщения можно добавлять следующим образом:
Trace.TraceInformation("Info  test message");
Trace.TraceWarning("Warning test message");
Trace.TraceError("Error test message");
					
```
