# Inicialização do adaptador: TWSE

O código a seguir inicializa [TwseMessageAdapter](xref:StockSharp.Twse.TwseMessageAdapter) e o adiciona ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new TwseMessageAdapter(Connector.TransactionIdGenerator);

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Informe as credenciais e as demais propriedades necessárias descritas na página de [Configuração do conector](configuration_twse_openapi.md).

## Veja também

[Configuração do conector](configuration_twse_openapi.md)

[janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
