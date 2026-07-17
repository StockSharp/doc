# Inicialização do adaptador Kotak Neo

O código seguinte demonstra como inicializar o [KotakNeoMessageAdapter](xref:StockSharp.KotakNeo.KotakNeoMessageAdapter) e passá-lo ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new KotakNeoMessageAdapter(Connector.TransactionIdGenerator)
{
	ConsumerKey = "<Chave de consumidor>".ToSecureString(),
	MobileNumber = "<Número de telemóvel>",
	UserCode = "<Código de utilizador>",
	Mpin = "<MPIN>".ToSecureString(),
	TotpSecret = "<Segredo TOTP>".ToSecureString(),
	DefaultProduct = KotakNeoProducts.Intraday,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Substitua os valores de exemplo pelas credenciais e pelos endereços dos servidores emitidos para a sua conta.

## Conteúdo recomendado

[Janela de definições de ligação](../../../graphical_user_interface/connection_settings_window.md)
