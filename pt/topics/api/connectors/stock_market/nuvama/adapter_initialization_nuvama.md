# Inicialização do adaptador: Nuvama

O código a seguir inicializa [NuvamaMessageAdapter](xref:StockSharp.Nuvama.NuvamaMessageAdapter) e o adiciona ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new NuvamaMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<key>".ToSecureString(),
	Secret = "<secret>".ToSecureString(),
	RequestId = "<id>".ToSecureString(),
	AppIdKey = "<key>".ToSecureString(),
	PublicIpAddress = "<id>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Informe as credenciais e as demais propriedades necessárias descritas na página de [Configuração do conector](configuration_nuvama.md).

## Veja também

[Configuração do conector](configuration_nuvama.md)

[janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
