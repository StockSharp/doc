# 适配器初始化 BingX

下面的代码展示了如何初始化 [BingXMessageAdapter](xref:StockSharp.BingX.BingXMessageAdapter) 并将其传递给 [Connector](xref:StockSharp.Algo.Connector)。

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

## 另请参阅

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
