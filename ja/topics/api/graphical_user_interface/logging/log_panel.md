# ログパネル

[LogControl](xref:StockSharp.Xaml.LogControl) は、ログメッセージを表示するためのテーブルです。ツールバーのボタンを使用すると、異なるログレベルのメッセージをフィルタリングできます。

## LogControl

![GUI LogControl](../../../../images/gui_logcontrol.png)

サンプルコード

```xaml
<Window x:Class="LoggingControls.MainWindow"
		xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
		xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
		xmlns:sx="clr-namespace:StockSharp.Xaml;assembly=StockSharp.Xaml"
		Title="MainWindow" Height="350" Width="525">
	<Grid>
		<sx:LogControl x:Name="LogControl"/>
	</Grid>
</Window>
	  				
```
```cs
// LogManager の新しいインスタンスを作成します
_logManager = new LogManager();
// .NET トレースをログソースとして追加します。
_logManager.Sources.Add(new Ecng.Logging.TraceSource());
// LogControl をログリスナーとして追加します。
_logManager.Listeners.Add(new GuiLogListener(LogControl));
..........................                  
// TraceSource からテストメッセージを送信します:
Trace.TraceInformation("Info  test message");
Trace.TraceWarning("Warning test message");
Trace.TraceError("エラーテストメッセージ");
					
```
