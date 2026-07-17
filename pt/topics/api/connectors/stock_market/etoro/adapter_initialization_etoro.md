# Inicialização do adaptador: eToro

O código seguinte mostra como inicializar [EtoroMessageAdapter](xref:StockSharp.Etoro.EtoroMessageAdapter) e adicioná-lo ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new EtoroMessageAdapter(Connector.TransactionIdGenerator)
{
	PublicApiKey = "<valor>".ToSecureString(),
	UserKey = "<valor>".ToSecureString(),
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Substitua os valores de exemplo pelos parâmetros emitidos ou configurados para a sua conta.

## Consulte também

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
