# Inicialização do adaptador: TraderMade

O código a seguir inicializa [TraderMadeMessageAdapter](xref:StockSharp.TraderMade.TraderMadeMessageAdapter) e o adiciona ao [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new TraderMadeMessageAdapter(connector.TransactionIdGenerator)
{
	RestKey = "<Sua chave de API REST>".To<SecureString>(),
	StreamingKey = "<Sua chave de API de transmissão>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Informe os valores de acesso e as demais propriedades necessárias descritas na página de [Configuração do conector](configuration_tradermade.md).

## Veja também

[Configuração do conector](configuration_tradermade.md)

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
