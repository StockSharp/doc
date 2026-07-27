# Inicialização do adaptador: KRX Open API

O código a seguir inicializa [KrxOpenApiMessageAdapter](xref:StockSharp.KrxOpenApi.KrxOpenApiMessageAdapter) e o adiciona ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new KrxOpenApiMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Informe as credenciais e as demais propriedades necessárias descritas na página de [Configuração do conector](configuration_krx_open_api.md).

## Veja também

[Configuração do conector](configuration_krx_open_api.md)

[janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
