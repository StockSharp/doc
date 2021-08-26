# Threading model

For the adapters you create, it is guaranteed that all messages arriving in the overridden [MessageAdapter.OnSendInMessage](../api/StockSharp.Messages.MessageAdapter.OnSendInMessage.html) method arrive in the same thread. Therefore, no additional synchronization is required if shared data is used only for incoming messages. 

Outgoing messages are sent via [MessageAdapter.SendOutMessage](../api/StockSharp.Messages.MessageAdapter.SendOutMessage.html) from the threads from which they are received. The [Connector](../api/StockSharp.Algo.Connector.html) class will automatically add them to a single external message queue, and its events will also be called from the same thread. 

If data is used both in processing incoming and outgoing messages, then such data should be synchronized using standard **C\#** features (for example, **lock**). 
