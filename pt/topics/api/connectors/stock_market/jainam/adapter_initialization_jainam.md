# Inicialização do adaptador: Jainam

O código a seguir inicializa [JainamMessageAdapter](xref:StockSharp.Jainam.JainamMessageAdapter) e o adiciona ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new JainamMessageAdapter(Connector.TransactionIdGenerator)
{
	UserId = "<id>",
	AppCode = "<id>",
	ApiSecret = "<secret>".ToSecureString(),
	AuthCode = "<code>".ToSecureString(),
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Informe as credenciais e as demais propriedades necessárias descritas na página de [Configuração do conector](configuration_jainam.md).

## Veja também

[Configuração do conector](configuration_jainam.md)

[janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
