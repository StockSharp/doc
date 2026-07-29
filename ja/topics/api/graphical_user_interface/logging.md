# ログ

[S#](../../api.md) は、ログを表示するための複数のグラフィカルコンポーネントを提供しています: [LogControl](xref:StockSharp.Xaml.LogControl) と [Monitor](xref:StockSharp.Xaml.Monitor)。

ログ用のビジュアルコンポーネントを使用する場合は、[GuiLogListener](xref:StockSharp.Xaml.GuiLogListener) ロガーを「リスナー」として使用する必要があります。このロガーは、新しい [LogMessage](xref:Ecng.Logging.LogMessage) メッセージを記録するたびに GUI スレッドと同期し、ビジュアルコンポーネントへ渡します。

独自のクラスでログ機能を実装するには、[ILogReceiver](xref:Ecng.Logging.ILogReceiver) インターフェイスを実装する必要があります。より簡単な方法は、*Samples\/08\_Misc\/01\_Logging* の例に示すように、[BaseLogReceiver](xref:Ecng.Logging.BaseLogReceiver) クラスを継承することです。

```cs
private class TestSource : BaseLogReceiver
{
}
private readonly LogManager _logManager = new LogManager();
private readonly TestSource _testSource = new TestSource();
public MainWindow()
{
	InitializeComponent();
	// 即時フラッシュ
	_logManager.FlushInterval = TimeSpan.FromMilliseconds(1);
	// テスト用ログソースを設定
	_logManager.Sources.Add(_testSource);
	// .NET Trace システムベースのソースを設定
	_logManager.Sources.Add(new Ecng.Logging.TraceSource());
	// MainWindow にログを書き込む
	_logManager.Listeners.Add(new GuiLogListener(Monitor));
	// さらに logs.txt ファイルへ
	_logManager.Listeners.Add(new FileLogListener
	{
		FileName = "logs",
	});
}
	  				
```

## 推奨コンテンツ

[ログ](../logging.md)

[ログパネル](logging/log_panel.md)

[拡張ログパネル](logging/extended_log_panel.md)
