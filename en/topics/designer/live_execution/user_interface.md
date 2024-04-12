# Interface

After adding a strategy to the **Live** folder, by double-clicking the added strategy, a tab titled "Live [Strategy Name]" will open. Upon navigating to this tab, the **Live** tab will automatically open in the **Ribbon**. On the **Live** tab, you can specify the instrument and portfolio with which the strategy will work. By pressing the **Start** button - initiate Live trading for the strategy; by pressing the **Stop** button - halt it.

![Designer Interface Live trade 00](../../../images/designer_interface_live_trade_00.png)

The strategy tab contains the Strategy Designer for schemes and component elements, similar to the one described in [Strategy Designer](../strategies/using_visual_designer/diagram_panel.md). Additionally, the tab includes the [Live Trading Properties](../user_interface/components/live_settings.md) panel, which by default is collapsed and attached to the right side of the tab.

Adding a strategy to **Live** involves copying it from the original code (in the case of using [schemes](../strategies/using_visual_designer.md) or [C# code](../strategies/using_csharp.md)). Therefore, changes to the algorithm inside the **Live** copy do not affect the original. When launching the strategy, if there's a discrepancy between **Live** and the original, a warning will be displayed:

![Designer Interface Live trade 01](../../../images/designer_interface_live_trade_01.png)

- **Yes** means to apply changes from the original to the **live** copy.
- **No** means to ignore the difference and launch the **live** copy without applying changes.
- **Cancel** means not to launch anything.

Changes in the **Live** copy should be minimal, aimed at testing and subsequent transfer to the original. Otherwise, there's a risk of losing changes if the **Live** copy is updated to the original's version.

## See Also

[Connection Settings](../connections_settings.md)