# Inicialização do adaptador Polymarket

O código abaixo demonstra como inicializar o [PolymarketMessageAdapter](xref:StockSharp.Polymarket.PolymarketMessageAdapter) e passá-lo para o [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new PolymarketMessageAdapter(Connector.TransactionIdGenerator)
{
	ApiKey = "<Seu valor>",
	ApiSecret = "<Seu valor>".To<SecureString>(),
	Passphrase = "<Seu valor>".To<SecureString>(),
	SignerAddress = "<Seu valor>",
	FunderAddress = "<Seu valor>",
	PrivateKey = "<Seu valor>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Conteúdo recomendado

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
