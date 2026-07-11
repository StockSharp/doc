# 桌子

[PortfolioGrid](xref:StockSharp.Xaml.PortfolioGrid) 是一个显示投资组合和持仓状态的组件。

![桌子 截图](../../../../images/gui_portfoliogrid.png)

**主要属性**

- [PortfolioGrid.Positions](xref:StockSharp.Xaml.PortfolioGrid.Positions) – 持仓列表。
- [PortfolioGrid.SelectedPosition](xref:StockSharp.Xaml.PortfolioGrid.SelectedPosition) – 选定的位置。
- [PortfolioGrid.SelectedPositions](xref:StockSharp.Xaml.PortfolioGrid.SelectedPositions) - 已选择的位置。

以下代码片段演示了它的用法。代码示例取自 *Samples/InteractiveBrokers/SampleIB*。

```xaml
<Window x:Class="Sample.PortfoliosWindow"
		xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
		xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
		xmlns:loc="clr-namespace:StockSharp.Localization;assembly=StockSharp.Localization"
		xmlns:xaml="http://schemas.stocksharp.com/xaml"
		Title="{x:Static loc:LocalizedStrings.Portfolios}" Height="200" Width="470">
	<xaml:PortfolioGrid x:Name="PortfolioGrid" x:FieldModifier="public" />
</Window>
	  				
```
```cs
				  
private readonly Connector _connector = new Connector();
private void ConnectClick(object sender, RoutedEventArgs e)
{
	.........................................................				
	_connector.PositionReceived += (sub, p) => _portfoliosWindow.PortfolioGrid.Positions.TryAdd(position);
	.........................................................
}
	  				
```
