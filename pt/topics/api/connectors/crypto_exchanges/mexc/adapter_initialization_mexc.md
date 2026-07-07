# Inicialização do adaptador MEXC

O código abaixo mostra como inicializar [MexcMessageAdapter](xref:StockSharp.Mexc.MexcMessageAdapter) e passá-lo ao [Connector](xref:StockSharp.Algo.Connector).

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

## Consulte também

[Janela de definições de ligação](../../../graphical_user_interface/connection_settings_window.md)
