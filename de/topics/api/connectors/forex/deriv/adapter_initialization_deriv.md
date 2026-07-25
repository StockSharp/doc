# Adapter initialisieren: Deriv

Der folgende Code initialisiert [DerivMessageAdapter](xref:StockSharp.Deriv.DerivMessageAdapter) und fügt ihn zu [Connector](xref:StockSharp.Algo.Connector) hinzu.

```cs
var messageAdapter = new DerivMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
	AppId = "<app-id>",
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Ersetzen Sie die Beispielwerte durch das Token und die Anwendungskennung des gewählten Demo- oder Echtkontos.

## Siehe auch

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
