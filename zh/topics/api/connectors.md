# 连接器

在 [S#](../api.md) 中处理交易所和数据源时，建议使用基类 [Connector](xref:StockSharp.Algo.Connector)。

让我们来看一下使用 [Connector](xref:StockSharp.Algo.Connector) 的方法。示例的源代码可以在 Samples/01_Basic/01_ConnectAndDownloadInstruments 项目中找到。

![multiconnection main](../../images/multiconnection_main.png)

创建 [Connector](xref:StockSharp.Algo.Connector) 类的实例：

```cs
...
public Connector Connector;
...
public MainWindow()
{
	InitializeComponent();
	Connector = new Connector();
	InitConnector();
}
		
```

要配置 [Connector](xref:StockSharp.Algo.Connector)，**API** 提供了一个特殊的图形界面，允许您同时配置多个连接。如何使用它在 [Graphical Configuration](connectors/graphical_configuration.md) 一节中有说明。

```cs
...
private const string _connectorFile = "ConnectorFile.json";
...
private void Setting_Click(object sender, RoutedEventArgs e)
{
	if (Connector.Configure(this))
	{
		Connector.Save().Serialize(_connectorFile);
	}
}
	  				
```

![API 图形界面连接窗口](../../images/api_gui_connectorwindow.png)

同样，你可以通过使用扩展方法直接从代码（无需图形窗口）添加连接[TraderHelper.AddAdapter"<TAdapter\>](xref:StockSharp.Algo.TraderHelper.AddAdapter``1(StockSharp.Algo.Connector,System.Action{``0}))**(**[StockSharp.算法.连接器](xref:StockSharp.Algo.Connector)连接器，[System.Action<TAdapter\>](xref:System.Action`1)初始化 **)**:

```cs
...
// Add adapter for connecting to Binance
connector.AddAdapter<BinanceMessageAdapter>(a => 
{
	a.Key = "<Your API Key>";
	a.Secret = "<Your Secret Key>";
});

// Add RSS for news
connector.AddAdapter<RssMessageAdapter>(a => 
{
	a.Address = "https://news-source.com/feed";
	a.IsEnabled = true;
});
	  				
```

您可以向单个 [Connector](xref:StockSharp.Algo.Connector) 对象添加无限数量的连接。因此，您可以从程序同时连接到多个交易所和经纪商。

在 *InitConnector* 方法中，我们为 [IConnector](xref:StockSharp.BusinessEntities.IConnector) 设置了所需的事件处理程序:

```cs
private void InitConnector()
{
	// Subscribe to successful connection event
	Connector.Connected += () =>
	{
		this.GuiAsync(() => ChangeConnectStatus(true));
	};
	
	// Subscribe to connection error event
	Connector.ConnectionError += error => this.GuiAsync(() =>
	{
		ChangeConnectStatus(false);
		MessageBox.Show(this, error.ToString(), LocalizedStrings.ErrorConnection);
	});
	
	// Subscribe to disconnection event
	Connector.Disconnected += () => this.GuiAsync(() => ChangeConnectStatus(false));
	
	// Subscribe to error event
	Connector.Error += error =>
		this.GuiAsync(() => MessageBox.Show(this, error.ToString(), LocalizedStrings.Str2955));
	
	// Subscribe to market data subscription failure event
	Connector.SubscriptionFailed += (subscription, error) =>
		this.GuiAsync(() => MessageBox.Show(this, error.ToString(), 
			LocalizedStrings.Str2956Params.Put(subscription.DataType, subscription.SecurityId)));
	
	// Subscriptions for data reception
	
	// Instruments
	Connector.SecurityReceived += (sub, security) => _securitiesWindow.SecurityPicker.Securities.Add(security);
	
	// Tick trades
	Connector.TickTradeReceived += (sub, trade) => _tradesWindow.TradeGrid.Trades.TryAdd(trade);
	
	// Orders
	Connector.OrderReceived += (sub, order) => _ordersWindow.OrderGrid.Orders.TryAdd(order);
	
	// Own trades
	Connector.OwnTradeReceived += (sub, trade) => _myTradesWindow.TradeGrid.Trades.TryAdd(trade);
	
	// Positions
	Connector.PositionReceived += (sub, position) => _portfoliosWindow.PortfolioGrid.Positions.TryAdd(position);

	// Order registration failures
	Connector.OrderRegisterFailReceived += (sub, fail) => _ordersWindow.OrderGrid.AddRegistrationFail(fail);
	
	// Order cancellation failures
	Connector.OrderCancelFailReceived += (sub, fail) => OrderFailed(fail);
	
	// Set market data provider
	_securitiesWindow.SecurityPicker.MarketDataProvider = Connector;
	
	try
	{
		if (File.Exists(_connectorFile))
		{
			var ctx = new ContinueOnExceptionContext();
			ctx.Error += ex => ex.LogError();
			using (new Scope<ContinueOnExceptionContext>(ctx))
				Connector.Load(_connectorFile.Deserialize<SettingsStorage>());
		}
	}
	catch
	{
	}
	
	ConfigManager.RegisterService<IExchangeInfoProvider>(new InMemoryExchangeInfoProvider());
	
	// Register adapter provider for graphical configuration
	ConfigManager.RegisterService<IMessageAdapterProvider>(
		new FullInMemoryMessageAdapterProvider(Connector.Adapter.InnerAdapters));
}
```

有关如何将 [Connector](xref:StockSharp.Algo.Connector) 的设置保存到文件和从文件加载的内容，请参见 [Saving and Loading Settings](connectors/save_and_load_settings.md) 部分。

有关创建您自己的 [Connector](xref:StockSharp.Algo.Connector) 的信息，请参见 [Creating Your Own Connector](connectors/creating_own_connector.md) 部分。

下单操作描述如下部分：[Orders](orders_management.md)、[Creating a New Order](orders_management/create_new_order.md)、[Creating a New Stop Order](orders_management/create_new_stop_order.md)。

## 另请参阅

[图形配置](connectors/graphical_configuration.md)