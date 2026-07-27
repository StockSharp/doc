# Inicialização do adaptador: Paytm Money

O código a seguir inicializa [PaytmMoneyMessageAdapter](xref:StockSharp.PaytmMoney.PaytmMoneyMessageAdapter) e o adiciona ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new PaytmMoneyMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<key>".ToSecureString(),
	Secret = "<secret>".ToSecureString(),
	Token = "<token>".ToSecureString(),
	ReadAccessToken = "<token>".ToSecureString(),
	PublicAccessToken = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Informe as credenciais e as demais propriedades necessárias descritas na página de [Configuração do conector](configuration_paytm_money.md).

## Veja também

[Configuração do conector](configuration_paytm_money.md)

[janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
