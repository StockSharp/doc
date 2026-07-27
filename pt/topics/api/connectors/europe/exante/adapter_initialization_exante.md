# Inicialização do adaptador: EXANTE

O código a seguir inicializa [ExanteMessageAdapter](xref:StockSharp.Exante.ExanteMessageAdapter) e o adiciona ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new ExanteMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<key>".ToSecureString(),
	Secret = "<secret>".ToSecureString(),
	IsDemo = true,
	SummaryCurrency = "<id>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Informe as credenciais e as demais propriedades necessárias descritas na página de [Configuração do conector](configuration_exante.md).

## Veja também

[Configuração do conector](configuration_exante.md)

[janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
