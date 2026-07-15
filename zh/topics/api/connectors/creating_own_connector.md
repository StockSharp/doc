# 创建自己的连接器

消息传递机制是 [StockSharp](https://github.com/StockSharp/StockSharp) 架构中的一个内部逻辑层，它使用标准协议实现平台各元素之间的交互。

主要有两个类：

- [Message](xref:StockSharp.Messages.Message) - 承载信息的消息。
- [AsyncMessageAdapter](xref:StockSharp.Messages.AsyncMessageAdapter) - 消息适配器（=转换器）。

**消息** 充当传递信息的载体。消息拥有自己的类型 [MessageTypes](xref:StockSharp.Messages.MessageTypes)。每种消息类型对应一个特定的类。所有消息类都继承自抽象类 [Message](xref:StockSharp.Messages.Message)，该类为子类赋予了诸如消息类型 [Message.Type](xref:StockSharp.Messages.Message.Type) 和 [Message.LocalTime](xref:StockSharp.Messages.Message.LocalTime)（消息创建/接收的本地时间）等属性。

消息可以分为 *传入* 和 *传出* 两种：

- *传入* 消息 - 发送给外部系统的消息。通常是由程序生成的命令，例如 [ConnectMessage](xref:StockSharp.Messages.ConnectMessage) 消息——请求连接到服务器的命令。
- *传出* 消息 - 来自外部系统的消息。这些消息传递有关市场数据、交易、投资组合、连接事件等的信息。例如，[QuoteChangeMessage](xref:StockSharp.Messages.QuoteChangeMessage) 消息传递订单簿变化的信息。

**消息适配器** 在交易系统和程序之间充当中介的角色。每种类型的连接器都有一个单独的适配器类，该类继承自抽象类 [AsyncMessageAdapter](xref:StockSharp.Messages.AsyncMessageAdapter)。

适配器执行两个主要功能：

1. 将传入消息转换为特定交易系统的命令。
2. 将从交易系统接收到的信息（连接、市场数据、交易等）转换为传出消息。

下面介绍为 [Coinbase](https://github.com/StockSharp/Connectors/tree/main/Coinbase) 创建自定义适配器的过程（所有开源连接器都位于独立的 [Connectors 仓库](https://github.com/StockSharp/Connectors) 中，可作为实现示例）。

## 创建 Coinbase 消息适配器示例

### 1. 创建适配器类

首先，我们创建 **CoinbaseMessageAdapter** 消息适配器类，它继承自抽象类 [AsyncMessageAdapter](xref:StockSharp.Messages.AsyncMessageAdapter)。

```cs
public partial class CoinbaseMessageAdapter : AsyncMessageAdapter
{
	private Authenticator _authenticator;
	private HttpClient _restClient;
	private SocketClient _socketClient;

	// 适配器的其他字段和属性
}
```

### 2. 适配器构造函数

在适配器构造函数中，需要执行以下操作：

1. 传入用于创建消息 ID 的交易 ID 生成器。

2. 使用以下方法指定支持的消息类型：
 - [AddMarketDataSupport](xref:StockSharp.Messages.Extensions.AddMarketDataSupport(StockSharp.Messages.MessageAdapter)) - 支持订阅市场数据的消息。
 - [AddTransactionalSupport](xref:StockSharp.Messages.Extensions.AddTransactionalSupport(StockSharp.Messages.MessageAdapter)) - 支持事务性消息。

3. 使用 [AddSupportedMarketDataType](xref:StockSharp.Messages.Extensions.AddSupportedMarketDataType(StockSharp.Messages.MessageAdapter,StockSharp.Messages.DataType)) 方法指定适配器支持的具体市场数据类型。

```cs
public CoinbaseMessageAdapter(IdGenerator transactionIdGenerator)
	: base(transactionIdGenerator)
{
	HeartbeatInterval = TimeSpan.FromSeconds(5);

	// 添加市场数据和交易操作支持
	this.AddMarketDataSupport();
	this.AddTransactionalSupport();

	// 移除不支持的消息类型
	this.RemoveSupportedMessage(MessageTypes.Portfolio);
	this.RemoveSupportedMessage(MessageTypes.OrderGroupCancel);

	// 添加支持的市场数据类型
	this.AddSupportedMarketDataType(DataType.Ticks);
	this.AddSupportedMarketDataType(DataType.MarketDepth);
	this.AddSupportedMarketDataType(DataType.Level1);
	this.AddSupportedMarketDataType(DataType.CandleTimeFrame);
}
```

### 3. 连接和断开适配器

要将适配器连接到交易系统，需要调用 [AsyncMessageAdapter.ConnectAsync](xref:StockSharp.Messages.AsyncMessageAdapter.ConnectAsync(StockSharp.Messages.ConnectMessage,System.Threading.CancellationToken)) 方法。该方法接收传入的 [ConnectMessage](xref:StockSharp.Messages.ConnectMessage) 消息。如果连接成功，适配器会发送一条传出的 [ConnectMessage](xref:StockSharp.Messages.ConnectMessage) 消息。

```cs
public override async ValueTask ConnectAsync(ConnectMessage connectMsg, CancellationToken cancellationToken)
{
	// 检查交易模式所需密钥是否存在
	if (this.IsTransactional())
	{
		if (Key.IsEmpty())
			throw new InvalidOperationException(LocalizedStrings.KeyNotSpecified);

		if (Secret.IsEmpty())
			throw new InvalidOperationException(LocalizedStrings.SecretNotSpecified);
	}

	// 初始化认证器
	_authenticator = new(this.IsTransactional(), Key, Secret, Passphrase);

	// 检查客户端尚未创建
	if (_restClient != null)
		throw new InvalidOperationException(LocalizedStrings.NotDisconnectPrevTime);

	if (_socketClient != null)
		throw new InvalidOperationException(LocalizedStrings.NotDisconnectPrevTime);

	// 创建 REST 客户端
	_restClient = new(_authenticator) { Parent = this };

	// 创建并配置 WebSocket 客户端
	_socketClient = new(_authenticator, ReConnectionSettings.ReAttemptCount) { Parent = this };
	SubscribePusherClient();

	// 连接 WebSocket 客户端
	await _socketClient.Connect(cancellationToken);

	// 发送连接成功消息
	SendOutMessage(new ConnectMessage());
}
```

要断开适配器与交易系统的连接，需要调用 [AsyncMessageAdapter.DisconnectAsync](xref:StockSharp.Messages.AsyncMessageAdapter.DisconnectAsync(StockSharp.Messages.DisconnectMessage,System.Threading.CancellationToken)) 方法。如果断开成功，适配器会发送一条传出的 [DisconnectMessage](xref:StockSharp.Messages.DisconnectMessage) 消息。

```cs
public override ValueTask DisconnectAsync(DisconnectMessage disconnectMsg, CancellationToken cancellationToken)
{
	// 检查客户端已创建
	if (_restClient == null)
		throw new InvalidOperationException(LocalizedStrings.ConnectionNotOk);

	if (_socketClient == null)
		throw new InvalidOperationException(LocalizedStrings.ConnectionNotOk);

	// 释放 REST 客户端资源
	_restClient.Dispose();
	_restClient = null;

	// 断开 WebSocket 客户端
	_socketClient.Disconnect();

	// 发送断开连接消息
	SendOutDisconnectMessage(true);
	return default;
}
```

此外，适配器还提供了 [AsyncMessageAdapter.ResetAsync](xref:StockSharp.Messages.AsyncMessageAdapter.ResetAsync(StockSharp.Messages.ResetMessage,System.Threading.CancellationToken)) 方法用于重置状态，该方法会关闭连接并将适配器恢复到初始状态。

```cs
public override ValueTask ResetAsync(ResetMessage resetMsg, CancellationToken cancellationToken)
{
	// 释放 REST 客户端资源
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

	// 断开并清理 WebSocket 客户端
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

	// 释放认证器资源
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

	// 清理附加数据
	_candlesTransIds.Clear();

	// 发送重置消息
	SendOutMessage(new ResetMessage());
	return default;
}
```

### 开发完成后

连接器实现完成后，有两种使用方式：

1. 将其发布到 [StockSharp Store](https://stocksharp.com/zh/store)，作为付费或免费产品。在这种情况下，用户会通过 [安装程序](../../installer/setup.md) 自动安装该连接器。
2. 仅供个人使用时，将编译好的连接器 *.dll* 文件复制到你的应用程序（或任意 StockSharp 产品）的文件夹中。启动时，应用程序会按照以下标准扫描当前目录中的适配器：

   - 只考虑名称以 `StockSharp.` 开头、扩展名为 **.dll** 的文件。
   - 对每个剩余文件进行检查，以确保它是有效的 .NET 程序集。
   - 加载该程序集，并收集所有实现 `IMessageAdapter` 的类型。
   - 扫描或加载过程中遇到的任何错误都会被写入日志，且不会中断搜索。如果加载失败，请打开应用程序的日志窗口或日志文件查看错误详情。

本文档描述了适配器工作的一般原理、其创建方式以及与交易系统连接的管理方式。以下文档将专门介绍适配器功能的具体实现：

- [交易品种查询](creating_own_connector/instrument_lookup.md)
- [使用市场数据](creating_own_connector/market_data.md)
- [请求投资组合和订单的当前状态](creating_own_connector/portfolio_and_orders_state.md)
- [处理交易操作](creating_own_connector/trading_operations.md)
- [存储设置](creating_own_connector/settings.md)
- [扩展订单条件](creating_own_connector/order_extended.md)
