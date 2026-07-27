# Inicialização do adaptador: TPEx

O código a seguir inicializa [TpexMessageAdapter](xref:StockSharp.Tpex.TpexMessageAdapter) e o adiciona ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new TpexMessageAdapter(Connector.TransactionIdGenerator);

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Informe as credenciais e as demais propriedades necessárias descritas na página de [Configuração do conector](configuration_tpex.md).

## Veja também

[Configuração do conector](configuration_tpex.md)

[janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
