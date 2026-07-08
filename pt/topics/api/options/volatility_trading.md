# Negociação de volatilidade

Para cotação de opções, está implementada uma estratégia especial [VolatilityQuotingStrategy](xref:StockSharp.Algo.Strategies.Derivatives.VolatilityQuotingStrategy), que fornece cotação de volume dentro do intervalo de volatilidade especificado.

## Cotação por volatilidade

1. O pacote de instalação do [S#](../../api.md) inclui o exemplo SampleOptionQuoting, que cota o strike selecionado dentro do intervalo de volatilidade especificado.
2. Criar uma ligação ao [OpenECry](../connectors/stock_market/openecry.md) e iniciar a exportação:

   ```cs
   private void InitConnector()
   {
   	// assinar o evento de conexão bem-sucedida
   	Connector.Connected += () =>
   	{
   		// atualizar rótulos da interface
   		this.GuiAsync(() => ChangeConnectStatus(true));
   	};
   	// assinar o evento de desconexão
   	Connector.Disconnected += () =>
   	{
   		// atualizar rótulos da interface
   		this.GuiAsync(() => ChangeConnectStatus(false));
   	};
   	// assinar o evento de erro de conexão
   	Connector.ConnectionError += error => this.GuiAsync(() =>
   	{
   		// atualizar rótulos da interface
   		ChangeConnectStatus(false);
   		MessageBox.Show(this, error.ToString(), LocalizedStrings.ErrorConnection);
   	});
   	// preencher a lista de ativos subjacentes
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
   	// assinatura de preços tick e atualização do preço do ativo
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

3. Configurar a estratégia [VolatilityQuotingStrategy](xref:StockSharp.Algo.Strategies.Derivatives.VolatilityQuotingStrategy) (preenchimento do intervalo de volatilidade, bem como criação da ordem através da qual são especificados o volume necessário e a direção da cotação):

   ```none
   private void StartClick(object sender, RoutedEventArgs e)
   {
   	var option = SelectedOption;
   	// criar janela DOM
   	var wnd = new QuotesWindow { Title = option.Name };
   	wnd.Init(option);
   	// criar estratégia de hedge delta
   	var hedge = new DeltaHedgeStrategy
   	{
   		Security = option.GetUnderlyingAsset(Connector),
   		Portfolio = Portfolio.SelectedPortfolio,
   		Connector = Connector,
   	};
   	// criar cotação de opção para 20 contratos
   	var quoting = new VolatilityQuotingStrategy(Sides.Buy, 20,
   			new Range<decimal>(ImpliedVolatilityMin.Value ?? 0, ImpliedVolatilityMax.Value ?? 100))
   	{
   		// tamanho de trabalho é 1 contrato
   		Volume = 1,
   		Security = option,
   		Portfolio = Portfolio.SelectedPortfolio,
   		Connector = Connector,
   	};
        // vincular cotação e hedge
   	hedge.ChildStrategies.Add(quoting);
        // iniciar hedge
   	hedge.Start();
   	wnd.Closed += (s1, e1) =>
   	{
   		// forçar fechamento de todas as estratégias quando o DOM for fechado
   		hedge.Stop();
   	};
   	// show DOM
   	wnd.Show();
   }
   ```

4. Iniciar a cotação:

   ```cs
   hedge.Start();
   ```

5. Para uma apresentação visual da volatilidade, o exemplo mostra como é possível converter o livro de ofertas padrão com cotações para um livro de ofertas de volatilidade através da utilização do método [DerivativesHelper.ImpliedVolatility](xref:StockSharp.Algo.Derivatives.DerivativesHelper.ImpliedVolatility(StockSharp.Messages.IOrderBookMessage,StockSharp.BusinessEntities.ISecurityProvider,StockSharp.BusinessEntities.IMarketDataProvider,StockSharp.BusinessEntities.IExchangeInfoProvider,System.DateTimeOffset,System.Decimal,System.Decimal))**(**[StockSharp.Messages.IOrderBookMessage](xref:StockSharp.Messages.IOrderBookMessage) depth, [StockSharp.BusinessEntities.ISecurityProvider](xref:StockSharp.BusinessEntities.ISecurityProvider) securityProvider, [StockSharp.BusinessEntities.IMarketDataProvider](xref:StockSharp.BusinessEntities.IMarketDataProvider) dataProvider, [StockSharp.BusinessEntities.IExchangeInfoProvider](xref:StockSharp.BusinessEntities.IExchangeInfoProvider) exchangeInfoProvider, [System.DateTimeOffset](xref:System.DateTimeOffset) currentTime, [System.Decimal](xref:System.Decimal) riskFree, [System.Decimal](xref:System.Decimal) dividend **)**:

   ```cs
   private void OnQuotesChanged()
   {
       DepthCtrl.UpdateDepth(_depth.ImpliedVolatility(Connector, Connector, Connector.CurrentTime));
   }
   ```

   ![sample quote iv](../../../images/sample_quote_iv.png)

6. Terminar a cotação e parar a estratégia:

   ```none
   hedge.Stop();
   ```

## Conteúdo recomendado

[Cobertura delta](delta_hedging.md)
