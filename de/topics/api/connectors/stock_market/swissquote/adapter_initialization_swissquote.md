# Adapter-Initialisierung: Swissquote OpenWealth

Der folgende Code zeigt, wie [SwissquoteMessageAdapter](xref:StockSharp.Swissquote.SwissquoteMessageAdapter) initialisiert und zu [Connector](xref:StockSharp.Algo.Connector) hinzugefügt wird.

```cs
var messageAdapter = new SwissquoteMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<Wert>".ToSecureString(),
	CustomerId = "<Wert>",
	SafekeepingAccountId = "<Wert>",
	CashAccountId = "<Wert>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Ersetzen Sie die Beispielwerte durch die für Ihr Konto ausgegebenen oder konfigurierten Parameter.

## Siehe auch

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
