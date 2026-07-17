# Inicialização do adaptador NinjaTrader

O código seguinte demonstra como inicializar o [NinjaTraderMessageAdapter](xref:StockSharp.NinjaTrader.NinjaTraderMessageAdapter) e passá-lo ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new NinjaTraderMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<Nome de utilizador>",
	Password = "<Palavra-passe>".ToSecureString(),
	ClientId = "<Identificador do cliente>",
	Secret = "<Segredo>".ToSecureString(),
	AppId = "StockSharp",
	AppVersion = "1.0",
	DeviceId = Guid.NewGuid().ToString(),
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Substitua os valores de exemplo pelas credenciais e pelos endereços dos servidores emitidos para a sua conta.

## Conteúdo recomendado

[Janela de definições de ligação](../../../graphical_user_interface/connection_settings_window.md)
