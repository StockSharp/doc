# 源代码

[S#](../api.md) 的开源代码分布在多个仓库中。[StockSharp 核心仓库](https://github.com/StockSharp/StockSharp) 包含消息模型、业务实体、通用连接器抽象、算法、测试工具和其他平台基础组件。面向特定提供商的连接器实现不存放在核心仓库中。

所有面向特定提供商的开源连接器都在 [StockSharp/Connectors](https://github.com/StockSharp/Connectors) 中维护。每个连接器都是独立的 .NET 项目，仓库还包含用于统一构建的 `Connectors.slnx`。

独立的浏览器图表引擎和 Web 终端图表组件在 [StockSharp/Charts](https://github.com/StockSharp/Charts) 中维护。请参阅 [JavaScript 图表](../api/graphical_user_interface/charts/javascript_charts.md)。

[GitHub 使用说明](https://docs.github.com/zh/get-started/start-your-journey/hello-world)

提供源代码的组件包括：

- 用于创建自定义连接的通用类。
- 市场数据存储格式。
- 交易仿真器。
- 历史数据仿真器（回测器）。
- 技术分析指标（超过 140 种）。
- 盈亏、滑点和延迟计算算法。
- 用于构建任意时间周期K线以及非时间型K线（逐笔、范围等）的算法。
- 日志记录。
- 导入和导出。

购买后可以获得所有闭源组件以及现成程序的源代码。有关源代码价格的详细信息，请参阅 [源代码价格](https://stocksharp.com/zh/store/?groups=22)。

## 推荐内容

[安装说明](../api/setup.md)
