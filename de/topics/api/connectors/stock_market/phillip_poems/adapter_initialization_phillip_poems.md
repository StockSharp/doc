# Adapterinitialisierung Phillip POEMS

Der folgende Code zeigt, wie der [PhillipPoemsMessageAdapter](xref:StockSharp.PhillipPoems.PhillipPoemsMessageAdapter) initialisiert und an den [Connector](xref:StockSharp.Algo.Connector) übergeben wird.

```cs
Connector Connector = new Connector();
...
var messageAdapter = new PhillipPoemsMessageAdapter(Connector.TransactionIdGenerator)
{
	ClientId = "<Ihr Wert>",
	ClientSecret = "<Ihr Wert>".To<SecureString>(),
	ApiKey = "<Ihr Wert>".To<SecureString>(),
	AccessToken = "<Ihr Wert>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Empfohlene Inhalte

[Fenster Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
