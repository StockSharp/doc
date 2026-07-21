# Inicialização do adaptador Kalshi

O código abaixo demonstra como inicializar o [KalshiMessageAdapter](xref:StockSharp.Kalshi.KalshiMessageAdapter) e passá-lo para o [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new KalshiMessageAdapter(Connector.TransactionIdGenerator)
{
	ApiKey = "<Seu valor>",
	PrivateKey = "<Seu valor>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Conteúdo recomendado

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
