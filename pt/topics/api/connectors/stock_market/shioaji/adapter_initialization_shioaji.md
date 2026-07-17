# Inicialização do adaptador: SinoPac Shioaji

O código seguinte mostra como inicializar [ShioajiMessageAdapter](xref:StockSharp.Shioaji.ShioajiMessageAdapter) e adicioná-lo ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new ShioajiMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<valor>".ToSecureString(),
	Secret = "<valor>".ToSecureString(),
	Address = "<valor>",
	ReconnectAttempts = 10,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Substitua os valores de exemplo pelos parâmetros emitidos ou configurados para a sua conta.

## Consulte também

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
