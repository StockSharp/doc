# Inicialização do adaptador: Zebu

O código a seguir inicializa [ZebuMessageAdapter](xref:StockSharp.Zebu.ZebuMessageAdapter) e o adiciona ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new ZebuMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<key>".ToSecureString(),
	Secret = "<secret>".ToSecureString(),
	AuthorizationCode = "<code>".ToSecureString(),
	UserId = "<id>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Informe as credenciais e as demais propriedades necessárias descritas na página de [Configuração do conector](configuration_zebu.md).

## Veja também

[Configuração do conector](configuration_zebu.md)

[janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
