# Inicialização do adaptador Tardis

O código abaixo demonstra como inicializar o [TardisMessageAdapter](xref:StockSharp.Tardis.TardisMessageAdapter) e passá-lo para o [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new TardisMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<Seu valor>".To<SecureString>(),
	Exchange = "<Seu valor>",
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Conteúdo recomendado

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
