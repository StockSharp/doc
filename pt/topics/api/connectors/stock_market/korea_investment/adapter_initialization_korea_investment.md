# Inicialização do adaptador: Korea Investment & Securities

O código seguinte mostra como inicializar [KoreaInvestmentMessageAdapter](xref:StockSharp.KoreaInvestment.KoreaInvestmentMessageAdapter) e adicioná-lo ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new KoreaInvestmentMessageAdapter(Connector.TransactionIdGenerator)
{
	AppKey = "<valor>".ToSecureString(),
	AppSecret = "<valor>".ToSecureString(),
	AccountNumber = "<valor>",
	ProductCode = "<valor>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Substitua os valores de exemplo pelos parâmetros emitidos ou configurados para a sua conta.

## Consulte também

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
