# Доска опционов

Графический компонент [OptionDesk](../api/StockSharp.Xaml.OptionDesk.html) \- таблица для отображения доски опционов. Показывает "греки", вмененную волатильность, теоретическую цену, лучший оффер и бид для Put и Call опционов. 

Ниже показан пример OptionCalculator, в котором используется этот компонент. Исходные коды примера можно найти в папке *Samples\/Misc\/SampleOptionQuoting*. 

![option desk](~/images/option_desk.png)

### Пример OptionCalculator

Пример OptionCalculator

1. В коде XAML добавляем элемент [OptionDesk](../api/StockSharp.Xaml.OptionDesk.html) и присваиваем ему имя **Desk**.

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
2. В коде C\# создаем подключение и подписываемся на необходимые события. 

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
3. При подключении задаем провайдера сообщений для рыночных данных.

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
4. При получении инструментов добавляем базовые активы в список.

   ```cs
   \/\/ fill underlying asset's list
   Connector.NewSecurity +\= security \=\>
   {
   	if (security.Type \=\= SecurityTypes.Future)
   		\_assets.Add(security);
   };
   ```
5. При выборе инструмента:
   - Заполняем массив цепочкой опционов, где подлежащим активом выступает выбранный инструмент;
   - Присваиваем свойству 

     [OptionDeskModel.Options](../api/StockSharp.Xaml.OptionDeskModel.Options.html)

      этот массив;
   - Очищаем значения доски опционов при помощи метода 

     [OptionDeskModel.Clear](../api/StockSharp.Xaml.OptionDeskModel.Clear.html)

     .
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

## См. также

[Греки](OptionsGreeks.md)
