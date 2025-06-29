# Configuration OpenECry

The interaction mechanism is shown in this figure: 

![OECTrader](../../../../../images/oectrader.png)

As can be seen from the figure, [OpenECryMessageAdapter](xref:StockSharp.OpenECry.OpenECryMessageAdapter) communicates with the OEC server through the [GainFutures API](https://gainfutures.com/gainfuturesapi). Using the [GainFutures API](https://gainfutures.com/gainfuturesapi) does not require a working OEC Trader terminal.

To work with a connector, you have to specify the **Login** and **Password**. **Login** and **Password** are provided by the broker. To get the API access, it is recommended to contact the broker.
