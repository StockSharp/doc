# Inicialização do adaptador Copper

O código abaixo demonstra como inicializar o [CopperMessageAdapter](xref:StockSharp.Copper.CopperMessageAdapter) e passá-lo para o [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new CopperMessageAdapter(Connector.TransactionIdGenerator)
{
	ApiKey = "<Seu valor>",
	ApiSecret = "<Seu valor>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Conteúdo recomendado

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
