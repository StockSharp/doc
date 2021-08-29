# Адаптеры

Механизм сообщений позволяет создавать собственные подключения к любой внешней торговой системе. Для этого необходимо создать собственный класс *адаптера сообщений*, унаследованный от абстрактного класса [MessageAdapter](xref:StockSharp.Messages.MessageAdapter). 

При разработке собственного адаптера сообщений необходимо решить следующие задачи: 

1. Написать код, преобразующий входящие сообщения [S\#](StockSharpAbout.md) в команды внешней системы.
2. Написать код, преобразующий информацию, поступающую от внешней системы, в исходящие сообщения.
3. Выполнить преобразование кодированной информации внешней системы (коды инструментов и площадок, перечисления и т.п.) в типы [S\#](StockSharpAbout.md).
4. Выполнить дополнительные настройки, связанные с особенностями внешней торговой системы.

Прежде чем приступить к описанию разработки собственного адаптера, рассмотрим процесс создания и обработки входящих и исходящих сообщений в [S\#](StockSharpAbout.md) на примере сообщения [ConnectMessage](xref:StockSharp.Messages.ConnectMessage). Предположим, что в программе был вызван метод [Connect](xref:StockSharp.Algo.Connector.Connect), тогда в базовом классе [Connector](xref:StockSharp.Algo.Connector) будет происходить следующее: 

1. Вызывается защищенный метод [Connector.OnConnect](xref:StockSharp.Algo.Connector.OnConnect), в котором создается сообщение и передается в метод [Connector.SendInMessage](xref:StockSharp.Algo.Connector.SendInMessage).

   ```cs
   protected virtual void OnConnect()
   {
   	SendInMessage(new ConnectMessage());
   }
     				
   ```
2. В методе [Connector.SendInMessage](xref:StockSharp.Algo.Connector.SendInMessage) сообщение передается в одноименный метод адаптера.

   ```cs
   public void SendInMessage(Message message)
   {
   	_inAdapter.SendInMessage(message);
   }
     			
   ```
3. В методе [MessageAdapter.SendInMessage](xref:StockSharp.Messages.MessageAdapter.SendInMessage) адаптера выполняются дополнительные проверки. Если все нормально, то сообщение передается в метод [MessageAdapter.OnSendInMessage](xref:StockSharp.Messages.MessageAdapter.OnSendInMessage) (см.ниже). Если сгенерирована ошибка, то создается создается новое исходящее сообщение аналогичного типа, в свойство [Error](xref:StockSharp.Messages.BaseConnectionMessage.Error) сообщения передается объект исключения. Это новое сообщение передается в метод [SendOutMessage](xref:StockSharp.Messages.MessageAdapter.SendOutMessage), в котором будет сгенерировано событие появления нового исходящего сообщения [NewOutMessage](xref:StockSharp.Messages.MessageAdapter.NewOutMessage), сигнализирующего об ошибке. 

Исходящие сообщения создаются при помощи метода [MessageAdapter.SendOutMessage](xref:StockSharp.Messages.MessageAdapter.SendOutMessage), в который передается объект сообщения. В этом методе генерируется событие нового исходящего сообщения [NewOutMessage](xref:StockSharp.Messages.MessageAdapter.NewOutMessage). Далее это событие обрабатывается в базовом классе коннектора в защищенном методе [Connector.OnProcessMessage](xref:StockSharp.Algo.Connector.OnProcessMessage), где в зависимости от ситуации сообщение преобразуется в соответствующий тип [S\#](StockSharpAbout.md) и генерируется событие коннектора, а также могут создаваться дополнительное входящие сообщения. 

Ниже описан процесс создания собственного адаптера для [BitStamp](BitStamp.md) (коннектор доступен с исходными кодами). 

## Пример создания адаптера сообщений BitStamp

1. **Создание класса адаптера.**

   Вначале создаем класс адаптера сообщений **BitStampMessageAdapter** унаследованный от абстрактного класса [MessageAdapter](xref:StockSharp.Messages.MessageAdapter): 

   ```cs
   public class BitStampMessageAdapter : MessageAdapter 
   {
   }						
   						
   ```
2. **Конструктор адаптера.**
   - В конструктор адаптера передается генератор идентификаторов транзакций, который будет использоваться для создания идентификаторов сообщений.
   - При помощи метода [AddSupportedMessage](xref:StockSharp.Messages.Extensions.AddSupportedMessage) добавляем в массив [SupportedInMessages](xref:StockSharp.Messages.MessageAdapter.SupportedInMessages) типы сообщений, которые будет поддерживать адаптер. 
   ```cs
   public BitStampMessageAdapter(IdGenerator transactionIdGenerator)
   	: base(transactionIdGenerator)
   {
   	// для поддержания соединения шлем каждый 10 секунд пинг
   	HeartbeatInterval = TimeSpan.FromSeconds(10);
   	// адаптер поддерживает как транзакции, так и маркет-данные
   	this.AddMarketDataSupport();
   	this.AddTransactionalSupport();
   	
   	// удаляем не поддерживаемые типы сообщий (были добавлены через AddTransactionalSupport все возможные транзакционные сообщения)
   	this.RemoveSupportedMessage(MessageTypes.Portfolio);
   	this.RemoveSupportedMessage(MessageTypes.OrderReplace);
   	// адаптер поддерживает тики, стаканы и лог заявок
   	this.AddSupportedMarketDataType(DataType.Ticks);
   	this.AddSupportedMarketDataType(DataType.MarketDepth);
   	//this.AddSupportedMarketDataType(DataType.Level1);
   	this.AddSupportedMarketDataType(DataType.OrderLog);
   	// адаптер поддерживает результирующие сообщения для поиска инструментов, позиций и заявок
   	this.AddSupportedResultMessage(MessageTypes.SecurityLookup);
   	this.AddSupportedResultMessage(MessageTypes.PortfolioLookup);
   	this.AddSupportedResultMessage(MessageTypes.OrderStatus);
   }
   						
   ```
3. Метод [OnSendInMessage](xref:StockSharp.Messages.MessageAdapter.OnSendInMessage). 

   Далее необходимо переопределить метод [OnSendInMessage](xref:StockSharp.Messages.MessageAdapter.OnSendInMessage). Как говорилось выше, в этот метод передаются все входящие сообщения и для каждого типа сообщения нужно написать код, преобразующий сообщения в команды [BitStamp](BitStamp.md). 

   При получении сообщения [MessageTypes.Reset](xref:StockSharp.Messages.MessageTypes.Reset) необходимо выполнить "обнуление" состояния и освободить ресурсы. По завершении этих операций нужно отправить исходящие сообщение [ResetMessage](xref:StockSharp.Messages.ResetMessage). 

   При поступлении сообщения [MessageTypes.Connect](xref:StockSharp.Messages.MessageTypes.Connect) инициализируем переменные \_httpClient и \_pusherClient, подписываемся на события [BitStamp](BitStamp.md) и устанавливаем соединение при помощи метода **Connect** нативного API. В случае удачного соединения должно наступить событие **SessionOnPusherConnected**. 

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
4. Событие **SessionOnPusherConnected**. 

   В обработчике события соединения нативного API необходимо послать исходящее сообщение [ConnectMessage](xref:StockSharp.Messages.ConnectMessage). При обработке этого сообщения в коде класса [Connector](xref:StockSharp.Algo.Connector) будут проверены наличие в [SupportedInMessages](xref:StockSharp.Messages.MessageAdapter.SupportedInMessages) следующие типов сообщений: 
   - [PortfolioLookup](xref:StockSharp.Messages.MessageTypes.PortfolioLookup) \- необходимо ли сообщение [PortfolioLookupMessage](xref:StockSharp.Messages.PortfolioLookupMessage) для получения портфелей и позиций. 
   - [SecurityLookup](xref:StockSharp.Messages.MessageTypes.SecurityLookup) \- необходимо ли сообщение [SecurityLookupMessage](xref:StockSharp.Messages.SecurityLookupMessage) для получения инструментов. 
   - [OrderStatus](xref:StockSharp.Messages.MessageTypes.OrderStatus) \- необходимо ли сообщение [OrderStatusMessage](xref:StockSharp.Messages.OrderStatusMessage) для получения заявок и собственных сделок. 

   Если типы сообщения установлены у адаптера, то соответствующие сообщения будут посланы. В нашем примере (см. Конструктор адаптера.) в этот список были добавлены типы [MessageTypes.SecurityLookup](xref:StockSharp.Messages.MessageTypes.SecurityLookup) и [MessageTypes.PortfolioLookup](xref:StockSharp.Messages.MessageTypes.PortfolioLookup), поэтому следует ожидать получения входящих сообщений [SecurityLookupMessage](xref:StockSharp.Messages.SecurityLookupMessage) и [PortfolioLookupMessage](xref:StockSharp.Messages.PortfolioLookupMessage). 

   ```cs
   private void SessionOnPusherConnected()
   {
   	SendOutMessage(new ConnectMessage());
   }
   						
   ```
5. Входящие сообщения [PortfolioLookupMessage](xref:StockSharp.Messages.PortfolioLookupMessage) и [SecurityLookupMessage](xref:StockSharp.Messages.SecurityLookupMessage). 

   При получении этих сообщений необходимо вызвать функции [BitStamp](BitStamp.md), запрашивающие портфели и инструменты соответственно. А после получения всех данных необходимо послать сообщение [SubscriptionFinishedMessage](xref:StockSharp.Messages.SubscriptionFinishedMessage). Обратите внимание, как результирующему сообщению, так и сообщениям с данными, присваивается идентификатор подписки: 

   ```cs
   // Запрашиваем список портфелей
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
   		OriginalTransactionId = transactionId, // <- идентификатор подписки
   	});
   	
   	var account = _httpClient.GetAccount(section);
   	SendOutMessage(new PositionChangeMessage
   	{
   		SecurityId = SecurityId.Money, // <- для денежной позиции устанавливаем спец код инструмента Money
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
   	
   	SendSubscriptionResult(message); // <- завершение подписки (если To == null, то тут отправляется что подписка перешла в Online, иначе Finished)
   	
   	if (message.To == null) // подписка не только на истори, но и на online
   		_pusher.SubscribeAccount();
   }
   // Запрашиваем инструменты
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
   			OriginalTransactionId = lookupMsg.TransactionId, // <- идентификатор подписки
   		};
   		if (!secMsg.IsMatch(lookupMsg, secTypes))
   			continue;
   		SendOutMessage(secMsg);
   	}
   	SendSubscriptionResult(lookupMsg); // <- завершение подписки
   }
   						
   						
   ```
6. Изменение счета или позиции, получаемые от внешней системы. 

   В обработчике события полученную информацию о портфеле необходимо преобразовать в исходящее сообщение [PositionChangeMessage](xref:StockSharp.Messages.PositionChangeMessage): 

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
7. **Подписка на тиковые данные.**

   При вызове методов [Connector.Subscribe](xref:StockSharp.Algo.Connector.Subscribe) или [Connector.UnSubscribe](xref:StockSharp.Algo.Connector.UnSubscribe) будет сформировано входящее сообщение [MarketDataMessage](xref:StockSharp.Messages.MarketDataMessage) . 

   При обработке этого сообщения необходимо вызвать методы [BitStamp](BitStamp.md) по подписке или отписке получения тиковых сделок. 

   Так как сообщение используется для работы со всеми типами рыночных данных, то для вычленения конкретного типа нужно использовать свойство [DataType2](xref:StockSharp.Messages.MarketDataMessage.DataType2). Для сделок значение этого свойства равно [DataType.Ticks](xref:StockSharp.Messages.DataType.Ticks). 

   После вызова метода **SubscribeTrades** сделки будут поступать в событии **SessionOnNewTrade**. 

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
8. Событие **SessionOnNewTrade**. 

   В обработчике события **SessionOnNewTrade** полученную информацию о сделке необходимо преобразовать во исходящее сообщение [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage). Обратите внимание, что сообщения [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage) используются как для сделок (собственных или анонимных), так и для заявок. Поэтому в сообщении уточняется, что оно относится к сделке \- [ExecutionType](xref:StockSharp.Messages.ExecutionMessage.ExecutionType) \= [ExecutionTypes.Tick](xref:StockSharp.Messages.ExecutionTypes.Tick). 

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
9. Код работы с заявками (отмена\-регистрация), как и полный код адаптера, можно получить в репозитарии [GitHub\/StockSharp](https://github.com/StockSharp/StockSharp). 
