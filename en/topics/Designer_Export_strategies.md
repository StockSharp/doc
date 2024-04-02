# Export

The Designer allows you to export any type of data: strategies, blocks, and indicators. There are several ways to export:

- In the **Schemes** panel, right-click on the strategy, block, or indicator. In the menu that appears, select **Export**.
- On the **Common** tab, press the **Export** button:

![Designer Export strategies 00](../images/Designer_Export_strategies_00.png)

After pressing **Export**, depending on the content type, a window will appear:

- for a [scheme](Designer_Creating_strategy_out_of_blocks.md):

  ![Designer Export strategies 01](../images/Designer_Export_strategies_01.png)

  - scheme - export the scheme as is. The **Standalone** mode is required for schemes that use their own elements or indicators. In this case, all inner elements will be exported within the strategy diagram.
  - code - convert the scheme into C# code.
  - file (assembly) - compile the scheme into a DLL. Suitable if you need to keep the scheme confidential.

- for [code](Designer_Creating_strategy_from_code.md):

  ![Designer Export strategies 02](../images/Designer_Export_strategies_02.png)

  - scheme - export the code as a JSON file, which will include both the code itself and the references needed for compiling this code.
  - code - export the code as is.
  - file (assembly) - compile the code into a DLL. Suitable if you need to keep the code confidential.

- for a [dll](Designer_Creating_strategy_from_dll.md) a file selection window will appear.

## See Also

[Running Strategies Outside of Designer](Designer_run_strategy_on_server.md)
