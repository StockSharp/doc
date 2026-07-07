# Inicialização do adaptador Hyperliquid

O código abaixo demonstra como inicializar o [HyperliquidMessageAdapter](xref:StockSharp.Hyperliquid.HyperliquidMessageAdapter) e passá-lo para o [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new HyperliquidMessageAdapter(Connector.TransactionIdGenerator)
{
	WalletAddress = "<Your Wallet Address>",
	PrivateKey = "<Your Private Key>".To<SecureString>(),
	Section = HyperliquidSections.Derivatives,
	IsTestnet = false,
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Conteúdo recomendado

[Janela de definições de ligação](../../../graphical_user_interface/connection_settings_window.md)

