# Inicialização do adaptador: Open DART

O código a seguir inicializa [OpenDartMessageAdapter](xref:StockSharp.OpenDart.OpenDartMessageAdapter) e o adiciona ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new OpenDartMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Informe as credenciais e as demais propriedades necessárias descritas na página de [Configuração do conector](configuration_open_dart.md).

## Veja também

[Configuração do conector](configuration_open_dart.md)

[janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
