# MEXC-Adapter initialisieren

Der folgende Code zeigt, wie [MexcMessageAdapter](xref:StockSharp.Mexc.MexcMessageAdapter) initialisiert und an den [Connector](xref:StockSharp.Algo.Connector) übergeben wird.

```cs
                        Connector Connector = new Connector();
                        ...
                        var messageAdapter = new MexcMessageAdapter(Connector.TransactionIdGenerator)
                        {
                                Key = "<Your API Key>".To<SecureString>(),
                                Secret = "<Your API Secret>".To<SecureString>(),
                        };
                        Connector.Adapter.InnerAdapters.Add(messageAdapter);
                        ...
```

## Siehe auch

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)

