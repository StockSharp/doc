# Picker

The [SecurityPicker](xref:StockSharp.Xaml.SecurityPicker) component is designed to find and select instruments. It supports both single and multiple choice. The component allows you to filter the list of instruments by their type. This component can also be used to display financial information (level1 fields), as shown in the [SecurityGrid](table.md) section. 

![GUI SecurityPicker2](../../../../images/gui_securitypicker2.png)

[SecurityPicker](xref:StockSharp.Xaml.SecurityPicker) consists of: 

1. A text field, to enter the code (or Id) of the instrument. When you enter, the list is filtered by the entered substring.
2. The special [SecurityTypeComboBox](xref:StockSharp.Xaml.SecurityTypeComboBox) combo box for filtering instruments by their type.
3. The [SecurityGrid](xref:StockSharp.Xaml.SecurityGrid) table to display the list of instruments.

**Main properties**

- [SecurityPicker.SelectionMode](xref:StockSharp.Xaml.SecurityPicker.SelectionMode) \- instrument selection mode: single, multiple.
- [SecurityPicker.ShowCommonStatColumns](xref:StockSharp.Xaml.SecurityPicker.ShowCommonStatColumns) \- to display the main columns.
- [SecurityPicker.ShowCommonOptionColumns](xref:StockSharp.Xaml.SecurityPicker.ShowCommonOptionColumns) \- to display the main columns for options.
- [SecurityPicker.Title](xref:StockSharp.Xaml.SecurityPicker.Title) \- the title that is displayed at the top of the component.
- [SecurityPicker.Securities](xref:StockSharp.Xaml.SecurityPicker.Securities) \- the list of instruments.
- [SecurityPicker.SelectedSecurity](xref:StockSharp.Xaml.SecurityPicker.SelectedSecurity) \- the selected instrument.
- [SecurityPicker.SelectedSecurities](xref:StockSharp.Xaml.SecurityPicker.SelectedSecurities) \- the list of selected instruments.
- [SecurityPicker.FilteredSecurities](xref:StockSharp.Xaml.SecurityPicker.FilteredSecurities) \- the list of filtered instruments.
- [SecurityPicker.ExcludeSecurities](xref:StockSharp.Xaml.SecurityPicker.ExcludeSecurities) \- the list of hidden instruments.
- [SecurityPicker.SelectedType](xref:StockSharp.Xaml.SecurityPicker.SelectedType) \- the selected instrument type.
- [SecurityPicker.SecurityProvider](xref:StockSharp.Xaml.SecurityPicker.SecurityProvider) \- the provider of information about instruments.
- [SecurityPicker.MarketDataProvider](xref:StockSharp.Xaml.SecurityPicker.MarketDataProvider) \- the provider of market data.

Below is the code snippet with its use, taken from example *Samples\/InteractiveBrokers\/SampleIB*. 

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
	_connector.NewSecurity += security => _securitiesWindow.SecurityPicker.Securities.Add(security);
	_securitiesWindow.SecurityPicker.MarketDataProvider = _connector;
	......................................
}
private void SecurityPicker_OnSecuritySelected(Security security)
{
	NewStopOrder.IsEnabled = NewOrder.IsEnabled =
	Level1.IsEnabled = Depth.IsEnabled = security != null;
}
```
