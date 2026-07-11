# Optionsdesk

Die grafische Komponente [OptionDesk](xref:StockSharp.Xaml.OptionDesk) ist eine Tabelle zur Anzeige des Optionsdesks. Sie zeigt die Optionsgriechen, die implizite Volatilität, den theoretischen Preis sowie das beste Brief- und Geldangebot für Put- und Call-Optionen.

Unten folgt das Beispiel **OptionCalculator**, das diese Komponente verwendet. Der Quellcode des Beispiels befindet sich im Ordner *Samples\/06\_Strategies\/09\_LiveOptionsQuoting*.

![option desk](../../../../images/option_desk.png)

## Beispiel OptionCalculator

1. Fügen Sie im XAML-Code das Element [OptionDesk](xref:StockSharp.Xaml.OptionDesk) hinzu und weisen Sie ihm den Namen **Desk** zu.

   ```xaml
   <Window x:Class="OptionCalculator.MainWindow"
           xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
           xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
           xmlns:loc="clr-namespace:StockSharp.Localization;assembly=StockSharp.Localization"
           xmlns:xaml="http://schemas.stocksharp.com/xaml"
           Title="{x:Static loc:LocalizedStrings.XamlStr396}" Height="400" Width="1030">
       <Grid Margin="5,5,5,5">

   	    .........................................................

   	    <xaml:OptionDesk x:Name="Desk" Grid.Row="6" Grid.ColumnSpan="3" Grid.Column="0" />

   	</Grid>
   </Window>

   ```
2. Erstellen Sie im C#-Code eine Verbindung und abonnieren Sie die erforderlichen Ereignisse.

   ```cs
   ...
   public readonly Connector Connector = new Connector();
   ...
   // Ereignis für erfolgreiche Verbindung abonnieren
   Connector.Connected += () =>
   {
   	// GUI-Beschriftungen aktualisieren
   	this.GuiAsync(() => ChangeConnectStatus(true));
   };
   // Ereignis für Trennung abonnieren
   Connector.Disconnected += () =>
   {
   	// GUI-Beschriftungen aktualisieren
   	this.GuiAsync(() => ChangeConnectStatus(false));
   };
   // Ereignis für Verbindungsfehler abonnieren
   Connector.ConnectionError += error => this.GuiAsync(() =>
   {
   	// GUI-Beschriftungen aktualisieren
   	ChangeConnectStatus(false);
   	MessageBox.Show(this, error.ToString(), LocalizedStrings.ErrorConnection);
   });
   // Liste der Basiswerte füllen
   Connector.SecurityReceived += (sub, security) =>
   {
   	if (security.Type == SecurityTypes.Future)
   		_assets.Add(security);
   };
   Connector.Level1Received += (sub, security) =>
   {
   	if (_model.UnderlyingAsset == security || _model.UnderlyingAsset.Id == security.UnderlyingSecurityId)
   		_isDirty = true;
   };
   // Tick-Preise abonnieren und Asset-Preis aktualisieren
   Connector.TickTradeReceived += (sub, trade) =>
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
   	if (assetPos)
   		PosChart.AssetPosition = position;
   	if (newPos)
   		PosChart.Positions.Add(position);
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
   		Connector.Load(new JsonSerializer<SettingsStorage>().Deserialize(_settingsFile));
   }
   ...
   ```
3. Legen Sie beim Verbinden den Nachrichtenprovider für Marktdaten fest.

   ```cs
   private void ConnectClick(object sender, RoutedEventArgs e)
   {
   	if (!_isConnected)
   	{
   		ConnectBtn.IsEnabled = false;
   		_model.Clear();
   		_model.MarketDataProvider = Connector;
   ...
   		Connector.Connect();
   	}
   	else
   		Connector.Disconnect();
   }
   ```
4. Beim Empfang von Instrumenten fügen wir die Basiswerte zur Liste hinzu.

   ```cs
   // Liste der Basiswerte füllen
   Connector.SecurityReceived += (sub, security) =>
   {
   	if (security.Type == SecurityTypes.Future)
   		_assets.Add(security);
   };
   ```
5. Beim Auswählen eines Instruments:
   - Füllen Sie das Array mit einer Optionskette, wobei das ausgewählte Instrument als Basiswert dient;
   - Weisen Sie dieses Array der Eigenschaft [OptionDeskModel.Options](xref:StockSharp.Xaml.OptionDeskModel.Options) zu;
   - Leeren Sie die Werte des Optionsboards mit der Methode [OptionDeskModel.Clear](xref:StockSharp.Xaml.OptionDeskModel.Clear).
   ```cs
   private void Assets_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
   {
   	var asset = SelectedAsset;
   	_model.UnderlyingAsset = asset;
   	_model.Clear();
   	_options.Clear();
   	var options = asset.GetDerivatives(Connector);
   	foreach (var security in options)
   	{
   		_model.Add(security);
   		_options.Add(security);
   	}
   	ProcessPositions();
   }
   ```

## Empfohlene Inhalte

[Optionsgriechen](../../options/greeks.md)
