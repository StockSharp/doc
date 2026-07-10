# ボラティリティ取引

オプションのクォーティング向けに、指定されたボラティリティ範囲内で数量をクォートする特別な [VolatilityQuotingStrategy](xref:StockSharp.Algo.Strategies.Derivatives.VolatilityQuotingStrategy) 戦略が実装されています。

## ボラティリティによるクォーティング

1. [S#](../../api.md) インストールパッケージには、指定されたボラティリティ範囲内で選択したストライクをクォートする SampleOptionQuoting サンプルが含まれています。
2. [OpenECry](../connectors/stock_market/openecry.md) への接続を作成し、エクスポートを開始します。

   ```cs
   private void InitConnector()
   {
   	// 接続成功イベントにサブスクライブ
   	Connector.Connected += () =>
   	{
   		// GUI ラベルを更新
   		this.GuiAsync(() => ChangeConnectStatus(true));
   	};
   	// 切断イベントにサブスクライブ
   	Connector.Disconnected += () =>
   	{
   		// GUI ラベルを更新
   		this.GuiAsync(() => ChangeConnectStatus(false));
   	};
   	// 接続エラーイベントにサブスクライブ
   	Connector.ConnectionError += error => this.GuiAsync(() =>
   	{
   		// GUI ラベルを更新
   		ChangeConnectStatus(false);
   		MessageBox.Show(this, error.ToString(), LocalizedStrings.ErrorConnection);
   	});
   	// 原資産のリストを埋める
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
   	// ティック価格にサブスクライブし、資産価格を更新
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

3. [VolatilityQuotingStrategy](xref:StockSharp.Algo.Strategies.Derivatives.VolatilityQuotingStrategy) 戦略を設定します（ボラティリティ範囲を入力し、必要な数量とクォーティング方向を指定するための注文を作成します）。

   ```none
   private void StartClick(object sender, RoutedEventArgs e)
   {
   	var option = SelectedOption;
   	// DOM ウィンドウを作成
   	var wnd = new QuotesWindow { Title = option.Name };
   	wnd.Init(option);
   	// デルタヘッジ戦略を作成
   	var hedge = new DeltaHedgeStrategy
   	{
   		Security = option.GetUnderlyingAsset(Connector),
   		Portfolio = Portfolio.SelectedPortfolio,
   		Connector = Connector,
   	};
   	// 20 コントラクト分のオプションクォーティングを作成
   	var quoting = new VolatilityQuotingStrategy(Sides.Buy, 20,
   			new Range<decimal>(ImpliedVolatilityMin.Value ?? 0, ImpliedVolatilityMax.Value ?? 100))
   	{
   		// 稼働数量は 1 コントラクト
   		Volume = 1,
   		Security = option,
   		Portfolio = Portfolio.SelectedPortfolio,
   		Connector = Connector,
   	};
        // クォーティングとヘッジを関連付ける
   	hedge.ChildStrategies.Add(quoting);
        // ヘッジを開始
   	hedge.Start();
   	wnd.Closed += (s1, e1) =>
   	{
   		// DOM が閉じられたときにすべての戦略を強制終了
   		hedge.Stop();
   	};
   	// DOM を表示
   	wnd.Show();
   }
   ```

4. クォーティングを開始します。

   ```cs
   hedge.Start();
   ```

5. ボラティリティを視覚的に表示するため、このサンプルでは、[DerivativesHelper.ImpliedVolatility](xref:StockSharp.Algo.Derivatives.DerivativesHelper.ImpliedVolatility(StockSharp.Messages.IOrderBookMessage,StockSharp.BusinessEntities.ISecurityProvider,StockSharp.BusinessEntities.IMarketDataProvider,StockSharp.BusinessEntities.IExchangeInfoProvider,System.DateTimeOffset,System.Decimal,System.Decimal))**(**[StockSharp.Messages.IOrderBookMessage](xref:StockSharp.Messages.IOrderBookMessage) depth, [StockSharp.BusinessEntities.ISecurityProvider](xref:StockSharp.BusinessEntities.ISecurityProvider) securityProvider, [StockSharp.BusinessEntities.IMarketDataProvider](xref:StockSharp.BusinessEntities.IMarketDataProvider) dataProvider, [StockSharp.BusinessEntities.IExchangeInfoProvider](xref:StockSharp.BusinessEntities.IExchangeInfoProvider) exchangeInfoProvider, [System.DateTimeOffset](xref:System.DateTimeOffset) currentTime, [System.Decimal](xref:System.Decimal) riskFree, [System.Decimal](xref:System.Decimal) dividend **)** メソッドを使用して、クォートを含む標準の板情報をボラティリティの板情報に変換する方法を示しています。

   ```cs
   private void OnQuotesChanged()
   {
       DepthCtrl.UpdateDepth(_depth.ImpliedVolatility(Connector, Connector, Connector.CurrentTime));
   }
   ```

   ![IV クォートの例](../../../images/sample_quote_iv.png)

6. クォーティングを終了し、戦略を停止します。

   ```none
   hedge.Stop();
   ```

## 推奨コンテンツ

[デルタヘッジ](delta_hedging.md)
