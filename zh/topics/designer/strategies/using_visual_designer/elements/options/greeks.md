# Greeks

![Designer Greek 00](../../../../../../images/designer_greek_00.png)

该模块用于计算当前时刻的主要 Greeks：Delta、Gamma、Vega、Theta 和 Rho。

### 输入端口

输入端口

- **Model** – 计算模型（例如 Black-Scholes）。
- **Price of the Underlying Asset** – 标的资产的价格。
- **Maximum Deviation** – 最大偏差。

### 输出端口

输出端口

- **Result** – 当前时刻主要 Greeks（Delta、Gamma、Vega、Theta 和 Rho）的计算结果。

### 参数

参数

- **Value** – 可选择 Delta、Gamma、Vega、Theta 或 Rho，用于指定模块输出哪一个 Greek 值。

## 另请参阅

[Hedging](black_scholes.md)
