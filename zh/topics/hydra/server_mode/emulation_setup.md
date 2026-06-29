# 仿真设置

在服务器模式下，可以启用仿真模式。

![hydra emulator start](../../../images/hydra_emulator_start.png)

在仿真模式下，[Hydra](../../hydra.md) 支持以下功能：

- 可以配置连接数据源所需的密钥，并在不同程序（[Designer](../../designer.md)、[Terminal](../../terminal.md)）中同时使用同一个连接。
- 如果市场数据源支持下载历史数据，则可以同时使用这些数据进行测试。
- 如果数据源支持实时数据，仿真模式可用于模拟交易。在此模式下，用户操作数据（订单注册、成交等）会直接传输到 Hydra，同时每个程序的操作会分别记录。例如，在 Terminal 中注册订单后，对该订单的修改仅在 Terminal 中可见，不会记录到 Designer 中。这样可以避免两个程序通过同一连接运行时发生冲突。
- 重要！在仿真模式下，交易及相关操作均按实时行情进行模拟。关闭该模式后，操作将进入真实交易环境。

该模式用于[策略测试](../../shell/user_interface/emulation.md)。

## 仿真参数

![hydra emulator prop](../../../images/hydra_emulator_prop.png)

- **Match on touch** \- 模拟撮合时，如果成交价等于订单价格，则撮合该订单。
- **Order book (time in force)** \- 模拟器中订单簿的最长有效时间。如果订单簿在指定时间内未更新，其值将被清除。数据存在缺口时，此参数用于删除陈旧的订单簿数据。
- **Percentage of errors** \- 注册新订单时产生错误的比例，取值范围为 0 到 100。
- **Latency** \- 已注册订单的最小延迟。
- **Re\-registration** \- 是否支持将订单重新注册作为单个事务处理。
- **Buffering period** \- 完整数据包的发送周期，用于模拟网络延迟并对交易所核心的处理进行缓冲。
- **Order ID** \- 模拟器生成订单标识符时使用的起始编号。
- **Trade identifier** \- 模拟器生成成交标识符时使用的起始编号。
- **Transaction** \- 模拟器生成订单事务标识符时使用的起始编号。
- **Spread size** \- 以价格步长表示的价差大小。从逐笔成交生成订单簿时，用于确定价差。
- **Order book depth** \- 根据逐笔成交生成的订单簿最大深度。
- **Number of volume steps** \- 订单量比逐笔成交量多出的数量步数，用于基于逐笔成交进行测试。
- **Portfolio interval** \- 重新计算投资组合数据的时间间隔。如果该值为 0，则不执行重新计算。
- **Adjust time** \- 将订单和成交时间调整为交易所时间。
- **Time zone** \- 交易所所在时区的信息。
- **Price shift** \- 相对于最新成交价的价格偏移，用于确定下一交易时段的最高价和最低价边界。
- **Add additional volume** \- 注册大额订单时，向订单簿中添加额外数量。
- **Trading session state** \- 检查交易时段状态。
- **Money** \- 检查资金余额。
- **Short** \- 是否允许开立空头头寸。
- **Storage** \- 存储。
