# Registro

[S#](../../api.md) ofrece varios componentes gráficos para mostrar registros: [LogControl](xref:StockSharp.Xaml.LogControl) y [Monitor](xref:StockSharp.Xaml.Monitor).

Al usar los componentes visuales de registro, debe usar el registrador [GuiLogListener](xref:StockSharp.Xaml.GuiLogListener) como receptor. Este registrador proporciona sincronización en flujo con la GUI al registrar nuevos mensajes [LogMessage](xref:Ecng.Logging.LogMessage).

Para implementar la posibilidad de registro en su propia clase, debe implementar la interfaz [ILogReceiver](xref:Ecng.Logging.ILogReceiver). Una forma más sencilla es heredar de la clase [BaseLogReceiver](xref:Ecng.Logging.BaseLogReceiver), como se muestra en el ejemplo *Samples\/08\_Misc\/01\_Logging*:

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
	// escribir registros en MainWindow
	_logManager.Listeners.Add(new GuiLogListener(Monitor));
	// y en el archivo logs.txt
	_logManager.Listeners.Add(new FileLogListener
	{
		FileName = "logs",
	});
}
	  				
```

## Contenido recomendado

[Registro](../logging.md)

[Panel de registros](logging/log_panel.md)

[Panel de registros extendido](logging/extended_log_panel.md)
