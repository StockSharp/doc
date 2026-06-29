# 蜡烛

![Designer Candles 00](../../../../../../images/designer_candles_00.png)

该模块用于为指定证券构建蜡烛。

### 输入端口

输入端口

- **Security** – 要按照给定参数构建蜡烛的证券。

### 输出端口

输出端口

- **Candles** – 构建完成的蜡烛。

### 参数

参数

- **Series** – 蜡烛序列类型以及该类型的参数；
- **Only Formed** – 指定仅向输出端传递已完全形成的蜡烛，还是传递蜡烛的每次变化；
- **Smaller Timeframe** – 使用更小的时间周期构建蜡烛；
- **Subscribe on Signal** – 仅在收到触发信号后订阅数据。

## 另请参阅

[Level 1](level_1.md)
