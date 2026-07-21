# Inicialização do adaptador Gains Network

O código abaixo demonstra como inicializar o [GainsNetworkMessageAdapter](xref:StockSharp.GainsNetwork.GainsNetworkMessageAdapter) e passá-lo para o [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new GainsNetworkMessageAdapter(Connector.TransactionIdGenerator)
{
	WalletAddress = "<Seu valor>",
	PrivateKey = "<Seu valor>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Conteúdo recomendado

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
