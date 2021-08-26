# Таблица редактирования свойств объектов

[PropertyGridEx](../api/StockSharp.Xaml.PropertyGrid.PropertyGridEx.html) \- таблица для редактирования свойств объектов. В состав компонента входит набор дополнительных редакторов для системных типов и типов [S\#](StockSharpAbout.md). 

![GUI PropertyDataGridEx](../images/GUI_PropertyDataGridEx.png)

[PropertyGridEx](../api/StockSharp.Xaml.PropertyGrid.PropertyGridEx.html) имеет собственные редакторы для следующих типов: 

- [StockSharp.Messages.Unit](../api/StockSharp.Messages.Unit.html). 
- [StockSharp.BusinessEntities.Security](../api/StockSharp.BusinessEntities.Security.html). 
- [StockSharp.BusinessEntities.Portfolio](../api/StockSharp.BusinessEntities.Portfolio.html). 
- [StockSharp.BusinessEntities.ExchangeBoard](../api/StockSharp.BusinessEntities.ExchangeBoard.html). 
- [StockSharp.BusinessEntities.Exchange](../api/StockSharp.BusinessEntities.Exchange.html). 
- [StockSharp.Algo.Candles.CandleSeries](../api/StockSharp.Algo.Candles.CandleSeries.html). 
- Словарь **ExtensionInfo**. 
- [System.TimeSpan](../api/System.TimeSpan.html), [System.DateTime](../api/System.DateTime.html) и [System.DateTimeOffset](../api/System.DateTimeOffset.html). 
- [System.Net.EndPoint](../api/System.Net.EndPoint.html) и [System.Net.IPAddress](../api/System.Net.IPAddress.html). 
- [System.Security.SecureString](../api/System.Security.SecureString.html). 
- [System.Text.Encoding](../api/System.Text.Encoding.html). 
- [System.Enum](../api/System.Enum.html). 

**Основные свойства**

- [SecurityProvider](../api/StockSharp.Xaml.PropertyGrid.PropertyGridEx.SecurityProvider.html) \- поставщик информации об инструментах. 
- [ExchangeInfoProvider](../api/StockSharp.Xaml.PropertyGrid.PropertyGridEx.ExchangeInfoProvider.html) \- поставщик информации о площадках. 
- [Portfolios](../api/StockSharp.Xaml.PropertyGrid.PropertyGridEx.Portfolios.html) \- список доступных портфелей. 
- **SelectedObject** \- объект, чьи свойства будут отображены в таблице. 

Ниже показаны фрагменты кода с его использованием. Пример кода взят из *Samples\/Common\/SampleConnection*. 

```xaml
\<Window x:Class\="SampleFix.MainWindow"
		xmlns\="http:\/\/schemas.microsoft.com\/winfx\/2006\/xaml\/presentation"
		xmlns:x\="http:\/\/schemas.microsoft.com\/winfx\/2006\/xaml"
		xmlns:loc\="clr\-namespace:StockSharp.Localization;assembly\=StockSharp.Localization"
		xmlns:xctk\="http:\/\/schemas.xceed.com\/wpf\/xaml\/toolkit"
        xmlns:propertyGrid\="http:\/\/schemas.stocksharp.com\/xaml"
        Title\="{x:Static loc:LocalizedStrings.XamlStr540}" Height\="110" Width\="512"\>
	\<Grid\>
		\<Grid.ColumnDefinitions\>
			\<ColumnDefinition \/\>
			\<ColumnDefinition \/\>
			\<ColumnDefinition \/\>
			\<ColumnDefinition \/\>
			\<ColumnDefinition \/\>
		\<\/Grid.ColumnDefinitions\>
		\<Grid.RowDefinitions\>
			\<RowDefinition Height\="24" \/\>
			\<RowDefinition Height\="Auto" \/\>
			\<RowDefinition Height\="Auto" \/\>
		\<\/Grid.RowDefinitions\>
		\<StackPanel Grid.Column\="0" Grid.Row\="0" Grid.ColumnSpan\="5" Orientation\="Horizontal"\>
			\<xctk:DropDownButton Content\="{x:Static loc:LocalizedStrings.TransactionalSession}"\>
				\<xctk:DropDownButton.DropDownContent\>
					\<propertyGrid:PropertyGridEx x:Name\="TransactionSessionSettings" \/\>
				\<\/xctk:DropDownButton.DropDownContent\>
			\<\/xctk:DropDownButton\>
			\<xctk:DropDownButton Content\="{x:Static loc:LocalizedStrings.MarketDataSession}"\>
				\<xctk:DropDownButton.DropDownContent\>
					\<propertyGrid:PropertyGridEx x:Name\="MarketDataSessionSettings" \/\>
				\<\/xctk:DropDownButton.DropDownContent\>
			\<\/xctk:DropDownButton\>
		\<\/StackPanel\>
		
		\<Button x:Name\="ConnectBtn" Background\="LightPink" Grid.Column\="0" Grid.Row\="1" Grid.RowSpan\="2" Content\="{x:Static loc:LocalizedStrings.Connect}" Click\="ConnectClick" \/\>
		\<Button x:Name\="ShowSecurities" Grid.Column\="1" Grid.Row\="1" IsEnabled\="False" Content\="{x:Static loc:LocalizedStrings.Securities}" Click\="ShowSecuritiesClick" \/\>
		\<Button x:Name\="ShowPortfolios" Grid.Column\="2" Grid.Row\="1" IsEnabled\="False" Content\="{x:Static loc:LocalizedStrings.Portfolios}" Click\="ShowPortfoliosClick" \/\>
		\<Button x:Name\="ShowStopOrders" Grid.Column\="3" Grid.Row\="1" IsEnabled\="False" Content\="{x:Static loc:LocalizedStrings.StopOrders}" Click\="ShowStopOrdersClick" \/\>
		\<Button x:Name\="ShowNews" Grid.Column\="4" Grid.Row\="1" IsEnabled\="False" Content\="{x:Static loc:LocalizedStrings.News}" Click\="ShowNewsClick" \/\>
		
		\<Button x:Name\="ShowTrades" Grid.Column\="1" Grid.Row\="2" IsEnabled\="False" Content\="{x:Static loc:LocalizedStrings.Ticks}" Click\="ShowTradesClick" \/\>
		\<Button x:Name\="ShowMyTrades" Grid.Column\="2" Grid.Row\="2" IsEnabled\="False" Content\="{x:Static loc:LocalizedStrings.MyTrades}" Click\="ShowMyTradesClick" \/\>
		\<Button x:Name\="ShowOrders" Grid.Column\="3" Grid.Row\="2" IsEnabled\="False" Content\="{x:Static loc:LocalizedStrings.Orders}" Click\="ShowOrdersClick" \/\>
	\<\/Grid\>
\<\/Window\>
	  				
```
```cs
private readonly Connector \_connector \= new Connector();
public MainWindow()
{
	InitializeComponent();
	Title \= Title.Put("FIX");
	\_ordersWindow.MakeHideable();
	\_myTradesWindow.MakeHideable();
	\_tradesWindow.MakeHideable();
	\_securitiesWindow.MakeHideable();
	\_stopOrdersWindow.MakeHideable();
	\_portfoliosWindow.MakeHideable();
	\_newsWindow.MakeHideable();
	if (File.Exists(\_settingsFile))
	{
		\_connector\_connector.Load(new XmlSerializer\<SettingsStorage\>().Deserialize(\_settingsFile));
	}
	MarketDataSessionSettings.SelectedObject \= ((ChannelMessageAdapter)\_connector.MarketDataAdapter).InnerAdapter;
	TransactionSessionSettings.SelectedObject \= ((ChannelMessageAdapter)\_connector.TransactionAdapter).InnerAdapter;
	Instance \= this;
	\_connector.LogLevel \= LogLevels.Debug;
	\_logManager.Sources.Add(\_connector);
	\_logManager.Listeners.Add(new FileLogListener { LogDirectory \= "StockSharp\_Fix" });
}
	  				
```
