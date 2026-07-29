# Inicialização do adaptador: SimFin

O código a seguir inicializa [SimFinMessageAdapter](xref:StockSharp.SimFin.SimFinMessageAdapter) e o adiciona ao [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new SimFinMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<Sua chave de API>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Informe os valores de acesso e as demais propriedades necessárias descritas na página de [Configuração do conector](configuration_simfin.md).

## Veja também

[Configuração do conector](configuration_simfin.md)

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
