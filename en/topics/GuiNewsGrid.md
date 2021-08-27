# News

[NewsGrid](xref:StockSharp.Xaml.NewsGrid) \- the table to display news. 

**Main properties**

- [News](xref:StockSharp.Xaml.NewsGrid.News) \- the news list.
- [FirstSelectedNews](xref:StockSharp.Xaml.NewsGrid.FirstSelectedNews) \- the selected news.
- [SelectedNews](xref:StockSharp.Xaml.NewsGrid.SelectedNews) \- selected news.
- [NewsProvider](xref:StockSharp.Xaml.NewsGrid.NewsProvider) \- news provider.

Below is the appearance of the component, as well as code snippets with its use. The code example is taken from Samples\/AlfaDirect\/SampleAlfa. 

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
	
	_newsWindow.NewsPanel.NewsProvider = _connector;
	
	_connector.NewNews += news => _newsWindow.NewsPanel.NewsGrid.News.Add(news);
	.................................................
}
	  				
```
