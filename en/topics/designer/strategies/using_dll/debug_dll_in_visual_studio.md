# Debugging a DLL with Visual Studio

Visual Studio provides a mechanism for attaching to running processes using the Visual Studio debugger. The Visual Studio debugger is described in more detail in the documentation [Attach to running processes](https://learn.microsoft.com/en-us/visualstudio/debugger/attach-to-running-processes-with-the-visual-studio-debugger?view=vs-2022). The debugging process will be demonstrated using the example of a strategy added in the section [Using DLL](../using_dll.md).

1. To attach to a process and start debugging a DLL strategy, it needs to be loaded into memory. The DLL is loaded into memory after [adding the strategy](../using_dll.md). Once the DLL is loaded into memory, you can attach to the process.

![Designer_Creation_Strategy_Dll_01](../../../../images/designer_creation_strategy_dll_01.png)

2. In Visual Studio, select **Debug -> Attach to Process**.

![Designer Debugging DLL cube using Visual Studio 00](../../../../images/designer_debugging_dll_using_visual_studio_00.png)

3. In the **Attach to Process** dialog box, find the **Designer.exe** process in the **Available processes** list that you want to attach to.

![Designer Debugging DLL cube using Visual Studio 01](../../../../images/designer_debugging_dll_using_visual_studio_01.png)

If the process is running under a different user account, you need to check the **Show processes from all users** checkbox.

4. It's important that the **Attach to** window specifies the code type that needs to be debugged. The default **Auto** parameter tries to determine the code type to be debugged, but it does not always correctly identify the code type. To manually set the code type, you need to do the following steps.

- In the Attach to field, click **Select**.
- In the **Select Code Type** dialog box, click the **Debug these code types** button and select the types for debugging.
- Click OK.

![Designer Debugging DLL cube using Visual Studio 02](../../../../images/designer_debugging_dll_using_visual_studio_02.png)

5. Click the Attach button.

6. In Visual Studio, set breakpoints in the code. If the breakpoints are red and filled with red ![Designer Debugging DLL cube using Visual Studio 03](../../../../images/designer_debugging_dll_using_visual_studio_03.png) (and Studio is in debugging mode), it means that the exact version of the DLL was loaded. If the breakpoints are red and filled with white ![Designer Debugging DLL cube using Visual Studio 04](../../../../images/designer_debugging_dll_using_visual_studio_04.png) (and Studio is in debugging mode), it means that the wrong version of the DLL was loaded.

7. In the example, the breakpoint is set in the first line of the method **public void ProcessCandle(Candle candle)**. When the strategy runs in [Designer](../../../designer.md), as soon as candle values start being passed to the DLL, Visual Studio will stop at the breakpoint. From there, you can track the execution of the code:

![Designer Debugging DLL cube using Visual Studio 05](../../../../images/designer_debugging_dll_using_visual_studio_05.png)

> [!WARNING] 
> When the code is stopped under the debugger, all processes inside the **Designer** program are suspended. If the program is connected to real trading, then in the case of a long stop under the debugger, disconnections will occur.

## See Also

[Exporting Strategies](../../export_import/export.md)
