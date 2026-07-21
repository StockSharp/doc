# Inicialização do adaptador Synthetix

O código abaixo demonstra como inicializar o [SynthetixMessageAdapter](xref:StockSharp.Synthetix.SynthetixMessageAdapter) e passá-lo para o [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new SynthetixMessageAdapter(Connector.TransactionIdGenerator)
{
	SubAccountId = "<Seu valor>",
	PrivateKey = "<Seu valor>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Conteúdo recomendado

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
