# Inicialização do adaptador Rithmic

O código abaixo demonstra como inicializar o [RithmicMessageAdapter](xref:StockSharp.Rithmic.RithmicMessageAdapter) e passá-lo para o [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new RithmicMessageAdapter(Connector.TransactionIdGenerator)
{
	UserName = "<O seu login>",
	Password = "<A sua palavra-passe>".To<SecureString>(),
	CertFile = "<Caminho para o ficheiro de certificado>",
	Server = RithmicServers.Real,
	//Server = RithmicServers.Test,
	//Server = RithmicServers.Simulator,  
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

Uma forma alternativa e mais conveniente é utilizar o método de extensão `AddAdapter<T>()`:

```cs
Connector Connector = new Connector();
...
Connector.AddAdapter<RithmicMessageAdapter>(a =>
{
	a.UserName = "<O seu login>";
	a.Password = "<A sua palavra-passe>".To<SecureString>();
	a.CertFile = "<Caminho para o ficheiro de certificado>";
	a.Server = RithmicServers.Real;
});
```

## Ver também

[Janela de definições de ligação](../../../graphical_user_interface/connection_settings_window.md)
