# Position chart

The graphic component [OptionPositionChart](xref:StockSharp.Xaml.Charting.OptionPositionChart) is the chart showing the position and the options “Greeks” relating to the underlying asset. 

The following is the example SampleOptionQuoting, in which this chart is used. The source code of the example can be found in the *Samples\/Options\/SampleOptionQuoting* folder. 

![option volsmile](../images/option_volsmile.png)

### SampleOptionQuoting example

1. In the XAML code, add the [OptionPositionChart](xref:StockSharp.Xaml.Charting.OptionPositionChart) element and assign it the **PosChart**.

   ```xaml
   <Window x:Class="OptionCalculator.MainWindow"
           xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
           xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
           xmlns:loc="clr-namespace:StockSharp.Localization;assembly=StockSharp.Localization"
           xmlns:xaml="http://schemas.stocksharp.com/xaml"
           xmlns:xctk="http://schemas.xceed.com/wpf/xaml/toolkit"
           Title="{x:Static loc:LocalizedStrings.XamlStr396}" Height="400" Width="1030">
       <Grid Margin="5,5,5,5">
       
   	    .........................................................
   	    
   	    <xaml:OptionPositionChart x:Name="PosChart" Grid.Row="7" Grid.Column="0" Grid.ColumnSpan="6" />
   	</Grid>
   </Window>
   				
   ```
2. In the C\# code, creating a connection, and subscribing to the necessary events.

   ```cs
   ...                 
   public readonly Connector Connector = new Connector();
   ...                 
   // subscribe on connection successfully event
   Connector.Connected += () =>
   {
   	// update gui labels
   	this.GuiAsync(() => ChangeConnectStatus(true));
   };
   // subscribe on disconnection event
   Connector.Disconnected += () =>
   {
   	// update gui labels
   	this.GuiAsync(() => ChangeConnectStatus(false));
   };
   // subscribe on connection error event
   Connector.ConnectionError += error => this.GuiAsync(() =>
   {
   	// update gui labels
   	ChangeConnectStatus(false);
   	MessageBox.Show(this, error.ToString(), LocalizedStrings.Str2959);
   });
   // fill underlying asset's list
   Connector.NewSecurity += security =>
   {
   	if (security.Type == SecurityTypes.Future)
   		_assets.Add(security);
   };
   Connector.SecurityChanged += security =>
   {
   	if (_model.UnderlyingAsset == security || _model.UnderlyingAsset.Id == security.UnderlyingSecurityId)
   		_isDirty = true;
   };
   // subscribing on tick prices and updating asset price
   Connector.NewTrade += trade =>
   {
   	if (_model.UnderlyingAsset == trade.Security || _model.UnderlyingAsset.Id == trade.Security.UnderlyingSecurityId)
   		_isDirty = true;
   };
   Connector.NewPosition += position => this.GuiAsync(() =>
   {
   	var asset = SelectedAsset;
   	if (asset == null)
   		return;
   	var assetPos = position.Security == asset;
   	var newPos = position.Security.UnderlyingSecurityId == asset.Id;
   	if (!assetPos && !newPos)
   		return;
   	RefreshChart();
   });
   Connector.PositionChanged += position => this.GuiAsync(() =>
   {
   	if ((PosChart.AssetPosition != null && PosChart.AssetPosition == position) || PosChart.Positions.Cache.Contains(position))
   		RefreshChart();
   });
   try
   {
   	if (File.Exists(_settingsFile))
   		Connector.Load(new XmlSerializer<SettingsStorage>().Deserialize(_settingsFile));
   }
   ...
   ```
3. When connecting, set the initial control settings:
   1. Resetting the the underlying instrument of the [UnderlyingAsset](xref:StockSharp.Xaml.Charting.OptionPositionChart.UnderlyingAsset) control; 
   2. Redrawing the chart with the initial values [Refresh](xref:StockSharp.Xaml.Charting.OptionPositionChart.Refresh); 
   3. Specifying message provider for market data and instruments.
   ```cs
   private void ConnectClick(object sender, RoutedEventArgs e)
   {
   	if (!_isConnected)
   	{
   		ConnectBtn.IsEnabled = false;
   ...
   		PosChart.UnderlyingAsset = null;
   ...
   		PosChart.MarketDataProvider = Connector;
   		PosChart.SecurityProvider = Connector;
   		PosChart.PositionProvider = Connector;
   		Connector.Connect();
   	}
   	else
   		Connector.Disconnect();
   }
   ```
4. When receiving instruments, we add the underlying assets to the list.

   ```cs
   Connector.NewSecurity += security =>
   {
   	if (security.Type == SecurityTypes.Future)
   		_assets.Add(security);
   };
   ```
5. Upon changing the Level1 of the underlying instrument or options, as well as getting a new trade we set the \_isDirty flag. This allows to call the RefreshChart method (see below) in the timer event (the code is omitted) to redraw the chart. Thus we control the frequency of redrawing.

   ```cs
   Connector.SecurityChanged += security =>
   {
   	if (_model.UnderlyingAsset == security || _model.UnderlyingAsset.Id == security.UnderlyingSecurityId)
   		_isDirty = true;
   };
   Connector.NewTrade += trade =>
   {
   	if (_model.UnderlyingAsset == trade.Security || _model.UnderlyingAsset.Id == trade.Security.UnderlyingSecurityId)
   		_isDirty = true;
   };
   ```
6. In the new position occurrence event handler we call redraw the chart.

   ```cs
   Connector.NewPosition += position => this.GuiAsync(() =>
   {
   	var asset = SelectedAsset;
   	if (asset == null)
   		return;
   	var assetPos = position.Security == asset;
   	var newPos = position.Security.UnderlyingSecurityId == asset.Id;
   	if (!assetPos && !newPos)
   		return;
   	RefreshChart();
   });
   Connector.PositionChanged += position => this.GuiAsync(() =>
   {
   	if ((PosChart.AssetPosition != null && PosChart.AssetPosition == position) || PosChart.Positions.Cache.Contains(position))
   		RefreshChart();
   });
   ```
7. The method calls the redraw of the chart:

   ```cs
   private void RefreshChart()
   {
   	var asset = SelectedAsset;
   	var trade = asset.LastTrade;
   	if (trade != null)
   		PosChart.Refresh(trade.Price);
   }
   ```

## Recommended content

[Volatility trading](OptionsQuoting.md)
