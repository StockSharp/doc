# Inicialização do adaptador: Ventura

O código a seguir inicializa [VenturaMessageAdapter](xref:StockSharp.Ventura.VenturaMessageAdapter) e o adiciona ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new VenturaMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<key>".ToSecureString(),
	Secret = "<secret>".ToSecureString(),
	Token = "<token>".ToSecureString(),
	ClientId = "<id>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Informe as credenciais e as demais propriedades necessárias descritas na página de [Configuração do conector](configuration_ventura.md).

## Veja também

[Configuração do conector](configuration_ventura.md)

[janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
