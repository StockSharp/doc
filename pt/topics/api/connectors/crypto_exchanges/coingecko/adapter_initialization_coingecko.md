# Inicialização do adaptador CoinGecko

O código abaixo demonstra como inicializar o [CoinGeckoMessageAdapter](xref:StockSharp.CoinGecko.CoinGeckoMessageAdapter) e passá-lo para o [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new CoinGeckoMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<Seu valor>".To<SecureString>(),
	QuoteCurrency = "<Seu valor>",
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Conteúdo recomendado

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
