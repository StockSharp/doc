# Inicialização do adaptador CTP

O código seguinte demonstra como inicializar o [CtpMessageAdapter](xref:StockSharp.Ctp.CtpMessageAdapter) e passá-lo ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new CtpMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<Nome de utilizador>",
	Password = "<Palavra-passe>".ToSecureString(),
	BrokerId = "<Identificador do corretor>",
	InvestorId = "<Identificador do investidor>",
	MarketDataAddress = "tcp://<Endereço de dados de mercado>",
	TraderAddress = "tcp://<Endereço do servidor de negociação>",
	AppId = "<Identificador da aplicação>",
	AuthCode = "<Código de autenticação>".ToSecureString(),
	ProductionMode = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Substitua os valores de exemplo pelas credenciais e pelos endereços dos servidores emitidos para a sua conta.

## Conteúdo recomendado

[Janela de definições de ligação](../../../graphical_user_interface/connection_settings_window.md)
