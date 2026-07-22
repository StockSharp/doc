# SBE アダプターの初期化

次のコードは [CryBroSBEMessageAdapter](xref:StockSharp.CryBro.SBE.CryBroSBEMessageAdapter) を初期化し、[Connector](xref:StockSharp.Algo.Connector) に追加します。

```cs
var messageAdapter = new CryBroSBEMessageAdapter(Connector.TransactionIdGenerator)
{
	Address = "127.0.0.1:5002".To<EndPoint>(),
	SenderCompId = "<login>",
	TargetCompId = "StockSharp",
	Password = "<password>".ToSecureString(),
	IsSupportNativeCandles = false,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

クライアントとサーバーでは、互換性のある SBE スキーマ識別子とバージョンを使用する必要があります。

## 関連項目

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
