# Adapterinitialisierung uSMART OpenAPI

Der folgende Code zeigt, wie der [UsmartMessageAdapter](xref:StockSharp.Usmart.UsmartMessageAdapter) initialisiert und an den [Connector](xref:StockSharp.Algo.Connector) übergeben wird.

```cs
Connector Connector = new Connector();
...
var messageAdapter = new UsmartMessageAdapter(Connector.TransactionIdGenerator)
{
	AccessToken = "<Ihr Wert>".To<SecureString>(),
	PrivateKey = "<Ihr Wert>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Empfohlene Inhalte

[Fenster Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
