# Inicialização do adaptador: GLEIF

O código a seguir inicializa [GleifMessageAdapter](xref:StockSharp.Gleif.GleifMessageAdapter) e o adiciona ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new GleifMessageAdapter(Connector.TransactionIdGenerator);

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Informe as credenciais e as demais propriedades necessárias descritas na página de [Configuração do conector](configuration_gleif.md).

## Veja também

[Configuração do conector](configuration_gleif.md)

[janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
