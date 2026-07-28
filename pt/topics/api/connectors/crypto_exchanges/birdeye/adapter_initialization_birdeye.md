# Inicialização do adaptador: Birdeye

O código a seguir inicializa [BirdeyeMessageAdapter](xref:StockSharp.Birdeye.BirdeyeMessageAdapter) e o adiciona ao [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new BirdeyeMessageAdapter(connector.TransactionIdGenerator)
{
	Token = "<Seu token de acesso>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Informe as credenciais e as demais propriedades necessárias descritas na página de [Configuração do conector](configuration_birdeye.md).

## Veja também

[Configuração do conector](configuration_birdeye.md)

[janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
