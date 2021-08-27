# Логирование

[S\#](StockSharpAbout.md) предлагает несколько графических компонент для отображения логов, в основе которых лежит [LogControl](xref:StockSharp.Xaml.LogControl). Другие компоненты: [LogWindow](xref:StockSharp.Xaml.LogWindow), [Monitor](xref:StockSharp.Xaml.Monitor) и [MonitorWindow](xref:StockSharp.Xaml.MonitorWindow) только дополняют функциональность [LogControl](xref:StockSharp.Xaml.LogControl). 

При использовании визуальных компонентов логирования в качестве "слушателя" необходимо использовать логгер [GuiLogListener](xref:StockSharp.Xaml.GuiLogListener). Этот логгер обеспечивает потоковую синхронизацию с GUI при записи новых сообщений [LogMessage](xref:StockSharp.Logging.LogMessage).

Чтобы в собственном классе реализовать возможность логирования необходимо реализовать интерфейс [ILogReceiver](xref:StockSharp.Logging.ILogReceiver). Более простой способ это унаследоваться от класса [BaseLogReceiver](xref:StockSharp.Logging.BaseLogReceiver), как это сделать продемонстрировано в примере *Samples\\Misc\\SampleLoggingGitHub*:

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
