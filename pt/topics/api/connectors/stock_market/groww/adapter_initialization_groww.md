# Inicialização do adaptador: Groww

O código seguinte mostra como inicializar [GrowwMessageAdapter](xref:StockSharp.Groww.GrowwMessageAdapter) e adicioná-lo ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new GrowwMessageAdapter(Connector.TransactionIdGenerator)
{
	AccessToken = "<valor>".ToSecureString(),
	ApiKey = "<valor>".ToSecureString(),
	ApiSecret = "<valor>".ToSecureString(),
	TotpSecret = "<valor>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Substitua os valores de exemplo pelos parâmetros emitidos ou configurados para a sua conta.

## Consulte também

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
