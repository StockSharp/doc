# Inicialização do adaptador TradeZero

O código seguinte demonstra como inicializar o [TradeZeroMessageAdapter](xref:StockSharp.TradeZero.TradeZeroMessageAdapter) e passá-lo ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new TradeZeroMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<TZ-API-KEY-ID>".ToSecureString(),
	Secret = "<TZ-API-SECRET-KEY>".ToSecureString(),
	DefaultRoute = "<rota de ordens opcional>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Pode omitir `DefaultRoute` para que o conector selecione uma rota compatível.

## Conteúdo recomendado

[Janela de definições de ligação](../../../graphical_user_interface/connection_settings_window.md)
