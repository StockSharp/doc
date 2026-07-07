# Inicialização do adaptador Rithmic

O código abaixo demonstra como inicializar o [RithmicMessageAdapter](xref:StockSharp.Rithmic.RithmicMessageAdapter) e passá-lo para o [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new RithmicMessageAdapter(Connector.TransactionIdGenerator)
{
	UserName = "<Your Login>",
	Password = "<Your Password>".To<SecureString>(),
	CertFile = "<Path to certificate file>",
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
	a.UserName = "<Your Login>";
	a.Password = "<Your Password>".To<SecureString>();
	a.CertFile = "<Path to certificate file>";
	a.Server = RithmicServers.Real;
});
```

## Ver também

[Janela de definições de ligação](../../../graphical_user_interface/connection_settings_window.md)
