# 希腊值

![Designer Greek 00](../../../../../../images/designer_greek_00.png)

该模块用于计算当前时刻的主要希腊值：Delta、Gamma、Vega、Theta 和 Rho。

### 输入端口

输入端口

- **模型** – 计算模型（例如 Black-Scholes）。
- **标的资产价格** – 标的资产的价格。
- **最大偏差** – 最大偏差。

### 输出端口

输出端口

- **结果** – 当前时刻主要希腊值（Delta、Gamma、Vega、Theta 和 Rho）的计算结果。

### 参数

参数

- **值** – 可选择 Delta、Gamma、Vega、Theta 或 Rho，用于指定模块输出哪一个 Greek 值。

## 另请参阅

[布莱克-斯科尔斯](black_scholes.md)
