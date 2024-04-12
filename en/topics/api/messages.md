# Messages

The messages mechanism is an internal logical layer of the architecture [S\#](../api.md), ensuring interaction of various platform elements by the standard protocol. 

The messages mechanism includes three key elements: it is the [Message](xref:StockSharp.Messages.Message) message itself, message adapter [MessageAdapter](xref:StockSharp.Messages.MessageAdapter) and transport channel [IMessageChannel](xref:StockSharp.Messages.IMessageChannel). 

- **Messages** acts as information transmitting agent. Messages feature their own type [MessageTypes](xref:StockSharp.Messages.MessageTypes). A certain class corresponds to each type of message. In turn, all classes of messages inherit from the abstract class [Message](xref:StockSharp.Messages.Message), which provides descendants with such properties, as message type [Message.Type](xref:StockSharp.Messages.Message.Type) and [Message.LocalTime](xref:StockSharp.Messages.Message.LocalTime) \- local time when the message was created\/received. 

  The messages can be *outgoing* and *incoming*. 
  - The *outgoing* messages \- the messages, sent to the external system. Usually those are the commands, generated by the software, for example, the message [ConnectMessage](xref:StockSharp.Messages.ConnectMessage) \- the command, requesting connection with server. 
  - The *incoming* messages \- the messages, coming from the external system. Those are the messages, transmitting information on market data, transactions, portfolio, connection events etc. For example, the [QuoteChangeMessage](xref:StockSharp.Messages.QuoteChangeMessage) message \- transmits information on change of the *order book*. 
- **Message adapter** plays role of an *intermediary* between the trading system and the software. Each type of connector has its own adapter class (\-es), inherited from the abstract class [MessageAdapter](xref:StockSharp.Messages.MessageAdapter). 

  The adapter performs two main functions: 
  1. Converts outgoing messages into commands of the specific trading system.
  2. Converts information from the trading system (connection, market data, transactions etc.) in to incoming messages.
- **Transport channel** \- ensures synchronization of incoming and outgoing messages. 

## Recommended content

[Description](messages/description.md)

[Adapters](messages/adapters.md)

[Storage](messages/storage.md)

[Networking](messages/networking.md)