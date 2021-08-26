# Debugging

In the process of strategies testing, it often becomes necessary to check what data enters the input of a given cube or passed at its output. To do that, the [S\#.Designer](Designer.md) features the **Debugger**.

![Designer Debug 00](../images/Designer_Debug_00.png)

The following buttons are located in the **Debugger** group of the **Emulation** Ribbon:

- ![Designer Debug 01](../images/Designer_Debug_01.png)**Add breakpoint** – to add a breakpoint at the selected element. Elements, for which a breakpoint is added, are highlighted with a red border.
- ![Designer Debug 02](../images/Designer_Debug_02.png)**Delete breakpoint** – to delete a breakpoint.
- ![Designer Debug 03](../images/Designer_Debug_03.png)**Next element** – when the breakpoint is triggered, moves to the next element of the scheme.
- **Step to out** – when the breakpoint is triggered, moves to the output of the current element, is used to check the values, passed at the element output.
- ![Designer Debug 04](../images/Designer_Debug_04.png)**Step in** – when the breakpoint is triggered, moves inside the composite element. Automatically opens the composite element scheme and stops at the element to which the data is first transmitted.
- ![Designer Debug 05](../images/Designer_Debug_05.png)**Step out** – when the breakpoint is triggered and is within the composite element, it exits one level up where an open composite element is used.
- ![Designer Debug 06](../images/Designer_Debug_06.png)**Continue** – Continues until the next breakpoint is triggered.

## Recommended content

[Break points](Designer_Debug_Break_Points.md)
