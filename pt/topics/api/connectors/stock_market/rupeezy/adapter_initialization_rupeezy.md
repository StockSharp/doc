# Inicialização do adaptador: Rupeezy

O código a seguir inicializa [RupeezyMessageAdapter](xref:StockSharp.Rupeezy.RupeezyMessageAdapter) e o adiciona ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new RupeezyMessageAdapter(Connector.TransactionIdGenerator)
{
	ApplicationId = "<id>",
	ApiKey = "<key>".ToSecureString(),
	AuthCode = "<code>".ToSecureString(),
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Informe as credenciais e as demais propriedades necessárias descritas na página de [Configuração do conector](configuration_rupeezy.md).

## Veja também

[Configuração do conector](configuration_rupeezy.md)

[janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
