# Trading de volatilidad

Para la cotización de opciones, se implementa una estrategia especial [VolatilityQuotingStrategy](xref:StockSharp.Algo.Strategies.Derivatives.VolatilityQuotingStrategy), que proporciona cotización de volumen dentro del rango de volatilidad especificado.

## Cotización por volatilidad

1. El paquete de instalación de [S#](../../api.md) incluye el ejemplo SampleOptionQuoting, que cotiza el strike seleccionado dentro del rango de volatilidad especificado.
2. Creación de una conexión con [OpenECry](../connectors/stock_market/openecry.md) e inicio de la exportación:

   ```cs
   private void InitConnector()
   {
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
   	// suscripción a precios tick y actualización del precio del activo
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
   	catch
   	{
   	}
   }
   private void ConnectClick(object sender, RoutedEventArgs e)
   {
   	if (!_isConnected)
   	{
   		ConnectBtn.IsEnabled = false;
   		_model.Clear();
   		_model.MarketDataProvider = Connector;
   		ClearSmiles();
   		PosChart.Positions.Clear();
   		PosChart.AssetPosition = null;
   		PosChart.Refresh(1, 1, default(DateTimeOffset), default(DateTimeOffset));
   		Portfolio.Portfolios = new PortfolioDataSource(Connector);
   		PosChart.MarketDataProvider = Connector;
   		PosChart.SecurityProvider = Connector;
   		Connector.Connect();
   	}
   	else
   		Connector.Disconnect();
   }            		
   	  				
   ```

3. Configure la estrategia [VolatilityQuotingStrategy](xref:StockSharp.Algo.Strategies.Derivatives.VolatilityQuotingStrategy) (rellenando el rango de volatilidad, así como creando la orden mediante la cual se especifican el volumen requerido y la dirección de cotización): 

   ```none
   private void StartClick(object sender, RoutedEventArgs e)
   {
   	var option = SelectedOption;
   	// crear ventana DOM
   	var wnd = new QuotesWindow { Title = option.Name };
   	wnd.Init(option);
   	// crear estrategia de cobertura delta
   	var hedge = new DeltaHedgeStrategy
   	{
   		Security = option.GetUnderlyingAsset(Connector),
   		Portfolio = Portfolio.SelectedPortfolio,
   		Connector = Connector,
   	};
   	// crear cotización de opciones para 20 contratos
   	var quoting = new VolatilityQuotingStrategy(Sides.Buy, 20,
   			new Range<decimal>(ImpliedVolatilityMin.Value ?? 0, ImpliedVolatilityMax.Value ?? 100))
   	{
   		// tamaño de trabajo: 1 contrato
   		Volume = 1,
   		Security = option,
   		Portfolio = Portfolio.SelectedPortfolio,
   		Connector = Connector,
   	};
        // vincular cotización y cobertura
   	hedge.ChildStrategies.Add(quoting);
        // iniciar cobertura
   	hedge.Start();
   	wnd.Closed += (s1, e1) =>
   	{
   		// cerrar forzosamente todas las estrategias mientras el DOM esté cerrado
   		hedge.Stop();
   	};
   	// mostrar DOM
   	wnd.Show();
   }
   ```

4. Inicio de la cotización:

   ```cs
   hedge.Start();
   ```

5. Para una presentación visual de la volatilidad, el ejemplo muestra cómo puede convertir el libro de órdenes estándar con cotizaciones al libro de órdenes de volatilidad mediante el método [DerivativesHelper.ImpliedVolatility](xref:StockSharp.Algo.Derivatives.DerivativesHelper.ImpliedVolatility(StockSharp.Messages.IOrderBookMessage,StockSharp.BusinessEntities.ISecurityProvider,StockSharp.BusinessEntities.IMarketDataProvider,StockSharp.BusinessEntities.IExchangeInfoProvider,System.DateTimeOffset,System.Decimal,System.Decimal))**(**[StockSharp.Messages.IOrderBookMessage](xref:StockSharp.Messages.IOrderBookMessage) depth, [StockSharp.BusinessEntities.ISecurityProvider](xref:StockSharp.BusinessEntities.ISecurityProvider) securityProvider, [StockSharp.BusinessEntities.IMarketDataProvider](xref:StockSharp.BusinessEntities.IMarketDataProvider) dataProvider, [StockSharp.BusinessEntities.IExchangeInfoProvider](xref:StockSharp.BusinessEntities.IExchangeInfoProvider) exchangeInfoProvider, [System.DateTimeOffset](xref:System.DateTimeOffset) currentTime, [System.Decimal](xref:System.Decimal) riskFree, [System.Decimal](xref:System.Decimal) dividend **)**: 

   ```cs
   private void OnQuotesChanged()
   {
       DepthCtrl.UpdateDepth(_depth.ImpliedVolatility(Connector, Connector, Connector.CurrentTime));
   }
   ```

   ![sample quote iv](../../../images/sample_quote_iv.png)

6. Finalización de la cotización y detención de la estrategia:

   ```none
   hedge.Stop();
   ```

## Contenido recomendado

[Cobertura delta](delta_hedging.md)
