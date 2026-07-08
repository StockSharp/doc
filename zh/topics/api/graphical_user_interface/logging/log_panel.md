# 日志面板

[LogControl](xref:StockSharp.Xaml.LogControl) - 用于显示日志消息的表格。工具栏按钮可以让你按不同的日志级别筛选消息。

## 日志控制

![GUI 日志控制](../../../../images/gui_logcontrol.png)

示例代码

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
// 创建 LogManager 的新实例
_logManager = new LogManager();
// 将 .NET tracing 添加为日志源。
_logManager.Sources.Add(new Ecng.Logging.TraceSource());
// 将 LogControl 添加为日志监听器。
_logManager.Listeners.Add(new GuiLogListener(LogControl));
..........................                  
// 从 TraceSource 发送测试消息：
Trace.TraceInformation("Info  test message");
Trace.TraceWarning("Warning test message");
Trace.TraceError("Error test message");
					
```
