# Inicialização do adaptador J.P. Morgan DataQuery

O código abaixo demonstra como inicializar o [JpmDataQueryMessageAdapter](xref:StockSharp.J.P. Morgan DataQuery.JpmDataQueryMessageAdapter) e enviá-lo para o [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new JpmDataQueryMessageAdapter(Connector.TransactionIdGenerator)
{
	ClientId = "<Seu valor>",
	ClientSecret = "<Seu valor>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Ver também

[Janela de definições de ligação](../../../graphical_user_interface/connection_settings_window.md)
