# Inicialização do adaptador Webull

O código seguinte demonstra como inicializar o [WebullMessageAdapter](xref:StockSharp.Webull.WebullMessageAdapter) e passá-lo ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new WebullMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<chave da aplicação>".ToSecureString(),
	Secret = "<segredo da aplicação>".ToSecureString(),
	Token = "<token de acesso>".ToSecureString(),
	Account = "<identificador da conta>",
	IsDemo = false,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Os parâmetros `Token` e `Account` podem ser omitidos se não forem necessários.

## Ver também

[Janela de definições de ligação](../../../graphical_user_interface/connection_settings_window.md)
