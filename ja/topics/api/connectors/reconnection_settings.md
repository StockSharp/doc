# 再接続設定

すべてのコネクターでは、切断時に再接続を設定できます。[接続設定ウィンドウ](../graphical_user_interface/connection_settings_window.md) のグラフィック要素では、次のように表示されます。

![API GUI ReconnectionSettings](../../../images/api_gui_reconnectionsettings.png)

**再接続プロパティ**

- **間隔** - 接続試行が行われる間隔。
- **初期試行回数** - 初回接続が確立されなかった場合（タイムアウト、ネットワーク障害など）に、初回接続を確立する試行回数。
- **再接続** - 動作中に接続が切断された場合に、再接続を試行する回数。
- **タイムアウト** - 接続\/切断が成功するまでのタイムアウト。
- **動作モード** - 接続試行を行うべき動作モード。

## コードでの再接続設定

再接続機構は [ReConnectionSettings](xref:StockSharp.Messages.IMessageAdapter.ReConnectionSettings) プロパティを通じて設定され、次のエラーシナリオを監視できます。

- 接続を確立できない（通信不可、ユーザー名\/パスワードの誤りなど）。[ReConnectionSettings.AttemptCount](xref:StockSharp.Messages.ReConnectionSettings.AttemptCount) プロパティは、接続を確立する試行回数を設定します。既定値は 0 で、これはモードが無効であることを意味します。-1 は無限回の試行を意味します。
- 動作中に接続が切断された。[ReConnectionSettings.ReAttemptCount](xref:StockSharp.Messages.ReConnectionSettings.ReAttemptCount) プロパティは、接続を再接続する試行回数を設定します。既定値は 100 です。-1 は無限回の試行を意味します。0 はモードが無効であることを意味します。
- 接続または切断を行う際、対応する [IConnector.Connected](xref:StockSharp.BusinessEntities.IConnector.Connected) または [IConnector.Disconnected](xref:StockSharp.BusinessEntities.IConnector.Disconnected) イベントが長時間受信されない場合があります。このような状況では、[ReConnectionSettings.TimeOutInterval](xref:StockSharp.Messages.ReConnectionSettings.TimeOutInterval) プロパティを使用して、成功イベントに対する最大許容タイムアウトを設定できます。この時間が経過しても目的のイベントが発生しない場合、[IConnector.ConnectionError](xref:StockSharp.BusinessEntities.IConnector.ConnectionError) イベントがタイムアウトエラー付きで発生します。

1. ゲートウェイを作成する際は、[ReConnectionSettings](xref:StockSharp.Messages.IMessageAdapter.ReConnectionSettings) プロパティを通じて再接続機構の設定を初期化する必要があります。

   ```cs
   // 再接続機構を初期化する (ゲートウェイがサーバーとの接続を失った場合、
   // 10 秒ごとに自動的に接続する)
   Connector.Adapter.ReConnectionSettings.Interval = TimeSpan.FromSeconds(10);
   // 再接続は、選択したボードの取引時間中にのみ動作する
   // (通常取引がない時間帯、たとえば夜間に再接続を無効にするため)
   Connector.Adapter.ReConnectionSettings.WorkingTime = ExchangeBoard.Nasdaq.WorkingTime;
   ```
2. 接続制御機構の動作を確認するには、インターネット接続をオフにできます。

   ![transactions](../../../images/transactions.png)
3. 下記はプログラムログです。アプリケーションが最初は接続済み状態であり、インターネット接続をオフにした後、アプリケーションが再接続を試行していることを示しています。インターネット接続を復元すると、アプリケーションの接続も復元されます。

   ![API ReconnectionLog](../../../images/api_reconnectionlog.png)
4. [Connector](xref:StockSharp.Algo.Connector) では複数の接続を使用できるため、既定では [ConnectionRestored](xref:StockSharp.Algo.Connector.ConnectionRestored) などの再接続に関連するイベントはトリガーされず、接続アダプターが自分自身で再接続を試行します。このイベントを発生させるには、アダプターの [BasketMessageAdapter.SuppressReconnectingErrors](xref:StockSharp.Algo.BasketMessageAdapter.SuppressReconnectingErrors) プロパティの値を **false** に設定する必要があります。

   ```cs
   Connector.Adapter.SuppressReconnectingErrors = false;
   Connector.ConnectionError += error => this.Sync(() => MessageBox.Show(this, "接続が失われました"));
   Connector.ConnectionRestored += adapter => this.Sync(() => MessageBox.Show(this, "接続が復元されました"));
   ```

   ![接続エラーの例](../../../images/sample_connection_error.png)![接続復旧の例](../../../images/sample_connection_restored.png)

