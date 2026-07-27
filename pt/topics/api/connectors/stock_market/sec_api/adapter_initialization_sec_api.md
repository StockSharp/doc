# Inicialização do adaptador: SEC API

O código a seguir inicializa [SecApiMessageAdapter](xref:StockSharp.SecApi.SecApiMessageAdapter) e o adiciona ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new SecApiMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Informe as credenciais e as demais propriedades necessárias descritas na página de [Configuração do conector](configuration_sec_api.md).

## Veja também

[Configuração do conector](configuration_sec_api.md)

[janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
