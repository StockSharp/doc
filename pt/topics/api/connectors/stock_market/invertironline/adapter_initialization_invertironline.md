# Inicialização do adaptador: InvertirOnline

O código a seguir inicializa [InvertirOnlineMessageAdapter](xref:StockSharp.InvertirOnline.InvertirOnlineMessageAdapter) e o adiciona ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new InvertirOnlineMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<id>",
	Password = "<secret>".ToSecureString(),
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Informe as credenciais e as demais propriedades necessárias descritas na página de [Configuração do conector](configuration_invertironline.md).

## Veja também

[Configuração do conector](configuration_invertironline.md)

[janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
