# Inicialização do adaptador: ESMA FIRDS

O código a seguir inicializa [EsmaFirdsMessageAdapter](xref:StockSharp.EsmaFirds.EsmaFirdsMessageAdapter) e o adiciona ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new EsmaFirdsMessageAdapter(Connector.TransactionIdGenerator);

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Informe as credenciais e as demais propriedades necessárias descritas na página de [Configuração do conector](configuration_esma_firds.md).

## Veja também

[Configuração do conector](configuration_esma_firds.md)

[janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
