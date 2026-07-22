# Inicialização do adaptador Zero Hash

O código abaixo demonstra como inicializar o [ZeroHashMessageAdapter](xref:StockSharp.ZeroHash.ZeroHashMessageAdapter) e passá-lo para o [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new ZeroHashMessageAdapter(Connector.TransactionIdGenerator)
{
	ApiKey = "<Seu valor>",
	Secret = "<Seu valor>".To<SecureString>(),
	Passphrase = "<Seu valor>".To<SecureString>(),
	Account = "<Seu valor>",
	User = "<Seu valor>",
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Conteúdo recomendado

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
