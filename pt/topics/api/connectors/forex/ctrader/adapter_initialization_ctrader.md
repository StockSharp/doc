# Inicialização do adaptador cTrader

O código abaixo demonstra como inicializar o [cTraderMessageAdapter](xref:StockSharp.cTrader.cTraderMessageAdapter) e passá-lo para o [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new cTraderMessageAdapter(Connector.TransactionIdGenerator)
{
	IsDemo = true, // Demo
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
							
```

## Conteúdo recomendado

[Janela de definições de ligação](../../../graphical_user_interface/connection_settings_window.md)
