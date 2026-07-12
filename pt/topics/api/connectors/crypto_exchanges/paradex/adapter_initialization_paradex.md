# Inicialização do adaptador Paradex

O código abaixo demonstra como inicializar o [ParadexMessageAdapter](xref:StockSharp.Paradex.ParadexMessageAdapter) e passá-lo para o [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new ParadexMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<A sua chave de API>".To<SecureString>(),
	Secret = "<O seu segredo de API>".To<SecureString>(),
	StarknetAccount = "<A sua conta Starknet>",
	StarknetPrivateKey = "<A sua chave privada Starknet>".To<SecureString>(),
	Section = ParadexSections.Derivatives,
	AuthPath = "/v1/auth",
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Conteúdo recomendado

[Janela de definições de ligação](../../../graphical_user_interface/connection_settings_window.md)

