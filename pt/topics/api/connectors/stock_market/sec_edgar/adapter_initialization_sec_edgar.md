# Inicialização do adaptador: SEC EDGAR

O código a seguir inicializa [SecEdgarMessageAdapter](xref:StockSharp.SecEdgar.SecEdgarMessageAdapter) e o adiciona ao [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new SecEdgarMessageAdapter(connector.TransactionIdGenerator)
{
	UserAgent = "<Seu aplicativo seu-email@example.com>",
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Informe os valores de acesso e as demais propriedades necessárias descritas na página de [Configuração do conector](configuration_sec_edgar.md).

## Veja também

[Configuração do conector](configuration_sec_edgar.md)

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
