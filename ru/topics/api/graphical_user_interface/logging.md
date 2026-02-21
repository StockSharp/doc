# Логирование

[S\#](../../api.md) предлагает несколько графических компонент для отображения логов: [LogControl](xref:StockSharp.Xaml.LogControl) и [Monitor](xref:StockSharp.Xaml.Monitor).

При использовании визуальных компонентов логирования в качестве "слушателя" необходимо использовать логгер [GuiLogListener](xref:StockSharp.Xaml.GuiLogListener). Этот логгер обеспечивает потоковую синхронизацию с GUI при записи новых сообщений [LogMessage](xref:Ecng.Logging.LogMessage).

Чтобы в собственном классе реализовать возможность логирования необходимо реализовать интерфейс [ILogReceiver](xref:Ecng.Logging.ILogReceiver). Более простой способ это унаследоваться от класса [BaseLogReceiver](xref:Ecng.Logging.BaseLogReceiver), как это сделано в примере *Samples/08_Misc/01_Logging*:

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
	_logManager.Sources.Add(new Ecng.Logging.TraceSource());
	// write logs into MainWindow
	_logManager.Listeners.Add(new GuiLogListener(Monitor));
	// and file logs.txt
	_logManager.Listeners.Add(new FileLogListener
	{
		FileName = "logs",
	});
}
```

## См. также

[Логирование](../logging.md)

[Панель логов](logging/log_panel.md)

[Расширенная панель логов](logging/extended_log_panel.md)
