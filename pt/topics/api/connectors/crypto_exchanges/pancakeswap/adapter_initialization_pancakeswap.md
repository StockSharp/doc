# Inicialização do adaptador PancakeSwap

O código abaixo demonstra como inicializar o [PancakeSwapMessageAdapter](xref:StockSharp.PancakeSwap.PancakeSwapMessageAdapter) e passá-lo para o [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new PancakeSwapMessageAdapter(Connector.TransactionIdGenerator)
{
	GraphApiKey = "<Seu valor>".To<SecureString>(),
	WalletAddress = "<Seu valor>",
	PrivateKey = "<Seu valor>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Conteúdo recomendado

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
