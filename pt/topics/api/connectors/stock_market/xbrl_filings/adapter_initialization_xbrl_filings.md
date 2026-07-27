# Inicialização do adaptador: XBRL Filings

O código a seguir inicializa [XbrlFilingsMessageAdapter](xref:StockSharp.XbrlFilings.XbrlFilingsMessageAdapter) e o adiciona ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new XbrlFilingsMessageAdapter(Connector.TransactionIdGenerator);

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Informe as credenciais e as demais propriedades necessárias descritas na página de [Configuração do conector](configuration_xbrl_filings.md).

## Veja também

[Configuração do conector](configuration_xbrl_filings.md)

[janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
