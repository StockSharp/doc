# 指标

![Designer Indicator 00](../../../../../../images/designer_indicator_00.png)

该模块用于计算指标值。

## 输入端口

- **Any Data** – 计算所选指标所依据的特定数据类型（具体类型取决于指标，例如数值、蜡烛等）。

## 输出端口

- **Indicator** – 计算得到的指标值，可用于显示在图表面板上或参与后续计算。

## 参数

- **Indicator Type** - 用于选择所需指标的参数，以及与所选指标类型对应的若干附加参数。更改指标类型后，这组参数也会随之变化。
- **Final** - 仅传递指标的[最终值](../../../../../api/indicators.md)。
- **Formed** - 仅在指标已经完全[形成](../../../../../api/indicators.md)后传递其值。

![Designer Indicator 01](../../../../../../images/designer_indicator_01.png)

## 另请参阅

[指标列表](../../../../../api/indicators/list_of_indicators.md)
[逻辑条件](logical_condition.md)
