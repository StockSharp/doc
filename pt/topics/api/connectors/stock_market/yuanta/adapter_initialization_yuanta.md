# Inicialização do adaptador: Yuanta SPARK

O código seguinte mostra como inicializar [YuantaMessageAdapter](xref:StockSharp.Yuanta.YuantaMessageAdapter) e adicioná-lo ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new YuantaMessageAdapter(Connector.TransactionIdGenerator)
{
	Password = "<valor>".ToSecureString(),
	CertificatePassword = "<valor>".ToSecureString(),
	SdkPath = "<valor>",
	Account = "<valor>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Substitua os valores de exemplo pelos parâmetros emitidos ou configurados para a sua conta.

## Consulte também

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
