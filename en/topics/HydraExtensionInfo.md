# Extended instrument info

Sources of extended information on instruments are **CSV** files located in the "c:\\Users\\Users\\Documents\\StockSharp\\Hydra\\Extended info\\" folder, which are automatically loaded when [S\#.Data](Hydra.md) starts.

Extended information can be any necessary information on an instrument (for example, country, city, board, etc.). 

Each source of extended information (CSV file) contains a list of instruments and properties of extended information available in the source for each of the instruments. For each source of extended information, the extended information will be unique.

If the source does not contain extended information on an instrument, then empty cells will be displayed in the columns of the list of all instruments corresponding to the extended information

To select the required extended information, you need:

1. in the Securities tab, click on the **Extended information** button

   ![hydra Extension Info securities](~/images/hydra_ExtensionInfo_securities.png)
2. Then a window will appear in which you need to select the path to the required CSV file

   ![hydra Extension Info window](~/images/hydra_ExtensionInfo_window.png)

Below is an example of **CSV** file of extended information opened in different editors **MS Excel** and **NotePad.**

![hydra ExtensionInfo csv excel](~/images/hydra_ExtensionInfo_csv_excel.png)

![hydra ExtensionInfo csv notepad](~/images/hydra_ExtensionInfo_csv_notepad.png)

## Recommended content
