# 回测设置

默认情况下，**Properties** 面板会最小化显示在策略选项卡的右侧。该面板以表格形式列出仿真或实盘交易属性。选中某个属性后，表格底部会显示该属性的详细说明。所有属性按以下分组排列：

![Designer Properties emulation 00](../../../../images/designer_properties_emulation_00.png)

**Settings**

- **Market data** – 数据存储。
- **Storage format** – 存储格式。
- **Data type** – 数据类型。
- **Time frame** – 使用指定时间周期的蜡烛。
- **Maximum quote volume in generated depth** – 所生成市场深度中的最大报价数量。
- **Interval** – 时间间隔。
- **Unrealized P\/L** – 未实现盈亏的重新计算间隔。
- **Trades** – 指定使用哪些成交。
- **Marked depth** – 指定使用哪些市场深度。
- **Order log** – 是否使用订单日志。
- **Number of strategies** – 同时测试的策略数量。
- **Logging level** – 日志记录级别。
- **Combine on touch** – 仿真期间，当成交价格触及订单价格（即等于订单价格）时撮合订单。
- **Marked depth (lifetime)** – 市场深度在仿真器中的最长保留时间。如果在此期间没有更新，则删除该市场深度。当数据存在缺口时，可以使用此属性清除旧的市场深度。
- **Errors percentage** – 注册新订单时产生错误的百分比。取值范围为 0（不会发生错误）到 100。
- **Latency** – 已注册订单的最小延迟值。
- **Reregistering** – 是否支持将订单重新注册作为单次交易操作。
- **Buffering period** – 按指定间隔将响应合并到一个数据包中发送，用于仿真网络延迟和交易所核心的缓冲处理。
- **Order ID** – 仿真器开始生成订单标识符的起始编号。
- **Trade ID** – 仿真器开始生成成交标识符的起始编号。
- **Transaction** – 仿真器开始生成订单事务标识符的起始编号。
- **Spread size** – 以价格步长表示的价差大小。根据逐笔成交生成市场深度时，使用该值指定价差。
- **Depth of book** – 根据逐笔成交生成的市场深度最大档数。
- **Number of volume steps** – 订单数量大于逐笔成交数量的数量步数，用于基于逐笔成交的测试。
- **Portfolios interval** – 投资组合重新计算的间隔。间隔为零时不执行重新计算。
- **Change time** – 将订单和成交的时间改为交易所时间。
- **Time zone** – 交易所所在时区的信息。
- **Price shift** – 相对于最近一笔成交的价格偏移，用于指定下一交易时段的最高价和最低价限制。
- **Add extra volume** – 注册大数量订单时，向市场深度添加额外数量。
- **[Commissions](../commissions.md)** – 佣金（经纪商佣金、交易所费用等）。

**Logging**

- **Logging level** – 此元素的日志记录级别。

**Setting**

- **[Risk management](../risk_management.md)** – 风险管理设置。

**Diagram parameters**

- **Security** \- 证券。
- **Portfolio** \- 投资组合。

如果未填写 **Diagram parameters**，仿真时将使用 **Emulation** 选项卡的 **Instrument** 字段中指定的证券，并默认使用测试投资组合作为投资组合。

## 推荐内容

[Chart](chart.md)
