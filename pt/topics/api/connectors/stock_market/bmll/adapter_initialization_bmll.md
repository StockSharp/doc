# Inicialização do adaptador BMLL

O código abaixo demonstra como inicializar o [BmllMessageAdapter](xref:StockSharp.Bmll.BmllMessageAdapter) e enviá-lo para o [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new BmllMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<Seu valor>",
	Password = "<Seu valor>".To<SecureString>(),
	Token = "<Seu valor>".To<SecureString>(),
	ApiKey = "<Seu valor>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Ver também

[Janela de definições de ligação](../../../graphical_user_interface/connection_settings_window.md)
