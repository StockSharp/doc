# Inicialização do adaptador Fireblocks

O código abaixo demonstra como inicializar o [FireblocksMessageAdapter](xref:StockSharp.Fireblocks.FireblocksMessageAdapter) e passá-lo para o [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new FireblocksMessageAdapter(Connector.TransactionIdGenerator)
{
	ApiKey = "<Seu valor>",
	PrivateKey = "<Seu valor>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Conteúdo recomendado

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
