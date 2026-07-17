# Inicialização do adaptador IG Markets

O código seguinte demonstra como inicializar o [IgMessageAdapter](xref:StockSharp.IG.IgMessageAdapter) e passá-lo ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new IgMessageAdapter(Connector.TransactionIdGenerator)
{
	ApiKey = "<Chave de API>",
	UserName = "<Nome de utilizador>",
	Password = "<Palavra-passe>".ToSecureString(),
	AccountId = "<Identificador da conta>",
	Environment = IgEnvironments.Demo,
	EncryptPassword = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Substitua os valores de exemplo pelas credenciais e pelos endereços dos servidores emitidos para a sua conta.

## Conteúdo recomendado

[Janela de definições de ligação](../../../graphical_user_interface/connection_settings_window.md)
