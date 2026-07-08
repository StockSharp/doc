# 波动率交易

对于期权报价，实施了一种特殊的[VolatilityQuotingStrategy](xref:StockSharp.Algo.Strategies.Derivatives.VolatilityQuotingStrategy)策略，该策略在指定的波动率范围内提供成交量报价。

## 按波动率报价

1. [S#](../../api.md) 安装包包括示例 SampleOptionQuoting，该示例在指定的波动率范围内对选定的执行价进行报价。
2. 正在创建到 [OpenECry](../connectors/stock_market/openecry.md) 的连接并开始导出：

   ```cs
   private void InitConnector()
   {
   	// 订阅连接成功事件
   	Connector.Connected += () =>
   	{
   		// 更新界面标签
   		this.GuiAsync(() => ChangeConnectStatus(true));
   	};
   	// 订阅断开连接事件
   	Connector.Disconnected += () =>
   	{
   		// 更新界面标签
   		this.GuiAsync(() => ChangeConnectStatus(false));
   	};
   	// 订阅连接错误事件
   	Connector.ConnectionError += error => this.GuiAsync(() =>
   	{
   		// 更新界面标签
   		ChangeConnectStatus(false);
   		MessageBox.Show(this, error.ToString(), LocalizedStrings.ErrorConnection);
   	});
   	// 填充标的资产列表
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
   	// 订阅 tick 价格并更新资产价格
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

3. 设置[VolatilityQuotingStrategy](xref:StockSharp.Algo.Strategies.Derivatives.VolatilityQuotingStrategy) 策略（填写波动率范围，以及创建订单，通过该订单指定所需的数量和报价方向）：

   ```none
   private void StartClick(object sender, RoutedEventArgs e)
   {
   	var option = SelectedOption;
   	// 创建 DOM 窗口
   	var wnd = new QuotesWindow { Title = option.Name };
   	wnd.Init(option);
   	// 创建 delta 对冲策略
   	var hedge = new DeltaHedgeStrategy
   	{
   		Security = option.GetUnderlyingAsset(Connector),
   		Portfolio = Portfolio.SelectedPortfolio,
   		Connector = Connector,
   	};
   	// 为 20 份合约创建期权报价
   	var quoting = new VolatilityQuotingStrategy(Sides.Buy, 20,
   			new Range<decimal>(ImpliedVolatilityMin.Value ?? 0, ImpliedVolatilityMax.Value ?? 100))
   	{
   		// 工作数量为 1 份合约
   		Volume = 1,
   		Security = option,
   		Portfolio = Portfolio.SelectedPortfolio,
   		Connector = Connector,
   	};
        // 关联报价和对冲
   	hedge.ChildStrategies.Add(quoting);
        // 启动对冲
   	hedge.Start();
   	wnd.Closed += (s1, e1) =>
   	{
   		// DOM 关闭时强制关闭所有策略
   		hedge.Stop();
   	};
   	// show DOM
   	wnd.Show();
   }
   ```

4. 开始引用：

   ```cs
   hedge.Start();
   ```

5. 为了直观展示波动性，示例展示了如何通过使用 [DerivativesHelper.ImpliedVolatility](xref:StockSharp.Algo.Derivatives.DerivativesHelper.ImpliedVolatility(StockSharp.Messages.IOrderBookMessage,StockSharp.BusinessEntities.ISecurityProvider,StockSharp.BusinessEntities.IMarketDataProvider,StockSharp.BusinessEntities.IExchangeInfoProvider,System.DateTimeOffset,System.Decimal,System.Decimal))**(**[StockSharp.Messages.IOrderBookMessage](xref:StockSharp.Messages.IOrderBookMessage) 深度, [StockSharp.BusinessEntities.ISecurityProvider](xref:StockSharp.BusinessEntities.ISecurityProvider) 交易品种提供者, [StockSharp.BusinessEntities.IMarketDataProvider](xref:StockSharp.BusinessEntities.IMarketDataProvider) 数据提供者, [StockSharp.BusinessEntities.IExchangeInfoProvider](xref:StockSharp.BusinessEntities.IExchangeInfoProvider) 交易所信息提供者, [System.DateTimeOffset](xref:System.DateTimeOffset) 当前时间, [System.Decimal](xref:System.Decimal) 无风险利率, [System.Decimal](xref:System.Decimal) 股息**) **方法将标准报价订单簿转换为波动性订单簿：

   ```cs
   private void OnQuotesChanged()
   {
       DepthCtrl.UpdateDepth(_depth.ImpliedVolatility(Connector, Connector, Connector.CurrentTime));
   }
   ```

   ![示例引用 iv](../../../images/sample_quote_iv.png)

6. 结束引用并停止策略：

   ```none
   hedge.Stop();
   ```

## 推荐内容

[Delta对冲](delta_hedging.md)
