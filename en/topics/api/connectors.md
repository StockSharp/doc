# Connectors

To work with exchanges and data sources in [S\#](../api.md) it is recommended to use the base class [Connector](xref:StockSharp.Algo.Connector). 

Let's consider working with [Connector](xref:StockSharp.Algo.Connector). The example source codes are in the project Samples\/Common\/SampleConnection.

![multiconnection main](../../images/multiconnection_main.png)

We create an instance of the [Connector](xref:StockSharp.Algo.Connector) class:

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

To configure [Connector](xref:StockSharp.Algo.Connector)**API** has a special graphical interface in which you can configure several connections at once. How to use it see [Graphical configuration](connectors/graphical_configuration.md). 

```cs
...
private const string _connectorFile = "ConnectorFile.json";
...
private void Setting_Click(object sender, RoutedEventArgs e)
{
	if (Connector.Configure(this))
	{
		new JsonSerializer<SettingsStorage>().Serialize(Connector.Save(), _connectorFile);
	}
}
	  				
```

![API GUI ConnectorWindow](../../images/api_gui_connectorwindow.png)

Similarly, you can add connections directly from the code (without graphic windows) using the extension method [TraderHelper.AddAdapter\<TAdapter\>](xref:StockSharp.Algo.TraderHelper.AddAdapter``1(StockSharp.Algo.Connector,System.Action{``0}))**(**[StockSharp.Algo.Connector](xref:StockSharp.Algo.Connector) connector, [System.Action\<TAdapter\>](xref:System.Action`1) init **)**:

```cs
...
connector.AddAdapter<BarChartMessageAdapter>(a => { });
```

You can add an unlimited number of connections to a single [Connector](xref:StockSharp.Algo.Connector) object. Therefore, from the program you can simultaneously connect to several exchanges and brokers at once.

In the *InitConnector* method, we set the required [IConnector](xref:StockSharp.BusinessEntities.IConnector) event handlers:

```cs
private void InitConnector()
{
	// subscribe on connection successfully event
	Connector.Connected += () =>
	{
		this.GuiAsync(() => ChangeConnectStatus(true));
	};
	// subscribe on connection error event
	Connector.ConnectionError += error => this.GuiAsync(() =>
	{
		ChangeConnectStatus(false);
		MessageBox.Show(this, error.ToString(), LocalizedStrings.Str2959);
	});
	Connector.Disconnected += () => this.GuiAsync(() => ChangeConnectStatus(false));
	// subscribe on error event
	Connector.Error += error =>
		this.GuiAsync(() => MessageBox.Show(this, error.ToString(), LocalizedStrings.Str2955));
	// subscribe on error of market data subscription event
	Connector.MarketDataSubscriptionFailed += (security, msg, error) =>
		this.GuiAsync(() => MessageBox.Show(this, error.ToString(), LocalizedStrings.Str2956Params.Put(msg.DataType, security)))
	Connector.NewSecurity += _securitiesWindow.SecurityPicker.Securities.Add;
	Connector.NewTrade += _tradesWindow.TradeGrid.Trades.Add;
	Connector.NewOrder += _ordersWindow.OrderGrid.Orders.Add;
	Connector.NewStopOrder += _stopOrdersWindow.OrderGrid.Orders.Add;
	Connector.NewMyTrade += _myTradesWindow.TradeGrid.Trades.Add;
	
	Connector.PositionReceived += (sub, p) => _portfoliosWindow.PortfolioGrid.Positions.TryAdd(p);

	// subscribe on error of order registration event
	Connector.OrderRegisterFailed += _ordersWindow.OrderGrid.AddRegistrationFail;
	// subscribe on error of order cancelling event
	Connector.OrderCancelFailed += OrderFailed;
	// subscribe on error of stop-order registration event
	Connector.OrderRegisterFailed += _stopOrdersWindow.OrderGrid.AddRegistrationFail;
	// subscribe on error of stop-order cancelling event
	Connector.StopOrderCancelFailed += OrderFailed;
	// set market data provider
	_securitiesWindow.SecurityPicker.MarketDataProvider = Connector;
	try
	{
		if (File.Exists(_settingsFile))
		{
			var ctx = new ContinueOnExceptionContext();
			ctx.Error += ex => ex.LogError();
			using (new Scope<ContinueOnExceptionContext> (ctx))
				Connector.Load(new JsonSerializer<SettingsStorage>().Deserialize(_settingsFile));
		}
	}
	catch
	{
	}
	ConfigManager.RegisterService<IExchangeInfoProvider>(new InMemoryExchangeInfoProvider());
	
	// needed for graphical settings
	ConfigManager.RegisterService<IMessageAdapterProvider>(new FullInMemoryMessageAdapterProvider(Connector.Adapter.InnerAdapters));
}
```

To save and load [Connector](xref:StockSharp.Algo.Connector) settings to a file, see [Save and load settings](connectors/save_and_load_settings.md).

To create your own [Connector](xref:StockSharp.Algo.Connector), see [Creating own connector](connectors/creating_own_connector.md).

For registering orders, see [Orders management](orders_management.md), [Create new order](orders_management/create_new_order.md), [Create new stop order](orders_management/create_new_stop_order.md). 

## Recommended content

[Graphical configuration](connectors/graphical_configuration.md)
