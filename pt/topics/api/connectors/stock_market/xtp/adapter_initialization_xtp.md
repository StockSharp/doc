# Inicialização do adaptador Zhongtai XTP

O código seguinte demonstra como inicializar o [XtpMessageAdapter](xref:StockSharp.Xtp.XtpMessageAdapter) e passá-lo ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new XtpMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<Nome de utilizador>",
	Password = "<Palavra-passe>".ToSecureString(),
	ClientId = 1,
	QuoteAddress = new IPEndPoint(IPAddress.Parse("203.0.113.10"), 6001),
	TransactionAddress = new IPEndPoint(IPAddress.Parse("203.0.113.10"), 6002),
	Protocol = XtpProtocols.Tcp,
	SoftwareKey = "<Chave do software>",
	SoftwareVersion = "1.0",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Substitua os valores de exemplo pelas credenciais e pelos endereços dos servidores emitidos para a sua conta.

## Conteúdo recomendado

[Janela de definições de ligação](../../../graphical_user_interface/connection_settings_window.md)
