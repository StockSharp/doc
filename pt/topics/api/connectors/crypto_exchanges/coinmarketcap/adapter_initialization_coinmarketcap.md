# Inicialização do adaptador CoinMarketCap

O código abaixo demonstra como inicializar o [CoinMarketCapMessageAdapter](xref:StockSharp.CoinMarketCap.CoinMarketCapMessageAdapter) e passá-lo para o [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new CoinMarketCapMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<Seu valor>".To<SecureString>(),
	QuoteCurrency = "<Seu valor>",
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Conteúdo recomendado

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
