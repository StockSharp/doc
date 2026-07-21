# Inicialização do adaptador Orderly Network

O código abaixo demonstra como inicializar o [OrderlyNetworkMessageAdapter](xref:StockSharp.OrderlyNetwork.OrderlyNetworkMessageAdapter) e passá-lo para o [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new OrderlyNetworkMessageAdapter(Connector.TransactionIdGenerator)
{
	AccountId = "<Seu valor>",
	Secret = "<Seu valor>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Conteúdo recomendado

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
