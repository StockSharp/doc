# Adapters

The messaging allows you to create your own connections to any external trading system. To do this, you shall create your own **message adapter** class inherited from the abstract[MessageAdapter](xref:StockSharp.Messages.MessageAdapter) class. 

When developing your own message adapter, you need to solve the following tasks: 

1. Write a code that translates incoming [S\#](../../api.md) messages into external system commands.
2. Write a code that converts information from an external system into outgoing messages.
3. Convert the encoded information of the external system (codes of instruments and boards, enumerations, etc.) to [S\#](../../api.md) types. 
4. Perform additional settings related to the features of the external trading system.

Before we start describing how to develop your own adapter, let's look at how to create and process incoming and outgoing messages in [S\#](../../api.md) using the [ConnectMessage](xref:StockSharp.Messages.ConnectMessage) as an example. Suppose that the [Connector.Connect](xref:StockSharp.Algo.Connector.Connect) method was called in the program, then the following will happen in the base [Connector](xref:StockSharp.Algo.Connector) class: 

1. The protected [Connector.OnConnect](xref:StockSharp.Algo.Connector.OnConnect) method is called, in which a message is generated and passed to the [Connector.SendInMessage](xref:StockSharp.Algo.Connector.SendInMessage(StockSharp.Messages.Message))**(**[StockSharp.Messages.Message](xref:StockSharp.Messages.Message) message **)** method. 

   ```cs
   protected virtual void OnConnect()
   {
   	SendInMessage(new ConnectMessage());
   }
     				
   ```
2. In the [method M:StockSharp.Algo.Connector.Send In Message](xref:method M:StockSharp.Algo.Connector.Send In Message) the message is passed to the adapter method of the same name. 

   ```cs
   public void SendInMessage(Message message)
   {
   	_inAdapter.SendInMessage(message);
   }
     			
   ```
3. Additional checks are performed in the adapter's [MessageAdapter.SendInMessage](xref:StockSharp.Messages.MessageAdapter.SendInMessage(StockSharp.Messages.Message))**(**[StockSharp.Messages.Message](xref:StockSharp.Messages.Message) message **)** method. If everything is fine, then the message is passed to the [MessageAdapter.OnSendInMessage](xref:StockSharp.Messages.MessageAdapter.OnSendInMessage(StockSharp.Messages.Message))**(**[StockSharp.Messages.Message](xref:StockSharp.Messages.Message) message **)** method (see below). If an error is generated, a new outgoing message of the same type is created, the exception object is passed to the [BaseConnectionMessage.Error](xref:StockSharp.Messages.BaseConnectionMessage.Error) property of the message. This new message is passed to the [MessageAdapter.SendOutMessage](xref:StockSharp.Messages.MessageAdapter.SendOutMessage(StockSharp.Messages.Message))**(**[StockSharp.Messages.Message](xref:StockSharp.Messages.Message) message **)** method, which will generate a new outgoing message event \- [MessageAdapter.NewOutMessage](xref:StockSharp.Messages.MessageAdapter.NewOutMessage), that signals an error. 

Outgoing messages are created using the [MessageAdapter.SendOutMessage](xref:StockSharp.Messages.MessageAdapter.SendOutMessage(StockSharp.Messages.Message))**(**[StockSharp.Messages.Message](xref:StockSharp.Messages.Message) message **)** method, to which a message object is passed. This method generates a new outgoing message event \- [MessageAdapter.NewOutMessage](xref:StockSharp.Messages.MessageAdapter.NewOutMessage). This event is then handled in the connector base class in the protected [Connector.OnProcessMessage](xref:StockSharp.Algo.Connector.OnProcessMessage(StockSharp.Messages.Message))**(**[StockSharp.Messages.Message](xref:StockSharp.Messages.Message) message **)** method, where, depending on the situation, the message is converted to the appropriate [S\#](../../api.md) type and a connector event is generated, and additional incoming messages can also be generated. 

The following describes the process for creating your own adapter for [BitStamp](../connectors/crypto_exchanges/bitstamp.md) (the connector is available with the source codes). 

## Example of creating a BitStamp message adapter

1. **Creating an adapter class.**

   First, we create the **BitStampMessageAdapter** message adapter class inherited from the abstract [MessageAdapter](xref:StockSharp.Messages.MessageAdapter) class: 

   ```cs
   public class BitStampMessageAdapter : MessageAdapter 
   {
   }						
   						
   ```
2. **Adapter constructor.**
   - A transaction ID generator is passed to the adapter constructor, which will be used to generate message IDs.
   - Using the[Extensions.AddSupportedMessage](xref:StockSharp.Messages.Extensions.AddSupportedMessage(StockSharp.Messages.MessageAdapter,StockSharp.Messages.MessageTypeInfo))**(**[StockSharp.Messages.MessageAdapter](xref:StockSharp.Messages.MessageAdapter) adapter, [StockSharp.Messages.MessageTypeInfo](xref:StockSharp.Messages.MessageTypeInfo) info **)** method, we add the message types that the adapter will support to the [MessageAdapter.SupportedInMessages](xref:StockSharp.Messages.MessageAdapter.SupportedInMessages)array 
   ```cs
   public BitStampMessageAdapter(IdGenerator transactionIdGenerator)
   	: base(transactionIdGenerator)
   {
   	// to maintain the connection, ping every 10 seconds
   	HeartbeatInterval = TimeSpan.FromSeconds(10);
   	// the adapter supports both transactions and market data
   	this.AddMarketDataSupport();
   	this.AddTransactionalSupport();
   	
   	// deleting unsupported message types (all possible transactional messages were added via AddTransactionalSupport)
   	this.RemoveSupportedMessage(MessageTypes.Portfolio);
   	this.RemoveSupportedMessage(MessageTypes.OrderReplace);
   	// the adapter supports ticks, glasses, and logs
   	this.AddSupportedMarketDataType(DataType.Ticks);
   	this.AddSupportedMarketDataType(DataType.MarketDepth);
   	//this.AddSupportedMarketDataType(DataType.Level1);
   	this.AddSupportedMarketDataType(DataType.OrderLog);
   	// the adapter supports result messages for searching for tools, positions, and bids
   	this.AddSupportedResultMessage(MessageTypes.SecurityLookup);
   	this.AddSupportedResultMessage(MessageTypes.PortfolioLookup);
   	this.AddSupportedResultMessage(MessageTypes.OrderStatus);
   }
   						
   ```
3. [MessageAdapter.OnSendInMessage](xref:StockSharp.Messages.MessageAdapter.OnSendInMessage(StockSharp.Messages.Message))**(**[StockSharp.Messages.Message](xref:StockSharp.Messages.Message) message **)** method. 

   Next, you need to override the [MessageAdapter.OnSendInMessage](xref:StockSharp.Messages.MessageAdapter.OnSendInMessage(StockSharp.Messages.Message))**(**[StockSharp.Messages.Message](xref:StockSharp.Messages.Message) message **)** method. As mentioned above, all incoming messages are passed to this method, and for each message type you need to write code that converts the messages into [BitStamp](../connectors/crypto_exchanges/bitstamp.md) commands. Далее необходимо переопределить метод Как говорилось выше, в этот метод передаются все входящие сообщения и для каждого типа сообщения нужно написать код, преобразующий сообщения в команды 

   When the [MessageTypes.Reset](xref:StockSharp.Messages.MessageTypes.Reset) message is received, it is required to reset the state and free up resources. When these operations are complete, it is required to send an outgoing [ResetMessage](xref:StockSharp.Messages.ResetMessage) message. 

   When a [MessageTypes.Connect](xref:StockSharp.Messages.MessageTypes.Connect) message arrives, we initialize the \_httpClient and \_pusherClient variables, subscribe to [BitStamp](../connectors/crypto_exchanges/bitstamp.md) events, and establish a connection using the native API's **Connect** method. If the connection is successful, the **SessionOnPusherConnected** event should occur. 

   ```cs
   private void SubscribePusherClient()
   {
   	_pusherClient.Connected += SessionOnPusherConnected;
   	_pusherClient.Disconnected += SessionOnPusherDisconnected;
   	_pusherClient.Error += SessionOnPusherError;
   	_pusherClient.NewOrderBook += SessionOnNewOrderBook;
   	_pusherClient.NewOrderLog += SessionOnNewOrderLog;
   	_pusherClient.NewTrade += SessionOnNewTrade;
   }
   private void UnsubscribePusherClient()
   {
   	_pusherClient.Connected -= SessionOnPusherConnected;
   	_pusherClient.Disconnected -= SessionOnPusherDisconnected;
   	_pusherClient.Error -= SessionOnPusherError;
   	_pusherClient.NewOrderBook -= SessionOnNewOrderBook;
   	_pusherClient.NewOrderLog -= SessionOnNewOrderLog;
   	_pusherClient.NewTrade -= SessionOnNewTrade;
   }
   		
   protected override bool OnSendInMessage(Message message)
   {
       switch (message.Type)
       {
            case MessageTypes.Reset:
            {
            	_lastMyTradeId = 0;
   			_lastTimeBalanceCheck = null;
   			if (_httpClient != null)
   			{
   				try
   				{
   					_httpClient.Dispose();
   				}
   				catch (Exception ex)
   				{
   					SendOutError(ex);
   				}
   				_httpClient = null;
   			}
   			if (_pusherClient != null)
   			{
   				try
   				{
   					UnsubscribePusherClient();
   					_pusherClient.Disconnect();
   				}
   				catch (Exception ex)
   				{
   					SendOutError(ex);
   				}
   				_pusherClient = null;
   			}
   			SendOutMessage(new ResetMessage());
             	break;
           }
           case MessageTypes.Connect:
           {
               if (_httpClient != null)
   				throw new InvalidOperationException(LocalizedStrings.Str1619);
   			if (_pusherClient != null)
   				throw new InvalidOperationException(LocalizedStrings.Str1619);
   			_httpClient = new HttpClient(ClientId, Key, Secret, AuthV2) { Parent = this };
   			_pusherClient = new PusherClient { Parent = this };
   			SubscribePusherClient();
   			_pusherClient.Connect();
               break;
           }
           case MessageTypes.Disconnect:
           {
               if (_httpClient == null)
   				throw new InvalidOperationException(LocalizedStrings.Str1856);
   			if (_pusherClient == null)
   				throw new InvalidOperationException(LocalizedStrings.Str1856);
   			_httpClient.Dispose();
   			_httpClient = null;
   			_pusherClient.Disconnect();
               break;
           }
           
           case MessageTypes.PortfolioLookup:
   		{
   			ProcessPortfolioLookup((PortfolioLookupMessage)message);
   			break;
   		}
   		case MessageTypes.OrderStatus:
   		{
   			ProcessOrderStatus((OrderStatusMessage)message);
   			break;
   		}
   		case MessageTypes.OrderRegister:
   		{
   			ProcessOrderRegister((OrderRegisterMessage)message);
   			break;
   		}
   		case MessageTypes.OrderCancel:
   		{
   			ProcessOrderCancel((OrderCancelMessage)message);
   			break;
   		}
   		case MessageTypes.OrderGroupCancel:
   		{
   			ProcessOrderGroupCancel((OrderGroupCancelMessage)message);
   			break;
   		}
   				
   		case MessageTypes.SecurityLookup:
   		{
   			ProcessSecurityLookup((SecurityLookupMessage)message);
   			break;
   		}
   		case MessageTypes.MarketData:
   		{
   			ProcessMarketData((MarketDataMessage)message);
   			break;
   		}
   				
   		default:
   			return false;
   	}
   	return true;
   }
   private void SessionOnPusherConnected()
   {
   	SendOutMessage(new ConnectMessage());
   }
   private void SessionOnPusherError(Exception exception)
   {
   	SendOutError(exception);
   }
   private void SessionOnPusherDisconnected(bool expected)
   {
   	SendOutDisconnectMessage(expected);
   }
   ```
4. **SessionOnPusherConnected** event. 

   It is required to send an outgoing [ConnectMessage](xref:StockSharp.Messages.ConnectMessage) message in the native API connection event handler. When processing this message in the [Connector](xref:StockSharp.Algo.Connector) class code, the following message types will be checked in [MessageAdapter.SupportedInMessages](xref:StockSharp.Messages.MessageAdapter.SupportedInMessages): 
   - [MessageTypes.PortfolioLookup](xref:StockSharp.Messages.MessageTypes.PortfolioLookup) \- whether [PortfolioLookupMessage](xref:StockSharp.Messages.PortfolioLookupMessage) message necessary for obtaining portfolios and positions. 
   - [MessageTypes.SecurityLookup](xref:StockSharp.Messages.MessageTypes.SecurityLookup) \- whether [SecurityLookupMessage](xref:StockSharp.Messages.SecurityLookupMessage) message necessary for obtaining instruments. 
   - OrderStatus \- OrderStatusMessage [MessageTypes.OrderStatus](xref:StockSharp.Messages.MessageTypes.OrderStatus) \- whether [OrderStatusMessage](xref:StockSharp.Messages.OrderStatusMessage) message necessary for obtaining orders and own trades. 

   If the message types are set on the adapter, then the corresponding messages will be sent. In our example (see Adapter Constructor), the [MessageTypes.SecurityLookup](xref:StockSharp.Messages.MessageTypes.SecurityLookup) and [MessageTypes.PortfolioLookup](xref:StockSharp.Messages.MessageTypes.PortfolioLookup) types have been added to this list, so you should expect to receive incoming [SecurityLookupMessage](xref:StockSharp.Messages.SecurityLookupMessage) and [PortfolioLookupMessage](xref:StockSharp.Messages.PortfolioLookupMessage). 

   ```cs
   private void SessionOnPusherConnected()
   {
   	SendOutMessage(new ConnectMessage());
   }
   						
   ```
5. [PortfolioLookupMessage](xref:StockSharp.Messages.PortfolioLookupMessage) and [SecurityLookupMessage](xref:StockSharp.Messages.SecurityLookupMessage) incoming messages. 

   Upon receipt of these messages, it is necessary to call the [BitStamp](../connectors/crypto_exchanges/bitstamp.md) functions requesting portfolios and instruments, respectively. And after receiving all the data, you need to send the [SubscriptionFinishedMessage](xref:StockSharp.Messages.SubscriptionFinishedMessage) message. Note that the subscription ID is assigned to both the resulting message and the data messages: 

   ```cs
   // Requesting a list of portfolios
   private void ProcessPortfolioLookup(PortfolioLookupMessage message)
   {
   	if (!message.IsSubscribe)
   		return;
   	var transactionId = message.TransactionId;
   	var pfName = PortfolioName;
   	SendOutMessage(new PortfolioMessage
   	{
   		PortfolioName = pfName,
   		BoardCode = Extensions.BitStampBoard,
   		OriginalTransactionId = transactionId, // <- the subscription ID
   	});
   	
   	var account = _httpClient.GetAccount(section);
   	SendOutMessage(new PositionChangeMessage
   	{
   		SecurityId = SecurityId.Money, // <- for a money position set the special code of the Money tool
   		PortfolioName = GetPortfolioName(section),
   		ServerTime = time,	
   	}
   	.TryAdd(PositionChangeTypes.Leverage, (decimal?)account.MarginLevel)
   	.TryAdd(PositionChangeTypes.CommissionTaker, (decimal?)account.TakerCommissionRate)
   	.TryAdd(PositionChangeTypes.CommissionMaker, (decimal?)account.MakerCommissionRate));
   	var tuple = _httpClient.GetBalances();
   	foreach (var pair in tuple.Item1)
   	{
   		var currValue = pair.Value.First;
   		var currPrice = pair.Value.Second;
   		var blockValue = pair.Value.Third;
   		if (currValue == null && currPrice == null && blockValue == null)
   			continue;
   		var msg = this.CreatePositionChangeMessage(pfName, pair.Key.ToUpperInvariant().ToStockSharp(false));
   		msg
   		.TryAdd(PositionChangeTypes.CurrentValue, currValue, true)
   		.TryAdd(PositionChangeTypes.CurrentPrice, currPrice, true)
   		.TryAdd(PositionChangeTypes.BlockedValue, blockValue, true);
   		SendOutMessage(msg);	
   	}
   	
   	SendSubscriptionResult(message); // <- end of subscription (if To == null, then it is sent that the subscription went Online, otherwise Finished)
   	
   	if (message.To == null) // subscribe not only to stories, but also to online
   		_pusher.SubscribeAccount();
   }
   // The requested tools
   private void ProcessSecurityLookup(SecurityLookupMessage lookupMsg)
   {
   	var secTypes = lookupMsg.GetSecurityTypes();
   	foreach (var info in _httpClient.GetPairsInfo())
   	{
   		var secMsg = new SecurityMessage
   		{
   			SecurityId = info.Name.ToStockSharp(),
   			SecurityType = info.UrlSymbol == _eurusd ? SecurityTypes.Currency : SecurityTypes.CryptoCurrency,
   			MinVolume = info.MinimumOrder.Substring(0, info.MinimumOrder.IndexOf(' ')).To<decimal>(),
   			Decimals = info.BaseDecimals,
   			Name = info.Description,
   			VolumeStep = info.UrlSymbol == _eurusd ? 0.00001m : 0.00000001m,
   			OriginalTransactionId = lookupMsg.TransactionId, // - the subscription ID
   		};
   		if (!secMsg.IsMatch(lookupMsg, secTypes))
   			continue;
   		SendOutMessage(secMsg);
   	}
   	SendSubscriptionResult(lookupMsg); // - the completion of the subscription
   }
   						
   ```

6. Numeration or position changes received from an external system. 

   In the event handler, the received portfolio information should be converted into the outgoing [PositionChangeMessage](xref:StockSharp.Messages.PositionChangeMessage) message: 

   ```cs
   		private void SessionOnAccountUpdated(AccountUpdate account)
   		{
   			var time = account.LastAccountUpdate ?? account.EventTime;
   			var futData = account.FuturesData;
   			if (account.Balances != null)
   			{
   				foreach (var balance in account.Balances)
   				{
   					SendOutMessage(new PositionChangeMessage
   					{
   						PortfolioName = GetPortfolioName(section),
   						SecurityId = balance.Asset.InternalCreateSecurityId(),
   						ServerTime = time,
   					}
   					.TryAdd(PositionChangeTypes.CurrentValue, (decimal)balance.Free, true)
   					.TryAdd(PositionChangeTypes.BlockedValue, (decimal)balance.Locked, true));
   				}
   			}
   			else if (futData != null)
   			{
   				foreach (var balance in futData.Balances)
   				{
   					SendOutMessage(new PositionChangeMessage
   					{
   						PortfolioName = GetPortfolioName(section),
   						SecurityId = balance.Asset.InternalCreateSecurityId(),
   						ServerTime = time,
   					}
   					.TryAdd(PositionChangeTypes.CurrentValue, (decimal)balance.Balance, true));
   				}
   				foreach (var position in futData.Positions)
   				{
   					SendOutMessage(new PositionChangeMessage
   					{
   						PortfolioName = GetPortfolioName(),
   						SecurityId = position.Symbol.ToStockSharp(),
   						ServerTime = time,
   					}
   					.TryAdd(PositionChangeTypes.CurrentValue, (decimal)position.Amount, true)
   					.TryAdd(PositionChangeTypes.AveragePrice, (decimal?)position.EntryPrice, true)
   					.TryAdd(PositionChangeTypes.RealizedPnL, (decimal?)position.RealizedPnL, true)
   					.TryAdd(PositionChangeTypes.UnrealizedPnL, (decimal?)position.UnrealizedPnL, true)
   					);
   				}
   			}
   		}
   ```
7. **Tick data subscription**

   When the [Connector.Subscribe](xref:StockSharp.Algo.Connector.Subscribe(StockSharp.Algo.Subscription))**(**[StockSharp.Algo.Subscription](xref:StockSharp.Algo.Subscription) subscription **)** or [Connector.UnSubscribe](xref:StockSharp.Algo.Connector.UnSubscribe(StockSharp.Algo.Subscription))**(**[StockSharp.Algo.Subscription](xref:StockSharp.Algo.Subscription) subscription **)** methods are called the incoming [MarketDataMessage](xref:StockSharp.Messages.MarketDataMessage) message will be generated. 

   When processing this message, you should call the [BitStamp](../connectors/crypto_exchanges/bitstamp.md) methods by subscribing or unsubscribing from receiving tick trades. 

   Since the message is used to work with all types of market data, the [MarketDataMessage.DataType2](xref:StockSharp.Messages.MarketDataMessage.DataType2) property should be used to select a specific type. For trades, this property value is equal to [DataType.Ticks](xref:StockSharp.Messages.DataType.Ticks). 

   After calling the **SubscribeTrades** method, trades will arrive in the **SessionOnNewTrade** event. 

   ```cs
   		private void ProcessMarketData(MarketDataMessage mdMsg)
   		{
   			if (!mdMsg.SecurityId.IsAssociated())
   			{
   				SendSubscriptionNotSupported(mdMsg.TransactionId);
   				return;
   			}
   			var currency = mdMsg.SecurityId.ToCurrency();
   			if (mdMsg.DataType2 == DataType.OrderLog)
   			{
   				if (mdMsg.IsSubscribe)
   					_pusherClient.SubscribeOrderLog(currency);
   				else
   					_pusherClient.UnSubscribeOrderLog(currency);
   			}
   			else if (mdMsg.DataType2 == DataType.MarketDepth)
   			{
   				if (mdMsg.IsSubscribe)
   					_pusherClient.SubscribeOrderBook(currency);
   				else
   					_pusherClient.UnSubscribeOrderBook(currency);
   			}
   			else if (mdMsg.DataType2 == DataType.Ticks)
   			{
   				if (mdMsg.IsSubscribe)
   				{
   					if (mdMsg.To != null)
   					{
   						SendSubscriptionReply(mdMsg.TransactionId);
   						var diff = DateTimeOffset.Now - (mdMsg.From ?? DateTime.Today);
   						string interval;
   						if (diff.TotalMinutes < 1)
   							interval = "minute";
   						else if (diff.TotalDays < 1)
   							interval = "hour";
   						else
   							interval = "day";
   						var trades = _httpClient.RequestTransactions(currency, interval);
   						foreach (var trade in trades.OrderBy(t => t.Time))
   						{
   							SendOutMessage(new ExecutionMessage
   							{
   								ExecutionType = ExecutionTypes.Tick,
   								SecurityId = mdMsg.SecurityId,
   								TradeId = trade.Id,
   								TradePrice = (decimal)trade.Price,
   								TradeVolume = trade.Amount.ToDecimal(),
   								ServerTime = trade.Time,
   								OriginSide = trade.Type.ToSide(),
   								OriginalTransactionId = mdMsg.TransactionId
   							});
   						}
   						SendSubscriptionResult(mdMsg);
   						return;
   					}
   					else
   						_pusherClient.SubscribeTrades(currency);
   				}
   				else
   				{
   					_pusherClient.UnSubscribeTrades(currency);
   				}
   			}
   			else
   			{
   				SendSubscriptionNotSupported(mdMsg.TransactionId);
   				return;
   			}
   			SendSubscriptionReply(mdMsg.TransactionId);
   		}
   				
   ```
8. **SessionOnNewTrade** event. 

   In the event handler **Session On New Trade** the received information about the transaction must be converted to an outgoing message [Execution Message](xref:StockSharp.Messages.Execution Message). Note that the [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage) is used both for transactions (private or anonymous) and for orders. Therefore, the message specifies that it relates to the transaction \- [ExecutionMessage.ExecutionType](xref:StockSharp.Messages.ExecutionMessage.ExecutionType) \= [ExecutionTypes.Tick](xref:StockSharp.Messages.ExecutionTypes.Tick). 

   ```cs
   private void SessionOnNewTrade(string pair, Trade trade)
   {
   	SendOutMessage(new ExecutionMessage
   	{
   		ExecutionType = ExecutionTypes.Tick,
   		SecurityId = pair.ToStockSharp(),
   		TradeId = trade.Id,
   		TradePrice = (decimal)trade.Price,
   		TradeVolume = (decimal)trade.Amount,
   		ServerTime = trade.Time,
   		OriginSide = trade.Type.ToSide(),
   	});
   }
   		  
   ```
9. The code for handling orders (cancel\-register), as well as the full adapter code, can be obtained from the [GitHub\/StockSharp](https://github.com/StockSharp/StockSharp) repository. 
