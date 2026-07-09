# 表格

[SecurityGrid](xref:StockSharp.Xaml.SecurityGrid) 组件用于以表格形式显示与金融工具相关的财务信息（一级字段）及其变动。该组件允许您选择一个或多个工具。

![GUI SecurityPicker2](../../../../images/gui_securitypicker2.png)

**主要属性**

- [SecurityGrid.Securities](xref:StockSharp.Xaml.SecurityGrid.Securities) - 交易品种列表。
- [SecurityGrid.SelectedSecurity](xref:StockSharp.Xaml.SecurityGrid.SelectedSecurity) - 所选交易品种。
- [SecurityGrid.SelectedSecurities](xref:StockSharp.Xaml.SecurityGrid.SelectedSecurities) - 所选交易品种列表。
- [SecurityGrid.MarketDataProvider](xref:StockSharp.Xaml.SecurityGrid.MarketDataProvider) - 市场数据提供商。

请注意，对于市场信息变化的显示，您必须指定市场数据提供商。

以下是其使用的代码示例。

在图中，[SecurityGrid](xref:StockSharp.Xaml.SecurityGrid) 组件显示在[选择器](picker.md)图形组件中。

```xaml
<Window x:Class="SecurityGridSample.MainWindow"
		xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
		xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
		xmlns:sx="clr-namespace:StockSharp.Xaml;assembly=StockSharp.Xaml"
		Title="MainWindow" Height="350" Width="525">
	<Grid>
		<sx:SecurityGrid x:Name="SecurityGrid"/>
	</Grid>
</Window>
	  				
```
```cs
private readonly Connector _connector = new Connector();
SecurityGrid.MarketDataProvider = _connector;
..........................
_connector.SecurityReceived += (sub, security) =>
{
	SecurityGrid.Securities.Add(security);
};
..........................
private void ColumnsFilter()
{
	string[]  columns = { "Board", "BestAsk.Price", "BestAsk.Volume" };
	
	foreach (var column in SecurityGrid.Columns)
	{
		column.Visibility = columns.Contains(column.SortMemberPath) ? Visibility.Visible : Visibility.Collapsed;
	}
}
				
```
