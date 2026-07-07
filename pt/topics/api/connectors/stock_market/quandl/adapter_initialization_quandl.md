# Inicialização do adaptador Quandl

O código abaixo demonstra como inicializar o [QuandlMessageAdapter](xref:StockSharp.Quandl.QuandlMessageAdapter) e passá-lo para o [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new QuandlMessageAdapter(Connector.TransactionIdGenerator);
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## Conteúdo recomendado

[Janela de definições de ligação](../../../graphical_user_interface/connection_settings_window.md)
