# Таблица портфелей

[PortfolioGrid](xref:StockSharp.Xaml.PortfolioGrid) \- компонент, отображающий состояние портфелей и позиций. 

![GUI PortfolioGrid](../../../../images/gui_portfoliogrid.png)

**Основные свойства**

- [PortfolioGrid.Positions](xref:StockSharp.Xaml.PortfolioGrid.Positions) \- список портфелей и позиций.
- [PortfolioGrid.SelectedPosition](xref:StockSharp.Xaml.PortfolioGrid.SelectedPosition) \- выбранная позиция.
- [PortfolioGrid.SelectedPositions](xref:StockSharp.Xaml.PortfolioGrid.SelectedPositions) \- выбранные позиции.

Ниже показан внешний вид компонента, а также фрагменты кода с его использованием. Пример кода взят из *Samples\/Common\/SampleConnection*.

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
