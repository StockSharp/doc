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
	// немедленный сброс буфера
	_logManager.FlushInterval = TimeSpan.FromMilliseconds(1);
	// установить тестовый источник логов
	_logManager.Sources.Add(_testSource);
	// установить источник на основе системы .NET Trace
	_logManager.Sources.Add(new Ecng.Logging.TraceSource());
	// записывать логи в MainWindow
	_logManager.Listeners.Add(new GuiLogListener(Monitor));
	// и в файл logs.txt
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
