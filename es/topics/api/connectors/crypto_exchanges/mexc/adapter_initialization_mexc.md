# Inicialización del adaptador MEXC

El siguiente código muestra cómo inicializar [MexcMessageAdapter](xref:StockSharp.Mexc.MexcMessageAdapter) y enviarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
                        Connector Connector = new Connector();
                        ...
                        var messageAdapter = new MexcMessageAdapter(Connector.TransactionIdGenerator)
                        {
                                Key = "<Su clave API>".To<SecureString>(),
                                Secret = "<Su secreto API>".To<SecureString>(),
                        };
                        Connector.Adapter.InnerAdapters.Add(messageAdapter);
                        ...
```

## Ver también

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
