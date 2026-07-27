# Inicialização do adaptador: Mastertrust

O código a seguir inicializa [MastertrustMessageAdapter](xref:StockSharp.Mastertrust.MastertrustMessageAdapter) e o adiciona ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new MastertrustMessageAdapter(Connector.TransactionIdGenerator)
{
	ClientId = "<id>",
	OAuthClientSecret = "<secret>".ToSecureString(),
	AuthorizationCode = "<code>".ToSecureString(),
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Informe as credenciais e as demais propriedades necessárias descritas na página de [Configuração do conector](configuration_mastertrust.md).

## Veja também

[Configuração do conector](configuration_mastertrust.md)

[janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
