# Inicialização do adaptador: Directa

O código a seguir inicializa [DirectaMessageAdapter](xref:StockSharp.Directa.DirectaMessageAdapter) e o adiciona ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new DirectaMessageAdapter(Connector.TransactionIdGenerator);

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Informe as credenciais e as demais propriedades necessárias descritas na página de [Configuração do conector](configuration_directa.md).

## Veja também

[Configuração do conector](configuration_directa.md)

[janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
