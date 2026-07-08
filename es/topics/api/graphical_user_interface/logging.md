# Registro de logs

[S#](../../api.md) ofrece varios componentes gráficos para mostrar logs: [LogControl](xref:StockSharp.Xaml.LogControl) y [Monitor](xref:StockSharp.Xaml.Monitor). 

Al usar los componentes visuales de logging, debe usar el logger [GuiLogListener](xref:StockSharp.Xaml.GuiLogListener) como "listener". Este logger proporciona sincronización en streaming con la GUI al registrar nuevos mensajes [LogMessage](xref:Ecng.Logging.LogMessage).

Para implementar la posibilidad de logging en su propia clase, debe implementar la interfaz [ILogReceiver](xref:Ecng.Logging.ILogReceiver). Una forma más sencilla es heredar de la clase [BaseLogReceiver](xref:Ecng.Logging.BaseLogReceiver), como se muestra en el ejemplo *Samples\/08\_Misc\/01\_Logging*:

```cs
private class TestSource : BaseLogReceiver
{
}
private readonly LogManager _logManager = new LogManager();
private readonly TestSource _testSource = new TestSource();
public MainWindow()
{
	InitializeComponent();
	// vaciado inmediato
	_logManager.FlushInterval = TimeSpan.FromMilliseconds(1);
	// establecer origen de log de prueba
	_logManager.Sources.Add(_testSource);
	// establecer origen basado en el sistema .NET Trace
	_logManager.Sources.Add(new Ecng.Logging.TraceSource());
	// escribir logs en MainWindow
	_logManager.Listeners.Add(new GuiLogListener(Monitor));
	// y en el archivo logs.txt
	_logManager.Listeners.Add(new FileLogListener
	{
		FileName = "logs",
	});
}
	  				
```

## Contenido recomendado

[Registro de logs](../logging.md)

[Panel de logs](logging/log_panel.md)

[Panel de logs extendido](logging/extended_log_panel.md)

