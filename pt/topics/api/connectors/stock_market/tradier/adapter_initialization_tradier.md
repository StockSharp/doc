# Inicialização do adaptador Tradier

O código abaixo demonstra como inicializar o [TradierMessageAdapter](xref:StockSharp.Tradier.TradierMessageAdapter) e enviá-lo para o [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new TradierMessageAdapter(Connector.TransactionIdGenerator);
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## Conteúdo recomendado

[Janela de definições de ligação](../../../graphical_user_interface/connection_settings_window.md)
