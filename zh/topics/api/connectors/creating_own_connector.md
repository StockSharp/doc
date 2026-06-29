# 创建你自己的连接器

消息传递机制是[StockSharp](https://github.com/StockSharp/StockSharp)架构的一个内部逻辑层，它使用标准协议在各种平台元素之间提供交互。

有两类主要类型：

- [消息](xref:StockSharp.Messages.Message) - 承载信息的消息。
- [AsyncMessageAdapter](xref:StockSharp.Messages.AsyncMessageAdapter) - 一个消息适配器（=转换器）。

一条**消息**充当传递信息的代理。消息有其自身的类型 [MessageTypes](xref:StockSharp.Messages.MessageTypes)。每种消息类型对应一个特定的类。反过来，所有消息类都继承自抽象类 [Message](xref:StockSharp.Messages.Message)，这赋予子类诸如消息类型 [Message.Type](xref:StockSharp.Messages.Message.Type) 和 [Message.LocalTime](xref:StockSharp.Messages.Message.LocalTime) —— 消息创建/接收的本地时间等属性。

消息可以是*接收*的或*发送*的:

- *传入* 消息 - 发送到外部系统的消息。通常，这些是程序生成的命令，例如 [ConnectMessage](xref:StockSharp.Messages.ConnectMessage) 消息——请求连接到服务器的命令。
- *发出的* 消息 - 来自外部系统的消息。这些消息传递有关市场数据、交易、投资组合、连接事件等的信息。例如，[QuoteChangeMessage](xref:StockSharp.Messages.QuoteChangeMessage) 消息传递有关订单簿变化的信息。

**消息适配器**在交易系统和程序之间起中介作用。对于每种类型的连接器，都有一个单独的适配器类，该类继承自抽象类 [AsyncMessageAdapter](xref:StockSharp.Messages.AsyncMessageAdapter)。

适配器执行两个主要功能：

1. 将传入消息转换为特定交易系统的命令。
2. 将从交易系统接收的信息（连接、市场数据、交易等）转换为外发消息。

下面是创建您自己的 [Coinbase](https://github.com/StockSharp/StockSharp/tree/master/Connectors/Coinbase) 适配器的过程描述（所有带有源代码的连接器都可以在 [StockSharp 仓库](https://github.com/StockSharp/StockSharp/tree/master/Connectors) 中获得，并作为教程提供）。

## 创建 Coinbase 消息适配器的示例

### 1. 创建一个适配器类

首先，我们创建 **CoinbaseMessageAdapter** 消息适配器类，该类继承自抽象类 [AsyncMessageAdapter](xref:StockSharp.Messages.AsyncMessageAdapter)。

```cs
public partial class CoinbaseMessageAdapter : AsyncMessageAdapter
{
	private Authenticator _authenticator;
	private HttpClient _restClient;
	private SocketClient _socketClient;

	// Other adapter fields and properties
}
```

### 2. 适配器构造函数

在适配器构造函数中，您需要执行以下操作：

1. 传递将用于创建消息 ID 的交易 ID 生成器。

2. 使用以下方法指定支持的消息类型：
 - [AddMarketDataSupport](xref:StockSharp.Messages.Extensions.AddMarketDataSupport(StockSharp.Messages.MessageAdapter)) - 支持订阅市场数据的消息。
 - [AddTransactionalSupport](xref:StockSharp.Messages.Extensions.AddTransactionalSupport(StockSharp.Messages.MessageAdapter)) - 支持事务消息。

3. 使用 [AddSupportedMarketDataType](xref:StockSharp.Messages.Extensions.AddSupportedMarketDataType(StockSharp.Messages.MessageAdapter,StockSharp.Messages.DataType) 方法指定适配器支持的具体市场数据类型。

```cs
public CoinbaseMessageAdapter(IdGenerator transactionIdGenerator)
	: base(transactionIdGenerator)
{
	HeartbeatInterval = TimeSpan.FromSeconds(5);

	// Add support for market data and transactions
	this.AddMarketDataSupport();
	this.AddTransactionalSupport();

	// Remove unsupported message types
	this.RemoveSupportedMessage(MessageTypes.Portfolio);
	this.RemoveSupportedMessage(MessageTypes.OrderGroupCancel);

	// Add supported market data types
	this.AddSupportedMarketDataType(DataType.Ticks);
	this.AddSupportedMarketDataType(DataType.MarketDepth);
	this.AddSupportedMarketDataType(DataType.Level1);
	this.AddSupportedMarketDataType(DataType.CandleTimeFrame);
}
```

### 3. 连接和断开适配器

要将适配器连接到交易系统，会调用 [AsyncMessageAdapter.ConnectAsync](xref:StockSharp.Messages.AsyncMessageAdapter.ConnectAsync(StockSharp.Messages.ConnectMessage,System.Threading.CancellationToken) 方法。它会传入收到的 [ConnectMessage](xref:StockSharp.Messages.ConnectMessage) 消息。如果连接成功，适配器会发送一条外发的 [ConnectMessage](xref:StockSharp.Messages.ConnectMessage) 消息。

```cs
public override async ValueTask ConnectAsync(ConnectMessage connectMsg, CancellationToken cancellationToken)
{
	// Check the presence of keys for transactional mode
	if (this.IsTransactional())
	{
		if (Key.IsEmpty())
			throw new InvalidOperationException(LocalizedStrings.KeyNotSpecified);

		if (Secret.IsEmpty())
			throw new InvalidOperationException(LocalizedStrings.SecretNotSpecified);
	}

	// Initialize the authenticator
	_authenticator = new(this.IsTransactional(), Key, Secret, Passphrase);

	// Check that clients are not yet created
	if (_restClient != null)
		throw new InvalidOperationException(LocalizedStrings.NotDisconnectPrevTime);

	if (_socketClient != null)
		throw new InvalidOperationException(LocalizedStrings.NotDisconnectPrevTime);

	// Create REST client
	_restClient = new(_authenticator) { Parent = this };

	// Create and configure WebSocket client
	_socketClient = new(_authenticator, ReConnectionSettings.ReAttemptCount) { Parent = this };
	SubscribePusherClient();

	// Connect WebSocket client
	await _socketClient.Connect(cancellationToken);

	// Send successful connection message
	SendOutMessage(new ConnectMessage());
}
```

要将适配器从交易系统断开，调用 [AsyncMessageAdapter.DisconnectAsync](xref:StockSharp.Messages.AsyncMessageAdapter.DisconnectAsync(StockSharp.Messages.DisconnectMessage,System.Threading.CancellationToken) 方法。如果断开成功，适配器会发送一个出站的 [DisconnectMessage](xref:StockSharp.Messages.DisconnectMessage) 消息。

```cs
public override ValueTask DisconnectAsync(DisconnectMessage disconnectMsg, CancellationToken cancellationToken)
{
	// Check that clients are created
	if (_restClient == null)
		throw new InvalidOperationException(LocalizedStrings.ConnectionNotOk);

	if (_socketClient == null)
		throw new InvalidOperationException(LocalizedStrings.ConnectionNotOk);

	// Free REST client resources
	_restClient.Dispose();
	_restClient = null;

	// Disconnect WebSocket client
	_socketClient.Disconnect();

	// Send disconnection message
	SendOutDisconnectMessage(true);
	return default;
}
```

此外，适配器提供了 [AsyncMessageAdapter.ResetAsync](xref:StockSharp.Messages.AsyncMessageAdapter.ResetAsync(StockSharp.Messages.ResetMessage,System.Threading.CancellationToken) 方法来重置状态，该方法会关闭连接并将适配器恢复到初始状态。

```cs
public override ValueTask ResetAsync(ResetMessage resetMsg, CancellationToken cancellationToken)
{
	// Free REST client resources
	if (_restClient != null)
	{
		try
		{
			_restClient.Dispose();
		}
		catch (Exception ex)
		{
			SendOutError(ex);
		}

		_restClient = null;
	}

	// Disconnect and clear WebSocket client
	if (_socketClient != null)
	{
		try
		{
			UnsubscribePusherClient();
			_socketClient.Disconnect();
		}
		catch (Exception ex)
		{
			SendOutError(ex);
		}

		_socketClient = null;
	}

	// Free authenticator resources
	if (_authenticator != null)
	{
		try
		{
			_authenticator.Dispose();
		}
		catch (Exception ex)
		{
			SendOutError(ex);
		}

		_authenticator = null;
	}

	// Clear additional data
	_candlesTransIds.Clear();

	// Send reset message
	SendOutMessage(new ResetMessage());
	return default;
}
```

### 开发完成后

一旦连接器被实现，有两种使用它的选项：

1. 将其发布到 [StockSharp 商店](https://stocksharp.com/store)，可以作为付费或免费的产品。在这种情况下，用户通过 [安装程序](../../installer/setup.md) 自动安装连接器。
2. 个人使用时，将构建好的连接器 *.dll* 文件复制到您的应用程序文件夹（或任何 StockSharp 产品的文件夹）中。启动时，应用程序会根据以下标准扫描当前目录中的适配器：

   - 仅考虑名称以 `StockSharp.` 开头且扩展名为 **.dll** 的文件。
   - 检查每个剩余的文件以确保它是有效的 .NET 程序集。
   - 程序集已加载，所有实现 `IMessageAdapter` 的类型都已收集。
   - 在扫描或加载过程中遇到的任何错误都会被写入日志，但不会停止搜索。如果加载失败，请打开应用程序的日志窗口或日志文件查看错误详情。

本文件描述了适配器操作的一般原则、其创建过程以及与交易系统的连接管理。以下文件将专门介绍适配器功能的实现：

- [仪器查询](creating_own_connector/instrument_lookup.md)
- [使用市场数据](creating_own_connector/market_data.md)
- [请求当前投资组合和订单的状态](creating_own_connector/portfolio_and_orders_state.md)
- [与交易操作合作](creating_own_connector/trading_operations.md)
- [存储设置](creating_own_connector/settings.md)
- [扩展订单条件](creating_own_connector/order_extended.md)
