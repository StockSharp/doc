# Initialisierung des SBE-Adapters

Der folgende Code initialisiert [CryBroSBEMessageAdapter](xref:StockSharp.CryBro.SBE.CryBroSBEMessageAdapter) und fügt ihn zu [Connector](xref:StockSharp.Algo.Connector) hinzu.

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

Client und Server müssen kompatible SBE-Schema-IDs und Versionen verwenden.

## Siehe auch

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
