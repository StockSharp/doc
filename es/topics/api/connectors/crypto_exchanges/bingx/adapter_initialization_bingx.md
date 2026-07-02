# Inicialización del adaptador BingX

El código a continuación muestra cómo inicializar [BingXMessageAdapter](xref:StockSharp.BingX.BingXMessageAdapter) y pasarlo al [Connector](xref:StockSharp.Algo.Connector).

```cs
                        Connector Connector = new Connector();
                        ...
                        var messageAdapter = new BingXMessageAdapter(Connector.TransactionIdGenerator)
                        {
                                Key = "<Your API Key>".To<SecureString>(),
                                Secret = "<Your API Secret>".To<SecureString>(),
                        };
                        Connector.Adapter.InnerAdapters.Add(messageAdapter);
                        ...
```

## Ver también

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
