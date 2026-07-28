# Inicialização do adaptador: Quidax

O código a seguir inicializa [QuidaxMessageAdapter](xref:StockSharp.Quidax.QuidaxMessageAdapter) e o adiciona ao [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new QuidaxMessageAdapter(connector.TransactionIdGenerator)
{
	Token = "<Seu token de acesso>".To<SecureString>(),
	UserId = "<Seu identificador de usuário>",
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Informe as credenciais e as demais propriedades necessárias descritas na página de [Configuração do conector](configuration_quidax.md).

## Veja também

[Configuração do conector](configuration_quidax.md)

[janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
