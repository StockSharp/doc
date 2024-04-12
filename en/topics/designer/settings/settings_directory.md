# Settings directory

The following directories are important for work with the [Designer](../../designer.md):

1. The directory, where the [Designer](../../designer.md), was installed, from this folder you can launch [Designer](../../designer.md) by running the **Designer.exe** file. Or update Designer, by running the **Designer.Update.exe** file. Deleting this directory will delete Designer, however, the Designer settings will not be deleted.

2. The [Designer](../../designer.md) settings directory is located in the folders of user's documents …\\StockSharp\\Designer\\ (for example, c:\\Users\\User\\Documents\\StockSharp\\Designer\\). Deleting this directory will cause all Designer settings to be reset to the default, **all created strategies and downloaded instruments and other information stored in the settings directory will be DESTROYED.**

![Designer Directory and edit the data manually 00](../../../images/designer_directory_and_edit_data_manually_00.png)

This directory contains the following folders and files:

- The **Compositions** folder, containing in the form of XML files all cubes, contained in the Composite elements folder on the [Schemas](../user_interface/schemas.md) panel. Deleting files from this directory will delete the **Composite element** from the **Composite elements** folder on the Schemas panel. You should not manually edit the files in this directory, this can lead to a malfunction of the corresponding **Composite elements** cube.
- The **LiveStrategies** folder, containing in the form of XML files all cubes, contained in the **Trading** folder on the [Schemas](../user_interface/schemas.md) panel. Deleting files from this directory will delete strategy from the Trading folder on the Schemas panel. You should not manually edit the files in this directory, this can lead to a malfunction of the corresponding strategy.
- The **Logs** folder, containing all [Designer](../../designer.md) logs, which simplifies the Designer troubleshooting.
- The **SourceCode** folder, containing in the form of XML files all cubes, contained in the SourceCode folder on the [Schemas](../user_interface/schemas.md) panel. Deleting files from this directory will delete the **SourceCode** cube from the **SourceCode** folder on the **Schemas** panel. You should not manually edit the files in this directory, this can lead to a malfunction of the corresponding **SourceCode** cube.
- The **Strategies** folder, containing in the form of XML files all cubes, contained in the Strategies folder on the [Schemas](../user_interface/schemas.md) panel. Deleting files from this directory will delete strategy from the **Strategies** folder on the Schemas panel. You should not manually edit the files in this directory, this can lead to a malfunction of the corresponding strategy. If you manually add a strategy file to this folder and reboot Designer, the strategy will appear in the **Strategies** folder on the **Schemas** panel.
- The **Storage** folder \- market data, downloaded by [Designer](../../designer.md)into the corresponding [Market data storage](../market_data_storage.md). The folder is created at the Market data storage creation. By default, the path is specified to this folder. Deleting this folder will delete all the downloaded market data from the corresponding storage. If the storage contains CSV format files, then they can be edited with a standard notepad or MS Excel. The BIN files cannot be edited manually.
- The **exchange.csv and exchangeboard.csv** files contain the list of **Stock Exchanges**, list of instrument codes and trading modes. These files can be edited with standard notepad or MS Excel.
- The **security.csv** file contains all received and created securities by all sources. Deleting this file will delete all securities in [Designer](../../designer.md). Adding new instruments is described in [Download instruments](../market_data_storage/download_instruments.md), [Create instrument](../market_data_storage/create_instrument.md) sections. This file can be edited with standard notepad or MS Excel.
- The **portfolio.csv and position.csv** files contain all received and created portfolios and current position on them. D eleting this file will delete all corresponding data in [Designer](../../designer.md). If information on portfolios is received by Designer at each connection, information on the positions can be lost irrevocably. These files can be edited with standard notepad or MS Excel.
- The **settings.json** file contains current settings. This file is created by [Designer](../../designer.md) when settings were changes or when at closing the program. Deleting this file will reset current settings to the default values. You should not manually edit this file, this can lead to a malfunction of [Designer](../../designer.md).

If it is necessary to edit individual files or reset the Designer settings, it will be useful to backup both individual files and the entire directory.

## Recommended content

[Update to the new version](../update_to_the_new_version.md)
