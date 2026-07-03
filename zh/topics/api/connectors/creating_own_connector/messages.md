# 消息列表

消息用于承载市场数据、交易或命令的信息。下面列出连接器中使用的主要消息。

| 消息 | 描述 |
| --- | --- |
| [Message](xref:StockSharp.Messages.Message) | 抽象消息类，其他消息类都从它继承。包含消息时间、消息类型 [MessageTypes](xref:StockSharp.Messages.MessageTypes)，以及创建该消息的适配器信息。 |
| [BoardMessage](xref:StockSharp.Messages.BoardMessage) | 包含电子交易板信息。 |
| [CandleMessage](xref:StockSharp.Messages.CandleMessage) | 抽象消息类，包含 K线的一般信息。特定 K线类型的消息类从它继承：[TimeFrameCandleMessage](xref:StockSharp.Messages.TimeFrameCandleMessage)、[TickCandleMessage](xref:StockSharp.Messages.TickCandleMessage)、[VolumeCandleMessage](xref:StockSharp.Messages.VolumeCandleMessage)、[RangeCandleMessage](xref:StockSharp.Messages.RangeCandleMessage)、[PnFCandleMessage](xref:StockSharp.Messages.PnFCandleMessage) 和 [RenkoCandleMessage](xref:StockSharp.Messages.RenkoCandleMessage)。 |
| [ConnectMessage](xref:StockSharp.Messages.ConnectMessage) | 用作建立连接的命令，也用作表示连接成功或连接错误的传入消息。 |
| [DisconnectMessage](xref:StockSharp.Messages.DisconnectMessage) | 用作断开连接的命令，也用作表示连接中断的传入消息。 |
| [ErrorMessage](xref:StockSharp.Messages.ErrorMessage) | 错误消息。 |
| [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage) | 见下文说明。 |
| [Level1ChangeMessage](xref:StockSharp.Messages.Level1ChangeMessage) | 包含某种 Level1 字段值的信息。可用字段类型在 [Level1Fields](xref:StockSharp.Messages.Level1Fields) 枚举中定义。 |
| [NewsMessage](xref:StockSharp.Messages.NewsMessage) | 包含新闻信息。 |
| [OrderCancelMessage](xref:StockSharp.Messages.OrderCancelMessage) | 包含取消订单所需的信息。另有 [OrderGroupCancelMessage](xref:StockSharp.Messages.OrderGroupCancelMessage) 消息，用于按筛选条件取消一组订单。 |
| [OrderRegisterMessage](xref:StockSharp.Messages.OrderRegisterMessage) | 包含注册订单所需的信息。 |
| [OrderStatusMessage](xref:StockSharp.Messages.OrderStatusMessage) | 请求当前已注册订单和成交的信息。 |
| [OrderReplaceMessage](xref:StockSharp.Messages.OrderReplaceMessage) | 包含替换订单所需的信息。 |
| [PortfolioMessage](xref:StockSharp.Messages.PortfolioMessage) | 包含投资组合信息。 |
| [PortfolioLookupMessage](xref:StockSharp.Messages.PortfolioLookupMessage) | 按指定条件请求投资组合信息。请求结果通过 [PortfolioMessage](xref:StockSharp.Messages.PortfolioMessage) 返回。 |
| [PositionChangeMessage](xref:StockSharp.Messages.PositionChangeMessage) | 包含持仓信息，以及某个持仓属性发生变化的信息。属性类型在 [PositionChangeTypes](xref:StockSharp.Messages.PositionChangeTypes) 枚举中定义。 |
| [QuoteChangeMessage](xref:StockSharp.Messages.QuoteChangeMessage) | 包含订单簿报价信息。 |
| [ResetMessage](xref:StockSharp.Messages.ResetMessage) | 重置状态。 |
| [SecurityMessage](xref:StockSharp.Messages.SecurityMessage) | 包含交易品种信息。 |
| [SecurityLookupMessage](xref:StockSharp.Messages.SecurityLookupMessage) | 按指定条件请求交易品种列表。请求结果通过 [SecurityMessage](xref:StockSharp.Messages.SecurityMessage) 返回。 |
| [BoardStateMessage](xref:StockSharp.Messages.BoardStateMessage) | 包含交易时段状态变化的信息。可用的时段状态值在 [SessionStates](xref:StockSharp.Messages.SessionStates) 枚举中定义。 |
| [TimeMessage](xref:StockSharp.Messages.TimeMessage) | 包含当前时间信息。 |
| [DataTypeLookupMessage](xref:StockSharp.Messages.DataTypeLookupMessage) | 请求支持的数据类型列表。请求结果通过 [DataTypeInfoMessage](xref:StockSharp.Messages.DataTypeInfoMessage) 返回。 |
| [DataTypeInfoMessage](xref:StockSharp.Messages.DataTypeInfoMessage) | 包含受支持数据类型的信息。 |
| [SubscriptionResponseMessage](xref:StockSharp.Messages.SubscriptionResponseMessage) | 对订阅或取消订阅请求的响应。如果请求执行出错，错误说明会写入 [SubscriptionResponseMessage.Error](xref:StockSharp.Messages.SubscriptionResponseMessage.Error)。 |
| [SubscriptionOnlineMessage](xref:StockSharp.Messages.SubscriptionOnlineMessage) | 表示订阅已切换到在线状态的消息。 |
| [SubscriptionFinishedMessage](xref:StockSharp.Messages.SubscriptionFinishedMessage) | 表示已收到全部必要数据、订阅结束的消息。 |

[ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage) 是一种通用消息，可用于传递与订单和成交相关的各种交易所信息：逐笔成交、订单日志、自有订单和自有成交。

消息中的信息类型由 [ExecutionMessage.ExecutionType](xref:StockSharp.Messages.ExecutionMessage.ExecutionType) 属性值决定：

- [ExecutionTypes.Tick](xref:StockSharp.Messages.ExecutionTypes.Tick) - 逐笔成交。
- [ExecutionTypes.Transaction](xref:StockSharp.Messages.ExecutionTypes.Transaction) - 交易事务（自有成交或订单信息）。
- [ExecutionTypes.OrderLog](xref:StockSharp.Messages.ExecutionTypes.OrderLog) - 订单日志。

如果使用 [ExecutionTypes.Transaction](xref:StockSharp.Messages.ExecutionTypes.Transaction) 类型，则表示自有订单或自有成交。此时，如果消息包含订单信息，[ExecutionMessage.HasOrderInfo](xref:StockSharp.Messages.ExecutionMessage.HasOrderInfo) 属性为 true；如果包含成交信息，[ExecutionMessage.HasTradeInfo](xref:StockSharp.Messages.ExecutionMessage.HasTradeInfo) 属性为 true。注意，*自有成交*同时包含成交本身以及与该成交关联的订单信息。因此在这种情况下，上述两个属性都会为 true。这些属性可用于区分包含自有成交和自有订单的消息。
