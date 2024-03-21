# Cloud Testing

To test strategies in the cloud, it's necessary to first find all the instruments of interest. For this, in the **Designer**, you need to open the instrument search panel available for testing in the **Cloud** tab:

![Designer_Backtest_Cloud_01](../images/Designer_Backtest_Cloud_01.png)

By entering the name of the instrument in the test field and clicking on **Search** (or **Enter**), the StockSharp server will return suitable search results. The date ranges of historical data will also be indicated to the right of the instrument names.

This procedure needs to be done only once for each new instrument. After that, the found instruments will be saved locally on the disk, and upon restarting the **Designer**, they will already be loaded from the local storage. This step is necessary because specifying the instrument is required when launching the strategy (as well as when specifying instruments directly in the [Variable](Designer_Variable.md) block).

After this, you need to return to the strategy and enable the cloud option in the **Backtest** tab:

![Designer_Backtest_Cloud_00](../images/Designer_Backtest_Cloud_00.png)

By starting the test, the strategy will be sent to the StockSharp cloud instead of being tested locally:

![Designer_Backtest_Cloud_02](../images/Designer_Backtest_Cloud_02.png)

After the testing is completed, the report with results will be shown in the task waiting tab:

![Designer_Backtest_Cloud_03](../images/Designer_Backtest_Cloud_03.png)

If you want to view the history of testing in the cloud, as well as see the current active tasks, open the **Tasks** panel in the **Cloud** tab:

![Designer_Backtest_Cloud_04](../images/Designer_Backtest_Cloud_04.png)