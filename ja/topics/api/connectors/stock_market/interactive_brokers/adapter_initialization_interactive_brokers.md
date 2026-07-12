# Interactive Brokers アダプターの初期化

以下のコードは、[InteractiveBrokersMessageAdapter](xref:StockSharp.InteractiveBrokers.InteractiveBrokersMessageAdapter) を初期化し、それを [Connector](xref:StockSharp.Algo.Connector) に渡す方法を示しています。

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new InteractiveBrokersMessageAdapter(Connector.TransactionIdGenerator)
{
	Address = "<アドレス>".To<EndPoint>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

別の、より便利な方法として、`AddAdapter<T>()` 拡張メソッドを使用できます。

```cs
Connector Connector = new Connector();
...
Connector.AddAdapter<InteractiveBrokersMessageAdapter>(a =>
{
	a.Address = "<アドレス>".To<EndPoint>();
});
```

## 関連項目

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
