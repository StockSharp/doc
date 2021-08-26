# Новости

[NewsGrid](../api/StockSharp.Xaml.NewsGrid.html) \- таблица для отображения новостей. 

**Основные свойства**

- [News](../api/StockSharp.Xaml.NewsGrid.News.html) \- список новостей.
- [FirstSelectedNews](../api/StockSharp.Xaml.NewsGrid.FirstSelectedNews.html) \- выбранная новость.
- [SelectedNews](../api/StockSharp.Xaml.NewsGrid.SelectedNews.html) \- выбранные новости.
- [NewsProvider](../api/StockSharp.Xaml.NewsGrid.NewsProvider.html) \- поставщик новостей.

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
