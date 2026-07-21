# Inicialização do adaptador GMTrade

O código abaixo demonstra como inicializar o [GMTradeMessageAdapter](xref:StockSharp.GMTrade.GMTradeMessageAdapter) e passá-lo para o [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new GMTradeMessageAdapter(Connector.TransactionIdGenerator)
{
	WalletAddress = "<Seu valor>",
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Conteúdo recomendado

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
