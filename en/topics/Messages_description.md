# Description

Message contains information on market data, transactions or command. Below is a list of key messages, used in the [S\#.API](StockSharpAbout.md). 

| Message
                                                                                    | Description
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        |
| -------------------------------------------------------------------------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| [Message](../api/StockSharp.Messages.Message.html)
                                         | Abstract class of messages, from which other messages classes are inheriting. Contains information on time, type [MessageTypes](../api/StockSharp.Messages.MessageTypes.html) and local time, when has created the message.
                                                                                                                                                                                                                                                                                                                                                                                        |
| [BoardMessage](../api/StockSharp.Messages.BoardMessage.html)
                               | Contains information on electronic trade board.
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    |
| [CandleMessage](../api/StockSharp.Messages.CandleMessage.html)
                             | Abstract class of message, containing general information on candle. Message classes for specific candle types are inheriting from this class: [TimeFrameCandleMessage](../api/StockSharp.Messages.TimeFrameCandleMessage.html), [TickCandleMessage](../api/StockSharp.Messages.TickCandleMessage.html), [VolumeCandleMessage](../api/StockSharp.Messages.VolumeCandleMessage.html), [RangeCandleMessage](../api/StockSharp.Messages.RangeCandleMessage.html), [PnFCandleMessage](../api/StockSharp.Messages.PnFCandleMessage.html) and [RenkoCandleMessage](../api/StockSharp.Messages.RenkoCandleMessage.html). 
 |
| [ConnectMessage](../api/StockSharp.Messages.ConnectMessage.html)
                           | Used as command for establishing connection, as well as for incoming message on successful connection or connection error.
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         |
| [DisconnectMessage](../api/StockSharp.Messages.DisconnectMessage.html)
                     | Used as command for disconnection, as well as for outgoing message on successful disconnection.
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    |
| [ErrorMessage](../api/StockSharp.Messages.ErrorMessage.html)
                               | Error message.
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     |
| [ExecutionMessage](../api/StockSharp.Messages.ExecutionMessage.html)
                       | See description below.
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             |
| [Level1ChangeMessage](../api/StockSharp.Messages.Level1ChangeMessage.html)
                 | The message contains information on value of the level1 field of the certain type. Available field types are determined in the listing [Level1Fields](../api/StockSharp.Messages.Level1Fields.html).
                                                                                                                                                                                                                                                                                                                                                                                                               |
| [NewsMessage](../api/StockSharp.Messages.NewsMessage.html)
                                 | The message contains information on news.
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          |
| [OrderCancelMessage](../api/StockSharp.Messages.OrderCancelMessage.html)
                   | The message contains information for order cancelling. Also, there is the [OrderGroupCancelMessage](../api/StockSharp.Messages.OrderGroupCancelMessage.html) message, containing information for cancelling a group of orders by filter.
                                                                                                                                                                                                                                                                                                                                                                           |
| [OrderRegisterMessage](../api/StockSharp.Messages.OrderRegisterMessage.html)
               | The message contains information for orders registering.
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           |
| [OrderStatusMessage](../api/StockSharp.Messages.OrderStatusMessage.html)
                   | The message contains information on current registered orders and trades.
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          |
| [OrderReplaceMessage](../api/StockSharp.Messages.OrderReplaceMessage.html)
                 | The message contains information for order replacement.
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            |
| [PortfolioMessage](../api/StockSharp.Messages.PortfolioMessage.html)
                       | Contains portfolio information.
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    |
| [PortfolioLookupMessage](../api/StockSharp.Messages.PortfolioLookupMessage.html)
           | Requests portfolio information according to the given criteria. The request result returns by means of the [PortfolioMessage](../api/StockSharp.Messages.PortfolioMessage.html) message.
                                                                                                                                                                                                                                                                                                                                                                                                                           |
| [PositionChangeMessage](../api/StockSharp.Messages.PositionChangeMessage.html)
             | Contains position information. Containing information on change of the position's certain feature. The feature type is determined in the listing [PositionChangeTypes](../api/StockSharp.Messages.PositionChangeTypes.html).
                                                                                                                                                                                                                                                                                                                                                                                       |
| [QuoteChangeMessage](../api/StockSharp.Messages.QuoteChangeMessage.html)
                   | Contains information on the order book quotes.
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     |
| [ResetMessage](../api/StockSharp.Messages.ResetMessage.html)
                               | Status reset.
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                      |
| [SecurityMessage](../api/StockSharp.Messages.SecurityMessage.html)
                         | Contains instrument information.
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                   |
| [SecurityLookupMessage](../api/StockSharp.Messages.SecurityLookupMessage.html)
             | Requests list of instruments according to the given criteria. The request result returns by means of the [SecurityMessage](../api/StockSharp.Messages.SecurityMessage.html).
                                                                                                                                                                                                                                                                                                                                                                                                                                       |
| [BoardStateMessage](../api/StockSharp.Messages.BoardStateMessage.html)
                     | Contains information on trading session statute changes. Available values of the session statute are determined in the listing [SessionStates](../api/StockSharp.Messages.SessionStates.html).
                                                                                                                                                                                                                                                                                                                                                                                                                     |
| [TimeMessage](../api/StockSharp.Messages.TimeMessage.html)
                                 | Contains current time information.
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 |
| [TimeFrameLookupMessage](../api/StockSharp.Messages.TimeFrameLookupMessage.html)
           | Requests list of supported timeframes. The request result returns by means of the [TimeFrameInfoMessage](../api/StockSharp.Messages.TimeFrameInfoMessage.html).
                                                                                                                                                                                                                                                                                                                                                                                                                                                    |
| [TimeFrameInfoMessage](../api/StockSharp.Messages.TimeFrameInfoMessage.html)
               | Timeframes search result message.
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  |
| [SubscriptionResponseMessage](../api/StockSharp.Messages.SubscriptionResponseMessage.html)
 | A response of subscription request (subscribe or unsubscribe). In case of error, a description will be put into [Error](../api/StockSharp.Messages.SubscriptionResponseMessage.Error.html).
                                                                                                                                                                                                                                                                                                                                                                                                                        |
| [SubscriptionOnlineMessage](../api/StockSharp.Messages.SubscriptionOnlineMessage.html)
     | Message means a subscription goes online.
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          |
| [SubscriptionFinishedMessage](../api/StockSharp.Messages.SubscriptionFinishedMessage.html)
 | Message means a subscription finished in case of received all necessary data.
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                      |

Message [ExecutionMessage](../api/StockSharp.Messages.ExecutionMessage.html) is a universal message, allowing transmission of various information, related to orders, own trades, tick trades and order log.

Type of information in the message is determined by value of the property [ExecutionType](../api/StockSharp.Messages.ExecutionMessage.ExecutionType.html): 

- [Tick](../api/StockSharp.Messages.ExecutionTypes.Tick.html) \- tick trade.
- [Transaction](../api/StockSharp.Messages.ExecutionTypes.Transaction.html) \- transaction (information on own trade or order).
- [OrderLog](../api/StockSharp.Messages.ExecutionTypes.OrderLog.html) \- order log.

If the [Transaction](../api/StockSharp.Messages.ExecutionTypes.Transaction.html) type is used, it is about own orders or trades. At that, if the message contains the order information, the feature [HasOrderInfo](../api/StockSharp.Messages.ExecutionMessage.HasOrderInfo.html) \= true, if there is information on trade, the feature [HasTradeInfo](../api/StockSharp.Messages.ExecutionMessage.HasTradeInfo.html) \= true. Please note, that *own trade* contains information on the trade itself and on the order, related to this trade. Therefore, in this case the abovementioned features have the true value. These features allow differentiating the messages with own trades and orders. 

[S\#](StockSharpAbout.md) contains a set of extension methods to convert trade objects to messages and vice versa. For example, order [Order](../api/StockSharp.BusinessEntities.Order.html) can be converted into message [ExecutionMessage](../api/StockSharp.Messages.ExecutionMessage.html), and vice verse, as it is shown in the next code portion. 

```cs
	var security \= new Security() {Id \= "ESM6@BATS" };
	
	\/\/ sample Order's instance
	var order \= new Order();
	
	\/\/ converting to message
	var message \= order.ToMessage();
	
	\/\/ and back to Order's instance
	var order1 \= message.ToOrder(security);
```
