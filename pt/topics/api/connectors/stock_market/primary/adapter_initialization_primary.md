# Inicialização do adaptador: Primary

O código a seguir inicializa [PrimaryMessageAdapter](xref:StockSharp.Primary.PrimaryMessageAdapter) e o adiciona ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new PrimaryMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<id>",
	Password = "<secret>".ToSecureString(),
	IsDemo = true,
	Account = "<id>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Informe as credenciais e as demais propriedades necessárias descritas na página de [Configuração do conector](configuration_primary.md).

## Veja também

[Configuração do conector](configuration_primary.md)

[janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
