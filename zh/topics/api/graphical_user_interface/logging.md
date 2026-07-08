# 记录

[S#](../../api.md) 提供了几个用于显示日志的图形组件：[LogControl](xref:StockSharp.Xaml.LogControl) 和 [Monitor](xref:StockSharp.Xaml.Monitor)。

在使用日志可视化组件时，您需要使用 [GuiLogListener](xref:StockSharp.Xaml.GuiLogListener) 日志器作为“监听器”。当记录新的 [LogMessage](xref:Ecng.Logging.LogMessage) 消息时，该日志器会与 GUI 提供流式同步。

为了在您自己的类中实现日志功能，您需要实现 [ILogReceiver](xref:Ecng.Logging.ILogReceiver) 接口。更简单的方法是继承 [BaseLogReceiver](xref:Ecng.Logging.BaseLogReceiver) 类，如 *Samples/08_Misc/01_Logging* 示例所示：

```cs
private class TestSource : BaseLogReceiver
{
}
private readonly LogManager _logManager = new LogManager();
private readonly TestSource _testSource = new TestSource();
public MainWindow()
{
	InitializeComponent();
	// 立即刷新
	_logManager.FlushInterval = TimeSpan.FromMilliseconds(1);
	// 设置测试日志源
	_logManager.Sources.Add(_testSource);
	// 设置基于 .NET Trace 系统的源
	_logManager.Sources.Add(new Ecng.Logging.TraceSource());
	// 将日志写入 MainWindow
	_logManager.Listeners.Add(new GuiLogListener(Monitor));
	// 以及文件 logs.txt
	_logManager.Listeners.Add(new FileLogListener
	{
		FileName = "logs",
	});
}
	  				
```

## 推荐内容

[日志记录](../logging.md)

[日志面板](logging/log_panel.md)

[扩展日志面板](logging/extended_log_panel.md)
