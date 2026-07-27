# Inicialização do adaptador: DNSE

O código a seguir inicializa [DnseMessageAdapter](xref:StockSharp.Dnse.DnseMessageAdapter) e o adiciona ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new DnseMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<key>".ToSecureString(),
	Secret = "<secret>".ToSecureString(),
	TradingToken = "<token>".ToSecureString(),
	Account = "<id>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Informe as credenciais e as demais propriedades necessárias descritas na página de [Configuração do conector](configuration_dnse.md).

## Veja também

[Configuração do conector](configuration_dnse.md)

[janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
