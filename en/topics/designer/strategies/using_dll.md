# Using DLL

Using ready-made DLLs is familiar for those who want to work continuously in **Visual Studio** and **JetBrains Rider** environments. This approach offers several advantages over writing [code](using_code.md) inside **Designer**:

- Enhanced code editor compared to the built-in editor inside **Designer**.
- Recompiling code automatically updates content inside **Designer**.
- Possibility to split code into several files (in the case of the [code](using_code.md) approach, only OneFile-OneStrategy variant is possible).
- Using the [debugger](using_dll/debug_dll_in_visual_studio.md).

### Creating a Project in Visual Studio

1. To create a strategy in **Visual Studio**, you need to create a project:

![Designer Creating a DLL cube in Visual Studio 00](../../../images/designer_creating_dll_element_in_visual_studio_00.png)

2. Next, you need to write the strategy code. For a quick start, you can copy the SmaStrategy code, which is created as a template in [strategy from code](using_code/csharp/first_strategy.md):

![Designer Creating a DLL cube in Visual Studio 03](../../../images/designer_creating_dll_element_in_visual_studio_03.png)

3. To compile the code, it is necessary to include the NuGet package [StockSharp.Algo](https://www.nuget.org/packages/stocksharp.algo), where the base class for all strategies [Strategy](xref:StockSharp.Algo.Strategies.Strategy) is located:

![Designer Creating a DLL cube in Visual Studio 04](../../../images/designer_creating_dll_element_in_visual_studio_04.png)

If the strategy uses charting interfaces, it is necessary to include the NuGet package [StockSharp.Charting.Interfaces](https://www.nuget.org/packages/stockSharp.charting.interfaces). These interfaces do not contain the logic of the actual chart and are only needed for compiling the code. In the case of launching the strategy in **Designer**, real data rendering on the chart will occur through these interfaces.

4. After creating the strategy, the project needs to be built by pressing **Build Solution** in the **Build** tab.

![Designer Creating a DLL cube in Visual Studio 01](../../../images/designer_creating_dll_element_in_visual_studio_01.png)

5. In Visual Studio by default, the project is built into the …\\bin\\Debug\\net6.0 folder.

![Designer Creating a DLL cube in Visual Studio 02](../../../images/designer_creating_dll_element_in_visual_studio_02.png)

### Adding DLL to Designer

1. Adding a strategy from a DLL is similar to creating a strategy from [code](using_code.md). But at the content type definition stage, you need to choose **DLL**:

![Designer_Creation_Strategy_Dll_00](../../../images/designer_creation_strategy_dll_00.png)

2. In the window, you need to specify the path to the assembly (must be compatible with .NET 6.0), and choose the type. The latter is necessary because one DLL can contain several strategies (or [cubes with indicators](using_dll/create_element_and_indicator.md)). After clicking **OK**, the strategy will be added to the **Scheme** panel and is ready for use:

![Designer_Creation_Strategy_Dll_01](../../../images/designer_creation_strategy_dll_01.png)

3. Launching the strategy on [backtest](../backtesting/user_interface.md), on [live](../live_execution/getting_started.md), and other operations - works similarly to strategy from diagrams and code:

![Designer_Creation_Strategy_Dll_02](../../../images/designer_creation_strategy_dll_02.png)
