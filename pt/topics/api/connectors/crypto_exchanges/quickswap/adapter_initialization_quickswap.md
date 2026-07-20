# Inicialização do adaptador QuickSwap

O código abaixo demonstra como inicializar o [QuickSwapMessageAdapter](xref:StockSharp.QuickSwap.QuickSwapMessageAdapter) e passá-lo para o [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new QuickSwapMessageAdapter(Connector.TransactionIdGenerator)
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
