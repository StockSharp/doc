# Пользовательский интерфейс (GUI)

## Графические компоненты S\#

В состав [S\#](../api.md) входит большое количество собственных графических компонент, которые размещены в пространствах имен [StockSharp.Xaml](xref:StockSharp.Xaml), [StockSharp.Xaml.Charting](xref:StockSharp.Xaml.Charting) и [StockSharp.Xaml.Diagram](xref:StockSharp.Xaml.Diagram). 

[S\#](../api.md) имеет различные контролы для: 

- поиска и выбора данных (инструментов, портфелей, адресов); 
- создания заявок; 
- отображения биржевой и другой информации (сделки, заявки, транзакции, стаканы, логи и т.д.);
- построения графиков.

Для доступа к графическим контролам [S\#](../api.md) в коде XAML необходимо определить псевдонимы для соответствующих пространств имен и использовать эти псевдонимы в коде XAML. Как это сделать показано в следующем примере: 

```xaml
<Window x:Class="SampleSmartSMA.MainWindow"
		xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
		xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
		xmlns:loc="clr-namespace:StockSharp.Localization;assembly=StockSharp.Localization"
		xmlns:sx="clr-namespace:StockSharp.Xaml;assembly=StockSharp.Xaml"
		xmlns:ss="clr-namespace:StockSharp.SmartCom.Xaml;assembly=StockSharp.SmartCom"
		xmlns:charting="http://schemas.stocksharp.com/xaml"
		Title="{x:Static loc:LocalizedStrings.XamlStr570}" Height="700" Width="900">
	
	<Grid>
	</Grid>
</Window>
	
```
