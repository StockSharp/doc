# Inicialização do adaptador: Firstock

O código a seguir inicializa [FirstockMessageAdapter](xref:StockSharp.Firstock.FirstockMessageAdapter) e o adiciona ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new FirstockMessageAdapter(Connector.TransactionIdGenerator)
{
	UserId = "<id>",
	Password = "<secret>".ToSecureString(),
	OneTimePassword = "<secret>".ToSecureString(),
	VendorCode = "<id>",
	ApiKey = "<key>".ToSecureString(),
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Informe as credenciais e as demais propriedades necessárias descritas na página de [Configuração do conector](configuration_firstock.md).

## Veja também

[Configuração do conector](configuration_firstock.md)

[janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
