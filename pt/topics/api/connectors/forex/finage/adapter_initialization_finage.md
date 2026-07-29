# Inicialização do adaptador: Finage

O código a seguir inicializa [FinageMessageAdapter](xref:StockSharp.Finage.FinageMessageAdapter) e o adiciona ao [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new FinageMessageAdapter(connector.TransactionIdGenerator)
{
	ApiKey = "<Sua chave de API>".To<SecureString>(),
	StreamingToken = "<Seu token de transmissão>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Informe os valores de acesso e as demais propriedades necessárias descritas na página de [Configuração do conector](configuration_finage.md).

## Veja também

[Configuração do conector](configuration_finage.md)

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
