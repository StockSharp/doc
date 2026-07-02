# Inicialização do adaptador RSS

O código abaixo demonstra como inicializar o [RssMessageAdapter](xref:StockSharp.Rss.RssMessageAdapter) e enviá-lo ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new RssMessageAdapter(Connector.TransactionIdGenerator)
{
	Address = new Uri("http://energy.rss"),
	CustomDateFormat = "ddd, dd MMM yyyy HH:mm:ss zzzz"
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...

```

## Conteúdo recomendado

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
