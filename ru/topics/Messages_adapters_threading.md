# Потоковая модель

Создаваемым адаптерам гарантируется, что все сообщения, приходящие в переопределенном методе [MessageAdapter.OnSendInMessage](xref:StockSharp.Messages.MessageAdapter.OnSendInMessage(StockSharp.Messages.Message))**(**[StockSharp.Messages.Message](xref:StockSharp.Messages.Message) message **)**, будут приходить в одном потоке. Поэтому дополнительной синхронизации не требуется, если используются общие данные только для входящих сообщений.

Исходящие сообщения отправляются через [MessageAdapter.SendOutMessage](xref:StockSharp.Messages.MessageAdapter.SendOutMessage(StockSharp.Messages.Message))**(**[StockSharp.Messages.Message](xref:StockSharp.Messages.Message) message **)** из тех потоков, из которых они получены. Класс [Connector](xref:StockSharp.Algo.Connector) автоматически добавит их в единую очередь внешних сообщений, и его события так же будут вызваны из одного потока.

Если используются данные как в при обработке входящих, так и при исходящих сообщений, то такие данные необходимо синхронизовать, используя стандартные возможности **C\#** (например, **lock**).
