# Simulator

Testing on market data is trading with a real connection to the exchange ("live" quotes), but without actual orders registering on the exchange. All the registered orders are intercepted, and their execution is emulated based on market order books. Such testing can be useful, for example, if trading simulator is developed. Or it is necessary to check the trading algorithm at short period of time with the real quotes. 

To emulate the trading on real data it is necessary to use [RealTimeEmulationTrader\<TUnderlyingMarketDataAdapter\>](xref:StockSharp.Algo.Testing.RealTimeEmulationTrader`1), which acts as a "wrapper" of the specific trading system connector ([OpenECry](../connectors/stock_market/openecry.md), [Rithmic](../connectors/stock_market/rithmic.md) etc.). Below is a description of an example of working with the emulator using the connection to [OpenECry](../connectors/stock_market/openecry.md). The example itself is in the *Samples\/Testing\/SampleRealTimeEmulation* folder. 

## Work with the trading emulator on real data

1. Creating the [RealTimeEmulationTrader\<TUnderlyingMarketDataAdapter\>](xref:StockSharp.Algo.Testing.RealTimeEmulationTrader`1) instance and passing to its constructor the [OpenECryMessageAdapter](xref:StockSharp.OpenECry.OpenECryMessageAdapter) adapter. To create identifiers of the "virtual" transactions using the **MillisecondIncrementalIdGenerator** identifier generator. 

   ```cs
   					_connector = new RealTimeEmulationTrader<SmartComMessageAdapter>(new SmartComMessageAdapter(new MillisecondIncrementalIdGenerator())
   					{
   						Login = Login.Text,
   						Password = Password.Password.To<SecureString>(),
   						Address = Address.SelectedAddress
   					});
   					  
   ```
2. The created connector is used as a usual connector. In our case, subscribing to events, passing an information to the graphical components and establishing the connection. 

   ```cs
   SecurityPicker.SecurityProvider = new FilterableSecurityProvider(_connector);
   SecurityPicker.MarketDataProvider = _connector;
   _logManager.Sources.Add(_connector);
   					
   _connector.Connected += () =>
   {
   	// update gui labels
   	this.GuiAsync(() => { ChangeConnectStatus(true); });
   };
   // subscribe on disconnection event
   _connector.Disconnected += () =>
   {
   	// update gui labels
   	this.GuiAsync(() => { ChangeConnectStatus(false); });
   };
   // subscribe on connection error event
   _connector.ConnectionError += error => this.GuiAsync(() =>
   {
   	// update gui labels
   	ChangeConnectStatus(false);
   	MessageBox.Show(this, error.ToString(), LocalizedStrings.Str2959);
   });
   _connector.NewMarketDepth += OnDepth;
   _connector.MarketDepthChanged += OnDepth;
   _connector.NewPortfolio += PortfolioGrid.Portfolios.Add;
   _connector.NewPosition += PortfolioGrid.Positions.Add;
   _connector.NewOrder += OrderGrid.Orders.Add;
   _connector.NewMyTrade += TradeGrid.Trades.Add;
   // subscribe on error of order registration event
   _connector.OrderRegisterFailed += OrderGrid.AddRegistrationFail;
   _connector.CandleSeriesProcessing += (s, candle) =>
   {
   	if (candle.State == CandleStates.Finished)
   		_buffer.Add(candle);
   };
   _connector.SubscribeCandles(series, DateTime.Today.Subtract(TimeSpan.FromDays(5)), DateTime.Now);	
   _connector.MassOrderCancelFailed += (transId, error) =>
   	this.GuiAsync(() => MessageBox.Show(this, error.ToString(), LocalizedStrings.Str716));
   // subscribe on error event
   _connector.Error += error =>
   	this.GuiAsync(() => MessageBox.Show(this, error.ToString(), LocalizedStrings.Str2955));
   // subscribe on error of market data subscription event
   _connector.MarketDataSubscriptionFailed += (security, msg, error) =>
   {
   	if (error == null)
   		return;
   	this.GuiAsync(() => MessageBox.Show(this, error.ToString(), LocalizedStrings.Str2956Params.Put(msg.DataType, security)));
   };
   					  
   ```
3. The following figure shows the result of the example work. ![sample realtaime emulation](../../../images/sample_realtaime_emulation.png)
