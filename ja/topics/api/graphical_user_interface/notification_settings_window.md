# 通知設定ウィンドウ

[AlertSettingsWindow](xref:StockSharp.Alerts.AlertSettingsWindow) は、特定のイベントに対する通知を設定するためのウィンドウです。

![API GUI AlertWindow](../../../images/api_gui_alertwindow.png)

次のデータ型の変更に関する通知を設定できます: Portfolio、Client code、Broker、Depository、Server time、Transaction、Data type、Cancel、Order ID、Order ID (string)、Order ID (platform)、Derivative、Derivative (string)、Price、Volume (order)、Volume (trade)、Visible volume、Direction、Balance、Order type、Status、Comment、Order message、System order、Order expiration time、Execution condition、Price、Trade initiator、Open interest、Error、Condition、Uptrend、Commission、Delay、Slippage、Identifier (user)、Currency、P\/L、Position、Market maker。

通知は次の形式にできます。

- **Window** - メッセージ付きの小さなポップアップウィンドウが画面の隅に表示されます。
- **Melody** - メロディが再生されます。
- **SMS** - SMS でメッセージが送信されます。
- **Email** - email でメッセージが送信されます。
- **Speech** - コンピューター生成音声でメッセージが読み上げられます。
- **Log** - メッセージが Designer Logs ウィンドウに送信されます。
- **Disabled** - 通知は表示されません。
