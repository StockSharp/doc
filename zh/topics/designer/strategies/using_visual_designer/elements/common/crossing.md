# 交叉

![Designer Crossing 00](../../../../../../images/designer_crossing_00.png)

该元素用于跟踪两个值之间的相对位置，例如确定两条线发生交叉的时刻。

元素会比较 **Up** 和 **Down** 两个端口上的值。

## 输入端口

- **Up** – 可比较的值（例如数值、指标值等）。
- **Down** – 可比较的值（例如数值、指标值等）。

## 输出端口

- **Flag** – 当 **Up** 大于 **Down** 时为 true，否则为 false。

![Designer Crossing 01](../../../../../../images/designer_crossing_01.png)

此示例使用 Crossing 模块跟踪两个 [SMA 指标](../../../../../api/indicators/list_of_indicators/sma.md)的交叉。策略图中使用了两个 Crossing 模块：当长期 SMA 大于短期 SMA 时，其中一个模块输出 true；当长期 SMA 小于短期 SMA 时，另一个模块输出 true。

## 另请参阅

[值延迟](delay_value.md)
