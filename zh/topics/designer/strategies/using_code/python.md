# 使用 Python

通过代码创建策略适合偏好 Python 编程的用户。与策略图相比，此类策略不受功能限制，可以实现任意算法。

可以直接在 [Designer](../../../designer.md) 中创建策略，也可以使用 **Python** 开发环境（最常用的是 **Visual Studio** 和 **JetBrains Rider**），结合专业的 **Python** 交易机器人开发库和 [API](../../../api.md) 进行开发。

要添加新策略，可以在 **General** 选项卡中单击 **Add** 按钮 ![Designer Panel Circuits 01](../../../../images/designer_panel_circuits_01_button.png)，然后选择 **Strategy**。也可以在 **Schemes** 面板中右键单击 **Strategies** 文件夹，再在下拉菜单中单击 **Add** 按钮 ![Designer Panel Circuits 01](../../../../images/designer_panel_circuits_01_button.png)：

![Designer The creation of a strategy 00](../../../../images/designer_creation_of_strategy_00.png)

单击 **Add** 按钮 ![Designer Panel Circuits 01](../../../../images/designer_panel_circuits_01_button.png) 后，会显示用于选择策略内容类型的窗口：

![Designer_Creation_of_element_containing_source_code_00](../../../../images/designer_python_create_strategy_00.png)

要通过 Python 代码创建策略，请选择第二个选项卡。还可以选择一个模板作为初始代码。

单击 **OK** 后，新策略会显示在 **Schemes** 面板的 **Strategies** 文件夹中，与通过[策略图](../using_visual_designer.md)创建策略时相同。删除和重命名策略的操作也相同。

但此时显示的不是策略图，而是 Python 代码编辑器：

![Designer_Creation_of_element_containing_source_code_01](../../../../images/designer_python_create_strategy_01.png)

代码编辑器选项卡由 **Source Code** 和 **Error List** 面板组成。**Source Code** 面板包含 Python 代码编辑器。顶部工具栏可用于启用或禁用 **Current Line**、**Line Number** 等显示选项。可以使用 CTRL+鼠标滚轮放大或缩小字体。

**Error List** 面板以表格形式显示代码错误。双击某一行后，**Source Code** 面板中的光标会自动移动到对应的错误位置。

编辑代码时，**Error List** 面板右下角会显示 ![Designer The creation of the cube containing the source code 03](../../../../images/designer_creation_of_element_containing_source_code_03.png) 图标，表示程序已开始跟踪更改。代码停止变化后会自动编译。

策略的[回测](../../backtesting/user_interface.md)、[实盘运行](../../live_execution/getting_started.md)及其他操作，与使用策略图创建的策略相同。

## 限制

> [!WARNING]
> [Designer](../../../designer.md) 使用 IronPython，因此存在以下限制：
> - 兼容 Python 3.4。
> - 通过专用 .NET 实现提供部分 numpy 支持（使用示例参见[此处](https://github.com/StockSharp/StockSharp/blob/master/Algo.Analytics.Python/pearson_correlation_script.py)）。
> - 不支持 pandas、scipy 等使用 C 编写的其他常用库。
> - 对异步编程的支持有限。
> - 某些依赖 CPython 特定实现的 Python 内置模块无法使用。
> - 某些操作的性能可能低于 CPython。
>
> 在 [Designer](../../../designer.md) 中使用 Python 开发交易策略时，应充分考虑这些限制。
