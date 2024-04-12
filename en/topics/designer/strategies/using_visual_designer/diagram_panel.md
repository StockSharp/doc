# Strategy Designer

The main process of designing a strategy and its component elements takes place in the **Scheme** panel by combining blocks and connecting lines. The Scheme panel consists of the panels: **Palette**, **Designer**, and **Properties**.

![Designer Designer schemes strategies and component elements 00](../../../../images/designer_designer_schemes_strategies_and_component_elements_00.png)

## Palette Panel

The **Palette** panel contains blocks from which strategies are created. All elements in the palette are divided into categories, described in the section [Description of blocks](elements.md). To add a block to the **Designer** panel, right-click on the required block and, without releasing the button, drag it to the **Designer** panel. After that, the element will be automatically selected, and its parameters will be shown in the window for editing the block's properties.

## Designer Panel

The **Designer** panel is where the entire process of creating a strategy occurs through the combination of blocks and connections (lines). It visually represents the strategy scheme. Detailed information about creating a strategy is described in the section [Creating an algorithm from blocks](first_strategy.md).

## Properties Panel

The **Properties** panel displays the parameters of the selected block on the **Designer** panel. When a block is selected on the **Designer** panel, its frame is colored black.

![Designer The Properties Panel 00](../../../../images/designer_properties_panel_00.png)

The **Properties** panel can be displayed in two modes: *Basic settings* and *Advanced settings*.

By default, when building a scheme, the properties are initially displayed in *basic settings* mode. To switch to *advanced settings* mode, you need to click on the corresponding title.

In *basic settings* mode, only the most necessary properties of the block are displayed. For example, for the [candles](elements/data_sources/candles.md) block, the timeframe, the flag for receiving only formed candles, the flag for the possibility of constructing candles from a smaller timeframe, and the flag for subscribing to candles on signal will be displayed.

In *advanced settings* mode, all properties of the block available for change and setting will be displayed.

![Designer The Properties Panel 00](../../../../images/designer_properties_panel_01.png)

All blocks contain a set of predefined properties, which become visible in *advanced settings* mode:

- **Name** – the name of the element, displayed in the designer.
- **Logging level** – the logging level for this element.
- **Parameters** – display the element's parameters in higher-level elements.
- **Sockets** – display the element's sockets in higher-level elements.

Detailed information about the properties of each block is described in the section [Description of blocks](elements.md).

## See Also

[Description of blocks](elements.md)
