# 对冲

![Designer Hedging 00](../../../../../../images/designer_hedging_00.png)

该模块用于对冲期权持仓。

### 输入端口

输入端口

- **Model** – 计算模型（例如 Black-Scholes）。
- **Instrument** – 作为标的资产的证券。
- **Volume** \- 成交量的数值。
- **Position by underlying asset** – 标的资产的持仓。
- **Flag** – 启动对冲过程的信号（标志）。

### 输出端口

输出端口

- **Order** – 已注册的订单。可以使用 Trades by order 元素获取该订单的成交，并通过 Chart panel 模块将其显示在图表上。

### 参数

参数

- **Hedging type** \- 对冲类型，可取 Delta、Gamma、Vega、Theta 或 Rho。

## 推荐内容

[Options quoting](options_quoting.md)
