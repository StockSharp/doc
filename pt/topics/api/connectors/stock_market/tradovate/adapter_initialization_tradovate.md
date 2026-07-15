# Inicialização do adaptador Tradovate

O código seguinte demonstra como inicializar o [TradovateMessageAdapter](xref:StockSharp.Tradovate.TradovateMessageAdapter) e passá-lo ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new TradovateMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<nome de utilizador>",
	Password = "<palavra-passe>".ToSecureString(),
	ClientId = "<identificador do cliente da API>",
	Secret = "<segredo do cliente da API>".ToSecureString(),
	AppId = "StockSharp",
	AppVersion = "1.0",
	DeviceId = "<identificador estável do dispositivo>",
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Defina `IsDemo` como `false` para ligar ao ambiente real.

## Conteúdo recomendado

[Janela de definições de ligação](../../../graphical_user_interface/connection_settings_window.md)
