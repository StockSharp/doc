# Inicialização do adaptador Dow Jones

O código abaixo demonstra como inicializar o [DowJonesMessageAdapter](xref:StockSharp.DowJones.DowJonesMessageAdapter) e enviá-lo para o [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new DowJonesMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<Seu valor>".To<SecureString>(),
	ClientId = "<Seu valor>",
	Login = "<Seu valor>",
	Password = "<Seu valor>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Ver também

[Janela de definições de ligação](../../../graphical_user_interface/connection_settings_window.md)
