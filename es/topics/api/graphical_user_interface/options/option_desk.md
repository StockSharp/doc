# Tablero de opciones

El componente gráfico [OptionDesk](xref:StockSharp.Xaml.OptionDesk) es una tabla para mostrar el tablero de opciones. Muestra "griegas", volatilidad implícita, precio teórico, mejor oferta y mejor demanda para opciones Put y Call. 

A continuación se muestra el ejemplo **OptionCalculator**, que usa este componente. El código fuente del ejemplo se encuentra en la carpeta *Samples\/06\_Strategies\/09\_LiveOptionsQuoting*.

![Captura de Tablero de opciones](../../../../images/option_desk.png)

## Ejemplo OptionCalculator

1. En el código XAML, agregue el elemento [OptionDesk](xref:StockSharp.Xaml.OptionDesk) y asígnele el nombre **Desk**. 

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
2. En el código C#, cree una conexión y suscríbase a los eventos necesarios. 

   ```cs
   ...                 
   public readonly Connector Connector = new Connector();
   ...                 
   // suscribirse al evento de conexión correcta
   Connector.Connected += () =>
   {
   	// actualizar etiquetas de la GUI
   	this.GuiAsync(() => ChangeConnectStatus(true));
   };
   // suscribirse al evento de desconexión
   Connector.Disconnected += () =>
   {
   	// actualizar etiquetas de la GUI
   	this.GuiAsync(() => ChangeConnectStatus(false));
   };
   // suscribirse al evento de error de conexión
   Connector.ConnectionError += error => this.GuiAsync(() =>
   {
   	// actualizar etiquetas de la GUI
   	ChangeConnectStatus(false);
   	MessageBox.Show(this, error.ToString(), LocalizedStrings.ErrorConnection);
   });
   // llenar la lista de activos subyacentes
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
   // suscribirse a precios tick y actualizar el precio del activo
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
3. Al conectar, establezca el proveedor de mensajes para datos de mercado.

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
4. Al recibir instrumentos, agregamos los activos subyacentes a la lista.

   ```cs
   // llenar la lista de activos subyacentes
   Connector.SecurityReceived += (sub, security) =>
   {
   	if (security.Type == SecurityTypes.Future)
   		_assets.Add(security);
   };
   ```
5. Al seleccionar un instrumento:
   - Llenar el array con una cadena de opciones, donde el instrumento seleccionado actúa como activo subyacente;
   - Asignar este array a la propiedad [OptionDeskModel.Options](xref:StockSharp.Xaml.OptionDeskModel.Options);
   - Borrar los valores del tablero de opciones mediante el método [OptionDeskModel.Clear](xref:StockSharp.Xaml.OptionDeskModel.Clear).
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

## Contenido recomendado

[Griegas](../../options/greeks.md)
