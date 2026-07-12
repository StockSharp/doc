# Blackwood (Fusion) のアダプター初期化

以下のコードは、[BlackwoodMessageAdapter](xref:StockSharp.Blackwood.BlackwoodMessageAdapter) を初期化し、それを [Connector](xref:StockSharp.Algo.Connector) に渡す方法を示しています。

```cs
Connector Connector = new Connector();				
...				
var address = "<Address>".To<IPAddress>();
var messageAdapter = new BlackwoodMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<ログイン名>",
	Password = "<パスワード>".To<SecureString>(),
	ExecutionAddress = new IPEndPoint(address, BlackwoodAddresses.ExecutionPort),
	MarketDataAddress = new IPEndPoint(address, BlackwoodAddresses.MarketDataPort),
	HistoricalDataAddress = new IPEndPoint(address, BlackwoodAddresses.HistoricalDataPort)
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## 推奨コンテンツ

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
