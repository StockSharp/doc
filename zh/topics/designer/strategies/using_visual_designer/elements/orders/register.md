# 注册订单

![Designer Position opening 00](../../../../../../images/designer_position_opening_00.png)

“Order Registration”组件用于为所选证券提交交易订单。

## 输入端口

- **Instrument** – 订单所使用的证券。
- **Price** – 指定限价订单的价格。
- **Trigger** – 订单的激活信号，可以接收除 `False` 以外的任意值。
- **Volume** – 订单中的证券数量。
- **Portfolio** – 提交订单所使用的投资组合。

## 输出端口

- **Order** – 已提交订单的信息。
- **Error** – 注册订单时发生的错误信息。
- **Transaction** – 该订单产生的成交信息。
- **Cancellation** – 表示订单已撤销的信号。
- **Executed** – 表示订单已完全成交的信号。
- **Completed** – 将订单错误、撤销或完全成交事件合并在一起的信号。

## 参数

- **Direction** – 指定订单方向为买入还是卖出。
- **Market Order** – 指定订单是否为市价订单。
- **Zero Price** – 如果价格设置为零，则将订单注册为市价订单。
- **Lifetime** – 限价订单保持有效的时长。

## 条件订单设置

**Conditional Order** – 带有附加条件的订单。这些条件根据当前市场状况决定何时将订单提交到交易系统。

![Designer Conditional Application](../../../../../../images/designer_conditional_application.png)

- **Connection** – 用于提交订单的连接。
- **Stop Order Type** – 止损订单的类型。
- **Result** – 已执行止损订单的结果。
- **Instrument Identifier** – 当止损订单的条件与另一证券相关时，该证券的标识符。
- **Stop Price Condition** – 止损价格条件。用于“Stop price for another instrument”等订单。
- **Stop Price** – 设置止损订单触发条件的止损价格。
- **Stop-Limit Price** – 与 Stop Price 类似，但仅用于“Take-profit and stop-limit”类型的订单。
- **Stop-Limit at Market Price** – 指定“Stop-Limit”订单是否按市价执行。
- **Condition Check Interval** – 仅在指定时间段内检查订单条件的时间间隔（如果为 null，则不检查）。用于“Take-profit and stop-limit”和“Take-profit and stop-limit by order”类型。
- **Conditional Order Execution Identifier** – 基于成交条件的条件订单标识符。
- **Direction of Conditional Order by Execution** – 基于成交条件的条件订单方向。
- **Activation on Partial Execution** – 是否考虑订单的部分成交。条件订单部分成交时，将激活“on-execution”订单。
- **Executed Volume** – 使用订单的已成交数量作为提交止损订单的数量。“on-execution”订单中的证券数量取自条件订单的已成交数量。
- **Price of Linked Order** – 关联限价订单的价格。
- **Withdrawal on Partial Execution** – 指定关联限价订单部分成交时是否撤销止损订单。
- **Offset from Maximum** – 相对于最近一笔成交最高（最低）价格的偏移量。
- **Protective Spread** – 保护价差的大小。
- **Take-Profit at Market Price** – 指定“Take-Profit”订单是否按市价执行。

## 注意

直接操作订单是一种低层级的持仓管理方式。对于更高层级的管理，建议使用“Modify Position”组件，详见 [Modify Position](../positions/modify.md)。

## 另请参阅

[Modify Position](../positions/modify.md)
