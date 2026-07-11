# 事件模型

[Designer](../../../designer.md) 中的策略图构建方式以事件的生成和后续处理为基础。构建策略时，无法预先知道市场数据变化事件将在何时发生，但可以订阅该事件并对其进行相应处理。

每个具有输出参数的 [Designer](../../../designer.md) 模块都是事件生成器。具有输入参数的模块可以订阅输出参数所生成的事件。订阅事件实际上就是在两个模块之间创建一条连接线。

例如，[市场深度](elements/market_depths/order_book.md) 模块会生成市场深度变化事件，无法预先知道该变化将在何时发生。在 [市场深度](elements/market_depths/order_book.md) 模块与 [转换器](elements/converters/converter.md) 模块之间创建连接线后，就订阅了市场深度变化事件，以便由 [转换器](elements/converters/converter.md) 模块进行后续处理，依此类推：

![Designer 事件模型 00](../../../../images/designer_event_model_00.png)

## 推荐内容

[第一个策略](first_strategy.md)
