# Protokollierung

[S#](../../api.md) bietet mehrere grafische Komponenten zur Anzeige von Logs: [LogControl](xref:StockSharp.Xaml.LogControl) und [Monitor](xref:StockSharp.Xaml.Monitor).

Bei der Verwendung visueller Logging-Komponenten müssen Sie den Logger [GuiLogListener](xref:StockSharp.Xaml.GuiLogListener) als "Listener" verwenden. Dieser Logger stellt beim Aufzeichnen neuer [LogMessage](xref:Ecng.Logging.LogMessage)-Meldungen eine Streaming-Synchronisierung mit der GUI bereit.

Um Logging in Ihrer eigenen Klasse zu implementieren, müssen Sie die Schnittstelle [ILogReceiver](xref:Ecng.Logging.ILogReceiver) implementieren. Einfacher ist es, von der Klasse [BaseLogReceiver](xref:Ecng.Logging.BaseLogReceiver) zu erben, wie im Beispiel *Samples/08_Misc/01_Logging* gezeigt:

```cs
private class TestSource : BaseLogReceiver
{
}
private readonly LogManager _logManager = new LogManager();
private readonly TestSource _testSource = new TestSource();
public MainWindow()
{
	InitializeComponent();
	// Sofortiges Flushen
	_logManager.FlushInterval = TimeSpan.FromMilliseconds(1);
	// Test-Logquelle setzen
	_logManager.Sources.Add(_testSource);
	// .NET-Trace-System als Quelle setzen
	_logManager.Sources.Add(new Ecng.Logging.TraceSource());
	// Logs in MainWindow schreiben
	_logManager.Listeners.Add(new GuiLogListener(Monitor));
	// und in die Datei logs.txt
	_logManager.Listeners.Add(new FileLogListener
	{
		FileName = "logs",
	});
}

```

## Empfohlene Inhalte

[Protokollierung](../logging.md)

[Log-Panel](logging/log_panel.md)

[Erweitertes Log-Panel](logging/extended_log_panel.md)
