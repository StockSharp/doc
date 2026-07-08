# ピッカー

[SecurityPicker](xref:StockSharp.Xaml.SecurityPicker) コンポーネントは、銘柄を検索して選択するために設計されています。単一選択と複数選択の両方をサポートします。このコンポーネントでは、銘柄の種類で銘柄リストをフィルターできます。また、[SecurityGrid](table.md) セクションに示すように、金融情報（level1 フィールド）の表示にも使用できます。

![GUI SecurityPicker2](../../../../images/gui_securitypicker2.png)

[SecurityPicker](xref:StockSharp.Xaml.SecurityPicker) は次の要素で構成されます。

1. 銘柄のコード（または Id）を入力するためのテキストフィールド。入力すると、入力された部分文字列でリストがフィルターされます。
2. 銘柄の種類で銘柄をフィルターするための特殊な [SecurityTypeComboBox](xref:StockSharp.Xaml.SecurityTypeComboBox) コンボボックス。
3. 銘柄リストを表示するための [SecurityGrid](xref:StockSharp.Xaml.SecurityGrid) テーブル。

**主なプロパティ**

- [SecurityPicker.SelectionMode](xref:StockSharp.Xaml.SecurityPicker.SelectionMode) - 銘柄選択モード: 単一、複数。
- [SecurityPicker.ShowCommonStatColumns](xref:StockSharp.Xaml.SecurityPicker.ShowCommonStatColumns) - 主な列を表示します。
- [SecurityPicker.ShowCommonOptionColumns](xref:StockSharp.Xaml.SecurityPicker.ShowCommonOptionColumns) - オプション用の主な列を表示します。
- [SecurityPicker.Title](xref:StockSharp.Xaml.SecurityPicker.Title) - コンポーネントの上部に表示されるタイトル。
- [SecurityPicker.Securities](xref:StockSharp.Xaml.SecurityPicker.Securities) - 銘柄のリスト。
- [SecurityPicker.SelectedSecurity](xref:StockSharp.Xaml.SecurityPicker.SelectedSecurity) - 選択された銘柄。
- [SecurityPicker.SelectedSecurities](xref:StockSharp.Xaml.SecurityPicker.SelectedSecurities) - 選択された銘柄のリスト。
- [SecurityPicker.FilteredSecurities](xref:StockSharp.Xaml.SecurityPicker.FilteredSecurities) - フィルターされた銘柄のリスト。
- [SecurityPicker.ExcludeSecurities](xref:StockSharp.Xaml.SecurityPicker.ExcludeSecurities) - 非表示の銘柄のリスト。
- [SecurityPicker.SelectedType](xref:StockSharp.Xaml.SecurityPicker.SelectedType) - 選択された銘柄の種類。
- [SecurityPicker.SecurityProvider](xref:StockSharp.Xaml.SecurityPicker.SecurityProvider) - 銘柄に関する情報のプロバイダー。
- [SecurityPicker.MarketDataProvider](xref:StockSharp.Xaml.SecurityPicker.MarketDataProvider) - マーケットデータのプロバイダー。

以下は、*Samples\/InteractiveBrokers\/SampleIB* の例から取得した使用例のコードスニペットです。

```xaml
<Window x:Class="Sample.SecuritiesWindow"
	xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
	xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
	xmlns:loc="clr-namespace:StockSharp.Localization;assembly=StockSharp.Localization"
	xmlns:xaml="http://schemas.stocksharp.com/xaml"
	Title="{x:Static loc:LocalizedStrings.Securities}" Height="415" Width="1081">
	<Grid>
		<Grid.RowDefinitions>
			<RowDefinition Height="*" />
			<RowDefinition Height="Auto" />
		</Grid.RowDefinitions>
		<xaml:SecurityPicker x:Name="SecurityPicker" x:FieldModifier="public" SecuritySelected="SecurityPicker_OnSecuritySelected" ShowCommonStatColumns="True" />
	</Grid>
</Window>
	  	
```
```cs
private void ConnectClick(object sender, RoutedEventArgs e)
{
	......................................
	_connector.SecurityReceived += (sub, security) => _securitiesWindow.SecurityPicker.Securities.Add(security);
	_securitiesWindow.SecurityPicker.MarketDataProvider = _connector;
	......................................
}
private void SecurityPicker_OnSecuritySelected(Security security)
{
	NewStopOrder.IsEnabled = NewOrder.IsEnabled =
	Level1.IsEnabled = Depth.IsEnabled = security != null;
}
```
