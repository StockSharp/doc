# Описание

Сообщение несет информацию о рыночных данных, транзакциях или команде. Ниже приведен список основных сообщений, которые используются в [S\#.API](StockSharpAbout.md). 

| Сообщение
                                                                                  | Описание
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          |
| -------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| [Message](../api/StockSharp.Messages.Message.html)
                                         | Абстрактный класс сообщения, от которого наследуют классы других сообщений. Содержит информацию о времени, типе [MessageTypes](../api/StockSharp.Messages.MessageTypes.html) и адаптере, который создал сообщение.
                                                                                                                                                                                                                                                                                                                                                                                                |
| [BoardMessage](../api/StockSharp.Messages.BoardMessage.html)
                               | Содержит информацию об электронной торговой площадке.
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             |
| [CandleMessage](../api/StockSharp.Messages.CandleMessage.html)
                             | Абстрактный класс сообщения, который содержит общую информацию о свече. От этого класса наследуют классы сообщений для конкретных типов свечей: [TimeFrameCandleMessage](../api/StockSharp.Messages.TimeFrameCandleMessage.html), [TickCandleMessage](../api/StockSharp.Messages.TickCandleMessage.html), [VolumeCandleMessage](../api/StockSharp.Messages.VolumeCandleMessage.html), [RangeCandleMessage](../api/StockSharp.Messages.RangeCandleMessage.html), [PnFCandleMessage](../api/StockSharp.Messages.PnFCandleMessage.html) и [RenkoCandleMessage](../api/StockSharp.Messages.RenkoCandleMessage.html). 
 |
| [ConnectMessage](../api/StockSharp.Messages.ConnectMessage.html)
                           | Используется в качестве команды установления соединения, а также для входящего сообщения об удачном соединении или ошибке соединения.
                                                                                                                                                                                                                                                                                                                                                                                                                                                                             |
| [DisconnectMessage](../api/StockSharp.Messages.DisconnectMessage.html)
                     | Используется в качестве команды разрыва соединения, а также для входящего сообщения о разрыве соединения.
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         |
| [ErrorMessage](../api/StockSharp.Messages.ErrorMessage.html)
                               | Сообщение об ошибке.
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              |
| [ExecutionMessage](../api/StockSharp.Messages.ExecutionMessage.html)
                       | Описание см. ниже.
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                |
| [Level1ChangeMessage](../api/StockSharp.Messages.Level1ChangeMessage.html)
                 | Сообщение содержит информацию о значении поля Level1 определенного типа. Доступные типы полей определены в перечислении [Level1Fields](../api/StockSharp.Messages.Level1Fields.html).
                                                                                                                                                                                                                                                                                                                                                                                                                             |
| [NewsMessage](../api/StockSharp.Messages.NewsMessage.html)
                                 | Сообщение содержит информацию о новости.
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          |
| [OrderCancelMessage](../api/StockSharp.Messages.OrderCancelMessage.html)
                   | Сообщение содержит информацию для отмены заявки. Также существует сообщение [OrderGroupCancelMessage](../api/StockSharp.Messages.OrderGroupCancelMessage.html), которое содержит информацию для отмены группы заявок по фильтру.
                                                                                                                                                                                                                                                                                                                                                                                  |
| [OrderRegisterMessage](../api/StockSharp.Messages.OrderRegisterMessage.html)
               | Сообщение содержит информацию для регистрации заявки.
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             |
| [OrderStatusMessage](../api/StockSharp.Messages.OrderStatusMessage.html)
                   | Сообщение запрашивает информацию о текущих зарегистрированных заявках и сделках.
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  |
| [OrderReplaceMessage](../api/StockSharp.Messages.OrderReplaceMessage.html)
                 | Сообщение содержит информацию для замены заявки.
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  |
| [PortfolioMessage](../api/StockSharp.Messages.PortfolioMessage.html)
                       | Содержит информацию о портфеле.
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                   |
| [PortfolioLookupMessage](../api/StockSharp.Messages.PortfolioLookupMessage.html)
           | Запрашивает информацию о портфелях по заданному критерию. Результат запроса возвращается при помощи сообщения [PortfolioMessage](../api/StockSharp.Messages.PortfolioMessage.html).
                                                                                                                                                                                                                                                                                                                                                                                                                               |
| [PositionChangeMessage](../api/StockSharp.Messages.PositionChangeMessage.html)
             | Содержит информацию о позиции. Содержит информацию об изменении определенного свойства позиции. Тип свойства определяется в перечислении [PositionChangeTypes](../api/StockSharp.Messages.PositionChangeTypes.html).
                                                                                                                                                                                                                                                                                                                                                                                              |
| [QuoteChangeMessage](../api/StockSharp.Messages.QuoteChangeMessage.html)
                   | Содержит информацию о котировках стакана.
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         |
| [ResetMessage](../api/StockSharp.Messages.ResetMessage.html)
                               | Сброс состояния.
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  |
| [SecurityMessage](../api/StockSharp.Messages.SecurityMessage.html)
                         | Содержит информацию об инструменте.
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               |
| [SecurityLookupMessage](../api/StockSharp.Messages.SecurityLookupMessage.html)
             | Запрашивает список инструментов по заданному критерию. Результат запроса будет возвращен при помощи сообщения [SecurityMessage](../api/StockSharp.Messages.SecurityMessage.html).
                                                                                                                                                                                                                                                                                                                                                                                                                                 |
| [BoardStateMessage](../api/StockSharp.Messages.BoardStateMessage.html)
                     | Содержит информацию об изменении состояния торговой сессии. Доступные значения состояния сессии определены в перечислении [SessionStates](../api/StockSharp.Messages.SessionStates.html).
                                                                                                                                                                                                                                                                                                                                                                                                                         |
| [TimeMessage](../api/StockSharp.Messages.TimeMessage.html)
                                 | Содержит информацию о текущем времени.
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            |
| [TimeFrameLookupMessage](../api/StockSharp.Messages.TimeFrameLookupMessage.html)
           | Запрашивает список поддерживаемых таймфреймов. Результат запроса будет возвращен при помощи сообщения [TimeFrameInfoMessage](../api/StockSharp.Messages.TimeFrameInfoMessage.html).
                                                                                                                                                                                                                                                                                                                                                                                                                               |
| [TimeFrameInfoMessage](../api/StockSharp.Messages.TimeFrameInfoMessage.html)
               | Содержит информацию о поддерживаемых таймфреймах.
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 |
| [SubscriptionResponseMessage](../api/StockSharp.Messages.SubscriptionResponseMessage.html)
 | Ответ на запрос подписки или отписки. В случае ошибки выполнения запроса, описание ошибки заполняется [Error](../api/StockSharp.Messages.SubscriptionResponseMessage.Error.html).
                                                                                                                                                                                                                                                                                                                                                                                                                                 |
| [SubscriptionOnlineMessage](../api/StockSharp.Messages.SubscriptionOnlineMessage.html)
     | Сообщение о переходе подписки в online состояние.
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 |
| [SubscriptionFinishedMessage](../api/StockSharp.Messages.SubscriptionFinishedMessage.html)
 | Сообщение об окончании подписки в случае получения всех необходимых данных.
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       |

Сообщение [ExecutionMessage](../api/StockSharp.Messages.ExecutionMessage.html) является универсальным сообщением, которое позволяет передавать различную биржевую информацию, связанную с заявками и сделками: тиковые сделки, ордерлоги, собственные заявки и сделки.

Тип информации в сообщении определяется значением свойства [ExecutionType](../api/StockSharp.Messages.ExecutionMessage.ExecutionType.html): 

- [Tick](../api/StockSharp.Messages.ExecutionTypes.Tick.html) \- тиковая сделка.
- [Transaction](../api/StockSharp.Messages.ExecutionTypes.Transaction.html) \- транзакция (информация о собственной сделке или заявке).
- [OrderLog](../api/StockSharp.Messages.ExecutionTypes.OrderLog.html) \- лог заявок.

Если используется тип [Transaction](../api/StockSharp.Messages.ExecutionTypes.Transaction.html), то речь идет о собственных заявках или сделках. При этом, если сообщение содержит информацию о заявке то, свойство [HasOrderInfo](../api/StockSharp.Messages.ExecutionMessage.HasOrderInfo.html) \= true, если есть информация о сделке то, свойство [HasTradeInfo](../api/StockSharp.Messages.ExecutionMessage.HasTradeInfo.html) \= true. Обратите внимание, что *собственная сделка* содержит информацию как о самой сделке, так и о заявке, связанной с этой сделкой. Поэтому в этом случае вышеприведенные свойства имеют значение true. Эти свойства позволяют дифференцировать сообщения с собственными сделками и заявками. 

[S\#](StockSharpAbout.md) содержит набор методов расширения для преобразования торговых объектов в сообщения и наоборот. Например, можно преобразовать заявку [Order](../api/StockSharp.BusinessEntities.Order.html) в сообщение [ExecutionMessage](../api/StockSharp.Messages.ExecutionMessage.html), а также выполнить обратную операцию, как это показано в следующем фрагменте кода. 

```cs
	var security = new Security() {Id = "RIM6#FORTS" };
	
	// Для демонстрационных целей детали создания заявки опущены
	var order = new Order();
	
	// Конвертируем заявку в сообщение
	var message = order.ToMessage();
	
	// Выполняем обратное преобразование
	var order1 = message.ToOrder(security);
```
