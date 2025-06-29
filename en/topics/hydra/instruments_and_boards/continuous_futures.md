# Continuous futures

The [Hydra](../../hydra.md) program allows the user to combine different types of market data from various contracts into a single continuous instrument.

To do this, on the **Common** tab select **Securities** so that the **All securities** tab appears. Before gluing the data, check which market data is available. Select the path where the data is located and sequentially view the instruments that you plan to glue. If there are gaps, download the missing market data (for example, from **Финам**).

![HydraGluingCheckData](../../../images/hydragluingcheckdata.png)

As an example, consider combining RTS index futures.

1. To create a continuous futures contract, click the **Create security \=\> Continuous security** button on the **All securities** tab.![Hydra Gluing Check Data 00](../../../images/hydragluingcheckdata_00.png)

   After that, the following window will appear:![HydraGluingWindow](../../../images/hydragluingwindow.png)
2. To create a continuous future, you need to specify a name and add contracts.

   There are two ways to add contracts.
   - Manually by clicking the ![hydra add](../../../images/hydra_add.png) button.![HydraGluingCSCustom](../../../images/hydragluingcscustom.png)
   - If you set the first two letters of the contract as a name, for example, RI, and click the **Auto** button, then all the instruments found in the database will be added.![HydraGluingCSAuto](../../../images/hydragluingcsauto.png)
3. Select the required contracts and set their transition dates. ![Hydra GluingCSAuto 00](../../../images/hydragluingcsauto_00.png)
4. Next, assign the instrument identifier **RI\_long9@FORTS** and click the **OK** button, after which a new instrument will be created.
5. Next, click the [Candles](../working_with_data/view_and_export/candles.md) button on the **Common** tab, select the resulting instrument and data period, set the **Composite element** value in the **Build from** field, and then click the ![hydra find](../../../images/hydra_find.png) button. ![HydraGluingTrades](../../../images/hydragluingtrades.png)

The generated data can be exported to Excel, XML, JSON or TXT formats. Export is performed using the drop-down list.

![hydra export](../../../images/hydra_export.png)
