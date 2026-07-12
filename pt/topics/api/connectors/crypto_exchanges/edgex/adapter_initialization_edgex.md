# Inicialização do adaptador edgeX

O código abaixo demonstra como inicializar o [EdgeXMessageAdapter](xref:StockSharp.EdgeX.EdgeXMessageAdapter) e passá-lo para o [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new EdgeXMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<A sua chave de API>".To<SecureString>(),
	Secret = "<O seu segredo de API>".To<SecureString>(),
	ClearingAccount = "<A sua conta de compensação>",
	Passphrase = "<A sua frase-passe>".To<SecureString>(),
	Section = EdgeXSections.Derivatives,
	EnableSpotSection = false,
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Conteúdo recomendado

[Janela de definições de ligação](../../../graphical_user_interface/connection_settings_window.md)
