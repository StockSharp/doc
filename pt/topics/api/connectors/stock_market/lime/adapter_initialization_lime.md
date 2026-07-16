# Inicialização do adaptador Lime

O código seguinte demonstra como inicializar o [LimeMessageAdapter](xref:StockSharp.Lime.LimeMessageAdapter) e passá-lo ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new LimeMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<Nome de utilizador>",
	Password = "<Palavra-passe>".ToSecureString(),
	ClientId = "<Identificador do cliente>",
	ClientSecret = "<Segredo do cliente>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Substitua os valores de exemplo pelas credenciais e pelos endereços dos servidores emitidos para a sua conta.

## Conteúdo recomendado

[Janela de definições de ligação](../../../graphical_user_interface/connection_settings_window.md)
