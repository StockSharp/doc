# 风险管理

可以在[回测属性](components/backtesting_settings.md)和[实盘交易属性](components/live_settings.md)面板中配置风险控制设置。

在 Risks 窗口中，需要选择 **Risk Rule**，配置该 **Risk Rule** 的触发条件，并指定 **Risk Rule** 条件触发后要执行的操作（平仓、停止交易或撤销订单）。

可以添加多个类型相同但执行操作不同的风险规则。例如，在下图中，当订单数量达到 20 时，会执行撤销订单和停止交易操作。

![Designer Risk Rule](../../../images/designer_risk_rule.png)

### 风险规则列表

风险规则列表

- **P/L** - 监控盈亏金额的风险规则。
- **Position** - 监控持仓规模的风险规则。
- **Position (Time)** - 监控持仓持续时间的风险规则。
- **Commission** - 监控佣金金额的风险规则。
- **Slippage** - 监控滑点大小的风险规则。
- **Order Price** - 监控订单价格的风险规则。
- **Order Volume** - 监控订单数量的风险规则。
- **Order (Frequency)** - 监控下单频率的风险规则。
- **Error in Registration/Cancellation of Order** - 监控注册或撤销订单时错误数量的风险规则。
- **Trade Price** - 监控成交价格的风险规则。
- **Trade (Volume)** - 监控成交数量的风险规则。
- **Trade (Frequency)** - 监控成交频率的风险规则。
- **Error** - 监控所有错误数量的风险规则。
