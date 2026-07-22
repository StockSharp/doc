# Inicialização do adaptador: FXOpen TickTrader

O código a seguir inicializa [FXOpenMessageAdapter](xref:StockSharp.FXOpen.FXOpenMessageAdapter) e o adiciona ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new FXOpenMessageAdapter(Connector.TransactionIdGenerator)
{
	WebApiId = "<web-api-id>",
	Key = "<key>".ToSecureString(),
	Secret = "<secret>".ToSecureString(),
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Substitua os valores de exemplo pelos parâmetros do token da conta real ou demo escolhida.

## Veja também

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
