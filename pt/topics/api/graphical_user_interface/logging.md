# Logging

[S#](../../api.md) disponibiliza vários componentes gráficos para apresentar logs: [LogControl](xref:StockSharp.Xaml.LogControl) e [Monitor](xref:StockSharp.Xaml.Monitor).

Ao utilizar os componentes visuais de registo, é necessário utilizar o logger [GuiLogListener](xref:StockSharp.Xaml.GuiLogListener) como "listener". Este logger fornece sincronização em fluxo com a GUI ao gravar novas mensagens [LogMessage](xref:Ecng.Logging.LogMessage).

Para implementar a possibilidade de registo na sua própria classe, é necessário implementar a interface [ILogReceiver](xref:Ecng.Logging.ILogReceiver). Uma forma mais simples é herdar da classe [BaseLogReceiver](xref:Ecng.Logging.BaseLogReceiver), como mostrado no exemplo *Samples\/08\_Misc\/01\_Logging*:

```cs
private class TestSource : BaseLogReceiver
{
}
private readonly LogManager _logManager = new LogManager();
private readonly TestSource _testSource = new TestSource();
public MainWindow()
{
	InitializeComponent();
	// flush imediato
	_logManager.FlushInterval = TimeSpan.FromMilliseconds(1);
	// definir fonte de log de teste
	_logManager.Sources.Add(_testSource);
	// definir fonte baseada no sistema .NET Trace
	_logManager.Sources.Add(new Ecng.Logging.TraceSource());
	// gravar logs em MainWindow
	_logManager.Listeners.Add(new GuiLogListener(Monitor));
	// e no arquivo logs.txt
	_logManager.Listeners.Add(new FileLogListener
	{
		FileName = "logs",
	});
}
	  				
```

## Conteúdo recomendado

[Registo](../logging.md)

[Painel de log](logging/log_panel.md)

[Painel de log alargado](logging/extended_log_panel.md)
