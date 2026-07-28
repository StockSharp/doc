# Inicialização do adaptador: Settrade

O código a seguir inicializa [SettradeMessageAdapter](xref:StockSharp.Settrade.SettradeMessageAdapter) e o adiciona ao [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new SettradeMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<Sua chave de API>".To<SecureString>(),
	Secret = "<Seu segredo de API>".To<SecureString>(),
	AppCode = "<O código do seu aplicativo>",
	BrokerId = "<O identificador da sua corretora>",
	Account = "<O número da sua conta>",
	Pin = "<Seu PIN de negociação>".To<SecureString>(),
	AccountType = SettradeAccountTypes.Equity,
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Informe as credenciais, o tipo de conta e as demais propriedades necessárias descritas na página de [Configuração do conector](configuration_settrade.md).

## Veja também

[Configuração do conector](configuration_settrade.md)

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
