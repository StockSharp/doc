# 用户界面

要使用历史数据运行测试，请先选择需要回测的策略图。在策略文件夹的[策略图面板](../user_interface/schemas.md)中，双击所需策略即可选中该策略。将策略添加到工作区后，会显示一个新的策略选项卡；切换到该选项卡时，Ribbon 中会自动打开 **Emulation** 选项卡。

![Designer Interface Backtesting 00](../../../images/designer_interface_backtesting_00.png)

在 **Emulation** 选项卡中，可以修改策略名称并添加简短说明。

要使用历史数据运行测试，请在 **Emulation tab** 的 Market Data 字段中指定历史数据路径，并设置测试时间段。单击 **Start button** ![Designer Interface Backtesting 01](../../../images/designer_interface_backtesting_01.png) 启动策略测试。测试启动后，**Pause** ![Designer Interface Backtesting 02](../../../images/designer_interface_backtesting_02.png) 和 **Stop** ![Designer Interface Backtesting 03](../../../images/designer_interface_backtesting_03.png) 按钮会变为可用状态：前者用于暂停测试，后者用于彻底停止测试。编辑策略时，还可以使用 **Undo(Ctrl+Z)** ![Designer Interface Backtesting 04](../../../images/designer_interface_backtesting_04.png) 撤销上一步操作，使用 **Redo(Ctrl+Y)** ![Designer Interface Backtesting 05](../../../images/designer_interface_backtesting_05.png) 恢复被撤销的操作，并使用 **Refresh(Ctrl+R)** ![Designer Interface Backtesting 06](../../../images/designer_interface_backtesting_06.png) 完整刷新策略图。还可以从 **Emulation tab** 使用 **Debugger**（参阅[调试](debugging.md)），或运行策略**优化**。

默认情况下，所选策略的选项卡包含以下面板：

- **Scheme** 面板：通过组合模块和连接线，完成策略及其组件的主要设计工作。有关 Scheme 的详细说明，请参阅[图表面板](../strategies/using_visual_designer/diagram_panel.md)。
- 信息组件面板：包含 **Chart**、**Orders**、**Trades**、**Statistics** 等组件。可以在 **Emulation** 选项卡的 **Components** 组中选择并添加所需组件。
- **Properties** 面板：默认折叠在策略选项卡右侧。可以在 **Properties** 面板中配置 **Emulation** 的常规设置。例如，根据所选存储的文件格式，将 **Market-data storage format** 设置为 **BIN** 或 **CSV**。数据类型可以选择 Ticks 或 Candles。如果选择 Ticks，将根据[回测设置](../user_interface/components/backtesting_settings.md)中指定的逐笔成交生成K线。

## 推荐内容

[回测设置](../user_interface/components/backtesting_settings.md)
