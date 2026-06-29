# 获取证券的最佳价格

可以使用以下策略图，按证券当前的最佳价格注册买入订单：

![Designer Get the best rates for the tool 00](../../../../../images/designer_get_best_quote_for_instrument_00.png)

在 [Variable](../elements/data_sources/variable.md) 模块中选择 **Instrument** 数据类型。如果未指定证券，但已设置 **Common** 属性组中的 **Parameters** 标志，则会从策略中获取证券并将其传递给 [Order book](../elements/market_depths/order_book.md) 模块。[Order book](../elements/market_depths/order_book.md) 模块从变量接收到当前证券后，会通过输出参数传递该证券的市场深度变化。收到市场深度变化时，[Converter](../elements/converters/converter.md) 模块会从中选取当前最佳买入价。

## 推荐内容

[Get current position](get_current_position.md)
