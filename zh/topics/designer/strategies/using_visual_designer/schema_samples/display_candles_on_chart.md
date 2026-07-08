# 在图表上显示K线

可以使用以下策略图，将指定交易品种的K线输出到图表：

![Designer The conclusion of the candles on the chart 00](../../../../../images/designer_conclusion_of_candles_on_chart_00.png)

在 [变量](../elements/data_sources/variable.md) 模块中选择 **Instrument** 数据类型。如果未指定交易品种，但已设置 **Common** 属性组中的 **Parameters** 标志，则会从策略中获取交易品种并将其传递给 [K线](../elements/data_sources/candles.md) 模块。[K线](../elements/data_sources/candles.md) 模块设置为构建 5 分钟K线，并且仅传递已完全形成的K线。

在 [图表](../elements/common/chart.md) 模块中添加一个K线类型的图形元素，模块会自动为其添加输入参数。

向图表面板添加所需的图形元素后，在 [K线](../elements/data_sources/candles.md) 与 [图表](../elements/common/chart.md) 元素之间创建连接。构建完成的K线会通过该连接传递到图表中显示。

## 推荐内容

[获取交易品种的最佳价格](get_best_price_for_instrument.md)
