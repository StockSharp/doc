# K线

![Designer Candles 00](../../../../../../images/designer_candles_00.png)

该模块用于为指定交易品种构建K线。

### 输入端口

输入端口

- **Security** – 要按照给定参数构建K线的交易品种。

### 输出端口

输出端口

- **Candles** – 构建完成的K线。

### 参数

参数

- **Series** – K线序列类型以及该类型的参数；
- **Only Formed** – 指定仅向输出端传递已完全形成的K线，还是传递K线的每次变化；
- **Smaller Timeframe** – 使用更小的时间周期构建K线；
- **Subscribe on Signal** – 仅在收到触发信号后订阅数据。

## 另请参阅

[Level 1](level_1.md)
