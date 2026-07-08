# 获取交易品种的最佳价格

可以使用以下策略图，按交易品种当前的最佳价格注册买入订单：

![Designer Get the best rates for the tool 00](../../../../../images/designer_get_best_quote_for_instrument_00.png)

在 [变量](../elements/data_sources/variable.md) 模块中选择 **Instrument** 数据类型。如果未指定交易品种，但已设置 **Common** 属性组中的 **Parameters** 标志，则会从策略中获取交易品种并将其传递给 [市场深度](../elements/market_depths/order_book.md) 模块。[市场深度](../elements/market_depths/order_book.md) 模块从变量接收到当前交易品种后，会通过输出参数传递该交易品种的市场深度变化。收到市场深度变化时，[转换器](../elements/converters/converter.md) 模块会从中选取当前最佳买入价。

## 推荐内容

[获取当前持仓](get_current_position.md)
