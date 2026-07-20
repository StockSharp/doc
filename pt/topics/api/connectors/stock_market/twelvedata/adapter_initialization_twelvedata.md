# Inicialização do adaptador Twelve Data

O código abaixo demonstra como inicializar o [TwelveDataMessageAdapter](xref:StockSharp.TwelveData.TwelveDataMessageAdapter) e enviá-lo para o [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new TwelveDataMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<Seu valor>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Ver também

[Janela de definições de ligação](../../../graphical_user_interface/connection_settings_window.md)
