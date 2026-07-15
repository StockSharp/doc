# 连接器

要在 [S#](../api.md) 中处理交易所和数据源，建议使用基类 [Connector](xref:StockSharp.Algo.Connector)。

## 源代码仓库

基础 [Connector](xref:StockSharp.Algo.Connector)、消息模型和通用适配器协议仍位于 [StockSharp 核心仓库](https://github.com/StockSharp/StockSharp)。面向特定提供商的开源适配器则在 [StockSharp Connectors 仓库](https://github.com/StockSharp/Connectors) 中单独维护。

每个顶层连接器目录都包含独立的 .NET 项目，`Connectors.slnx` 可用于构建整个仓库。研究适配器实现、修复现有连接器或贡献新连接器时，请使用此仓库。

让我们看看如何使用 [Connector](xref:StockSharp.Algo.Connector)。示例的源代码位于 Samples\/01\_Basic\/01\_ConnectAndDownloadInstruments 项目中。

![MultiConnection 主界面](../../images/multiconnection_main.png)

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

为了配置 [Connector](xref:StockSharp.Algo.Connector)，**API** 提供了一个专门的图形界面，可以让你同时配置多个连接。有关如何使用它的说明，请参见 [图形化配置](connectors/graphical_configuration.md) 一节。

```cs
...
private readonly IFileSystem _fileSystem = Paths.FileSystem;
private const string _connectorFile = "ConnectorFile.json";
...
private void Setting_Click(object sender, RoutedEventArgs e)
{
	if (Connector.Configure(this))
	{
		Connector.Save().Serialize(_fileSystem, _connectorFile);
	}
}

```

![API GUI 连接窗口](../../images/api_gui_connectorwindow.png)

同样，你也可以直接从代码中添加连接（不使用图形窗口），方法是使用扩展方法 [TraderHelper.AddAdapter\<TAdapter\>](xref:StockSharp.Algo.TraderHelper.AddAdapter``1(StockSharp.Algo.Connector,System.Action{``0}))**(**[StockSharp.Algo.Connector](xref:StockSharp.Algo.Connector) connector, [System.Action\<TAdapter\>](xref:System.Action`1) init **)**：

```cs
...
// 添加用于连接 Binance 的适配器
connector.AddAdapter<BinanceMessageAdapter>(a => 
{
	a.Key = "<您的 API 访问密钥>";
	a.Secret = "<您的秘密密钥>";
});

// 添加用于新闻的 RSS
connector.AddAdapter<RssMessageAdapter>(a => 
{
	a.Address = "https://news-source.com/feed";
});
	  				
```

你可以向单个 [Connector](xref:StockSharp.Algo.Connector) 对象添加无限数量的连接。因此，你可以从程序中同时连接到多个交易所和经纪商。

在 *InitConnector* 方法中，我们为 [IConnector](xref:StockSharp.BusinessEntities.IConnector) 设置所需的事件处理程序：

```cs
private void InitConnector()
{
	// 订阅连接成功事件
	Connector.Connected += () =>
	{
		this.GuiAsync(() => ChangeConnectStatus(true));
	};
	
	// 订阅连接错误事件
	Connector.ConnectionError += error => this.GuiAsync(() =>
	{
		ChangeConnectStatus(false);
		MessageBox.Show(this, error.ToString(), LocalizedStrings.ErrorConnection);
	});
	
	// 订阅断开连接事件
	Connector.Disconnected += () => this.GuiAsync(() => ChangeConnectStatus(false));
	
	// 订阅错误事件
	Connector.Error += error =>
		this.GuiAsync(() => MessageBox.Show(this, error.ToString(), LocalizedStrings.Str2955));
	
	// 订阅市场数据订阅失败事件
	Connector.SubscriptionFailed += (subscription, error, isSubscribe) =>
		this.GuiAsync(() => MessageBox.Show(this, error.ToString(), 
			LocalizedStrings.Str2956Params.Put(subscription.DataType, subscription.SecurityId)));
	
	// 用于接收数据的订阅
	
	// 交易品种
	Connector.SecurityReceived += (sub, security) => _securitiesWindow.SecurityPicker.Securities.Add(security);
	
	// 逐笔成交
	Connector.TickTradeReceived += (sub, trade) => _tradesWindow.TradeGrid.Trades.TryAdd(trade);
	
	// 订单
	Connector.OrderReceived += (sub, order) => _ordersWindow.OrderGrid.Orders.TryAdd(order);
	
	// 自有成交
	Connector.OwnTradeReceived += (sub, trade) => _myTradesWindow.TradeGrid.Trades.TryAdd(trade);
	
	// 持仓
	Connector.PositionReceived += (sub, position) => _portfoliosWindow.PortfolioGrid.Positions.TryAdd(position);

	// 订单注册失败
	Connector.OrderRegisterFailReceived += (sub, fail) => _ordersWindow.OrderGrid.AddRegistrationFail(fail);
	
	// 订单撤销失败
	Connector.OrderCancelFailReceived += (sub, fail) => OrderFailed(fail);
	
	// 设置市场数据提供者
	_securitiesWindow.SecurityPicker.MarketDataProvider = Connector;
	
	try
	{
		if (File.Exists(_connectorFile))
		{
			var ctx = new ContinueOnExceptionContext();
			ctx.Error += ex => ex.LogError();
			using (new Scope<ContinueOnExceptionContext>(ctx))
				Connector.Load(_connectorFile.Deserialize<SettingsStorage>(_fileSystem));
		}
	}
	catch
	{
	}
	
	ConfigManager.RegisterService<IExchangeInfoProvider>(new InMemoryExchangeInfoProvider());
	
	// 为图形化配置注册适配器提供者
	ConfigManager.RegisterService<IMessageAdapterProvider>(
		new InMemoryMessageAdapterProvider(Connector.Adapter.InnerAdapters));
}
```

如何将 [Connector](xref:StockSharp.Algo.Connector) 的设置保存到文件并从文件中加载，请参见 [保存和加载设置](connectors/save_and_load_settings.md) 一节。

有关如何创建自己的 [Connector](xref:StockSharp.Algo.Connector) 的信息，请参见 [创建自己的连接器](connectors/creating_own_connector.md) 一节。

下单相关内容请参见 [订单](orders_management.md)、[创建新订单](orders_management/create_new_order.md)、[创建新止损订单](orders_management/create_new_stop_order.md) 各节。

## 附加功能

### IFileSystem 和 Paths.FileSystem

使用 `IFileSystem` 进行文件操作，例如序列化和反序列化设置。默认实例可以通过 `Paths.FileSystem` 获取：

```cs
private readonly IFileSystem _fileSystem = Paths.FileSystem;
```

不带 `IFileSystem` 参数的 `Serialize` 和 `Deserialize` 方法已标记为 `[Obsolete]`。

### 异步订单方法

以下是处理订单可用的异步方法：

- `RegisterOrderAsync` - 异步注册订单。
- `CancelOrderAsync` - 异步取消订单。
- `EditOrderAsync` - 异步编辑订单。

### SubscriptionsOnConnect

`SubscriptionsOnConnect` 属性控制在连接时自动执行的订阅。默认情况下，它包括对交易品种、投资组合和订单的订阅。

### 适配器事件

以下事件可用于跟踪特定适配器的事件：

- `ConnectedEx` - 特定适配器已连接。
- `DisconnectedEx` - 特定适配器已断开连接。
- `ConnectionErrorEx` - 特定适配器发生连接错误。

### 订阅生命周期

- `SubscriptionStarted` - 订阅已启动。
- `SubscriptionOnline` - 订阅已切换到在线状态：已接收历史数据，并已开始实时数据传输。
- `SubscriptionFailed` - 订阅错误。第三个参数 `isSubscribe` 表示错误发生在订阅还是取消订阅期间。
- `SubscriptionStopped` - 订阅已停止。

## 另请参阅

[图形化配置](connectors/graphical_configuration.md)
