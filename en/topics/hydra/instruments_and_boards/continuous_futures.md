# Continuous futures

The [Hydra](../../hydra.md) program allows the user to combine different types of market data from various contracts into a single continuous instrument.

To do this, select the **Securities** item in the **Market Data** menu, and the **Securities** panel appears. Before combining the data, check which market data is available. Select the path where the data is located and review the instruments that you plan to combine. If there are gaps, download the missing market data (for example, from a supported data source).

![HydraGluingCheckData](../../../images/hydragluingcheckdata.png)

As an example, consider combining E-mini S&P 500 futures.

1. To create a continuous futures contract, click the **New continuous** button on the **Securities** panel.![Hydra Gluing Check Data 00](../../../images/hydragluingcheckdata_00.png)

   After that, the following window will appear:![HydraGluingWindow](../../../images/hydragluingwindow.png)
2. To create a continuous future, you need to specify a name and add contracts.

   There are two ways to add contracts.
   - Manually: select a contract in the **Security** list and click the **Add** button.![HydraGluingCSCustom](../../../images/hydragluingcscustom.png)
   - If you set the first two letters of the contract in the **Asset code** field, for example, RI, and click the **Auto fill** button, then all the contracts found in the database will be added with their expiration dates.![HydraGluingCSAuto](../../../images/hydragluingcsauto.png)
3. Select the required contracts and set their transition dates. ![Hydra GluingCSAuto 00](../../../images/hydragluingcsauto_00.png)
4. Next, assign the instrument identifier **ES\_continuous@CME** and click the **OK** button, after which a new instrument will be created.
5. Next, select the [Candles](../working_with_data/view_and_export/candles.md) item in the **Market Data** menu, select the resulting instrument and data period, set the **Composite element** value in the **Build from** field, and then click the ![hydra find](../../../images/hydra_find.png) button. ![HydraGluingTrades](../../../images/hydragluingtrades.png)

The generated data can be exported to Excel, XML, JSON or TXT formats. Export is performed using the drop-down list.

![hydra export](../../../images/hydra_export.png)
