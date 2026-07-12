# 接続設定ウィンドウ

[ConnectorWindow](xref:StockSharp.Xaml.ConnectorWindow) は、コネクターを接続するためのアダプターを設定する特殊なウィンドウです。

![API GUI 接続ウィンドウ](../../../images/api_gui_connectorwindow.png)

これは接続設定ウィンドウです。ドロップダウンリスト（'+' ボタンで開きます）から必要なアダプターを選択し、右側にあるプロパティウィンドウでそれらのプロパティを設定する必要があります。

このウィンドウは、[Extensions.Configure](xref:StockSharp.Xaml.Extensions.Configure(StockSharp.Algo.Connector,System.Windows.Window))**(**[StockSharp.Algo.Connector](xref:StockSharp.Algo.Connector) connector, [System.Windows.Window](xref:System.Windows.Window) owner **)** 拡張メソッドを通じて呼び出す必要があります。このメソッドには [Connector](xref:StockSharp.Algo.Connector) と親ウィンドウを渡します。設定が成功した場合、[Extensions.Configure](xref:StockSharp.Xaml.Extensions.Configure(StockSharp.Algo.Connector,System.Windows.Window))**(**[StockSharp.Algo.Connector](xref:StockSharp.Algo.Connector) connector, [System.Windows.Window](xref:System.Windows.Window) owner **)** 拡張メソッドは 'true' を返します。以下は、コネクター接続設定ウィンドウを呼び出し、設定をファイルに保存するコードです。

```cs
		private void Setting_Click(object sender, RoutedEventArgs e)
		{
			if (_connector.Configure(this))
			{
				new JsonSerializer<SettingsStorage>().Serialize(_connector.Save(), _connectorFile);
			}
		}
	  				
```

> [!TIP]
> 接続の正確性は **確認** ボタンを使用して確認できます。

このウィンドウの結果として、[Connector.Adapter](xref:StockSharp.Algo.Connector.Adapter) プロパティの *内部アダプター* のリストにアダプターが作成され、追加されます。

コネクター設定の保存と読み込みの詳細については、[設定の保存と読み込み](../connectors/save_and_load_settings.md) を参照してください。
