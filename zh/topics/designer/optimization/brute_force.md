# 穷举优化

要切换到策略优化模式，请在 **Emulation** 选项卡中单击 **Optimization** 按钮。下面使用通过[模块](../strategies/using_visual_designer/first_strategy.md)创建的 SMA 策略介绍优化过程。

![Designer Optimization 00](../../../images/designer_optimization_00.png)

工作区会打开名为“Optimization + 策略名称”的选项卡。**Optimization** 选项卡分为两个区域：**Properties** 和 **Optimization Result**。

![Designer Optimization 02](../../../images/designer_optimization_02.png)

- **Properties** 区域包含多个带表格的选项卡。第一个用于设置要[遍历](optimization_parameters.md)的策略参数；第二个用于配置[遗传算法](genetic.md)；第三个用于配置优化器的系统参数，例如参与优化的线程数和 CPU 核心数。
- **Optimization Result** 区域是一个表格，每一行都表示使用一组唯一参数测试策略得到的结果。该区域还包含进度条，用于显示优化进度、已用时间和预计剩余时间。此外，还可以在另一个选项卡中以 [3D 图表](3d_chart.md)形式查看结果。

设置遍历参数后，迭代次数可能超过 1000。启动优化器后，结果上方的进度区域会显示计划迭代总数、已完成数量以及预计完成所需时间：

![Designer Optimization 03](../../../images/designer_optimization_03.png)

## 另请参阅

[回测示例](../backtesting/getting_started.md)
