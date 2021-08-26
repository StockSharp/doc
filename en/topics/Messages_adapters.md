# Adapters

The messaging allows you to create your own connections to any external trading system. To do this, you shall create your own **message adapter** class inherited from the abstract[MessageAdapter](../api/StockSharp.Messages.MessageAdapter.html) class. 

When developing your own message adapter, you need to solve the following tasks: 

1. Write a code that translates incoming [S\#](StockSharpAbout.md) messages into external system commands.
2. Write a code that converts information from an external system into outgoing messages.
3. Convert the encoded information of the external system (codes of instruments and boards, enumerations, etc.) to [S\#](StockSharpAbout.md) types. 
4. Perform additional settings related to the features of the external trading system.

Before we start describing how to develop your own adapter, let's look at how to create and process incoming and outgoing messages in [S\#](StockSharpAbout.md) using the [ConnectMessage](../api/StockSharp.Messages.ConnectMessage.html) as an example. Suppose that the [Connect](../api/StockSharp.Algo.Connector.Connect.html) method was called in the program, then the following will happen in the base [Connector](../api/StockSharp.Algo.Connector.html) class: 

1. The protected [Connector.OnConnect](../api/StockSharp.Algo.Connector.OnConnect.html) method is called, in which a message is generated and passed to the [Connector.SendInMessage](../api/StockSharp.Algo.Connector.SendInMessage.html) method. 

   ```cs
   protected virtual void OnConnect()
   {
   	SendInMessage(new ConnectMessage());
   }
     				
   ```
2. In the [method M:StockSharp.Algo.Connector.Send In Message](../api/method M:StockSharp.Algo.Connector.Send In Message.html) the message is passed to the adapter method of the same name. 

   ```cs
   public void SendInMessage(Message message)
   {
   	\_inAdapter.SendInMessage(message);
   }
     			
   ```
3. Additional checks are performed in the adapter's [MessageAdapter.SendInMessage](../api/StockSharp.Messages.MessageAdapter.SendInMessage.html) method. If everything is fine, then the message is passed to the [MessageAdapter.OnSendInMessage](../api/StockSharp.Messages.MessageAdapter.OnSendInMessage.html) method (see below). If an error is generated, a new outgoing message of the same type is created, the exception object is passed to the [Error](../api/StockSharp.Messages.BaseConnectionMessage.Error.html) property of the message. This new message is passed to the [SendOutMessage](../api/StockSharp.Messages.MessageAdapter.SendOutMessage.html) method, which will generate a new outgoing message event \- [NewOutMessage](../api/StockSharp.Messages.MessageAdapter.NewOutMessage.html), that signals an error. 

Outgoing messages are created using the [MessageAdapter.SendOutMessage](../api/StockSharp.Messages.MessageAdapter.SendOutMessage.html) method, to which a message object is passed. This method generates a new outgoing message event \- [NewOutMessage](../api/StockSharp.Messages.MessageAdapter.NewOutMessage.html). This event is then handled in the connector base class in the protected [Connector.OnProcessMessage](../api/StockSharp.Algo.Connector.OnProcessMessage.html) method, where, depending on the situation, the message is converted to the appropriate [S\#](StockSharpAbout.md) type and a connector event is generated, and additional incoming messages can also be generated. 

The following describes the process for creating your own adapter for [BitStamp](BitStamp.md) (the connector is available with the source codes). 

### Example of creating a BitStamp message adapter

Example of creating a BitStamp message adapter

1. **Creating an adapter class.**

   First, we create the **BitStampMessageAdapter** message adapter class inherited from the abstract [MessageAdapter](../api/StockSharp.Messages.MessageAdapter.html) class: 

   ```cs
   public class BitStampMessageAdapter : MessageAdapter 
   {
   }						
   						
   ```
2. **Adapter constructor.**
   - A transaction ID generator is passed to the adapter constructor, which will be used to generate message IDs.
   - Using the[AddSupportedMessage](../api/StockSharp.Messages.Extensions.AddSupportedMessage.html) method, we add the message types that the adapter will support to the [SupportedInMessages](../api/StockSharp.Messages.MessageAdapter.SupportedInMessages.html)array 
   ```cs
   public BitStampMessageAdapter(IdGenerator transactionIdGenerator)
   	: base(transactionIdGenerator)
   {
   	\/\/ to maintain the connection, ping every 10 seconds
   	HeartbeatInterval \= TimeSpan.FromSeconds(10);
   	\/\/ the adapter supports both transactions and market data
   	this.AddMarketDataSupport();
   	this.AddTransactionalSupport();
   	
   	\/\/ deleting unsupported message types (all possible transactional messages were added via AddTransactionalSupport)
   	this.RemoveSupportedMessage(MessageTypes.Portfolio);
   	this.RemoveSupportedMessage(MessageTypes.OrderReplace);
   	\/\/ the adapter supports ticks, glasses, and logs
   	this.AddSupportedMarketDataType(DataType.Ticks);
   	this.AddSupportedMarketDataType(DataType.MarketDepth);
   	\/\/this.AddSupportedMarketDataType(DataType.Level1);
   	this.AddSupportedMarketDataType(DataType.OrderLog);
   	\/\/ the adapter supports result messages for searching for tools, positions, and bids
   	this.AddSupportedResultMessage(MessageTypes.SecurityLookup);
   	this.AddSupportedResultMessage(MessageTypes.PortfolioLookup);
   	this.AddSupportedResultMessage(MessageTypes.OrderStatus);
   }
   						
   ```
3. [OnSendInMessage](../api/StockSharp.Messages.MessageAdapter.OnSendInMessage.html) method. 

   Next, you need to override the [OnSendInMessage](../api/StockSharp.Messages.MessageAdapter.OnSendInMessage.html) method. As mentioned above, all incoming messages are passed to this method, and for each message type you need to write code that converts the messages into [BitStamp](BitStamp.md) commands. Далее необходимо переопределить метод Как говорилось выше, в этот метод передаются все входящие сообщения и для каждого типа сообщения нужно написать код, преобразующий сообщения в команды 

   When the [MessageTypes.Reset](../api/StockSharp.Messages.MessageTypes.Reset.html) message is received, it is required to reset the state and free up resources. When these operations are complete, it is required to send an outgoing [ResetMessage](../api/StockSharp.Messages.ResetMessage.html) message. 

   When a [MessageTypes.Connect](../api/StockSharp.Messages.MessageTypes.Connect.html) message arrives, we initialize the \_httpClient and \_pusherClient variables, subscribe to [BitStamp](BitStamp.md) events, and establish a connection using the native API's **Connect** method. If the connection is successful, the **SessionOnPusherConnected** event should occur. 

   ```cs
   private void SubscribePusherClient()
   {
   	\_pusherClient.Connected +\= SessionOnPusherConnected;
   	\_pusherClient.Disconnected +\= SessionOnPusherDisconnected;
   	\_pusherClient.Error +\= SessionOnPusherError;
   	\_pusherClient.NewOrderBook +\= SessionOnNewOrderBook;
   	\_pusherClient.NewOrderLog +\= SessionOnNewOrderLog;
   	\_pusherClient.NewTrade +\= SessionOnNewTrade;
   }
   private void UnsubscribePusherClient()
   {
   	\_pusherClient.Connected \-\= SessionOnPusherConnected;
   	\_pusherClient.Disconnected \-\= SessionOnPusherDisconnected;
   	\_pusherClient.Error \-\= SessionOnPusherError;
   	\_pusherClient.NewOrderBook \-\= SessionOnNewOrderBook;
   	\_pusherClient.NewOrderLog \-\= SessionOnNewOrderLog;
   	\_pusherClient.NewTrade \-\= SessionOnNewTrade;
   }
   		
   protected override bool OnSendInMessage(Message message)
   {
       switch (message.Type)
       {
            case MessageTypes.Reset:
            {
            	\_lastMyTradeId \= 0;
   			\_lastTimeBalanceCheck \= null;
   			if (\_httpClient \!\= null)
   			{
   				try
   				{
   					\_httpClient.Dispose();
   				}
   				catch (Exception ex)
   				{
   					SendOutError(ex);
   				}
   				\_httpClient \= null;
   			}
   			if (\_pusherClient \!\= null)
   			{
   				try
   				{
   					UnsubscribePusherClient();
   					\_pusherClient.Disconnect();
   				}
   				catch (Exception ex)
   				{
   					SendOutError(ex);
   				}
   				\_pusherClient \= null;
   			}
   			SendOutMessage(new ResetMessage());
             	break;
           }
           case MessageTypes.Connect:
           {
               if (\_httpClient \!\= null)
   				throw new InvalidOperationException(LocalizedStrings.Str1619);
   			if (\_pusherClient \!\= null)
   				throw new InvalidOperationException(LocalizedStrings.Str1619);
   			\_httpClient \= new HttpClient(ClientId, Key, Secret, AuthV2) { Parent \= this };
   			\_pusherClient \= new PusherClient { Parent \= this };
   			SubscribePusherClient();
   			\_pusherClient.Connect();
               break;
           }
           case MessageTypes.Disconnect:
           {
               if (\_httpClient \=\= null)
   				throw new InvalidOperationException(LocalizedStrings.Str1856);
   			if (\_pusherClient \=\= null)
   				throw new InvalidOperationException(LocalizedStrings.Str1856);
   			\_httpClient.Dispose();
   			\_httpClient \= null;
   			\_pusherClient.Disconnect();
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

   It is required to send an outgoing [ConnectMessage](../api/StockSharp.Messages.ConnectMessage.html) message in the native API connection event handler. When processing this message in the [Connector](../api/StockSharp.Algo.Connector.html) class code, the following message types will be checked in [SupportedInMessages](../api/StockSharp.Messages.MessageAdapter.SupportedInMessages.html): 
   - [PortfolioLookup](../api/StockSharp.Messages.MessageTypes.PortfolioLookup.html) \- whether [PortfolioLookupMessage](../api/StockSharp.Messages.PortfolioLookupMessage.html) message necessary for obtaining portfolios and positions. 
   - [SecurityLookup](../api/StockSharp.Messages.MessageTypes.SecurityLookup.html) \- whether [SecurityLookupMessage](../api/StockSharp.Messages.SecurityLookupMessage.html) message necessary for obtaining instruments. 
   - OrderStatus \- OrderStatusMessage [OrderStatus](../api/StockSharp.Messages.MessageTypes.OrderStatus.html) \- whether [OrderStatusMessage](../api/StockSharp.Messages.OrderStatusMessage.html) message necessary for obtaining orders and own trades. 

   If the message types are set on the adapter, then the corresponding messages will be sent. In our example (see Adapter Constructor), the [MessageTypes.SecurityLookup](../api/StockSharp.Messages.MessageTypes.SecurityLookup.html) and [MessageTypes.PortfolioLookup](../api/StockSharp.Messages.MessageTypes.PortfolioLookup.html) types have been added to this list, so you should expect to receive incoming [SecurityLookupMessage](../api/StockSharp.Messages.SecurityLookupMessage.html) and [PortfolioLookupMessage](../api/StockSharp.Messages.PortfolioLookupMessage.html). 

   ```cs
   private void SessionOnPusherConnected()
   {
   	SendOutMessage(new ConnectMessage());
   }
   						
   ```
5. [PortfolioLookupMessage](../api/StockSharp.Messages.PortfolioLookupMessage.html) and [SecurityLookupMessage](../api/StockSharp.Messages.SecurityLookupMessage.html) incoming messages. 

   Upon receipt of these messages, it is necessary to call the [BitStamp](BitStamp.md) functions requesting portfolios and instruments, respectively. And after receiving all the data, you need to send the [SubscriptionFinishedMessage](../api/StockSharp.Messages.SubscriptionFinishedMessage.html) message. Note that the subscription ID is assigned to both the resulting message and the data messages: 

   ```cs
   \/\/ Requesting a list of portfolios
   private void ProcessPortfolioLookup(PortfolioLookupMessage message)
   {
   	if (\!message.IsSubscribe)
   		return;
   	var transactionId \= message.TransactionId;
   	var pfName \= PortfolioName;
   	SendOutMessage(new PortfolioMessage
   	{
   		PortfolioName \= pfName,
   		BoardCode \= Extensions.BitStampBoard,
   		OriginalTransactionId \= transactionId, \/\/ \<\- the subscription ID
   	});
   	
   	var account \= \_httpClient.GetAccount(section);
   	SendOutMessage(new PositionChangeMessage
   	{
   		SecurityId \= SecurityId.Money, \/\/ \<\- for a money position set the special code of the Money tool
   		PortfolioName \= GetPortfolioName(section),
   		ServerTime \= time,	
   	}
   	.TryAdd(PositionChangeTypes.Leverage, (decimal?)account.MarginLevel)
   	.TryAdd(PositionChangeTypes.CommissionTaker, (decimal?)account.TakerCommissionRate)
   	.TryAdd(PositionChangeTypes.CommissionMaker, (decimal?)account.MakerCommissionRate));
   	var tuple \= \_httpClient.GetBalances();
   	foreach (var pair in tuple.Item1)
   	{
   		var currValue \= pair.Value.First;
   		var currPrice \= pair.Value.Second;
   		var blockValue \= pair.Value.Third;
   		if (currValue \=\= null && currPrice \=\= null && blockValue \=\= null)
   			continue;
   		var msg \= this.CreatePositionChangeMessage(pfName, pair.Key.ToUpperInvariant().ToStockSharp(false));
   		msg
   		.TryAdd(PositionChangeTypes.CurrentValue, currValue, true)
   		.TryAdd(PositionChangeTypes.CurrentPrice, currPrice, true)
   		.TryAdd(PositionChangeTypes.BlockedValue, blockValue, true);
   		SendOutMessage(msg);	
   	}
   	
   	SendSubscriptionResult(message); \/\/ \<\- end of subscription (if To \=\= null, then it is sent that the subscription went Online, otherwise Finished)
   	
   	if (message.To \=\= null) \/\/ subscribe not only to stories, but also to online
   		\_pusher.SubscribeAccount();
   }
   \/\/ The requested tools
   private void ProcessSecurityLookup(SecurityLookupMessage lookupMsg)
   {
   	var secTypes \= lookupMsg.GetSecurityTypes();
   	foreach (var info in \_httpClient.GetPairsInfo())
   	{
   		var secMsg \= new SecurityMessage
   		{
   			SecurityId \= info.Name.ToStockSharp(),
   			SecurityType \= info.UrlSymbol \=\= \_eurusd ? SecurityTypes.Currency : SecurityTypes.CryptoCurrency,
   			MinVolume \= info.MinimumOrder.Substring(0, info.MinimumOrder.IndexOf(' ')).To\<decimal\>(),
   			Decimals \= info.BaseDecimals,
   			Name \= info.Description,
   			VolumeStep \= info.UrlSymbol \=\= \_eurusd ? 0.00001m : 0.00000001m,
   			OriginalTransactionId \= lookupMsg.TransactionId, \/\/ \<\- the subscription ID
   		};
   		if (\!secMsg.IsMatch(lookupMsg, secTypes))
   			continue;
   		SendOutMessage(secMsg);
   	}
   	SendSubscriptionResult(lookupMsg); \/\/ \<\- the completion of the subscription
   }
   						
   						
   ```
6. Numeration or position changes received from an external system. 

   In the event handler, the received portfolio information should be converted into the outgoing [PositionChangeMessage](../api/StockSharp.Messages.PositionChangeMessage.html) message: 

   ```cs
   		private void SessionOnAccountUpdated(AccountUpdate account)
   		{
   			var time \= account.LastAccountUpdate ?? account.EventTime;
   			var futData \= account.FuturesData;
   			if (account.Balances \!\= null)
   			{
   				foreach (var balance in account.Balances)
   				{
   					SendOutMessage(new PositionChangeMessage
   					{
   						PortfolioName \= GetPortfolioName(section),
   						SecurityId \= balance.Asset.InternalCreateSecurityId(),
   						ServerTime \= time,
   					}
   					.TryAdd(PositionChangeTypes.CurrentValue, (decimal)balance.Free, true)
   					.TryAdd(PositionChangeTypes.BlockedValue, (decimal)balance.Locked, true));
   				}
   			}
   			else if (futData \!\= null)
   			{
   				foreach (var balance in futData.Balances)
   				{
   					SendOutMessage(new PositionChangeMessage
   					{
   						PortfolioName \= GetPortfolioName(section),
   						SecurityId \= balance.Asset.InternalCreateSecurityId(),
   						ServerTime \= time,
   					}
   					.TryAdd(PositionChangeTypes.CurrentValue, (decimal)balance.Balance, true));
   				}
   				foreach (var position in futData.Positions)
   				{
   					SendOutMessage(new PositionChangeMessage
   					{
   						PortfolioName \= GetPortfolioName(),
   						SecurityId \= position.Symbol.ToStockSharp(),
   						ServerTime \= time,
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

   When the [Connector.Subscribe](../api/StockSharp.Algo.Connector.Subscribe.html) or [Connector.UnSubscribe](../api/StockSharp.Algo.Connector.UnSubscribe.html) methods are called the incoming [MarketDataMessage](../api/StockSharp.Messages.MarketDataMessage.html) message will be generated. 

   When processing this message, you should call the [BitStamp](BitStamp.md) methods by subscribing or unsubscribing from receiving tick trades. 

   Since the message is used to work with all types of market data, the [DataType2](../api/StockSharp.Messages.MarketDataMessage.DataType2.html) property should be used to select a specific type. For trades, this property value is equal to [DataType.Ticks](../api/StockSharp.Messages.DataType.Ticks.html). 

   After calling the **SubscribeTrades** method, trades will arrive in the **SessionOnNewTrade** event. 

   ```cs
   		private void ProcessMarketData(MarketDataMessage mdMsg)
   		{
   			if (\!mdMsg.SecurityId.IsAssociated())
   			{
   				SendSubscriptionNotSupported(mdMsg.TransactionId);
   				return;
   			}
   			var currency \= mdMsg.SecurityId.ToCurrency();
   			if (mdMsg.DataType2 \=\= DataType.OrderLog)
   			{
   				if (mdMsg.IsSubscribe)
   					\_pusherClient.SubscribeOrderLog(currency);
   				else
   					\_pusherClient.UnSubscribeOrderLog(currency);
   			}
   			else if (mdMsg.DataType2 \=\= DataType.MarketDepth)
   			{
   				if (mdMsg.IsSubscribe)
   					\_pusherClient.SubscribeOrderBook(currency);
   				else
   					\_pusherClient.UnSubscribeOrderBook(currency);
   			}
   			else if (mdMsg.DataType2 \=\= DataType.Ticks)
   			{
   				if (mdMsg.IsSubscribe)
   				{
   					if (mdMsg.To \!\= null)
   					{
   						SendSubscriptionReply(mdMsg.TransactionId);
   						var diff \= DateTimeOffset.Now \- (mdMsg.From ?? DateTime.Today);
   						string interval;
   						if (diff.TotalMinutes \< 1)
   							interval \= "minute";
   						else if (diff.TotalDays \< 1)
   							interval \= "hour";
   						else
   							interval \= "day";
   						var trades \= \_httpClient.RequestTransactions(currency, interval);
   						foreach (var trade in trades.OrderBy(t \=\> t.Time))
   						{
   							SendOutMessage(new ExecutionMessage
   							{
   								ExecutionType \= ExecutionTypes.Tick,
   								SecurityId \= mdMsg.SecurityId,
   								TradeId \= trade.Id,
   								TradePrice \= (decimal)trade.Price,
   								TradeVolume \= trade.Amount.ToDecimal(),
   								ServerTime \= trade.Time,
   								OriginSide \= trade.Type.ToSide(),
   								OriginalTransactionId \= mdMsg.TransactionId
   							});
   						}
   						SendSubscriptionResult(mdMsg);
   						return;
   					}
   					else
   						\_pusherClient.SubscribeTrades(currency);
   				}
   				else
   				{
   					\_pusherClient.UnSubscribeTrades(currency);
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

   In the event handler **Session On New Trade** the received information about the transaction must be converted to an outgoing message [Execution Message](../api/StockSharp.Messages.Execution Message.html). Note that the [ExecutionMessage](../api/StockSharp.Messages.ExecutionMessage.html) is used both for transactions (private or anonymous) and for orders. Therefore, the message specifies that it relates to the transaction \- [ExecutionType](../api/StockSharp.Messages.ExecutionMessage.ExecutionType.html) \= [ExecutionTypes.Tick](../api/StockSharp.Messages.ExecutionTypes.Tick.html). 

   ```cs
   private void SessionOnNewTrade(string pair, Trade trade)
   {
   	SendOutMessage(new ExecutionMessage
   	{
   		ExecutionType \= ExecutionTypes.Tick,
   		SecurityId \= pair.ToStockSharp(),
   		TradeId \= trade.Id,
   		TradePrice \= (decimal)trade.Price,
   		TradeVolume \= (decimal)trade.Amount,
   		ServerTime \= trade.Time,
   		OriginSide \= trade.Type.ToSide(),
   	});
   }
   		  
   ```
9. The code for handling orders (cancel\-register), as well as the full adapter code, can be obtained from the [GitHub\/StockSharp](https://github.com/StockSharp/StockSharp) repository. 
