# Inicialização do adaptador: Fubon Neo

O código seguinte mostra como inicializar [FubonNeoMessageAdapter](xref:StockSharp.FubonNeo.FubonNeoMessageAdapter) e adicioná-lo ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new FubonNeoMessageAdapter(Connector.TransactionIdGenerator)
{
	Password = "<valor>".ToSecureString(),
	ApiKey = "<valor>".ToSecureString(),
	CertificatePassword = "<valor>".ToSecureString(),
	SdkPath = "<valor>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Substitua os valores de exemplo pelos parâmetros emitidos ou configurados para a sua conta.

## Consulte também

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
