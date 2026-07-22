# Inicialização do adaptador Anchorage

O código abaixo demonstra como inicializar o [AnchorageMessageAdapter](xref:StockSharp.Anchorage.AnchorageMessageAdapter) e passá-lo para o [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new AnchorageMessageAdapter(Connector.TransactionIdGenerator)
{
	ApiKey = "<Seu valor>".To<SecureString>(),
	SigningKey = "<Seu valor>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Conteúdo recomendado

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
