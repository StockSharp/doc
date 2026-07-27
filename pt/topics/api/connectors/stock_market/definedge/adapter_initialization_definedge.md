# Inicialização do adaptador: Definedge

O código a seguir inicializa [DefinedgeMessageAdapter](xref:StockSharp.Definedge.DefinedgeMessageAdapter) e o adiciona ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new DefinedgeMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<key>".ToSecureString(),
	Secret = "<secret>".ToSecureString(),
	Token = "<token>".ToSecureString(),
	WebSocketToken = "<token>".ToSecureString(),
	UserId = "<id>",
	AccountId = "<id>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Informe as credenciais e as demais propriedades necessárias descritas na página de [Configuração do conector](configuration_definedge.md).

## Veja também

[Configuração do conector](configuration_definedge.md)

[janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
