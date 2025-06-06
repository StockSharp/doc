# Extended instrument info

Sources of extended information are **CSV** files located in the `c:\\Users\\Users\\Documents\\StockSharp\\Hydra\\Extended info\\` folder. They are automatically loaded when [Hydra](../../hydra.md) starts.

Extended information can include any additional details about an instrument (for example, country, city, board, etc.).

Each source of extended information (CSV file) contains a list of instruments and the available properties. For each source, the extended information is unique.

If the source does not contain extended information for an instrument, the corresponding columns in the instruments list will be empty.

To select the required extended information, you need:

1. In the **Securities** tab, click the **Extended information** button![hydra Extension Info securities](../../../images/hydra_extensioninfo_securities.png)
2. A window will appear in which you need to select the path to the required CSV file![hydra Extension Info window](../../../images/hydra_extensioninfo_window.png)

Below is an example of a **CSV** file of extended information opened in different editors: **MS Excel** and **Notepad**.

![hydra ExtensionInfo csv excel](../../../images/hydra_extensioninfo_csv_excel.png)

![hydra ExtensionInfo csv notepad](../../../images/hydra_extensioninfo_csv_notepad.png)
