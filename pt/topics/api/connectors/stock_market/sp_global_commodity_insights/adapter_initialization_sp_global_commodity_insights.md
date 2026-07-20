# Inicialização do adaptador S&P Global Commodity Insights

O código abaixo demonstra como inicializar o [SpGlobalMessageAdapter](xref:StockSharp.SpGlobal.SpGlobalMessageAdapter) e enviá-lo para o [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new SpGlobalMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<Seu valor>",
	Password = "<Seu valor>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Ver também

[Janela de definições de ligação](../../../graphical_user_interface/connection_settings_window.md)
