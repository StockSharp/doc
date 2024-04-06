# Котирование по волатильности

Для котирования опционов реализована специальная стратегия [VolatilityQuotingStrategy](xref:StockSharp.Algo.Strategies.Derivatives.VolatilityQuotingStrategy), которая предусматривает котирование объема по заданным границам волатильности. 

## Котирование по волатильности

1. В дистрибутиве [S\#](StockSharpAbout.md) идет пример SampleOptionQuoting, который котирует выбранный страйк по заданной границе волатильности. 
2. Создание подключения к [QUIK](Quik.md) и запуск экспорта: 

   ```cs
   private void InitConnector()
   {
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
3. Настройка стратегии [VolatilityQuotingStrategy](xref:StockSharp.Algo.Strategies.Derivatives.VolatilityQuotingStrategy) (заполнение границ волатильности, а также создание заявки, через которую указываются требуемый объем и направление котирования): 

   ```none
   private void StartClick(object sender, RoutedEventArgs e)
   {
   	var option = SelectedOption;
   	// create DOM window
   	var wnd = new QuotesWindow { Title = option.Name };
   	wnd.Init(option);
   	// create delta hedge strategy
   	var hedge = new DeltaHedgeStrategy
   	{
   		Security = option.GetUnderlyingAsset(Connector),
   		Portfolio = Portfolio.SelectedPortfolio,
   		Connector = Connector,
   	};
   	// create option quoting for 20 contracts
   	var quoting = new VolatilityQuotingStrategy(Sides.Buy, 20,
   			new Range<decimal>(ImpliedVolatilityMin.Value ?? 0, ImpliedVolatilityMax.Value ?? 100))
   	{
   		// working size is 1 contract
   		Volume = 1,
   		Security = option,
   		Portfolio = Portfolio.SelectedPortfolio,
   		Connector = Connector,
   	};
   	// link quoting and hending
   	hedge.ChildStrategies.Add(quoting);
   	// start henging
   	hedge.Start();
   	wnd.Closed += (s1, e1) =>
   	{
   		// force close all strategies while the DOM was closed
   		hedge.Stop();
   	};
   	// show DOM
   	wnd.Show();
   }
   ```
4. Запуск котирования: 

   ```cs
   hedge.Start();
   ```
5. Для визуального представления волатильности пример показывает, как можно перевести стандартный стакан с котировками в стакан волатильности за счет использования метода [DerivativesHelper.ImpliedVolatility](xref:StockSharp.Algo.Derivatives.DerivativesHelper.ImpliedVolatility(StockSharp.BusinessEntities.MarketDepth,StockSharp.BusinessEntities.ISecurityProvider,StockSharp.BusinessEntities.IMarketDataProvider,StockSharp.Algo.Storages.IExchangeInfoProvider,System.DateTimeOffset,System.Decimal,System.Decimal))**(**[StockSharp.BusinessEntities.MarketDepth](xref:StockSharp.BusinessEntities.MarketDepth) depth, [StockSharp.BusinessEntities.ISecurityProvider](xref:StockSharp.BusinessEntities.ISecurityProvider) securityProvider, [StockSharp.BusinessEntities.IMarketDataProvider](xref:StockSharp.BusinessEntities.IMarketDataProvider) dataProvider, [StockSharp.Algo.Storages.IExchangeInfoProvider](xref:StockSharp.Algo.Storages.IExchangeInfoProvider) exchangeInfoProvider, [System.DateTimeOffset](xref:System.DateTimeOffset) currentTime, [System.Decimal](xref:System.Decimal) riskFree, [System.Decimal](xref:System.Decimal) dividend **)**: 

   ```cs
   private void OnQuotesChanged()
   {
   	DepthCtrl.UpdateDepth(_depth.ImpliedVolatility(Connector, Connector, Connector.CurrentTime));
   }
   ```

   ![sample quote iv](../images/sample_quote_iv.png)
6. Окончание котирования и остановка стратегии: 

   ```none
   hedge.Stop();
   ```

## См. также

[Дельта\-хеджирование](OptionsHedge.md)
