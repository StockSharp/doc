# 持仓保护

![Designer Protect positions 00](../../../../../../images/designer_protect_positions_00.png)

![Designer Protect positions 01](../../../../../../images/designer_protect_positions_01.png)

该模块使用止损和止盈自动保护已开仓的成交。

### 输入端口

输入端口

- **Own trade** – 需要通过止损和止盈进行保护的成交。
- **Price** – 当前价格（可取自蜡烛、最近一笔逐笔成交等）。该值用于跟踪证券的当前价格并激活保护订单。

### 输出端口

输出端口

- **Take-profit** – 用于锁定利润的订单。
- **Stop-loss** – 用于限制损失的订单。
- **Own transaction** – 由上述任一订单产生的成交。

### 参数

止盈和止损参数

- **Value** - 止盈或止损的数值。
- **Trailing** – 是否使用跟踪保护。
- **Timeout** - 超时时长，超过该时间后将按市价强制触发保护。
- **Market orders** – 使用不指定价格的市价订单快速平仓。

![Designer Protect positions 02](../../../../../../images/designer_protect_positions_02.png)

> [!WARNING]
> 传入成交不能是整个策略的成交（即 [Strategy Trades](../common/trades_by_strategy.md) 模块的输出），否则会导致当前持仓计算错误：保护订单产生的成交也会成为策略成交。**Position Protection** 模块应接收 [Order Registration](../orders/register.md) 和 [Modify Position](modify.md) 模块的 **Transaction** 输出端口所产生的成交，或接收来自其他直接改变持仓的类似组件的成交。
