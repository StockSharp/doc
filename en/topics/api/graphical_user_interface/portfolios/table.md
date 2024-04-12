# Table

[PortfolioGrid](xref:StockSharp.Xaml.PortfolioGrid) is a component that displays the status of portfolios and positions. 

![GUI PortfolioGrid](../../../../images/gui_portfoliogrid.png)

**Main properties**

- [PortfolioGrid.Portfolios](xref:StockSharp.Xaml.PortfolioGrid.Portfolios) – the list of portfolios.
- [PortfolioGrid.Positions](xref:StockSharp.Xaml.PortfolioGrid.Positions) – the list of positions.
- [PortfolioGrid.SelectedPosition](xref:StockSharp.Xaml.PortfolioGrid.SelectedPosition) – the selected position.
- [PortfolioGrid.SelectedPositions](xref:StockSharp.Xaml.PortfolioGrid.SelectedPositions) \- selected positions.

Below is the code snippet with its use. The code example is taken from *Samples\/InteractiveBrokers\/SampleIB.*

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
	_connector.NewPortfolio += portfolio => _portfoliosWindow.PortfolioGrid.Portfolios.Add(portfolio);
	_connector.NewPosition += position => _portfoliosWindow.PortfolioGrid.Positions.Add(position);
	.........................................................
}
	  				
```
