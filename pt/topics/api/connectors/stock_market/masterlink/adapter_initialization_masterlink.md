# Inicialização do adaptador: MasterLink

O código a seguir inicializa [MasterLinkMessageAdapter](xref:StockSharp.MasterLink.MasterLinkMessageAdapter) e o adiciona ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new MasterLinkMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<id>",
	Password = "<secret>".ToSecureString(),
	CertificatePath = "<id>",
	CertificatePassword = "<secret>".ToSecureString(),
	NodePath = "<id>",
	GatewayDirectory = "<id>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Informe as credenciais e as demais propriedades necessárias descritas na página de [Configuração do conector](configuration_masterlink.md).

## Veja também

[Configuração do conector](configuration_masterlink.md)

[janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
