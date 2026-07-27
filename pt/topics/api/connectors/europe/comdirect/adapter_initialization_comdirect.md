# Inicialização do adaptador: comdirect

O código a seguir inicializa [ComdirectMessageAdapter](xref:StockSharp.Comdirect.ComdirectMessageAdapter) e o adiciona ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new ComdirectMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<key>".ToSecureString(),
	Secret = "<secret>".ToSecureString(),
	Login = "<id>",
	Password = "<secret>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Informe as credenciais e as demais propriedades necessárias descritas na página de [Configuração do conector](configuration_comdirect.md).

## Veja também

[Configuração do conector](configuration_comdirect.md)

[janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
