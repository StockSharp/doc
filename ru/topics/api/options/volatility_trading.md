# Котирование по волатильности

Для котирования опционов реализована специальная стратегия [VolatilityQuotingStrategy](xref:StockSharp.Algo.Strategies.Quoting.VolatilityQuotingStrategy), которая предусматривает котирование объема по заданным границам волатильности.

> [!WARNING]
> Класс [VolatilityQuotingStrategy](xref:StockSharp.Algo.Strategies.Quoting.VolatilityQuotingStrategy) помечен как `[Obsolete]`. Рекомендуется использовать [QuotingProcessor](xref:StockSharp.Algo.Strategies.Quoting.QuotingProcessor) вместо него.

## Котирование по волатильности

1. В дистрибутиве [S\#](../../api.md) идет пример SampleOptionQuoting, который котирует выбранный страйк по заданной границе волатильности. 
2. Создание подключения к [QUIK](../connectors/russia/quik.md) и запуск экспорта: 

   ```cs
   private void InitConnector()
   {
   	// подписаться на событие успешного подключения
   	Connector.Connected += () =>
   	{
   		// обновить надписи интерфейса
   		this.GuiAsync(() => ChangeConnectStatus(true));
   	};
   	// подписаться на событие отключения
   	Connector.Disconnected += () =>
   	{
   		// обновить надписи интерфейса
   		this.GuiAsync(() => ChangeConnectStatus(false));
   	};
   	// подписаться на событие ошибки подключения
   	Connector.ConnectionError += error => this.GuiAsync(() =>
   	{
   		// обновить надписи интерфейса
   		ChangeConnectStatus(false);
   		MessageBox.Show(this, error.ToString(), LocalizedStrings.ErrorConnection);
   	});
   	// заполнить список базовых активов
   	Connector.SecurityReceived += (sub, security) =>
   	{
   		if (security.Type == SecurityTypes.Future)
   			this.GuiAsync(() => _assets.TryAdd(security));

   		if (_model.UnderlyingAsset == security || _model.UnderlyingAsset.Id == security.UnderlyingSecurityId)
   			_isDirty = true;
   	};
   	// подписка на тиковые цены и обновление цены актива
   	Connector.TickTradeReceived += (sub, trade) =>
   	{
   		if (_model.UnderlyingAssetId == trade.SecurityId)
   			_isDirty = true;
   	};
   	Connector.PositionReceived += (sub, position) => this.GuiAsync(() =>
   	{
   		var asset = SelectedAsset;
   		if (asset == null)
   			return;
   		var assetPos = position.Security == asset;
   		var newPos = position.Security.UnderlyingSecurityId == asset.Id;
   		if (!assetPos && !newPos)
   			return;
   		if ((PosChart.Model != null && PosChart.Model.UnderlyingAsset == position.Security)
   			|| PosChart.Model.InnerModels.Any(m => m.Option == position.Security))
   			RefreshChart();
   	});
   	try
   	{
   		if (_settingsFile.IsConfigExists(_fileSystem))
   			Connector.LoadIfNotNull(_settingsFile.Deserialize<SettingsStorage>(_fileSystem));
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
   		PosChart.Model = null;
   		Portfolio.Portfolios = new PortfolioDataSource(Connector);
   		PosChart.Model = new BasketBlackScholes(Connector, Connector);
   		Connector.Connect();
   	}
   	else
   		Connector.Disconnect();
   }            		
   	  				
   ```
3. Настройка стратегии [VolatilityQuotingStrategy](xref:StockSharp.Algo.Strategies.Quoting.VolatilityQuotingStrategy) (заполнение границ волатильности, а также создание заявки, через которую указываются требуемый объем и направление котирования):

   ```cs
   private void StartClick(object sender, RoutedEventArgs e)
   {
   	var option = SelectedOption;
   	// создать окно DOM
   	var wnd = new QuotesWindow { Title = option.Name };
   	// create delta hedge strategy (requires BasketBlackScholes model)
   	var hedge = new DeltaHedgeStrategy(PosChart.Model)
   	{
   		Security = option.GetUnderlyingAsset(Connector),
   		Portfolio = Portfolio.SelectedPortfolio,
   		Connector = Connector,
   	};
   	// создать котирование опциона на 20 контрактов
   	var quoting = new VolatilityQuotingStrategy
   	{
   		QuotingSide = Sides.Buy,
   		QuotingVolume = 20,
   		IVRange = new Range<decimal>((decimal?)ImpliedVolatilityMin.EditValue ?? 0, (decimal?)ImpliedVolatilityMax.EditValue ?? 100),
   		// рабочий объём равен 1 контракту
   		Volume = 1,
   		Security = option,
   		Portfolio = Portfolio.SelectedPortfolio,
   		Connector = Connector,
   	};
   	// связать котирование и хеджирование
   	hedge.ChildStrategies.Add(quoting);
   	// запустить хеджирование
   	hedge.Start();
   	wnd.Closed += (s1, e1) =>
   	{
   		// принудительно закрыть все стратегии при закрытии DOM
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
5. Для визуального представления волатильности пример показывает, как можно перевести стандартный стакан с котировками в стакан волатильности за счет использования метода [DerivativesHelper.ImpliedVolatility](xref:StockSharp.Algo.Derivatives.DerivativesHelper.ImpliedVolatility(StockSharp.Messages.IOrderBookMessage,StockSharp.BusinessEntities.ISecurityProvider,StockSharp.BusinessEntities.IMarketDataProvider,System.DateTime,System.Decimal,System.Decimal))**(**[StockSharp.Messages.IOrderBookMessage](xref:StockSharp.Messages.IOrderBookMessage) depth, [StockSharp.BusinessEntities.ISecurityProvider](xref:StockSharp.BusinessEntities.ISecurityProvider) securityProvider, [StockSharp.BusinessEntities.IMarketDataProvider](xref:StockSharp.BusinessEntities.IMarketDataProvider) dataProvider, [System.DateTime](xref:System.DateTime) currentTime, [System.Decimal](xref:System.Decimal) riskFree, [System.Decimal](xref:System.Decimal) dividend **)**:

   ```cs
   private void TryUpdateDepth(Subscription subscription, IOrderBookMessage depth)
   {
   	wnd.Update(depth.ImpliedVolatility(Connector, Connector, depth.ServerTime));
   }
   ```

   ![sample quote iv](../../../images/sample_quote_iv.png)
6. Окончание котирования и остановка стратегии: 

   ```none
   hedge.Stop();
   ```

## См. также

[Дельта\-хеджирование](delta_hedging.md)
