# Inicialização do adaptador: Nubra

O código a seguir inicializa [NubraMessageAdapter](xref:StockSharp.Nubra.NubraMessageAdapter) e o adiciona ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new NubraMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
	DeviceId = "<id>",
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Informe as credenciais e as demais propriedades necessárias descritas na página de [Configuração do conector](configuration_nubra.md).

## Veja também

[Configuração do conector](configuration_nubra.md)

[janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
