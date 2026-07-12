# Adapterinitialisierung BingX

Der folgende Code zeigt, wie [BingXMessageAdapter](xref:StockSharp.BingX.BingXMessageAdapter) initialisiert und an den [Connector](xref:StockSharp.Algo.Connector) übergeben wird.

```cs
                        Connector Connector = new Connector();
                        ...
                        var messageAdapter = new BingXMessageAdapter(Connector.TransactionIdGenerator)
                        {
                                Key = "<Ihr API-Schlüssel>".To<SecureString>(),
                                Secret = "<Ihr API-Geheimnis>".To<SecureString>(),
                        };
                        Connector.Adapter.InnerAdapters.Add(messageAdapter);
                        ...
```

## Siehe auch

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
