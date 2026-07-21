# Inicialização do adaptador Aevo

O código abaixo demonstra como inicializar o [AevoMessageAdapter](xref:StockSharp.Aevo.AevoMessageAdapter) e passá-lo para o [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new AevoMessageAdapter(Connector.TransactionIdGenerator)
{
	ApiKey = "<Seu valor>",
	ApiSecret = "<Seu valor>".To<SecureString>(),
	WalletAddress = "<Seu valor>",
	SigningKey = "<Seu valor>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Conteúdo recomendado

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
