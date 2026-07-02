# FAST 适配器初始化

下面的代码演示了如何初始化 [FastMessageAdapter](xref:StockSharp.Fix.FastMessageAdapter) 并将其添加到 [Connector](xref:StockSharp.Algo.Connector) 中。

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new FastMessageAdapter(Connector.TransactionIdGenerator)
{
	// choose required dialect
	Dialect = typeof(StockSharp.Fix.Dialects.Bovespa.BovespaFastDialect),
};
// load all dialect settings from an exchange config file
messageAdapter.DialectSettings.LoadSettingsFromFile(configFile);
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## 推荐内容

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
