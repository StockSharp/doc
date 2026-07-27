# Inicialização do adaptador: Quiver Quantitative

O código a seguir inicializa [QuiverQuantMessageAdapter](xref:StockSharp.QuiverQuant.QuiverQuantMessageAdapter) e o adiciona ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new QuiverQuantMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Informe as credenciais e as demais propriedades necessárias descritas na página de [Configuração do conector](configuration_quiver_quant.md).

## Veja também

[Configuração do conector](configuration_quiver_quant.md)

[janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
