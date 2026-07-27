# Inicialização do adaptador: Marketaux

O código a seguir inicializa [MarketauxMessageAdapter](xref:StockSharp.Marketaux.MarketauxMessageAdapter) e o adiciona ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new MarketauxMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Informe as credenciais e as demais propriedades necessárias descritas na página de [Configuração do conector](configuration_marketaux.md).

## Veja também

[Configuração do conector](configuration_marketaux.md)

[janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
