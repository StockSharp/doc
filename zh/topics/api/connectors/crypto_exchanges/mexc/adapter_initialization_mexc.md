# MEXC 适配器初始化

下面的代码展示了如何初始化 [MexcMessageAdapter](xref:StockSharp.Mexc.MexcMessageAdapter)，并将其传递给 [Connector](xref:StockSharp.Algo.Connector)。

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

## 另请参阅

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
