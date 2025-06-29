# User interface

To run the test on the history, you shall select a strategy, the scheme of which will be tested on the history. The strategy is selected on the [Schemas](../user_interface/schemas.md) panel in the strategy folder, by double clicking on the strategy of interest. When you select a strategy for the workspace, a new tab with a strategy appears, when you switch to this tab, the **Emulation** tab will automatically open in the Ribbon.

![Designer Interface Backtesting 00](../../../images/designer_interface_backtesting_00.png)

On the **Emulation** tab, you can change the strategy name and give it a brief description.

To run the test on the history, on the **Emulation tab** specify the path to the historical data in the Market Data field and set the testing period. The strategy for testing is started by clicking the **Start button** ![Designer Interface Backtesting 01](../../../images/designer_interface_backtesting_01.png). After starting the strategy for testing, the **Pause** ![Designer Interface Backtesting 02](../../../images/designer_interface_backtesting_02.png) button, suspending the testing and the **Stop** ![Designer Interface Backtesting 03](../../../images/designer_interface_backtesting_03.png) button, completely stopping the testing, become active. When editing the strategy, the **Undo(Ctrl+Z)** ![Designer Interface Backtesting 04](../../../images/designer_interface_backtesting_04.png), canceling the last action, **Redo(Ctrl+Y)** ![Designer Interface Backtesting 05](../../../images/designer_interface_backtesting_05.png), returning undo, **Refresh(Ctrl+R)** ![Designer Interface Backtesting 06](../../../images/designer_interface_backtesting_06.png), completely updating the scheme, buttons will be useful. Also, from the **Emulation tab** you can use the **Debugger** ([Debugging](debugging.md)) or run strategy **Optimization**.

The selected strategy tab contains the following panels by default:

- The **Scheme** panel, in which the main working process on the strategy and its components design is performed, by combining cubes and connecting lines. The Scheme is described in detail in the [Diagram panel](../strategies/using_visual_designer/diagram_panel.md) section.
- Panel of information elements, containing the **Chart**, **Orders**, **Trades**, **Statistics** and other components. You can add required component, selecting it on the **Emulation** tab in the **Components** group.
- The **Properties** panel is collapsed by default on the right side of the strategy tab. On the **Properties** panel, you can arrange the **Emulation** general settings. For example, the **Market-data storage format** can be set to **BIN** or **CSV**, depending on the file format of the selected storage. The data type can be Ticks or Candles. If Ticks is selected, candles will be formed from the ticks specified in the [Backtesting settings](../user_interface/components/backtesting_settings.md).

## Recommended content

[Backtesting settings](../user_interface/components/backtesting_settings.md)
