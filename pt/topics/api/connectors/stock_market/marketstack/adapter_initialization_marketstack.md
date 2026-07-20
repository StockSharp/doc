# Inicialização do adaptador Marketstack

O código abaixo demonstra como inicializar o [MarketstackMessageAdapter](xref:StockSharp.Marketstack.MarketstackMessageAdapter) e enviá-lo para o [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new MarketstackMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<Seu valor>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Ver também

[Janela de definições de ligação](../../../graphical_user_interface/connection_settings_window.md)
