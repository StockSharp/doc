# Inicialização do adaptador IQFeed

O código abaixo demonstra como inicializar o [IQFeedMessageAdapter](xref:StockSharp.IQFeed.IQFeedMessageAdapter) e enviá-lo para o [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new IQFeedMessageAdapter(Connector.TransactionIdGenerator)
{
	Level1Address = "127.0.0.1:5009".To<EndPoint>(),
	Level2Address = "127.0.0.1:9200".To<EndPoint>(),
	LookupAddress = "127.0.0.1:9100".To<EndPoint>(),
	AdminAddress =  "127.0.0.1:9200".To<EndPoint>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## Conteúdo recomendado

[Janela de definições de ligação](../../../graphical_user_interface/connection_settings_window.md)
