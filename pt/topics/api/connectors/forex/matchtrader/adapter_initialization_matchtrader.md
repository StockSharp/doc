# Inicialização do adaptador Match-Trader

O código abaixo demonstra como inicializar o [MatchTraderMessageAdapter](xref:StockSharp.MatchTrader.MatchTraderMessageAdapter) e passá-lo para o [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new MatchTraderMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<Seu valor>",
	Password = "<Seu valor>".To<SecureString>(),
	AccountId = "<Seu valor>",
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Conteúdo recomendado

[Janela de definições de ligação](../../../graphical_user_interface/connection_settings_window.md)
