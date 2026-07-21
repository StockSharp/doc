# Inicialização do adaptador dYdX Chain

O código abaixo demonstra como inicializar o [DydxChainMessageAdapter](xref:StockSharp.DydxChain.DydxChainMessageAdapter) e passá-lo para o [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new DydxChainMessageAdapter(Connector.TransactionIdGenerator)
{
	WalletAddress = "<Seu valor>",
	PrivateKey = "<Seu valor>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Conteúdo recomendado

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
