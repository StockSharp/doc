# Inicialização do adaptador PolygonIO

O código abaixo demonstra como inicializar o [PolygonIOMessageAdapter](xref:StockSharp.PolygonIO.PolygonIOMessageAdapter) e passá-lo para o [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new PolygonIOMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<Your Token>".To<SecureString>(),
	ConnectionType = PolygonIOConnectionTypes.History, // connection for REST data sources
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
			
```

## Conteúdo recomendado

[Janela de definições de ligação](../../../graphical_user_interface/connection_settings_window.md)
