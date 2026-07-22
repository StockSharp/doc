# Inicialização do adaptador SBE

O código a seguir inicializa [CryBroSBEMessageAdapter](xref:StockSharp.CryBro.SBE.CryBroSBEMessageAdapter) e o adiciona ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new CryBroSBEMessageAdapter(Connector.TransactionIdGenerator)
{
	Address = "127.0.0.1:5002".To<EndPoint>(),
	SenderCompId = "<login>",
	TargetCompId = "StockSharp",
	Password = "<password>".ToSecureString(),
	IsSupportNativeCandles = false,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

O cliente e o servidor devem usar identificadores e versões compatíveis do esquema SBE.

## Veja também

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
