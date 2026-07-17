# Inicialização do adaptador: Capital.com

O código seguinte mostra como inicializar [CapitalComMessageAdapter](xref:StockSharp.CapitalCom.CapitalComMessageAdapter) e adicioná-lo ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new CapitalComMessageAdapter(Connector.TransactionIdGenerator)
{
	Password = "<valor>".ToSecureString(),
	ApiKey = "<valor>",
	Login = "<valor>",
	AccountId = "<valor>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Substitua os valores de exemplo pelos parâmetros emitidos ou configurados para a sua conta.

## Consulte também

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
