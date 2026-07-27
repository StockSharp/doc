# Inicialização do adaptador: FINRA

O código a seguir inicializa [FinraMessageAdapter](xref:StockSharp.Finra.FinraMessageAdapter) e o adiciona ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new FinraMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<key>".ToSecureString(),
	Secret = "<secret>".ToSecureString(),
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Informe as credenciais e as demais propriedades necessárias descritas na página de [Configuração do conector](configuration_finra.md).

## Veja também

[Configuração do conector](configuration_finra.md)

[janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
