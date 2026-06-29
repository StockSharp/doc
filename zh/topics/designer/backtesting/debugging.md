# 调试

测试策略时，经常需要检查某个模块接收到的输入数据，或从其输出端传出的数据。为此，[Designer](../../designer.md) 提供了 **Debugger**。

![Designer Debug 00](../../../images/designer_debug_00.png)

**Emulation** 功能区的 **Debugger** 组中包含以下按钮：

- ![Designer Debug 01](../../../images/designer_debug_01.png)**Add breakpoint** – 为选中的元素添加断点。已添加断点的元素会以红色边框突出显示。
- ![Designer Debug 02](../../../images/designer_debug_02.png)**Delete breakpoint** – 删除断点。
- ![Designer Debug 03](../../../images/designer_debug_03.png)**Next element** – 触发断点后，移动到策略图中的下一个元素。
- **Step to out** – 触发断点后，移动到当前元素的输出端，用于检查从该元素输出的值。
- ![Designer Debug 04](../../../images/designer_debug_04.png)**Step in** – 触发断点后，进入复合元素内部。程序会自动打开复合元素的策略图，并在最先接收到数据的元素处停止。
- ![Designer Debug 05](../../../images/designer_debug_05.png)**Step out** – 在复合元素内部触发断点后，退出到包含当前复合元素的上一级。
- ![Designer Debug 06](../../../images/designer_debug_06.png)**Continue** – 继续执行，直到触发下一个断点。

## 推荐内容

[断点](debugging/break_points.md)
