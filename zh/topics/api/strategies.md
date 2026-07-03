# StockSharp 中的策略

## 介绍

StockSharp 提供了一个强大的基础设施，用于创建、测试和运行交易策略。开发算法交易策略的基础是基类 [Strategy](xref:StockSharp.Algo.Strategies.Strategy)，该基类提供了一组用于处理市场数据、执行交易操作和分析结果的标准函数和抽象。

## 导航

### 战略基础

- [策略中的市场数据订阅](strategies/subscriptions.md) - 使用策略中的市场数据订阅的详细指南。解释了如何创建和配置订阅、管理其生命周期以及监控其状态。

- [策略中的指标](strategies/indicators.md) - 关于在策略中使用技术分析指标的信息。涵盖将指标添加到策略中、控制其形成以及在交易逻辑中使用它们。

- [策略中的交易操作](strategies/trading_operations.md) - 关于在策略中执行交易操作的指南。描述了创建和发送订单、平仓以及监控其状态的方法。

- [持仓保护](strategies/take_profit_and_stop_loss.md) - 描述使用止盈和止损保护未平仓持仓的机制。研究持仓保护的本地和服务器方法。

- [策略参数](strategies/parameters.md) - 通过 [StrategyParam\<T\>](xref:StockSharp.Algo.Strategies.StrategyParam`1) 使用策略参数的指南。描述了如何创建可配置参数、设置它们在图形界面中的显示以及在优化中使用它们。

- [策略中的日志记录](strategies/logging.md) - 关于在策略中使用日志机制以跟踪和调试算法性能的指南。

### 高级功能

- [策略平台兼容性](strategies/compatibility.md) - 创建与各种 StockSharp 平台兼容的策略的建议：[Designer](../designer.md)、[Shell](../shell.md)、[Runner](../runner.md) 以及云端测试。

- [策略中的高级 API](strategies/high_level_api.md) - 描述用于简化订阅、指标、图表和持仓保护的高级方法。解释如何通过专注于交易逻辑来编写更清晰的代码。

- [在策略中使用图表](strategies/chart.md) - 策略数据可视化图表指南。解释如何访问图表、创建区域、添加元素以及呈现数据。

- [保存和加载设置](strategies/settings_saving_and_loading.md) - 通过 [Strategy.Save](xref:StockSharp.Algo.Strategies.Strategy.Save(Ecng.Serialization.SettingsStorage)) 和 [Strategy.Load](xref:StockSharp.Algo.Strategies.Strategy.Load(Ecng.Serialization.SettingsStorage)) 方法保存和加载策略设置的机制说明。

- [状态加载](strategies/orders_and_trades_loading.md) - 指导如何将先前执行的订单和交易加载到策略中，例如，在交易会话中重新启动策略时。

- [价格四舍五入](strategies/shrink_price.md) - 使用 [ShrinkPrice](xref:StockSharp.BusinessEntities.EntitiesExtensions.ShrinkPrice(StockSharp.BusinessEntities.Security,System.Decimal)) 方法在策略中正确四舍五入价格的指南。

- [单位类型](strategies/unit_type.md) - [单位](xref:StockSharp.Messages.Unit) 数据类型的描述，用于简化对百分比、点数或基点等数量的算术运算。

- [事件模型](strategies/event_model.md) - 基于[IMarketRule](xref:StockSharp.Algo.IMarketRule)的策略事件模型说明。涵盖创建规则以响应市场事件、组合条件以及管理规则生命周期。

## 战略制定入门

要开始制定自己的策略，建议：

1. 熟悉使用策略的基础，以理解 StockSharp 中策略的一般原理。

2. 学习 [策略中的市场数据订阅](strategies/subscriptions.md) 部分，以了解接收和处理市场数据的机制。

3. 请查阅[策略中的指标](strategies/indicators.md)部分，以了解如何使用技术分析指标。

4. 探索[策略中的交易操作](strategies/trading_operations.md)部分，以了解交易操作机制。

5. 查看[策略参数](strategies/parameters.md)部分以了解策略配置机制。

6. 熟悉 [策略中的高级 API](strategies/high_level_api.md) 部分，以使用内置的高级函数简化策略代码。

## 策略测试

StockSharp 提供了多种测试策略的方法：

- **历史数据测试** - 允许在历史数据上评估策略的有效性。
- **参数优化** - 帮助找到最佳策略参数值。
- **虚拟账户测试** - 允许在实时模式下检查策略表现，而无需冒真实资金风险。

关于测试方法和策略性能评估的详细描述可以在 [测试](../api/testing.md) 部分找到。
