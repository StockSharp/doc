# 策略成交

![Designer The transaction strategy 00](../../../../../../images/designer_trades_strategy_00.png)

该模块用于获取策略的全部成交。

## 输入端口

- **Instrument** \- 要获取成交的交易品种。如果未传入交易品种，则输出策略在所有交易品种上的成交。

## 输出端口

- **Trades** \- 指定交易品种上产生的成交。这些成交既可以通过 **Chart panel** 元素显示在图表上，也可以通过 **Position protection** 元素用于持仓保护。
