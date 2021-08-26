# Логирование

[S\#](StockSharpAbout.md) предлагает несколько графических компонент для отображения логов, в основе которых лежит [LogControl](../api/StockSharp.Xaml.LogControl.html). Другие компоненты: [LogWindow](../api/StockSharp.Xaml.LogWindow.html), [Monitor](../api/StockSharp.Xaml.Monitor.html) и [MonitorWindow](../api/StockSharp.Xaml.MonitorWindow.html) только дополняют функциональность [LogControl](../api/StockSharp.Xaml.LogControl.html). 

При использовании визуальных компонентов логирования в качестве "слушателя" необходимо использовать логгер [GuiLogListener](../api/StockSharp.Xaml.GuiLogListener.html). Этот логгер обеспечивает потоковую синхронизацию с GUI при записи новых сообщений [LogMessage](../api/StockSharp.Logging.LogMessage.html).

Чтобы в собственном классе реализовать возможность логирования необходимо реализовать интерфейс [ILogReceiver](../api/StockSharp.Logging.ILogReceiver.html). Более простой способ это унаследоваться от класса [BaseLogReceiver](../api/StockSharp.Logging.BaseLogReceiver.html), как это сделать продемонстрировано в примере *Samples\\Misc\\SampleLoggingGitHub*:

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

## См. также

[Логирование](Logging.md)

[Панель логов](GUILogControl.md)

[Расширенная панель логов](GUIMonitor.md)
