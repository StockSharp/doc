# 是否允许交易

![Designer TradeAllowedDiagramElement 00](../../../../../../images/designer_tradealloweddiagramelement_00.png)

该模块用于检查当前是否允许交易。检查内容包括以下条件：

- 策略对市场数据的所有订阅都必须处于 [Online](../../../../../api/market_data/subscriptions.md) 状态（正在接收实时数据）。
- 所有指标都必须已经[形成](../../../../../api/indicators.md)。
- 在[实盘交易](../../../../live_execution/getting_started.md)中，传入触发值的时间戳必须晚于策略启动时间。

### 输入端口


- **Trigger** - 用于确定何时执行检查的信号。

### 输出端口


- **Flag** - 表示交易时段是否处于活动状态的标志。

## 另请参阅

[当前时间](current_time.md)
