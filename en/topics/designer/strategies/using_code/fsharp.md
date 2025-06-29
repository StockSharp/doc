# Using F#

Creating strategies from code is for users who prefer working with F# code. Such strategies are not limited in capabilities unlike diagrams, and any algorithm can be described.

The process of creating a strategy takes place directly in [Designer](../../../designer.md) or a **F#** development environment (the most popular of which are **Visual Studio** and **JetBrains Rider**), using a library for professional development of trading robots in **F#** and [API](../../../api.md).

You can add a new strategy by pressing the **Add** ![Designer Panel Circuits 01](../../../../images/designer_panel_circuits_01_button.png) button on the **Common** tab and choose **Strategy**. Or, by right-clicking on the **Strategies** folder in the **Scheme** panel, and pressing the **Add** ![Designer Panel Circuits 01](../../../../images/designer_panel_circuits_01_button.png) button in the dropdown menu:

![Designer The creation of a strategy 00](../../../../images/designer_creation_of_strategy_00.png)

After pressing the **Add** ![Designer Panel Circuits 01](../../../../images/designer_panel_circuits_01_button.png) button, a window will appear with a choice of content type to create the strategy on:

![Designer_Creation_of_element_containing_source_code_00](../../../../images/designer_fsharp_create_strategy_00.png)

To create a strategy from F# code, you need to select the second tab. You can also choose a template that will be used as the initial code.

After pressing **OK**, a new strategy will appear in the **Strategies** folder of the **Scheme** panel, similarly to when creating a strategy from [diagrams](../using_visual_designer.md). And similar actions for deleting or renaming the strategy.

But instead of a diagram, an F# code editor will be displayed:

![Designer_Creation_of_element_containing_source_code_01](../../../../images/designer_fsharp_create_strategy_01.png)

The code editor tab consists of **Source Code** and **Error List** panels. The **Source Code** panel contains the F# code editor itself. At the top, there is a toolbar where highlighting such things as **Current Line**, **Line Number**, etc., can be turned on or off. To increase the font size, you can use the CTRL+MouseWheel combination.

The **Error List** panel is a table with a list of errors in the code, double-clicking on a line will automatically move the cursor in the **Source Code** panel to the error location.

When editing the code, an icon ![Designer The creation of the cube containing the source code 03](../../../../images/designer_creation_of_element_containing_source_code_03.png) will appear in the bottom right corner of the **Error List** panel, indicating that change tracking has begun. The code compiles at the moment when the code stops changing.

Running the strategy on [backtest](../../backtesting/user_interface.md), on [live](../../live_execution/getting_started.md), and other operations are similar to the strategy from diagrams.
