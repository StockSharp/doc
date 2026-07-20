# Adapterinitialisierung Nasdaq Cloud Data Service

Der folgende Code zeigt, wie der [NasdaqCloudDataServiceMessageAdapter](xref:StockSharp.NasdaqCloudDataService.NasdaqCloudDataServiceMessageAdapter) initialisiert und an den [Connector](xref:StockSharp.Algo.Connector) übergeben wird.

```cs
Connector Connector = new Connector();
...
var messageAdapter = new NasdaqCloudDataServiceMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<Ihr Wert>",
	Password = "<Ihr Wert>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Empfohlene Inhalte

[Fenster Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
