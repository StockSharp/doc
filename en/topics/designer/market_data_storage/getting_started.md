# Getting started

In order to create historical data storage, click the ![Designer Creating a repository of historical data 00](../../../images/designer_creating_repository_of_historical_data_00.png) button on the **Market data** tab. By clicking the ![Designer Creating a repository of historical data 01](../../../images/designer_creating_repository_of_historical_data_01.png) button, you can change the current storage parameters. By clicking the ![Designer Creating a repository of historical data 02](../../../images/designer_creating_repository_of_historical_data_02.png) button, you can delete the current storage from the list of storages.

![Designer Creating a repository of historical data 03](../../../images/designer_creating_repository_of_historical_data_03.png)

Historical data storage can be local or remote.

Local storage \- when all data is stored on local computer. To set the local storage, it is sufficient to set the path to the folder with stored data.

Remote storage may be located on a remote computer. To set the remote storage you have to set the remote storage address, login and password, if necessary. 

You can reproduce the remote storage on a local machine, using the [Hydra](../../hydra.md) software (code name Hydra), designed for automatic loading of market data (instruments, candles, tick trades and order books etc.) from various sources and for storing them in the local storage. To do that, switch [Hydra](../../hydra.md) into the server mode.

![Designer Creating a repository of historical data 04](../../../images/designer_creating_repository_of_historical_data_04.png)

After that, in [Designer](../../designer.md), create a new storage by clicking the ![Designer Creating a repository of historical data 00](../../../images/designer_creating_repository_of_historical_data_00.png) button. In the storage settings, in the address field, specify "net.tcp:\/\/localhost:8000". Click OK. When using [Hydra](../../hydra.md) as a remote storage, don't forget, that [Hydra](../../hydra.md) shall be started up and configured accordingly.

![Designer Creating a repository of historical data 05](../../../images/designer_creating_repository_of_historical_data_05.png)

After adding a new storage, it can be selected in the **Storage** drop-down list.

![Designer Creating a repository of historical data 06](../../../images/designer_creating_repository_of_historical_data_06.png)

You also have to select the format of storage files \- BIN or CSV. Data can be stored in two formats: in a special binary BIN format, which provides the maximum compression ratio, or in text format CSV, which is convenient when analyzing data in other programs. The BIN format is preferred when there is a need to save space on disk. The CSV format is preferred when there is a need to adjust data manually. CSV is easily edited by standard notepad, MS Excel, etc.

## Recommended content

[Download instruments](download_instruments.md)
