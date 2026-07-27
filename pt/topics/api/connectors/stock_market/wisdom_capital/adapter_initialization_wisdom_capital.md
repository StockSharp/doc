# Inicialização do adaptador: Wisdom Capital

O código a seguir inicializa [WisdomCapitalMessageAdapter](xref:StockSharp.WisdomCapital.WisdomCapitalMessageAdapter) e o adiciona ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new WisdomCapitalMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<key>".ToSecureString(),
	Secret = "<secret>".ToSecureString(),
	MarketDataKey = "<key>".ToSecureString(),
	MarketDataSecret = "<secret>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Informe as credenciais e as demais propriedades necessárias descritas na página de [Configuração do conector](configuration_wisdom_capital.md).

## Veja também

[Configuração do conector](configuration_wisdom_capital.md)

[janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
