# 回测/仿真

使用 [Strategy](xref:StockSharp.Algo.Strategies.Strategy) 编写的策略可以在三种模式下进行测试：

1. [历史数据](testing/historical_data.md) 测试。使用这种数据，可以进行市场分析以发现模式，也可以进行策略参数优化。
2. [随机数据](testing/random_data.md) 测试。一种用于初步策略测试以识别算法错误的便捷工具。或者用于按计划运行的自动化测试。
3. [模拟器](testing/simulator.md) 测试基于从真实交易系统连接收到的数据（例如，从 [OpenECry](connectors/stock_market/openecry.md)），但不进行实际订单注册（执行是基于接收到的订单簿模拟的）。

在使用所有三种模式时，最大的重点是策略代码（使用 [Strategy](xref:StockSharp.Algo.Strategies.Strategy) 编写）在从真实交易切换到测试再切换回来时不会发生改变。这是通过实现 [IConnector](xref:StockSharp.BusinessEntities.IConnector) 主接口来实现的，该接口是通往交易系统的网关。接口的使用方式——已经在 [API](../api.md) 部分展示过。在测试模式下，不是真正的交易系统，而是模拟系统将充当交易系统（取决于所选择的模式）。因此，策略代码永远不会知道它是在与真实交易所交易，还是在与模拟系统交易。
