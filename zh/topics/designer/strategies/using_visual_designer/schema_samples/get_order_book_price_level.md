# 获取市场深度价格档位

可以使用以下策略图，从市场深度中获取所需的买方档位：

![Designer 事件模型 00](../../../../../images/designer_event_model_00.png)

在 [变量](../elements/data_sources/variable.md) 模块中选择 **交易品种** 数据类型。如果未指定交易品种，但已设置 **常规** 属性组中的 **参数** 标志，则会从策略中获取交易品种。在 [转换器](../elements/converters/converter.md) 模块中，选择数据类型以及模块集合中与买方报价 Bids 对应的字段。Indexer 模块从最佳买入价集合中获取所需元素。要取得某一档位的具体价格或数量值，可以使用 [转换器](../elements/converters/converter.md) 模块。

## 推荐内容

[策略库](../../../strategy_gallery.md)
