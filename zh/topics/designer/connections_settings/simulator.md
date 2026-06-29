# 模拟器

[Designer](../../designer.md) 支持在 **Simulation** 模式下运行已创建的策略。要配置 **Simulation**，请执行以下操作：

1. 单击 **Connect** ![Designer The quick access toolbar 00](../../../images/designer_quick_access_toolbar_00.png) 按钮旁的箭头，会显示 **Emulator settings** 按钮：

![Designer The connection settings 00](../../../images/designer_connection_settings_00.png)

2. 单击 **Emulator settings** 按钮，会打开 **Emulator settings** 窗口：

![Designer Properties emulation 00](../../../images/designer_properties_emulation_00.png)

1. **Simulator**

- **Use emulator** – 使用模拟器。
- **Instruments** – 证券。

2. **Settings**

- **Combine on touch** \- 仿真期间，当成交价触及订单价格（即两者相等）时撮合成交。
- **Market depth (lifetime)** \- 订单簿在模拟器中的最长有效时间。如果在此期间没有更新，订单簿将被清除。数据存在缺口时，可以使用此属性删除陈旧的订单簿。
- **Errors percentage** \- 注册新订单时的错误比例，取值范围为 0（完全无错误）到 100。
- **Latency** \- 已注册订单的最小延迟。
- **Reregistering** \- 是否支持将订单重新注册作为单个事务处理。
- **Buffering period** \- 将响应分批放入单个数据包中发送，以模拟网络延迟和交易所核心的缓冲处理。
- **Order ID** \- 模拟器开始生成订单标识符时使用的起始编号。
- **Trade ID** \- 模拟器开始生成成交标识符时使用的起始编号。
- **Transaction** \- 模拟器开始生成订单事务标识符时使用的起始编号。
- **Spread size** \- 以价格步长表示的价差大小。从逐笔成交生成订单簿时，用于确定价差。
- **Depth of book** \- 根据逐笔成交生成的订单簿最大深度。
- **Number of volume steps** \- 订单量比逐笔成交量多出的数量步数，用于基于逐笔成交进行测试。
- **Portfolios interval** \- 投资组合重新计算间隔。如果该值为 0，则不执行重新计算。
- **Change time** \- 将订单和成交时间调整为交易所时间。
- **Time zone** \- 交易所时区信息。
- **Price shift** \- 相对于最新成交价的价格偏移，用于确定下一交易时段的最高价和最低价边界。
- **Add extra volume** \- 注册大额订单时，向订单簿中添加额外数量。

## 推荐内容

[图表](../user_interface/components/chart.md)
