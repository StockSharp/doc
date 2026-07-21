# Inicialização do adaptador FalconX

O código abaixo demonstra como inicializar o [FalconXMessageAdapter](xref:StockSharp.FalconX.FalconXMessageAdapter) e passá-lo para o [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new FalconXMessageAdapter(Connector.TransactionIdGenerator)
{
	ApiKey = "<Seu valor>",
	Secret = "<Seu valor>".To<SecureString>(),
	Passphrase = "<Seu valor>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Conteúdo recomendado

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
