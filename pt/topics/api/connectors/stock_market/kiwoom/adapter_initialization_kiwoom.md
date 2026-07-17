# Inicialização do adaptador: Kiwoom

O código seguinte mostra como inicializar [KiwoomMessageAdapter](xref:StockSharp.Kiwoom.KiwoomMessageAdapter) e adicioná-lo ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new KiwoomMessageAdapter(Connector.TransactionIdGenerator)
{
	AppKey = "<valor>".ToSecureString(),
	AppSecret = "<valor>".ToSecureString(),
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Substitua os valores de exemplo pelos parâmetros emitidos ou configurados para a sua conta.

## Consulte também

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
