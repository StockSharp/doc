# Kauf-/Verkaufspanel

[BuySellPanel](xref:StockSharp.Xaml.BuySellPanel) - ein spezielles Panel, mit dem Sie schnell eine Order zu den besten Preisen registrieren können.

![API GUI BuySell](../../../../images/api_gui_buysell.png)

Unten sehen Sie ein Codebeispiel zum Hinzufügen von [BuySellPanel](xref:StockSharp.Xaml.BuySellPanel).

```xaml
<Window x:Class="MainWindow"
		xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
		xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
		xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
		xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
		xmlns:xaml="http://schemas.stocksharp.com/xaml"
		mc:Ignorable="d"
		Title="MainWindow" Height="662" Width="787" Left="10" Top="10">
	<Grid>
		<Grid.ColumnDefinitions>
			<ColumnDefinition Width="180"/>
			<ColumnDefinition Width="180"/>
			<ColumnDefinition Width="923*"/>
		</Grid.ColumnDefinitions>
		<Grid.RowDefinitions>
			<RowDefinition Height="30"/>
			<RowDefinition/>
		</Grid.RowDefinitions>
	    <xaml:BuySellPanel x:Name="BuySellPanel"  Grid.Row="1" Grid.ColumnSpan="3"/>
		<Button Grid.Row="0" Grid.Column="0" x:Name="Setting" Content="Setting" Click="Setting_Click" />
		<Button Grid.Row="0" Grid.Column="1" x:Name="Connect" Content="Connect" Click="Connect_Click" />
	</Grid>
</Window>

```

Um das Panel mit Daten zu füllen, müssen Sie die Marktdatenquelle und die Instrumentenquelle angeben.

```cs
		private void Connect_Click(object sender, RoutedEventArgs e)
		{
		    ...
			BuySellPanel.SecurityProvider = _connector;
			BuySellPanel.MarketDataProvider = _connector;
			...
		}

```
