# Панель Купить\/Продать

[BuySellPanel](../api/StockSharp.Xaml.BuySellPanel.html) \- Специальная панель, дающая возможность быстро выставить заявку по лучшим ценам. 

![API GUI BuySell](~/images/API_GUI_BuySell.png)

Ниже приведен пример кода добавления панели [BuySellPanel](../api/StockSharp.Xaml.BuySellPanel.html). 

```xaml
\<Window x:Class\="MainWindow"
        xmlns\="http:\/\/schemas.microsoft.com\/winfx\/2006\/xaml\/presentation"
        xmlns:x\="http:\/\/schemas.microsoft.com\/winfx\/2006\/xaml"
        xmlns:d\="http:\/\/schemas.microsoft.com\/expression\/blend\/2008"
        xmlns:mc\="http:\/\/schemas.openxmlformats.org\/markup\-compatibility\/2006"
        xmlns:xaml\="http:\/\/schemas.stocksharp.com\/xaml"
        mc:Ignorable\="d"
        Title\="MainWindow" Height\="662" Width\="787" Left\="10" Top\="10"\>
    \<Grid\>
        \<Grid.ColumnDefinitions\>
            \<ColumnDefinition Width\="180"\/\>
            \<ColumnDefinition Width\="180"\/\>
            \<ColumnDefinition Width\="923\*"\/\>
        \<\/Grid.ColumnDefinitions\>
        \<Grid.RowDefinitions\>
            \<RowDefinition Height\="30"\/\>
            \<RowDefinition\/\>
        \<\/Grid.RowDefinitions\>
	    \<xaml:BuySellPanel x:Name\="BuySellPanel"  Grid.Row\="1" Grid.ColumnSpan\="3"\/\>
		\<Button Grid.Row\="0" Grid.Column\="0" x:Name\="Setting" Content\="Setting" Click\="Setting\_Click" \/\>
		\<Button Grid.Row\="0" Grid.Column\="1" x:Name\="Connect" Content\="Connect" Click\="Connect\_Click" \/\>
    \<\/Grid\>
\<\/Window\>
	  				
```

Для наполнения панели данными необходимо указать источник маркет\-данных и источник инструментов.

```cs
		private void Connect\_Click(object sender, RoutedEventArgs e)
		{
		    ...
			BuySellPanel.SecurityProvider \= \_connector;
			BuySellPanel.MarketDataProvider \= \_connector;
            ...
		}
	  				
```
