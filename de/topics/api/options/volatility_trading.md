# Volatility Trading

Für Optionsquoting ist eine spezielle Strategie [VolatilityQuotingStrategy](xref:StockSharp.Algo.Strategies.Derivatives.VolatilityQuotingStrategy) implementiert, die Volumenquoting innerhalb des angegebenen Volatilitätsbereichs bereitstellt.

## Quoting nach Volatilität

1. Das Installationspaket von [S#](../../api.md) enthält das Beispiel SampleOptionQuoting, das den ausgewählten Strike innerhalb des angegebenen Volatilitätsbereichs quotet.
2. Erstellen einer Verbindung zu [OpenECry](../connectors/stock_market/openecry.md) und Starten des Exports:

   ```cs
   private void InitConnector()
   {
   	// Ereignis für erfolgreiche Verbindung abonnieren
   	Connector.Connected += () =>
   	{
   		// GUI-Labels aktualisieren
   		this.GuiAsync(() => ChangeConnectStatus(true));
   	};
   	// Ereignis für Trennung abonnieren
   	Connector.Disconnected += () =>
   	{
   		// GUI-Labels aktualisieren
   		this.GuiAsync(() => ChangeConnectStatus(false));
   	};
   	// Ereignis für Verbindungsfehler abonnieren
   	Connector.ConnectionError += error => this.GuiAsync(() =>
   	{
   		// GUI-Labels aktualisieren
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
   	// Tickpreise abonnieren und Asset-Preis aktualisieren
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

3. Konfigurieren Sie die Strategie [VolatilityQuotingStrategy](xref:StockSharp.Algo.Strategies.Derivatives.VolatilityQuotingStrategy): Befüllen des Volatilitätsbereichs sowie Erstellen der Order, über die das erforderliche Volumen und die Quoting-Richtung angegeben werden:

   ```none
   private void StartClick(object sender, RoutedEventArgs e)
   {
   	var option = SelectedOption;
   	// DOM-Fenster erstellen
   	var wnd = new QuotesWindow { Title = option.Name };
   	wnd.Init(option);
   	// Delta-Hedge-Strategie erstellen
   	var hedge = new DeltaHedgeStrategy
   	{
   		Security = option.GetUnderlyingAsset(Connector),
   		Portfolio = Portfolio.SelectedPortfolio,
   		Connector = Connector,
   	};
   	// Optionsquoting für 20 Kontrakte erstellen
   	var quoting = new VolatilityQuotingStrategy(Sides.Buy, 20,
   			new Range<decimal>(ImpliedVolatilityMin.Value ?? 0, ImpliedVolatilityMax.Value ?? 100))
   	{
   		// Arbeitsgröße ist 1 Kontrakt
   		Volume = 1,
   		Security = option,
   		Portfolio = Portfolio.SelectedPortfolio,
   		Connector = Connector,
   	};
        // Quoting und Hedging verknüpfen
   	hedge.ChildStrategies.Add(quoting);
        // Hedging starten
   	hedge.Start();
   	wnd.Closed += (s1, e1) =>
   	{
   		// Alle Strategien zwangsweise schließen, während das DOM geschlossen wurde
   		hedge.Stop();
   	};
   	// DOM anzeigen
   	wnd.Show();
   }
   ```

4. Quoting starten:

   ```cs
   hedge.Start();
   ```

5. Für eine visuelle Darstellung der Volatilität zeigt das Beispiel, wie das Standard-Orderbuch mit Quotes mithilfe der Methode [DerivativesHelper.ImpliedVolatility](xref:StockSharp.Algo.Derivatives.DerivativesHelper.ImpliedVolatility(StockSharp.Messages.IOrderBookMessage,StockSharp.BusinessEntities.ISecurityProvider,StockSharp.BusinessEntities.IMarketDataProvider,StockSharp.BusinessEntities.IExchangeInfoProvider,System.DateTimeOffset,System.Decimal,System.Decimal))**(**[StockSharp.Messages.IOrderBookMessage](xref:StockSharp.Messages.IOrderBookMessage) depth, [StockSharp.BusinessEntities.ISecurityProvider](xref:StockSharp.BusinessEntities.ISecurityProvider) securityProvider, [StockSharp.BusinessEntities.IMarketDataProvider](xref:StockSharp.BusinessEntities.IMarketDataProvider) dataProvider, [StockSharp.BusinessEntities.IExchangeInfoProvider](xref:StockSharp.BusinessEntities.IExchangeInfoProvider) exchangeInfoProvider, [System.DateTimeOffset](xref:System.DateTimeOffset) currentTime, [System.Decimal](xref:System.Decimal) riskFree, [System.Decimal](xref:System.Decimal) dividend **)** in ein Volatilitäts-Orderbuch umgewandelt werden kann:

   ```cs
   private void OnQuotesChanged()
   {
       DepthCtrl.UpdateDepth(_depth.ImpliedVolatility(Connector, Connector, Connector.CurrentTime));
   }
   ```

   ![sample quote iv](../../../images/sample_quote_iv.png)

6. Quoting beenden und Strategie stoppen:

   ```none
   hedge.Stop();
   ```

## Empfohlene Inhalte

[Delta-Hedging](delta_hedging.md)

