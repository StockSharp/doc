# Inicialização do adaptador BingX

O código abaixo mostra como inicializar o [BingXMessageAdapter](xref:StockSharp.BingX.BingXMessageAdapter) e passá-lo para o [Connector](xref:StockSharp.Algo.Connector).

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

## Ver também

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
