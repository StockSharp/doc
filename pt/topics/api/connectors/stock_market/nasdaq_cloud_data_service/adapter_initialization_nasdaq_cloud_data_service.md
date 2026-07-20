# Inicialização do adaptador Nasdaq Cloud Data Service

O código abaixo demonstra como inicializar o [NasdaqCloudDataServiceMessageAdapter](xref:StockSharp.NasdaqCloudDataService.NasdaqCloudDataServiceMessageAdapter) e enviá-lo para o [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new NasdaqCloudDataServiceMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<Seu valor>",
	Password = "<Seu valor>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Ver também

[Janela de definições de ligação](../../../graphical_user_interface/connection_settings_window.md)
