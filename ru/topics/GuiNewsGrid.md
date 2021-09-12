# Новости

[NewsGrid](xref:StockSharp.Xaml.NewsGrid) \- таблица для отображения новостей. 

**Основные свойства**

- [NewsGrid.News](xref:StockSharp.Xaml.NewsGrid.News) \- список новостей.
- [NewsGrid.FirstSelectedNews](xref:StockSharp.Xaml.NewsGrid.FirstSelectedNews) \- выбранная новость.
- [NewsGrid.SelectedNews](xref:StockSharp.Xaml.NewsGrid.SelectedNews) \- выбранные новости.
- [NewsGrid.NewsProvider](xref:StockSharp.Xaml.NewsGrid.NewsProvider) \- поставщик новостей.

Ниже показаны фрагменты кода с его использованием. Пример кода взят из *Samples\/Common\/SampleConnection*. 

```xaml
<Window	x:Class="SampleAlfa.NewsWindow"
		xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
		xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
		xmlns:loc="clr-namespace:StockSharp.Localization;assembly=StockSharp.Localization"
		xmlns:xaml="http://schemas.stocksharp.com/xaml"
		Title="{x:Static loc:LocalizedStrings.News}" Height="300" Width="1050">
	    <xaml:NewsPanel x:Name="NewsPanel"/>
</Window>
	  				
```
```cs
					  
private readonly Connector _connector = new Connector();
private void ConnectClick(object sender, RoutedEventArgs e)
{
	.................................................
	// устанавливаем поставщика новостей
	_newsWindow.NewsPanel.NewsProvider = _connector;
	// добавляет новости в сетку NewsGrid
	_connector.NewNews += news => _newsWindow.NewsPanel.NewsGrid.News.Add(news);
	.................................................
}
	  				
```
