# MetaTrader

[StockSharp](../../../api.md) integrates with MT4 and MT5 terminals through special connectors. To install these connectors, use [Installer](../../../installer.md) (for more details, see [Installing and Removing Programs](../../../installer/install_and_remove_apps.md)).

Both connectors are used in the same way, so below is the description of the process for connecting to MT5:

## Setting Up the MT Connector

> [!Video https://www.youtube.com/embed/qGnIa7YIS5Q]

1. Select the MT connector in [Installer](../../../installer.md) and start the installation process.

   ![MT Install 1](../../../../images/mt_install_1.png)

2. [Installer](../../../installer.md) will ask which folder to install the connector in (it must be installed in the Experts folder).

   ![MT Install 2](../../../../images/mt_install_2.png)

3. If multiple terminals are installed, you need to choose the one where you want to install the connector.

   ![MT Install 3](../../../../images/mt_install_3.png)

4. After selecting the desired terminal, the path to the Experts folder will be displayed.

   ![MT Install 4](../../../../images/mt_install_4.png)

   > [!TIP]
   > - If the path cannot be automatically determined, you need to manually select it through directory search *C:\\Users\\%your_user_name%\\AppData\\Roaming\\MetaQuotes\\Terminal\\%many_letters_and_numbers%\\MQL4\\Experts\\* (for MT5, the path will include MQL5).

5. Complete the installation and wait for it to finish. At the end of the installation, [Installer](../../../installer.md) will warn that you now need to configure the terminal. To do this, launch the MT terminal and connect to trading.
6. In the Tools->Options menu, select the **Experts Advisors** tab and make sure that permission for external DLL trading (**Allow DLL imports**) is enabled:![MT 1](../../../../images/mt_1.png)
7. If the terminal was running during the connector installation (step 2), you need to refresh the list of experts by right-clicking on Experts and selecting **Refresh** from the menu:

   ![MT 2](../../../../images/mt_2.png)

8. Select the S\# expert, right-click and choose **Attach to a chart** from the menu:

   ![MT 3](../../../../images/mt_3.png)

9. A settings window will appear where you can set the login-password (anonymous authorization is enabled by default), as well as the connection address (if connecting to multiple terminals at once, the addresses must contain unique ports).
10. A smiley icon should appear in the upper right corner of the chart (the first one encountered):

    ![MT 4](../../../../images/mt_4.png)

    Also, information about the successful script launch, the number of instruments, should appear in the expert's log window.
11. If the MT4 or MT5 license is not obtained, a line similar to the following will appear in the log:

    ![MT 5](../../../../images/mt_5.png)

12. Connection to MT goes through the FIX protocol, using the [FIX protocol](../common/fix_protocol.md) connector. The program [Terminal](../../../terminal.md) was used for demonstration. Below are settings for transactional connection and market data connection (for MT5, the default port is 23001 instead of 23000):

    ![MT 6](../../../../images/mt_6.png)![MT 7](../../../../images/mt_7.png)

    Similar settings need to be made in [Designer](../../../designer.md), [Hydra](../../../hydra.md), or any API programs.

    Login and password are left empty in case of anonymous authorization (previous item). If connecting to MT with multiple robots, a unique login must be provided for different connections identification.

    > [!TIP]
    > - The script must be launched before connecting StockSharp to MetaTrader and kept running as long as this connection is needed.  
    > - To see historical candles in StockSharp, they need to be downloaded from the MetaTrader server. How to do this, read in MetaTrader's documentation.

    In case of a successful connection, the example should show a list of instruments and accounts:

    ![MT 8](../../../../images/mt_8.png)

13. In case of errors, the connector logs are kept, which are available in the folder **Experts\\StockSharp\\Data\\Log**:

    ![MT 9](../../../../images/mt_9.png)