# 与 Visual Studio 集成

与 [Designer](../designer/strategies/using_dll/debug_dll_in_visual_studio.md) 类似，**Runner** 也可以用于调试策略。如果计划只在 **Runner** 中运行策略，这种方式会非常方便。否则，更适合直接在 [Designer](../designer.md) 中启动并调试策略。

要配置调试过程，请执行以下步骤：

1. 右键单击交易策略项目，然后在上下文菜单中选择 **属性**：

![与 Visual Studio 集成 00](../../images/runner_debug_00.png)

在打开的选项卡中找到 **调试**，选择 **常规** 部分，然后单击 **打开调试启动配置文件界面**。

2. 在随后打开的窗口中，创建一个启动外部程序的新调试配置文件：

![与 Visual Studio 集成 01](../../images/runner_debug_01.png)

3. 输入 **Runner** 的完整路径，并指定启动所需的命令行参数。有关详细信息，请参阅 [Runner 命令行](command_line.md)。

![与 Visual Studio 集成 02](../../images/runner_debug_02.png)

示例使用的命令行参数：

```cmd
l -s "$(TargetPath)" -c "C:\StockSharp\Runner\Data\connection.json" --sec BTCUSDT_PERPETUAL@BNB --pf Binance_-298049655_Futures
```

$(TargetPath) 是一个特殊的 **Visual Studio** 宏。开始调试时，它会自动替换为包含策略的已编译 DLL 文件路径。

4. 关闭项目设置窗口，然后开始调试项目，例如按 F5。此时会出现 **Runner** 程序窗口，并显示交易连接过程：

![与 Visual Studio 集成 03](../../images/runner_debug_03.png)

5. 设置断点后，程序执行到断点位置时会暂停。例如，可以在新K线出现时调试交易逻辑：

![与 Visual Studio 集成 04](../../images/runner_debug_04.png)
