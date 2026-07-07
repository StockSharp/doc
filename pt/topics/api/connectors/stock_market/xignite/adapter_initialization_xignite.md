# Inicialização do adaptador Xignite

O código abaixo demonstra como inicializar o [XigniteMessageAdapter](xref:StockSharp.Xignite.XigniteMessageAdapter) e enviá-lo para o [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new XigniteMessageAdapter(Connector.TransactionIdGenerator);
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## Conteúdo recomendado

[Janela de definições de ligação](../../../graphical_user_interface/connection_settings_window.md)
