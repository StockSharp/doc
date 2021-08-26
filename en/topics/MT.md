# S\#.MT4 and S\#.MT5

[S\#](StockSharpAbout.md) provides ingtegration with MT4 and MT5 terminals via special connectors. To install connectors need use [S\#.Installer](SharpInstaller.md) (more [Install  and remove apps](Installer_installing_removing_programs.md)).

Both connectors are used in the same way, so below I will describe the process of connecting to MT4:

### MT connector setup

MT connector setup

1. All MT files should be installed to the folder *C:\\Users\\%user\_name%\\AppData\\Roaming\\MetaQuotes\\Terminal\\%a lot of letters and numbers%\\MQL4\\Experts\\* (in case of MT5 the path will contain MQL5). The structure eventually should look like this (root folder contains the MQL script and MT connector, StockSharp sub\-folder contains necessary S\#.API files:

   ![MT 0](~/images/MT_0.png)
2. Start MT terminal and connect to trading.
3. In menu Tools\-\>Options select **Experts Advisors** tab and make sure you have enabled permission for external dll trading (**Allow DLL imports**):

   ![MT 1](~/images/MT_1.png)
4. If during the installation of the connector (пункт 2) the terminal was running, then you need to update the list of experts by right\-clicking on Experts and selecting in the **Refresh** menu item:

   ![MT 2](~/images/MT_2.png)
5. Select S\# expert in the tree, click the right button, and select in menu **Attach to a chart**:

   ![MT 3](~/images/MT_3.png)
6. In the popup window you can specify login\-password (leave it blank if you want anonymous access). Also, you can specify network address (in case of using multiple terminals you should specify unique port numbers).
7. On a chart (it should be the first random) at the right bottom corner you will see the smile icon:

   ![MT 4](~/images/MT_4.png)

   And also in the expert logs window should appear information about the successful launch of the script.
8. If MT4 or MT5 license is not valid, then the following line will appear in the log:

   ![MT 5](~/images/MT_5.png)
9. Connection to the MT is made via the FIX protocol, us uses [FIX protocol](Fix.md) connector. The app [S\#.Terminal](Terminal.md) is used as a demonstration. Below are the settings for transactional and market data connections (in case of MT5 by default port is 23001 instead of 23000):

   ![MT 6](~/images/MT_6.png)

   ![MT 7](~/images/MT_7.png)

   The same settings should be done in [S\#.Designer](Designer.md), [S\#.Data](Hydra.md) or any other apps uses S\#.API.

   Leave login and password are blank in case of anonymous access (prev step). In case of connection to MT by multiple applications, it is necessary to specify a unique login to identify different connections

   > [!TIP]
   > - The script must be run before connecting StockSharp to the MetaTrader and do not stop it until this connection is needed.  
   > - To see historical candles in StockSharp, they must be downloaded from the MetaTrader server. How to do it, read the documentation of MetaTrader itself.

   In case of successful connection, the example should show a list of instruments and accounts:

   ![MT 8](~/images/MT_8.png)
10. In case of errors, the logs of the connectors are placed into the folder **Experts\\StockSharp\\Data\\Log**:

    ![MT 9](~/images/MT_9.png)
