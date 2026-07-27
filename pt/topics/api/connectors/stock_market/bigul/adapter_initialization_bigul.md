# Inicialização do adaptador: Bigul

O código a seguir inicializa [BigulMessageAdapter](xref:StockSharp.Bigul.BigulMessageAdapter) e o adiciona ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new BigulMessageAdapter(Connector.TransactionIdGenerator)
{
	ClientCode = "<id>",
	ApiKey = "<key>".ToSecureString(),
	ApiSecret = "<secret>".ToSecureString(),
	OneTimePassword = "<secret>".ToSecureString(),
	Token = "<token>".ToSecureString(),
	Source = "<id>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Informe as credenciais e as demais propriedades necessárias descritas na página de [Configuração do conector](configuration_bigul.md).

## Veja também

[Configuração do conector](configuration_bigul.md)

[janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
