# Publishing Your Strategy

You can publish your strategy by clicking on the strategy with the left mouse button on the [Schemes](../user_interface/schemas.md) panel, and select **Publish**:

![Designer_publish_00](../../../images/designer_publish_00.png)

After clicking the **Publish** button, a window will open with the choice of export type. For more details, see the section [Exporting Strategies](../export_import/export.md).

After choosing the export type, the [Installer](../../installer.md) program is activated with the publishing parameters ([Installer](../../installer.md) must be launched in advance):

![Designer_publish_01](../../../images/designer_publish_01.png)

Fields required to be filled:

- Name
- Description
- Nuget package identifier. This parameter is needed to set the link to the product in the store. For example, in the address https://stocksharp.com/store/runner/, the word **runner** is specified through this parameter.

Access to the **Free** or **Paid** level is only granted after contacting via email [info@stocksharp.com](mailto:info@stocksharp.com). By default, the **Private** level is available, and it allows publishing strategies only in a private format (for selected users):

After clicking the **Save** button, the strategy will be sent to the StockSharp server.

When publishing updates, it is not required to enter all parameters again. Instead of entering product parameters, a window will appear for entering a note for the update:

![Designer_publish_02](../../../images/designer_publish_02.png)

After clicking the **OK** button, a window will appear indicating a successful update:

![Designer_publish_03](../../../images/designer_publish_03.png)
