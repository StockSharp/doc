# Inicialização do adaptador: BCS

O código a seguir inicializa [BcsMessageAdapter](xref:StockSharp.Bcs.BcsMessageAdapter) e o adiciona ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new BcsMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Informe as credenciais e as demais propriedades necessárias descritas na página de [Configuração do conector](configuration_bcs.md).

## Veja também

[Configuração do conector](configuration_bcs.md)

[janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
