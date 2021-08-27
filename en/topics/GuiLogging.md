# Logging

[S\#](StockSharpAbout.md) offers several graphical components for displaying logs, which are based on [LogControl](xref:StockSharp.Xaml.LogControl). Other components: [LogWindow](xref:StockSharp.Xaml.LogWindow), [Monitor](xref:StockSharp.Xaml.Monitor) and [MonitorWindow](xref:StockSharp.Xaml.MonitorWindow) only complement [LogControl](xref:StockSharp.Xaml.LogControl) functionality. 

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

[Logging](Logging.md)

[Log panel](GUILogControl.md)

[Extended log panel](GUIMonitor.md)
