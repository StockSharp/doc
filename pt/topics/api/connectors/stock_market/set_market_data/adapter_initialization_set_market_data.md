# Inicialização do adaptador: SET Market Data

O código a seguir inicializa [SetMarketDataMessageAdapter](xref:StockSharp.SetMarketData.SetMarketDataMessageAdapter) e o adiciona ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new SetMarketDataMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Informe as credenciais e as demais propriedades necessárias descritas na página de [Configuração do conector](configuration_set_market_data.md).

## Veja também

[Configuração do conector](configuration_set_market_data.md)

[janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
