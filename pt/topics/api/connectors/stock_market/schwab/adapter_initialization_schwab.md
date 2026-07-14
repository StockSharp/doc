# Inicialização do adaptador Charles Schwab

O código seguinte demonstra como inicializar o [SchwabMessageAdapter](xref:StockSharp.Schwab.SchwabMessageAdapter) e passá-lo ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new SchwabMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token de acesso>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

## Ver também

[Janela de definições de ligação](../../../graphical_user_interface/connection_settings_window.md)
