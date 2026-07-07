# Inicialização do adaptador Yahoo

O código abaixo demonstra como inicializar o [YahooMessageAdapter](xref:StockSharp.Yahoo.YahooMessageAdapter) e enviá-lo para o [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new YahooMessageAdapter(Connector.TransactionIdGenerator);
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## Conteúdo recomendado

[Janela de definições de ligação](../../../graphical_user_interface/connection_settings_window.md)
