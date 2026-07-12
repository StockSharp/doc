# Binance アダプターの初期化

以下のコードは、[BinanceMessageAdapter](xref:StockSharp.Binance.BinanceMessageAdapter) を初期化し、それを [Connector](xref:StockSharp.Algo.Connector) に渡す方法を示しています。

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new BinanceMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<APIキー>".To<SecureString>(),
	Secret = "<APIシークレット>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

別の、より便利な方法として、`AddAdapter<T>()` 拡張メソッドを使用できます。

```cs
Connector Connector = new Connector();
...
Connector.AddAdapter<BinanceMessageAdapter>(a =>
{
	a.Key = "<APIキー>".To<SecureString>();
	a.Secret = "<APIシークレット>".To<SecureString>();
});
```

## 関連項目

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
