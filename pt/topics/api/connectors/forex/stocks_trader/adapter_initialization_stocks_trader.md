# Inicialização do adaptador: StocksTrader

O código a seguir inicializa [StocksTraderMessageAdapter](xref:StockSharp.StocksTrader.StocksTraderMessageAdapter) e o adiciona ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new StocksTraderMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Substitua os valores de exemplo pelo token emitido para a conta demo ou real escolhida.

## Veja também

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
