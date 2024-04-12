# Logging

[S\#](../../api.md) offers several graphical components for displaying logs: [LogControl](xref:StockSharp.Xaml.LogControl) and [Monitor](xref:StockSharp.Xaml.Monitor). 

When using the logging visual components, you need to use the [GuiLogListener](xref:StockSharp.Xaml.GuiLogListener)logger as a "listener". This logger provides streaming synchronization with the GUI when recording new [LogMessage](xref:StockSharp.Logging.LogMessage) messages.

In order to implement the logging possibility in your own class, you need to implement the [ILogReceiver](xref:StockSharp.Logging.ILogReceiver)interface. An easier way is to inherit from the [BaseLogReceiver](xref:StockSharp.Logging.BaseLogReceiver)class, as shown in the *Samples\\Misc\\SampleLoggingGitHub* example:

```cs
private class TestSource : BaseLogReceiver
{
}
private readonly LogManager _logManager = new LogManager();
private readonly TestSource _testSource = new TestSource();
public MainWindow()
{
	InitializeComponent();
	// immediate flush
	_logManager.FlushInterval = TimeSpan.FromMilliseconds(1);
	// set test log source
	_logManager.Sources.Add(_testSource);
	// set .NET Trace system based source
	_logManager.Sources.Add(new StockSharp.Logging.TraceSource());
	// write logs into MainWindow
	_logManager.Listeners.Add(new GuiLogListener(MonitorW));
	// and file logs.txt
	_logManager.Listeners.Add(new FileLogListener
	{
		FileName = "logs",
	});
}
	  				
```

## Recommended content

[Logging](../logging.md)

[Log panel](logging/log_panel.md)

[Extended log panel](logging/extended_log_panel.md)
