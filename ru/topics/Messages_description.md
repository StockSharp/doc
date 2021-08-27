# Описание

Сообщение несет информацию о рыночных данных, транзакциях или команде. Ниже приведен список основных сообщений, которые используются в [S\#.API](StockSharpAbout.md). 

| Сообщение
                                                                           | Описание
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                |
| ------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| [Message](xref:StockSharp.Messages.Message)
                                         | Абстрактный класс сообщения, от которого наследуют классы других сообщений. Содержит информацию о времени, типе [MessageTypes](xref:StockSharp.Messages.MessageTypes) и адаптере, который создал сообщение.
                                                                                                                                                                                                                                                                                                                                                             |
| [BoardMessage](xref:StockSharp.Messages.BoardMessage)
                               | Содержит информацию об электронной торговой площадке.
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                   |
| [CandleMessage](xref:StockSharp.Messages.CandleMessage)
                             | Абстрактный класс сообщения, который содержит общую информацию о свече. От этого класса наследуют классы сообщений для конкретных типов свечей: [TimeFrameCandleMessage](xref:StockSharp.Messages.TimeFrameCandleMessage), [TickCandleMessage](xref:StockSharp.Messages.TickCandleMessage), [VolumeCandleMessage](xref:StockSharp.Messages.VolumeCandleMessage), [RangeCandleMessage](xref:StockSharp.Messages.RangeCandleMessage), [PnFCandleMessage](xref:StockSharp.Messages.PnFCandleMessage) и [RenkoCandleMessage](xref:StockSharp.Messages.RenkoCandleMessage). 
 |
| [ConnectMessage](xref:StockSharp.Messages.ConnectMessage)
                           | Используется в качестве команды установления соединения, а также для входящего сообщения об удачном соединении или ошибке соединения.
                                                                                                                                                                                                                                                                                                                                                                                                                                   |
| [DisconnectMessage](xref:StockSharp.Messages.DisconnectMessage)
                     | Используется в качестве команды разрыва соединения, а также для входящего сообщения о разрыве соединения.
                                                                                                                                                                                                                                                                                                                                                                                                                                                               |
| [ErrorMessage](xref:StockSharp.Messages.ErrorMessage)
                               | Сообщение об ошибке.
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    |
| [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage)
                       | Описание см. ниже.
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                      |
| [Level1ChangeMessage](xref:StockSharp.Messages.Level1ChangeMessage)
                 | Сообщение содержит информацию о значении поля Level1 определенного типа. Доступные типы полей определены в перечислении [Level1Fields](xref:StockSharp.Messages.Level1Fields).
                                                                                                                                                                                                                                                                                                                                                                                          |
| [NewsMessage](xref:StockSharp.Messages.NewsMessage)
                                 | Сообщение содержит информацию о новости.
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                |
| [OrderCancelMessage](xref:StockSharp.Messages.OrderCancelMessage)
                   | Сообщение содержит информацию для отмены заявки. Также существует сообщение [OrderGroupCancelMessage](xref:StockSharp.Messages.OrderGroupCancelMessage), которое содержит информацию для отмены группы заявок по фильтру.
                                                                                                                                                                                                                                                                                                                                               |
| [OrderRegisterMessage](xref:StockSharp.Messages.OrderRegisterMessage)
               | Сообщение содержит информацию для регистрации заявки.
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                   |
| [OrderStatusMessage](xref:StockSharp.Messages.OrderStatusMessage)
                   | Сообщение запрашивает информацию о текущих зарегистрированных заявках и сделках.
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        |
| [OrderReplaceMessage](xref:StockSharp.Messages.OrderReplaceMessage)
                 | Сообщение содержит информацию для замены заявки.
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        |
| [PortfolioMessage](xref:StockSharp.Messages.PortfolioMessage)
                       | Содержит информацию о портфеле.
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         |
| [PortfolioLookupMessage](xref:StockSharp.Messages.PortfolioLookupMessage)
           | Запрашивает информацию о портфелях по заданному критерию. Результат запроса возвращается при помощи сообщения [PortfolioMessage](xref:StockSharp.Messages.PortfolioMessage).
                                                                                                                                                                                                                                                                                                                                                                                            |
| [PositionChangeMessage](xref:StockSharp.Messages.PositionChangeMessage)
             | Содержит информацию о позиции. Содержит информацию об изменении определенного свойства позиции. Тип свойства определяется в перечислении [PositionChangeTypes](xref:StockSharp.Messages.PositionChangeTypes).
                                                                                                                                                                                                                                                                                                                                                           |
| [QuoteChangeMessage](xref:StockSharp.Messages.QuoteChangeMessage)
                   | Содержит информацию о котировках стакана.
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               |
| [ResetMessage](xref:StockSharp.Messages.ResetMessage)
                               | Сброс состояния.
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        |
| [SecurityMessage](xref:StockSharp.Messages.SecurityMessage)
                         | Содержит информацию об инструменте.
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     |
| [SecurityLookupMessage](xref:StockSharp.Messages.SecurityLookupMessage)
             | Запрашивает список инструментов по заданному критерию. Результат запроса будет возвращен при помощи сообщения [SecurityMessage](xref:StockSharp.Messages.SecurityMessage).
                                                                                                                                                                                                                                                                                                                                                                                              |
| [BoardStateMessage](xref:StockSharp.Messages.BoardStateMessage)
                     | Содержит информацию об изменении состояния торговой сессии. Доступные значения состояния сессии определены в перечислении [SessionStates](xref:StockSharp.Messages.SessionStates).
                                                                                                                                                                                                                                                                                                                                                                                      |
| [TimeMessage](xref:StockSharp.Messages.TimeMessage)
                                 | Содержит информацию о текущем времени.
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  |
| [TimeFrameLookupMessage](xref:StockSharp.Messages.TimeFrameLookupMessage)
           | Запрашивает список поддерживаемых таймфреймов. Результат запроса будет возвращен при помощи сообщения [TimeFrameInfoMessage](xref:StockSharp.Messages.TimeFrameInfoMessage).
                                                                                                                                                                                                                                                                                                                                                                                            |
| [TimeFrameInfoMessage](xref:StockSharp.Messages.TimeFrameInfoMessage)
               | Содержит информацию о поддерживаемых таймфреймах.
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       |
| [SubscriptionResponseMessage](xref:StockSharp.Messages.SubscriptionResponseMessage)
 | Ответ на запрос подписки или отписки. В случае ошибки выполнения запроса, описание ошибки заполняется [Error](xref:StockSharp.Messages.SubscriptionResponseMessage.Error).
                                                                                                                                                                                                                                                                                                                                                                                              |
| [SubscriptionOnlineMessage](xref:StockSharp.Messages.SubscriptionOnlineMessage)
     | Сообщение о переходе подписки в online состояние.
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       |
| [SubscriptionFinishedMessage](xref:StockSharp.Messages.SubscriptionFinishedMessage)
 | Сообщение об окончании подписки в случае получения всех необходимых данных.
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             |

Сообщение [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage) является универсальным сообщением, которое позволяет передавать различную биржевую информацию, связанную с заявками и сделками: тиковые сделки, ордерлоги, собственные заявки и сделки.

Тип информации в сообщении определяется значением свойства [ExecutionType](xref:StockSharp.Messages.ExecutionMessage.ExecutionType): 

- [Tick](xref:StockSharp.Messages.ExecutionTypes.Tick) \- тиковая сделка.
- [Transaction](xref:StockSharp.Messages.ExecutionTypes.Transaction) \- транзакция (информация о собственной сделке или заявке).
- [OrderLog](xref:StockSharp.Messages.ExecutionTypes.OrderLog) \- лог заявок.

Если используется тип [Transaction](xref:StockSharp.Messages.ExecutionTypes.Transaction), то речь идет о собственных заявках или сделках. При этом, если сообщение содержит информацию о заявке то, свойство [HasOrderInfo](xref:StockSharp.Messages.ExecutionMessage.HasOrderInfo) \= true, если есть информация о сделке то, свойство [HasTradeInfo](xref:StockSharp.Messages.ExecutionMessage.HasTradeInfo) \= true. Обратите внимание, что *собственная сделка* содержит информацию как о самой сделке, так и о заявке, связанной с этой сделкой. Поэтому в этом случае вышеприведенные свойства имеют значение true. Эти свойства позволяют дифференцировать сообщения с собственными сделками и заявками. 

[S\#](StockSharpAbout.md) содержит набор методов расширения для преобразования торговых объектов в сообщения и наоборот. Например, можно преобразовать заявку [Order](xref:StockSharp.BusinessEntities.Order) в сообщение [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage), а также выполнить обратную операцию, как это показано в следующем фрагменте кода. 

```cs
	var security = new Security() {Id = "RIM6#FORTS" };
	
	// Для демонстрационных целей детали создания заявки опущены
	var order = new Order();
	
	// Конвертируем заявку в сообщение
	var message = order.ToMessage();
	
	// Выполняем обратное преобразование
	var order1 = message.ToOrder(security);
```
