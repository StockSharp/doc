# 策略图面板

要打开 **策略图** 面板，请在 **常规** 选项卡中单击 **策略图** 按钮。**策略图** 面板包含按用途分组到不同文件夹中的脚本树。策略图与自定义模块本质上没有区别，均使用同一个编辑器 [策略设计器](../strategies/using_visual_designer/diagram_panel.md) 进行编辑。为了避免混淆，它们被分成两个独立列表并保存在不同文件夹中：策略保存在 **回测** 文件夹，自定义模块保存在 **自定义模块** 文件夹。双击列表中的所需项目即可选择要编辑的策略图，随后会在设计器中打开该策略图以供查看和编辑。下面介绍 **策略图** 面板中的文件夹：

![Designer Panel Circuits 00](../../../images/designer_panel_circuits_00.png)

1. **回测** 文件夹包含交易策略。这些策略既可以由一组元素及其连接构建为策略图，也可以使用代码创建。要添加新策略，可以在 **常规** 选项卡中单击 **添加** ![Designer Panel Circuits 01](../../../images/designer_panel_circuits_01_button.png) 按钮，然后选择 **策略**。也可以右键单击 **策略图** 面板中的 **回测** 文件夹，再在下拉菜单中单击 **添加** ![Designer Panel Circuits 01](../../../images/designer_panel_circuits_01_button.png) 按钮。在打开的窗口中选择具体的策略创建方式。
   
    ![Designer Panel Circuits 04](../../../images/designer_panel_circuits_04.png)
   
    可以使用可视化设计器创建策略而无需编写代码，也可以使用内置源代码编辑器。此外，还可以连接包含 Microsoft Visual Studio 所编写策略的外部 DLL 文件。有关 **Strategies** 的详细信息，请参阅[使用模块](../strategies/using_visual_designer.md)章节。

2. **自有元素** 文件夹包含能够实现完整功能的元素。这些元素可以在不同策略图中使用，也可以在同一个策略图中以不同属性值多次使用。可以将此类元素集合提取为单独的模块，之后像标准元素一样使用。**自定义模块** 是普通策略图，其保存、加载和编辑方式与策略图相同。要添加新的复合元素，可以在 **常规** 选项卡中单击 **添加** ![Designer Panel Circuits 01](../../../images/designer_panel_circuits_01_button.png) 按钮，然后选择 **自定义模块**。也可以右键单击 **策略图** 面板中的 **自定义模块** 文件夹，再在下拉菜单中单击 **添加** ![Designer Panel Circuits 01](../../../images/designer_panel_circuits_01_button.png) 按钮。添加新的自定义模块后，它会自动加入 **元素面板** 的 **自定义模块** 组，并可用于创建其他策略图和自定义模块。有关 **自定义模块** 的详细信息，请参阅[创建复合元素](../strategies/using_visual_designer/composite_elements.md)章节。

3. **Live** 文件夹包含已添加用于交易的策略。正在运行的策略以图标 ![Designer Panel Circuits 02](../../../images/designer_panel_circuits_02.png) 标记，已停止的策略以图标 ![Designer Panel Circuits 03](../../../images/designer_panel_circuits_03.png) 标记。有关如何向 **Live** 文件夹添加并启动策略，请参阅[实盘交易](../live_execution/getting_started.md)章节。

4. **Indicators** 文件夹包含自行编写、供交易策略使用的自定义指标。不能使用策略图创建新指标，只能使用代码或外部 DLL 文件。选择指标类型时，可以通过 [指标](../strategies/using_visual_designer/elements/common/indicator.md) 模块在策略图中使用自定义指标。

5. **Remote** 文件夹包含位于远程服务器上的策略。

## 另请参阅

[日志面板](logs.md)
