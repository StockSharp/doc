# Option desk

The [OptionDesk](../api/StockSharp.Xaml.OptionDesk.html)\- graphic component is the table for option desk display. It shows “Greeks”, implied volatility, the theoretical price, the best offer and bid for Put and Call options. 

Below is the **OptionCalculator** example, which uses this component. The source code of the example can be found in the *Samples\\Options\\SampleOptionQuoting* folder.

![option desk](../images/option_desk.png)

### OptionCalculator example

OptionCalculator example

1. In the XAML code, adding the [OptionDesk](../api/StockSharp.Xaml.OptionDesk.html) element and assigning it the **Desk** name. 

   ```xaml
   \<Window x:Class\="OptionCalculator.MainWindow"
           xmlns\="http:\/\/schemas.microsoft.com\/winfx\/2006\/xaml\/presentation"
           xmlns:x\="http:\/\/schemas.microsoft.com\/winfx\/2006\/xaml"
           xmlns:loc\="clr\-namespace:StockSharp.Localization;assembly\=StockSharp.Localization"
           xmlns:xaml\="http:\/\/schemas.stocksharp.com\/xaml"
           xmlns:xctk\="http:\/\/schemas.xceed.com\/wpf\/xaml\/toolkit"
           Title\="{x:Static loc:LocalizedStrings.XamlStr396}" Height\="400" Width\="1030"\>
       \<Grid Margin\="5,5,5,5"\>
       
   	    .........................................................
   	    
   	    \<xaml:OptionDesk x:Name\="Desk" Grid.Row\="6" Grid.ColumnSpan\="3" Grid.Column\="0" \/\>
       
   	\<\/Grid\>
   \<\/Window\>
   	  				
   ```
2. In the C\# code, creating a connection and subscribing to the necessary events. 

   ```cs
   ...                 
   public readonly Connector Connector \= new Connector();
   ...                 
   \/\/ subscribe on connection successfully event
   Connector.Connected +\= () \=\>
   {
   	\/\/ update gui labels
   	this.GuiAsync(() \=\> ChangeConnectStatus(true));
   };
   \/\/ subscribe on disconnection event
   Connector.Disconnected +\= () \=\>
   {
   	\/\/ update gui labels
   	this.GuiAsync(() \=\> ChangeConnectStatus(false));
   };
   \/\/ subscribe on connection error event
   Connector.ConnectionError +\= error \=\> this.GuiAsync(() \=\>
   {
   	\/\/ update gui labels
   	ChangeConnectStatus(false);
   	MessageBox.Show(this, error.ToString(), LocalizedStrings.Str2959);
   });
   \/\/ fill underlying asset's list
   Connector.NewSecurity +\= security \=\>
   {
   	if (security.Type \=\= SecurityTypes.Future)
   		\_assets.Add(security);
   };
   Connector.SecurityChanged +\= security \=\>
   {
   	if (\_model.UnderlyingAsset \=\= security \|\| \_model.UnderlyingAsset.Id \=\= security.UnderlyingSecurityId)
   		\_isDirty \= true;
   };
   \/\/ subscribing on tick prices and updating asset price
   Connector.NewTrade +\= trade \=\>
   {
   	if (\_model.UnderlyingAsset \=\= trade.Security \|\| \_model.UnderlyingAsset.Id \=\= trade.Security.UnderlyingSecurityId)
   		\_isDirty \= true;
   };
   Connector.NewPosition +\= position \=\> this.GuiAsync(() \=\>
   {
   	var asset \= SelectedAsset;
   	if (asset \=\= null)
   		return;
   	var assetPos \= position.Security \=\= asset;
   	var newPos \= position.Security.UnderlyingSecurityId \=\= asset.Id;
   	if (\!assetPos && \!newPos)
   		return;
   	if (assetPos)
   		PosChart.AssetPosition \= position;
   	if (newPos)
   		PosChart.Positions.Add(position);
   	RefreshChart();
   });
   Connector.PositionChanged +\= position \=\> this.GuiAsync(() \=\>
   {
   	if ((PosChart.AssetPosition \!\= null && PosChart.AssetPosition \=\= position) \|\| PosChart.Positions.Cache.Contains(position))
   		RefreshChart();
   });
   try
   {
   	if (File.Exists(\_settingsFile))
   		Connector.Load(new XmlSerializer\<SettingsStorage\>().Deserialize(\_settingsFile));
   }
   ...
   ```
3. When connecting, set the message provider for market data.

   ```cs
   private void ConnectClick(object sender, RoutedEventArgs e)
   {
   	if (\!\_isConnected)
   	{
   		ConnectBtn.IsEnabled \= false;
   		\_model.Clear();
   		\_model.MarketDataProvider \= Connector;
   ...
   		Connector.Connect();
   	}
   	else
   		Connector.Disconnect();
   }
   ```
4. When receiving instruments, we add the underlying assets to the list.

   ```cs
   \/\/ fill underlying asset's list
   Connector.NewSecurity +\= security \=\>
   {
   	if (security.Type \=\= SecurityTypes.Future)
   		\_assets.Add(security);
   };
   ```
5. When selecting an instrument:
   - Fill the array with a chain of options, where the selected instrument acts as the underlying asset;
   - Assign this array to the 

     [OptionDeskModel.Options](../api/StockSharp.Xaml.OptionDeskModel.Options.html)

      property;
   - • Clear the options board values using the 

     [OptionDeskModel.Clear](../api/StockSharp.Xaml.OptionDeskModel.Clear.html)

      method.
   ```cs
   private void Assets\_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
   {
   	var asset \= SelectedAsset;
   	\_model.UnderlyingAsset \= asset;
   	\_model.Clear();
   	\_options.Clear();
   	var options \= asset.GetDerivatives(Connector);
   	foreach (var security in options)
   	{
   		\_model.Add(security);
   		\_options.Add(security);
   	}
   	ProcessPositions();
   }
   ```

## Recommended content

[Greeks](OptionsGreeks.md)
