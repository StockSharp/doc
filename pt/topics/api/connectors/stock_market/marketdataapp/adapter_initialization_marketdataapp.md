# Inicialização do adaptador: MarketData.app

O código a seguir inicializa [MarketDataAppMessageAdapter](xref:StockSharp.MarketDataApp.MarketDataAppMessageAdapter) e o adiciona ao [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new MarketDataAppMessageAdapter(connector.TransactionIdGenerator)
{
	Token = "<Seu token de API>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Informe os valores de acesso e as demais propriedades necessárias descritas na página de [Configuração do conector](configuration_marketdataapp.md).

## Veja também

[Configuração do conector](configuration_marketdataapp.md)

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
