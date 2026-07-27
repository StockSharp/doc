# Inicialização do adaptador: Unusual Whales

O código a seguir inicializa [UnusualWhalesMessageAdapter](xref:StockSharp.UnusualWhales.UnusualWhalesMessageAdapter) e o adiciona ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new UnusualWhalesMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Informe as credenciais e as demais propriedades necessárias descritas na página de [Configuração do conector](configuration_unusual_whales.md).

## Veja também

[Configuração do conector](configuration_unusual_whales.md)

[janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
