# Inicialização do adaptador: GuruFocus

O código a seguir inicializa [GuruFocusMessageAdapter](xref:StockSharp.GuruFocus.GuruFocusMessageAdapter) e o adiciona ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new GuruFocusMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Informe as credenciais e as demais propriedades necessárias descritas na página de [Configuração do conector](configuration_gurufocus.md).

## Veja também

[Configuração do conector](configuration_gurufocus.md)

[janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
