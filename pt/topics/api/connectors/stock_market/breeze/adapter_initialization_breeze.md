# Inicialização do adaptador ICICI Direct Breeze

O código seguinte demonstra como inicializar o [BreezeMessageAdapter](xref:StockSharp.Breeze.BreezeMessageAdapter) e passá-lo ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new BreezeMessageAdapter(Connector.TransactionIdGenerator)
{
	ApiKey = "<Chave de API>",
	SecretKey = "<Chave secreta>".ToSecureString(),
	ApiSession = "<Sessão de API>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Substitua os valores de exemplo pelas credenciais e pelos endereços dos servidores emitidos para a sua conta.

## Conteúdo recomendado

[Janela de definições de ligação](../../../graphical_user_interface/connection_settings_window.md)

